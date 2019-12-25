using System;
using System.Collections;
using HuffmanCodingCore.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.CoreTest.UtilsTest
{
    [TestClass]
    public class ExtensionMethodsTest
    {
        [TestMethod]
        public void ToBytesTest()
        {
            var bitArray = new BitArray(8, false);
            var bytes = bitArray.ToBytes();
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual<byte>(0, bytes[0]);
            // 非 8 倍数位数组测试
            bitArray = new BitArray(13, false);
            bytes = bitArray.ToBytes();
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual<byte>(0, bytes[0]);
            // 测试值转换是否正确
            bitArray.Set(2, true);
            bitArray.Set(0, true);
            bitArray.Set(11, true);
            bitArray.Set(12, true);
            // 根据上面的设定，现在得到的一个 byte 的内容应该为 1010 0000
            bytes = bitArray.ToBytes();
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(Convert.ToByte("10100000", 2), bytes[0]);
        }
    }
}