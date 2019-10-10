using System.Collections.Generic;
using System.Linq;

namespace HuffmanCodingCore.Utils
{
   public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
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