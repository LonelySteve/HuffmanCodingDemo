using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HuffmanCodingCore.Exceptions;

namespace HuffmanCodingCore
{
    /// <summary>
    /// 解压缩读取器
    /// </summary>
    public class CompressStreamReader : BinaryReader
    {
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

        protected BitArray BufferedBitArray = new BitArray(8);
        protected byte BufferedBitArrayActualLength { get; private set; }
        public CompressStreamReader(Stream input) : base(input)
        {
            BufferedBitArrayActualLength = 0;
        }

        public CompressStreamReader(Stream input, Encoding encoding) : base(input, encoding)
        {
            BufferedBitArrayActualLength = 0;
        }

        public CompressStreamReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            BufferedBitArrayActualLength = 0;
        }

        public DataWrapper Read(byte[] key=null,Func<Stream> onStreamWrapperCallback = null, Func<string, FileStream> onFileStreamWrapperCallback = null)
        {
            // 读取文件格式标识
            ReadFileFormat();
            ReadVersion();
            ReadCopyRight();
            var compressLevelFlag = ReadByte();
            var encryptTypeFlag = ReadByte();
            var dataTypeFlag = ReadByte();
            if (dataTypeFlag == 0) // 0-Stream
            {
                if (onStreamWrapperCallback != null)
                {
                    var outputStream = onStreamWrapperCallback();
                    var wrapper = new StreamWrapper(outputStream);
                    var codeBook =  ReadCodeBook();  // 获取编码字典
                    var streamLength = ReadInt64();  // 获取压缩数据字节长度
                    // Seek 到指示剩余位的处，读取不足位个数
                    BaseStream.Seek(streamLength, SeekOrigin.Current);
                    var remainBitsLength = ReadByte();
                    ReadCompressData(wrapper.BaseStream,codeBook, compressLevelFlag, encryptTypeFlag, key, remainBitsLength);
                    return wrapper;
                }
            }else if (dataTypeFlag == 1) // 0-FileStream
            {
                if (onFileStreamWrapperCallback != null)
                {
                    Dictionary<string, FileStream> fileStreamsDictionary = new Dictionary<string, FileStream>();
                    var codeBook = ReadCodeBook();  // 获取编码字典
                    while (BaseStream.Position < BaseStream.Length)
                    {
                        var fileRelativePath = ReadString(); // 获取文件相对路径
                        var fileStream = onFileStreamWrapperCallback(fileRelativePath);
                        var streamLength = ReadInt64(); // 获取压缩数据字节长度
                        // Seek 到指示剩余位的处，读取不足位个数
                        BaseStream.Seek(streamLength, SeekOrigin.Current);
                        var remainBitsLength = ReadByte();
                        ReadCompressData(fileStream, codeBook, compressLevelFlag, encryptTypeFlag, key, remainBitsLength);
                        fileStreamsDictionary[fileRelativePath] = fileStream;
                    }
                    return new FileStreamWrapper(fileStreamsDictionary);
                }
            }
            return null;
        }

        private void ReadCompressData(long streamLength, IReadOnlyDictionary<byte[], BitArray> codeBook,
            byte compressLevel, byte encryptType, byte[] key, byte remainBitsLength)
        {
            var hash = ReadHashData();
            // 循环读取
        }

        private byte[] ReadHashData()
        {
            throw new NotImplementedException();
        }

        private Dictionary<byte[],BitArray> ReadCodeBook()
        {
            var kpsCount = Read7BitEncodedInt(); // 获取键值对数量
            for (int i = 0; i < kpsCount; i++)
            {
                // 读被编码字节长度
                var encodedByteLength = Read7BitEncodedInt();
                
            }
        }

        public override byte ReadByte()
        {
            if (BufferedBitArrayActualLength == 0)
            {
                return base.ReadByte();
            }

            var newByte = base.ReadByte();
            
        }

        public bool ReadBit()
        {
            throw new NotImplementedException();
        }

        public BitArray ReadBits(byte num)
        {
            throw new NotImplementedException();
        }


        private void ReadCopyRight()
        {
            var copyRightString  = ReadString();
            var encoding = new UTF8Encoding();
            var copyRightBytes = (IEnumerable<byte>)encoding.GetBytes(copyRightString);
            if (!copyRightBytes.Equals(Copyright))
            {
                throw new CopyRightMismatchException();
            }
        }

        private void ReadVersion()
        {
           var ver = ReadByte();
           if (ver != Version)
           {
               throw new VersionMismatchException();
           }
        }

        private void ReadFileFormat()
        {
            var chars = ReadChars(3);
            if (FileFormat != new string(chars))
            {
                throw new FileFormatException();
            }
        }
    }
}