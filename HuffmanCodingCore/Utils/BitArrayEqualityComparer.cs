using System.Collections;
using System.Collections.Generic;

namespace HuffmanCodingCore.Utils
{
    public class BitArrayEqualityComparer : IEqualityComparer<BitArray>
    {
        public bool Equals(BitArray x, BitArray y)
        {
            return x.Equal(y);
        }

        // https://stackoverflow.com/questions/3125676/generating-a-good-hash-code-gethashcode-for-a-bitarray
        public int GetHashCode(BitArray obj)
        {
            var hash = 0;
            foreach (var value in obj.GetInternalValues()) hash ^= value;
            return hash;
        }
    }
}