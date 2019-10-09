using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HuffmanCodingCore.Exceptions;
using HuffmanCodingCore.Structs.EncodeArgument;
using HuffmanCodingCore.Structs.HuffmanTrees;
using HuffmanCodingCore.Utils;

namespace HuffmanCodingCore
{
    /// <summary>
    ///     哈夫曼压缩二进制写入器，直接支持从一个流中读取数据计算字典然后写入至另一个流中
    /// </summary>
    public class CompressStreamWriter : BinaryWriter
    {
        protected BitArray BufferedBitArray = new BitArray(8);
        protected byte BufferedBitArrayActualLength { get; private set; }

        /// <summary>
        ///     清理当前编写器的所有缓冲区，使所有缓冲数据写入基础设备
        /// </summary>
        public override void Flush()
        {
            // 清空位数组缓存，即使位数组不足八位也要写入一个新的字节把数据丢到流里，不足的位补零
            Write(BufferedBitArray.Append(new BitArray(8 - BufferedBitArrayActualLength, false)).ToBytes());
            // 强制使位数组缓存实际长度记为 0
            BufferedBitArrayActualLength = 0;
            // 调用基类 Flush() 函数完成其他缓存数据的写入
            base.Flush();
        }

        /// <summary>
        ///     <para>向流中写入一个布尔数组</para>
        ///     <para>注意：这个方法是按位写入布尔数组数据的，这也意味着它可能不会立即完全写入给定的布尔数组数据，如果需要立即完全写入给定布尔数组数据，可以使用<see cref="Flush" />方法</para>
        /// </summary>
        /// <param name="value"></param>
        public void WriteBits(BitArray value)
        {
            // 计算 新写入的位数组 和 缓存的位数组 位数之和
            var totalBitsNum = value.Length + BufferedBitArrayActualLength;
            // 如果 新写入的位数组 和 缓存的位数组 位数之和还不够 8 位，则将它俩连接一起重新放回缓存
            if (totalBitsNum < 8)
            {
                BufferedBitArray = BufferedBitArray.Append(value);
                // 别忘了改变缓存的实际长度
                BufferedBitArrayActualLength = (byte)totalBitsNum;
            }
            else // 对于位数之和大于 8 的情况，只需要把尾部不足 8 位的数据作为新的位缓存数组数据就行
            {
                var combinedBitArray = BufferedBitArray.Append(value);
                var remainBitsNum = totalBitsNum % 8;
                var tmpBoolArr = new bool[remainBitsNum];
                // 将尾部不足 8 位的数据拷贝到临时布尔数组里
                combinedBitArray.CopyTo(tmpBoolArr, totalBitsNum - remainBitsNum);
                // 别忘了改变缓存的实际长度
                BufferedBitArrayActualLength = (byte)remainBitsNum;
                // 循环给位缓存数组重新赋值
                for (var i = 0; i < tmpBoolArr.Length; i++)
                    BufferedBitArray[i] = tmpBoolArr[i];
                // 写拼接好的位数组，如果尾部有多出来不足 8 位的数据会被直接丢弃
                Write(combinedBitArray.ToBytes());
            }
        }
        /// <summary>
        /// 使用指定的数据包装实例，压缩等级，加密类型和加密秘钥写入当前压缩流
        /// </summary>
        /// <param name="wrapper">数据包装实例</param>
        /// <param name="compressLevel">压缩等级</param>
        /// <param name="encryptType">加密类型</param>
        /// <param name="key">加密秘钥</param>
        public void Write(DataWrapper wrapper, CompressLevel compressLevel = CompressLevel.Normal,
            EncryptType encryptType = EncryptType.None, byte[] key = null)
        {
            Write(wrapper, (byte) compressLevel, (byte) encryptType, key);
        }
        /// <summary>
        /// 使用指定的数据包装实例，压缩等级标志，加密类型标志和加密秘钥写入当前压缩流
        /// </summary>
        /// <param name="wrapper">数据包装实例</param>
        /// <param name="compressLevelFlag">压缩等级标志</param>
        /// <param name="encryptTypeFlag">加密类型标志</param>
        /// <param name="key">加密秘钥</param>
        public void Write(DataWrapper wrapper, byte compressLevelFlag , byte encryptTypeFlag,
            byte[] key = null)
        {
            // 检查输入的参数是否有效
            CheckCompressArgument(compressLevelFlag, encryptTypeFlag, key);
            // 判断数据包装器实际类型
            var dataType = (byte) (wrapper is FileStreamWrapper ? 1 : 0);
            WriteFileFormat(); // 写文件扩展信息
            WriteVersion(); // 写版本信息
            WriteCopyright(); // 写版权信息
            Write(compressLevelFlag); // 写压缩级别
            Write(encryptTypeFlag); // 写加密类型
            Write(dataType); // 写数据类型 0-Stream 1-FileStream
            // 根据数据类型的不同写字典数据（因为对于 FileSteam 类型的数据来说，字典是以所有数据为样本计算的，所以需要分开处理）
            if (dataType == 0) // Stream
            {
                var streamWrapper = wrapper as StreamWrapper;
                Debug.Assert(streamWrapper != null, nameof(streamWrapper) + " != null");
                var weightDictionary = GetCodeBook(new[] {streamWrapper.BaseStream}, compressLevelFlag);
                WriteCodeBook(weightDictionary); // 写字典
                Write(streamWrapper.BaseStream.Length); // 写流长度信息
                WriteCompressData(streamWrapper.BaseStream, weightDictionary, compressLevelFlag, encryptTypeFlag, key,out var remainBitsLength);  // 压缩写入原数据
                Write(remainBitsLength); // 写不足的位信息
            }
            else // FileStream
            {
                var fileStreamWrapper = wrapper as FileStreamWrapper;
                Debug.Assert(fileStreamWrapper != null, nameof(fileStreamWrapper) + " != null");
                var weightDictionary =
                    GetCodeBook(fileStreamWrapper.FileStreams.Cast<Stream>().ToArray(), compressLevelFlag);
                WriteCodeBook(weightDictionary); // 写字典
                Write(fileStreamWrapper.FileStreams.Count); // 写文件数量
                // 循环写每个文件流
                foreach (var kps in fileStreamWrapper.FileStreams)
                {
                    Write(kps.Value.Length); // 写流长度信息
                    WriteString(kps.Key); // 写相对路径
                    WriteCompressData(kps.Value, weightDictionary, compressLevelFlag, encryptTypeFlag, key,out var remainBitsLength); // 压缩写入原数据
                    Write(remainBitsLength); // 写不足的位信息
                }
            }
        }

