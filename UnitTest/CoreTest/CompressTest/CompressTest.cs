using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HuffmanCodingCore;
using HuffmanCodingCore.Exceptions;
using HuffmanCodingCore.Structs.EncodeArgument;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.CoreTest.CompressTest
{
    [TestClass]
    public class CompressTest
    {
        [TestMethod]
        public void TestCompressAndUnCompressBytes()
        {
            //Assert.ThrowsException<EncodeArgumentException>(() =>
            //    AssertCompressAndUnCompressBytes("-1", new byte[] { 0x00 }, 0));
            //AssertCompressAndUnCompressBytes("0", new byte[] {  }, 1);
            //AssertCompressAndUnCompressBytes("1", new byte[] { 0x00 }, 1);
            //AssertCompressAndUnCompressBytes("2", new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, 1);
            //AssertCompressAndUnCompressBytes("3", new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, 2);
            //AssertCompressAndUnCompressBytes("4", new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, 5);
            //AssertCompressAndUnCompressBytes("5", new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, 10);

            //AssertCompressAndUnCompressBytes("6", new byte[] { 0x00, 0x00, 0x01, 0x02, 0x03 }, 1);
            //AssertCompressAndUnCompressBytes("7", new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, 2);
            //AssertCompressAndUnCompressBytes("8", new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, 5);
            //AssertCompressAndUnCompressBytes("9", new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, 10);
            //AssertCompressAndUnCompressBytes("10", new byte[] { 0x00, 0x01, 0x01, 0x01, 0x01 }, 4);
            //AssertCompressAndUnCompressBytes("11", new byte[] { 0x00, 0x01, 0x01, 0x01, 0x01 }, 5);
            //AssertCompressAndUnCompressBytes("12", new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, 10);

           

         

            using (var sw = new StreamWriter("test_file_1.txt"))
            {
                sw.Write("test1");
            }

            using (var sw = new StreamWriter("test_file_2.txt"))
            {
                sw.Write("test2");
            }


            var fw = new FileStreamWrapper(new Dictionary<string, FileStream>()
            {
                {"test_file_1.txt", File.OpenRead("test_file_1.txt") } ,
                {"test_file_2.txt", File.OpenRead("test_file_2.txt") } 
            });

            AssertCompressAndUnCompressFileStream("13", fw,1);
        }


        public void AssertCompressAndUnCompressBytes(string saveFileName, byte[] sourceData, byte compressLevelFlag)
        {
            var output = new MemoryStream();
            var input = new MemoryStream(sourceData);
            var compressStream = new FileStream(saveFileName + ".test.huf", FileMode.Create);
            compressStream.Seek(0, SeekOrigin.Begin);
            var cw = new CompressStreamWriter(compressStream);
            cw.Write(new StreamWrapper(input), compressLevelFlag, (int) EncryptType.None);
            compressStream.Seek(0, SeekOrigin.Begin);
            var cr = new CompressStreamReader(compressStream);
            cr.Read(() => output);
            output.Seek(0, SeekOrigin.Begin);
            var br = new BinaryReader(output);
            Assert.AreEqual(BitConverter.ToString(sourceData), BitConverter.ToString(br.ReadBytes(sourceData.Length)));
        }

        public void AssertCompressAndUnCompressFileStream(string saveFileName, FileStreamWrapper wrapper, byte compressLevelFlag)
        {
            var compressStream = new FileStream(saveFileName+".multifile.huf",FileMode.Create);
            compressStream.Seek(0, SeekOrigin.Begin);
            var cw = new CompressStreamWriter(compressStream);
            cw.Write(wrapper, compressLevelFlag, (int)EncryptType.None);
            compressStream.Seek(0, SeekOrigin.Begin);
            // 解压缩
            using (var cr = new CompressStreamReader(compressStream))
            {
                cr.Read(null, s => File.Create("recover."+s));
            }

            foreach (var s in wrapper.FileStreams.Keys)
            {
                using (var fs = File.OpenRead("recover."+s))
                {
                    using (var br = new BinaryReader(fs))
                    {
                        wrapper.FileStreams[s].Seek(0, SeekOrigin.Begin);
                        using (var br2 = new BinaryReader(wrapper.FileStreams[s]))
                        {
                            // TODO 长度有可能会溢出，不过这是测试，一般不用管
                            Assert.AreEqual(BitConverter.ToString(br2.ReadBytes((int)br2.BaseStream.Length)), BitConverter.ToString(br.ReadBytes((int)br.BaseStream.Length)));
                        }
                    }
                }
            }
        }
    }
}
