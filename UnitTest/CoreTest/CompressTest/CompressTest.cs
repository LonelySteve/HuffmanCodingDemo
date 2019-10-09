using System;
using System.IO;
using HuffmanCodingCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.CoreTest.CompressTest
{
    [TestClass]
    public class CompressTest
    {
        /// <summary>
        /// 测试空流情况
        /// </summary>
        [TestMethod]
        public void TestEmptyCompress()
        {
            var input = new MemoryStream();
            var output = new MemoryStream();
            var wrapper = new StreamWrapper(input);
            var cw = new CompressStreamWriter(output);
            cw.Write(wrapper);
            // 对于空的输入流来说，字典数据部分和实际压缩数据应该不会写出

        }
 
        [TestMethod]
        public void TestCompressAndUnCompress()
        {
            
            var output = new MemoryStream();
            var input = new MemoryStream(new byte[] { 0x01, 0x02, 0x03 });
            // 测试只有一个字节的情况
            // 测试多个字节的情况
            // 测试与压缩级别相同字节数的情况
            // 测试比测试级别数更多的情况
            var output = new FileStream("test.dat",FileMode.Create);
            var input = new MemoryStream(new byte[] { 0x01,0x02,0x03});
            var data = new StreamWrapper(input);
            var cw = new CompressStreamWriter(output);
 
        }
    }
}
