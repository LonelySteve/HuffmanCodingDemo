using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HuffmanCodingCore.BitStream
{
     public class BitStreamReader:BinaryReader
    {
        private int _bitsBuffer;

        protected int BitBuffer
        {
            get => _bitsBuffer;
            set
            {
                _bitsBuffer = value;
                BitsBufferActualLength = 8;
            }
        }
        /// <summary>
        /// <para>获取或设置位缓存实际长度</para>
        /// <para>一般不用设置位缓存实际长度，除非你知道你在干什么</para>
        /// </summary>
        protected int BitsBufferActualLength { get; set; }

        public BitStreamReader(Stream input) : base(input)
        {
        }

        public BitStreamReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public BitStreamReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        /// <summary>
        /// 软清空位缓存，仅强制设位缓存实际长度为 0
        /// </summary>
        protected void SoftClearBitsBuffer()
        {
            BitsBufferActualLength = 0;
        }

        /// <summary>
        /// 硬清空位缓存，除了强制设位缓存实际长度为 0，还将位缓存字段重置为 0
        /// </summary>
        protected void HardClearBitsBuffer()
        {
            _bitsBuffer = BitsBufferActualLength = 0;
        }

        /// <summary>
        /// 读取一个位，返回一个布尔值表示这个位（0 为 false，1 为 true）
        /// </summary>
        /// <returns></returns>
        public bool ReadBit()
        {
            if (BitsBufferActualLength == 0)
            {
                // 读一个临时字节
                var tempReadByte = ReadByte();
                var tempBitBuffer = 0;
                // 将这个字节的位反向处理再送到 BitBuffer
                for (var j = 0; j < 8; j++)
                {
                    tempBitBuffer |= ((tempReadByte >> j) & 1) << (7 - j);
                }
                BitBuffer = tempBitBuffer;
            }
            var retVal = (_bitsBuffer & 1) == 1;
            _bitsBuffer >>= 1; // 右移一位
            BitsBufferActualLength--;
            return retVal;
        }

        /// <summary>
        /// 读取指定数量的位，返回包含这些位信息的位数组
        /// </summary>
        /// <param name="count">指定的位数量</param>
        /// <returns></returns>
        public BitArray ReadBits(int count)
        {
            // 检查参数是否有效
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if(count == 0)
                return new BitArray(0);
            // 创建合适的布尔值数组作为位缓存
            var buff = new bool[count];
            // 直接读取
            ReadBitBuffer(buff, count);
            return new BitArray(buff);
        }

        /// <summary>
        /// 从位缓存中读取指定数量的位数据到指定布尔型缓存数组中
        /// </summary>
        /// <param name="buff">布尔型缓存数组</param>
        /// <param name="count">指定数量的位数据</param>
        /// <param name="offset">从<paramref name="buff"/>数组开头偏移指定量</param>
        private void ReadBitBuffer(IList<bool> buff, int count,int offset= 0)
        {
            for (var i = 0; i < count; i++)
            {
                // 如果当前缓存区实际缓存减到为 0，则再读一个字节到缓冲区
                if (BitsBufferActualLength == 0)
                {
                    // 读一个临时字节
                    var tempReadByte = ReadByte();
                    var tempBitBuffer = 0;
                    // 将这个字节的位反向处理再送到 BitBuffer
                    for (var j = 0; j < 8; j++)
                    {
                        tempBitBuffer |= ((tempReadByte >> j) & 1) << (7 - j);
                    }

                    BitBuffer = tempBitBuffer;
                }
                
                buff[offset+i] = (_bitsBuffer & 1) == 1;
                _bitsBuffer >>= 1; // 右移一位
                BitsBufferActualLength--; // 实际缓存位数自减一
            }
        }
    }
}