using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuffmanCodingCore.Utils;

namespace HuffmanCodingCore.BitStream
{
    public class BitStreamWriter : BinaryWriter
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

        public BitStreamWriter(Stream output) : base(output) { }
        public BitStreamWriter(Stream output,Encoding encoding) : base(output, encoding) { }
        public BitStreamWriter(Stream output,Encoding encoding,bool leaveOpen) : base(output, encoding,leaveOpen) { }

        /// <summary>
        /// 带缓存位写入的 Flush，不足的补零
        /// </summary>
        public override void Flush()
        {
            // 如果实际缓存位数不等于 0 ，就需要补零写出
            if (BitsBufferActualLength != 0)
            {
                var lackBitCount = 8 - BitsBufferActualLength;
                _bitsBuffer <<= lackBitCount; // 左移补上缺少的零
                Write((byte)BitBuffer);
                // 不要忘了清空缓存
                HardClearBitsBuffer();
            }
            base.Flush();
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


        public void WriteBit(int value)
        {
            WriteBit(value == 1);
        }

        public void WriteBit(bool value)
        {
            if (BitsBufferActualLength < 8)
            {
                _bitsBuffer <<= 1; // 右移一位
                _bitsBuffer |= value ? 1 : 0; // 根据指定值最低位填充 1 或 0
            }
            else // 缓存区满了，，
            {
                // 先写出缓存
                Write((byte)_bitsBuffer);
                SoftClearBitsBuffer();
                // 递归自己再写一次
                WriteBit(value);
            }
        }
 
        public void WriteBits(BitArray value)
        {
            // 为了处理方便，先将 BitArray 对象转换成 bool 型数组
            var tempBuff = new bool[value.Length];
            value.CopyTo(tempBuff, 0);
            // 不优化了，还不如直接写 = =          
            WriteBitBuffer(tempBuff, tempBuff.Length,0);
        }

        private void WriteBitBuffer(IList<bool> buff, int count, int offset = 0)
        {
            for (var i = offset; i < offset + count; i++, BitsBufferActualLength++)
            {
                // 当缓冲区满了，就写出一个字节的数据
                if (BitsBufferActualLength == 8)
                {
                    Write((byte)BitBuffer);
                    SoftClearBitsBuffer();
                }
                _bitsBuffer <<= 1; // 左移一位
                _bitsBuffer |= buff[i] ? 1 : 0; // 根据指定值最低位填充 1 或 0
            }
        }
    }
}
