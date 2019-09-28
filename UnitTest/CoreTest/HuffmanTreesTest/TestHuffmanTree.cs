using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HuffmanCodingDemo.Core.HuffmanTrees;

namespace UnitTest.CoreTest.HuffmanTreesTest
{
    [TestClass]
    public class TestHuffmanTree
    {
        [TestMethod]
        public void TestHuffmanTreeCreate()
        {
            var huffmanTree = new HuffmanTree<char>();
        }
    }
}