        /// <summary>
        ///     检查编码参数
        /// </summary>
        /// <param name="compressLevel"></param>
        /// <param name="encryptType"></param>
        /// <param name="key"></param>
        private void CheckCompressArgument(byte compressLevel, byte encryptType, byte[] key)
        {
            if (Array.IndexOf(new byte[] {0, 1}, encryptType) == -1)
                throw new EncodeArgumentException("encryptTypeFlag");
            if (encryptType != 0 && key.Length != 160) throw new EncodeArgumentException("key");
        }

        protected static Dictionary<byte[], BitArray> GetCodeBook(IEnumerable<Stream> streams, byte compressLevel)
        {
            var weightDictionary = new Dictionary<byte[], ulong>();

            foreach (var stream in streams)
            {
                // 移动输入流的位置到开头
                stream.Seek(0, SeekOrigin.Begin);
                // 循环读取单位字节数到缓冲数组中，之后进行计数操作
                while (stream.Position < stream.Length)
                {
                    int readByte;
                    // 建立一个具有合适大小的字节缓存数组
                    var buffer = new byte[Math.Min(compressLevel, stream.Length - stream.Position)];
                    for (var i = 0; i < compressLevel && (readByte = stream.ReadByte()) >= 0; i++)
                        buffer[i] = (byte) readByte;
                    if (weightDictionary.ContainsKey(buffer))
                        weightDictionary[buffer]++;
                    else
                        weightDictionary[buffer] = 1;
                }
            }
 
            // 创建哈夫曼树
            var huffmanTree = HuffmanTree<byte[]>.CreateFromWeightDictionary(weightDictionary,new ByteArrayEqualityComparer());
            // 获取编码本
            var codeBook = huffmanTree.CodeBook;
            return codeBook;
        }

        #region 只读字段

        public static readonly string FileFormat = "HUF";
        public static readonly byte Version = 0x01;

        public static readonly byte[] Copyright =
        {
            0x4d, 0x61, 0x64, 0x65, 0x20, 0x42, 0x79, 0x20,
            0x4a, 0x4c, 0x6f, 0x65, 0x76, 0x65, 0x27, 0x48,
            0x75, 0x66, 0x66, 0x6d, 0x61, 0x6e, 0x43, 0x6f,
            0x6d, 0x70, 0x72, 0x65, 0x73, 0x73, 0x42, 0x69,
            0x6e, 0x61, 0x72, 0x79, 0x57, 0x72, 0x69, 0x74,
            0x65, 0x72, 0x20, 0x76, 0x31
        };

