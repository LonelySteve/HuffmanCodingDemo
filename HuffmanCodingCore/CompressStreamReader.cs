﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HuffmanCodingCore.BitStream;
using HuffmanCodingCore.Exceptions;
using HuffmanCodingCore.Utils;

namespace HuffmanCodingCore
{
    /// <summary>
    ///     解压缩读取器
    /// </summary>
    public class CompressStreamReader : BitStreamReader
    {
        public CompressStreamReader(Stream input) : base(input)
        {
        }

        public CompressStreamReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public CompressStreamReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public long Read(Func<Stream> onStreamCallback = null,
            Func<string, FileStream> onFileStreamCallback = null, byte[] key = null,
            bool autoCloseOutputFileStream = true)
        {
            var readBytesCount = 0L;
            // 读取文件格式标识
            ReadFileFormat();
            ReadVersion();
            var compressDataBlockByteCount = ReadInt64();
            ReadCopyRight();
            var compressLevelFlag = ReadByte();
            var encryptTypeFlag = ReadByte();
            var dataTypeFlag = ReadByte();

            var currentPosition = BaseStream.Position;
            BaseStream.Seek(compressDataBlockByteCount, SeekOrigin.Current);
            var compressDataBlockMetaData = ReadCompressDataBlockMetaData().ToList();
            BaseStream.Seek(currentPosition, SeekOrigin.Begin);

            if (dataTypeFlag == 0) // 0-Stream
            {
                if (onStreamCallback != null)
                {
                    var codeBook = ReadCodeBook(); // 获取编码字典
                    var metaData = compressDataBlockMetaData.First(); // 对于这种情形就有且只有一种压缩数据块元数据
                    readBytesCount = ReadCompressDataBlock(onStreamCallback(), metaData.Item1, metaData.Item2, codeBook,
                        compressLevelFlag, encryptTypeFlag, key);
                }
            }
            else if (dataTypeFlag == 1) // 0-FileStream
            {
                if (onFileStreamCallback != null)
                {
                    var codeBook = ReadCodeBook(); // 获取编码字典
                    foreach (var metaData in compressDataBlockMetaData)
                    {
                        // 获取文件相对路径
                        var fileRelativePath = ReadString();
                        // 获取输出文件流
                        var fileStream = onFileStreamCallback(fileRelativePath);
                        // 读取压缩数据块
                        readBytesCount += ReadCompressDataBlock(fileStream, metaData.Item1, metaData.Item2, codeBook, compressLevelFlag,
                            encryptTypeFlag, key);
                        if (autoCloseOutputFileStream) fileStream.Dispose();
                    }
                }
            }

            return readBytesCount;
        }

        private IEnumerable<Tuple<long, byte>> ReadCompressDataBlockMetaData()
        {
            var compressDataBlockCount = Read7BitEncodedInt();
            var retList = new List<Tuple<long, byte>>(compressDataBlockCount);

            // 循环获取压缩数据块元数据
            for (var i = 0; i < compressDataBlockCount; i++)
                retList.Add(new Tuple<long, byte>(Read7BitEncodedInt(), ReadByte()));
            return retList;
        }

        private long ReadCompressDataBlock(Stream outputStream, long compressDataBytes, byte remainBitsCount,
            IReadOnlyDictionary<BitArray, byte[]> codeBook,
            byte compressLevel, byte encryptType, byte[] key)
        {
            // 如果压缩的数据字节数和多余的位数为 0 就不管，不读任何东西
            if (compressDataBytes == 0 && remainBitsCount == 0)
            {
                return 0;
            }
            // 读取 hash 数据
            var srcHashValue = ReadHashData();
            // 创建一个 List 用于读取编码位的缓存
            var encodedBitsBuff = new List<bool>();

            if (remainBitsCount != 0)
                compressDataBytes--;
            // 记录输出流当前位置，作为输出流开始位置
            var outputStreamStartPosition = outputStream.Position;

            for (var i = 0; i < compressDataBytes * 8 + remainBitsCount; i++)
            {
                // 取一位的数据加入编码位缓存列表
                encodedBitsBuff.Add(ReadBit());
                // 尝试将当前编码位缓存列表作为有效的位数组
                var assumptiveValidBits = new BitArray(encodedBitsBuff.ToArray());
                // 比较试图作为有效的位数组是否满足编码字典（尝试解码）
                if (codeBook.TryGetValue(assumptiveValidBits, out var validBytes))
                {
                    // 输出解码后的字节
                    outputStream.Write(validBytes, 0, validBytes.Length);
                    // 清空读取编码位用到的列表缓存
                    encodedBitsBuff.Clear();
                }
            }

            // 理论上列表缓存应该刚好被最后一次解码给清空，这里验证一下，如果不是这样很有可能解码出问题了
            if (encodedBitsBuff.Count != 0) throw new DecodeException();
            // TODO 实现解密
            // 通过 Hash 检验输出流是否有效
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                outputStream.Seek(outputStreamStartPosition, SeekOrigin.Begin);
                var currentHashValue = sha1.ComputeHash(outputStream);
                if (!new ByteArrayEqualityComparer().Equals(srcHashValue, currentHashValue))
                    throw new HashMismatchException();
            }

            // 记得别忘了清空缓存，读下一个压缩数据块的时候不能带上上次的缓存的
            HardClearBitsBuffer();
            // 记录输出流当前位置，作为输出流结束位置
            var outputStreamEndPosition = outputStream.Position;

            return outputStreamEndPosition - outputStreamStartPosition;
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private byte[] ReadHashData()
        {
            // 读 hash 数据占用的字节数
            var hashByteLength = Read7BitEncodedInt();
            // 读 hash 数据
            var hashBytes = ReadBytes(hashByteLength);
            return hashBytes;
        }

        private Dictionary<BitArray, byte[]> ReadCodeBook()
        {
            var retVal = new Dictionary<BitArray, byte[]>(new BitArrayEqualityComparer());
            var kpsCount = Read7BitEncodedInt(); // 获取键值对数量
            for (var i = 0; i < kpsCount; i++)
            {
                // 读被编码字节长度
                var encodedByteLength = Read7BitEncodedInt();
                // 读被编码字节数据
                var encodedBytes = ReadBytes(encodedByteLength);
                // 读编码位数
                var encodedBitCount = Read7BitEncodedInt();
                // 读编码数据
                var encodedBits = ReadBits(encodedBitCount);
                // 对于可能存在的填充数据就丢掉啦~
                SoftClearBitsBuffer();
                retVal[encodedBits] = encodedBytes;
            }

            return retVal;
        }


        private string ReadCopyRight()
        {
            return ReadString();
        }

        private void ReadVersion()
        {
            var ver = ReadByte();
            if (ver != Version) throw new VersionMismatchException();
        }

        private void ReadFileFormat()
        {
            var chars = ReadChars(3);
            if (FileFormat != new string(chars)) throw new FileFormatException();
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
    }
}