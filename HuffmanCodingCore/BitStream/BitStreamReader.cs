using System;
using System.Collections;
using System.IO;
using System.Text;

namespace HuffmanCodingCore.BitStream
{
    public class BitStreamReader:BinaryReader
    {
        private byte _bitsBuffer = 0;
        private int _bitsActualLength = 0;

        public BitStreamReader(Stream input) : base(input)
        {
        }

        public BitStreamReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public BitStreamReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public override byte ReadByte()
        {
            
            return base.ReadByte();
        }

        public BitArray ReadBits(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if(count == 0)
                return new BitArray(0);
           
            bool[] buff = new bool[count];
            if (_bitsActualLength == 0)
            {
                var byteBuff = ReadBytes(count / 8);
                _bitsBuffer = ReadByte();
                _bitsActualLength = 8;
                var bitBuffer = new bool[count % 8];
                var value = (_bitsBuffer >> 1) & 1;
            }
          

            throw new NotImplementedException();
        }

        public bool ReadBit()
        {
            throw new NotImplementedException();
        }
    }
}