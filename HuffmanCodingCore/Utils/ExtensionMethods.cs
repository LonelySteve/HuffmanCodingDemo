using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuffmanCodingCore.Utils
{ 
    /// <summary>
    /// BitArray 扩展方法类
    /// </summary>
    public static class BitArrayExtensionMethods
    {
        /// <summary>
        ///     转换到字节数组，对于不足一个字节的位将补零
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        [Obsolete("这个方法是不舍弃多余位而补零的转换到字节数组的方法，本项目使用的 ToBytes 方法由此方法改进得到")]
        // 写了个复杂版本发现并没有用，删掉又有点可惜，所以标为弃用
        public static byte[] ToBytesByFillZero(this BitArray bitArray)
        {
            var groupNum = bitArray.Length / 8;
            // 如果位数组的长度不能够被 8 给整除，则让分组数自增加一
            if (bitArray.Length % 8 != 0)
                groupNum++;

            var retVal = new byte[groupNum];

            for (var i = 0; i < groupNum; i++)
            {
                var tmp = 0;
                var remainBitsNum = bitArray.Length - i * 8;
                remainBitsNum = remainBitsNum > 8 ? 8 : remainBitsNum;
                for (var j = 0; j < remainBitsNum; j++)
                    // 将第 i + 1 个分组的第 j + 1 位的真值复制到临时整型 tmp 底字节的高位向底位的方向的第 j + 1 位的值
                    if (bitArray[i * 8 + j])
                        // 将 临时整型 tmp 的最低的一个字节从高位往低位方向的第 j + 1 位的值进行取反
                        tmp ^= 1 << (7 - j); // 将 1 向左移 7 - (j + 1) 位 然后和 临时整型 tmp 进行异或后重新赋值给 tmp
                retVal[i] = (byte) tmp;
            }

            return retVal;
        }

        /// <summary>
        ///     转换到字节数组，对于不足一个字节的位将舍弃
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this BitArray bitArray)
        {
            var groupNum = bitArray.Length / 8;

            var retVal = new byte[groupNum];

            for (var i = 0; i < groupNum; i++)
            {
                var tmp = 0;
                for (var j = 0; j < 8; j++)
                    // 将第 i + 1 个分组的第 j + 1 位的真值复制到临时整型 tmp 底字节的高位向底位的方向的第 j + 1 位的值
                    if (bitArray[i * 8 + j])
                        // 将 临时整型 tmp 的最低的一个字节从高位往低位方向的第 j + 1 位的值进行取反
                        tmp ^= 1 << (7 - j); // 将 1 向左移 7 - (j + 1) 位 然后和 临时整型 tmp 进行异或后重新赋值给 tmp
                retVal[i] = (byte) tmp;
            }

            return retVal;
        }

        public static BitArray Before(this BitArray current, int index)
        {
            var values = new bool[current.Length];
            current.CopyTo(values,0);
            return new BitArray(values.Skip(index).ToArray());
        }

        public static BitArray After(this BitArray current, int index)
        {
            var values = new bool[current.Length];
            current.CopyTo(values, 0);
            return new BitArray(values.Reverse().Skip(current.Length - 1 - index).ToArray());
        }

        #region   连接两个位数组 https://stackoverflow.com/questions/518513/is-there-any-simple-way-to-concatenate-two-bitarray-c-net

        /// <summary>
        ///     在当前位数组之后插入一个指定的位数组，返回拼接后的新位数组
        /// </summary>
        /// <param name="current"></param>
        /// <param name="after">欲插入的位数组</param>
        /// <returns></returns>
        public static BitArray Append(this BitArray current, BitArray after)
        {
            var values = new bool[current.Count + after.Count];
            current.CopyTo(values, 0);
            after.CopyTo(values, current.Count);
            return new BitArray(values);
        }

        /// <summary>
        ///     在当前位数组之前插入一个指定的位数组，返回拼接后的新位数组
        /// </summary>
        /// <param name="current"></param>
        /// <param name="before">欲插入的位数组</param>
        /// <returns></returns>
        public static BitArray Prepend(this BitArray current, BitArray before)
        {
            var values = new bool[current.Count + before.Count];
            before.CopyTo(values, 0);
            current.CopyTo(values, before.Count);
            return new BitArray(values);
        }

        #endregion
        /// <summary>
        /// 判断当前位数组与指定位数组是否相等
        /// </summary>
        /// <param name="current"></param>
        /// <param name="otherBitArray">指定位数组</param>
        /// <returns></returns>
        public static bool Equal(this BitArray current,BitArray otherBitArray)
        {
            if (current.Length != otherBitArray.Length)
            {
                return false;
            }
            // 通过将两个具有相同位数的位数组异或可以得知这两个位数组是否一致
            var result = current.Xor(otherBitArray);
            // 如果异或结果中存在不为 0 (false) 的元素，则证明两个位数组是不一样的
            return result.Cast<bool>().All(i=>i==false);
        }
        /// <summary>
        /// 检测当前可迭代的位数组是否存在指定的位数组
        /// </summary>
        /// <param name="bitArrays"></param>
        /// <param name="bitArray">指定的位数组</param>
        /// <returns></returns>
        public static bool Contain(this IEnumerable<BitArray> bitArrays, BitArray bitArray) => bitArrays.Any(bitArray.Equal);
    }
    /// <summary>
    /// 一些其他的扩展方法
    /// </summary>
    public static class OtherExtensionMethods
    {
        public static bool Contains(this IEnumerable<byte[]> current, byte[] target) => current.Any(target.SequenceEqual);
    }
}