        #endregion

        #region 构造函数

        public CompressStreamWriter()
        {
            BufferedBitArrayActualLength = 0;
        }

        public CompressStreamWriter(Stream output) : base(output)
        {
            BufferedBitArrayActualLength = 0;
        }

        public CompressStreamWriter(Stream output, Encoding encoding) : base(output, encoding)
        {
            BufferedBitArrayActualLength = 0;
        }


        public CompressStreamWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding,
            leaveOpen)
        {
            BufferedBitArrayActualLength = 0;
        }

        #endregion

        #region 受保护的写入方法

        /// <summary>
        /// <para>使用指定的编码字典，压缩等级标志，加密类型标志和秘钥压缩写入指定流中的数据。</para>
        /// <para>返回压缩后使用的字节数</para>
        /// </summary>
        /// <param name="value">欲压缩写入的流</param>
        /// <param name="codeBook">编码字典</param>
        /// <param name="compressLevelFlag">压缩等级标志</param>
        /// <param name="encryptTypeFlag">加密类型标志</param>
        /// <param name="key">加密秘钥</param>
        /// <param name="remainBufferBitArrayLength">编码时不足八位而强行补零的位数</param>
        /// <returns></returns>
        protected long WriteCompressData(Stream value, IReadOnlyDictionary<byte[], BitArray> codeBook,
            byte compressLevelFlag, byte encryptTypeFlag, byte[] key,out byte remainBufferBitArrayLength)
        {
            // 写 Hash 值
            WriteHashData(value);
            // 记住未写压缩数据是当前压缩流的位置
            var currentPosition = BaseStream.Position;
            // 循环压缩写入指定流的数据
            value.Seek(0, SeekOrigin.Begin);
            while (value.Position < value.Length)
            {
                // 取压缩等级标志和当前被压缩流剩余字节数最小的值作为缓存长度
                var buffLength = Math.Min(compressLevelFlag, value.Length - value.Position);
                // 初始化新的未压缩字节缓存
                var unCompressByteBuff = new byte[buffLength];
                // 加载缓存，由于上面算法的原因，这里在读取字节只会读小于或等于压缩等级标志值的数量
                value.Read(unCompressByteBuff, 0, (int)buffLength);
                // 检查该键是否在编码字典中 
                if (!codeBook.Keys.Contains(unCompressByteBuff)) throw new CodeBookKeyNotFoundException(unCompressByteBuff);
                // 通过查字典获取编码后的位数组
                var compressedBits = codeBook[unCompressByteBuff];
                // 向自身输出流输出编码后的位数组
                WriteBits(compressedBits);
 
                // TODO 实现加密
            }
            remainBufferBitArrayLength = BufferedBitArrayActualLength;
            // 将当前压缩流所在位置减去未写压缩数据时所在位置就能得到压缩后数据占用的字节数
            return BaseStream.Position - currentPosition; // 调用 BaseStream 时 会调用 Flush() 函数完成 位数组缓存的 写入
        }

        protected void WriteHashData(Stream value)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var hashBytes = sha1.ComputeHash(value);
                Write(hashBytes);
            }
        }

        protected void WriteString(string str)
        {
            Write(str.Length);
            Write(str);
        }

        protected void WriteFileFormat()
        {
            Write(FileFormat.ToCharArray());
        }

        protected void WriteVersion()
        {
            Write(Version);
        }

 
        protected void WriteCopyright()
        {
            var encoding = new UTF8Encoding();
            Write(encoding.GetString(Copyright));
        }

        protected void WriteCodeBook(Dictionary<byte[], BitArray> codeBook)
        {
            // 写键值对数量
            Write7BitEncodedInt(codeBook.Count);
            foreach (var kps in codeBook)
            {
                // 写被编码字节长度
                Write7BitEncodedInt(kps.Key.Length);
                // 写被编码字节数据
                Write(kps.Key);
                // 写编码位数
                Write7BitEncodedInt(kps.Value.Length);
                // 写编码数据
                WriteBits(kps.Value);
                // 缺失位数补零
                Flush();
            }
        }

        #endregion
    }

    class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] x, byte[] y)
        {
            return x != null && y != null ? x.SequenceEqual(y) : x == y;
        }

        public int GetHashCode(byte[] obj)
        {
            return obj.Sum(b => b.GetHashCode());
        }
    }
}