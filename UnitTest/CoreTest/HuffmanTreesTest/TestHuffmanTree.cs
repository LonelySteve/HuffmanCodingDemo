using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HuffmanCodingCore.Structs.HuffmanTrees;
using System.Collections.Generic;
using HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data;

namespace UnitTest.CoreTest.HuffmanTreesTest
{
    [TestClass]
    public class TestHuffmanTree
    {
        /// <summary>
        /// 空哈夫曼树
        /// </summary>
        public static HuffmanTree<char> EmptyHuffmanTree => new HuffmanTree<char>();
        /// <summary>
        /// 一个简单的哈夫曼树
        /// 权重字典：
        /// A:1 B:2 C:3
        /// 理论上的结构：
        ///      6
        ///   ___|___
        ///   3     C
        /// __|__
        /// A   B
        /// </summary>
        public static HuffmanTree<char> HuffmanTreeInstance_1 => HuffmanTree<char>.CreateFromWeightDictionary(new Dictionary<char, ulong>() { { 'A', 1 }, { 'B', 2 }, { 'C', 3 } });

        [TestMethod]
        public void TestHuffmanTreeCreate()
        {
            TestHuffmanTrees((treeName, tree) =>
            {
                switch (treeName)
                {
                    case "EmptyTree":
                        Assert.IsNull(tree.RootNode);
                        break;
                    case "HuffmanTreeInstance1":
                        Assert.AreEqual<ulong>(6, tree.RootNode.Data.Weight);
                        Assert.AreEqual<ulong>(3, tree.RootNode.LeftNode.Data.Weight);
                        Assert.AreEqual<ulong>(3, tree.RootNode.RightNode.Data.Weight); // C
                        Assert.AreEqual<ulong>(1, tree.RootNode.LeftNode.LeftNode.Data.Weight); // A
                        Assert.AreEqual<ulong>(2, tree.RootNode.LeftNode.RightNode.Data.Weight); // B
                        Assert.IsTrue(tree.RootNode.RightNode.IsLeafNode); // C
                        Assert.IsTrue(tree.RootNode.LeftNode.LeftNode.IsLeafNode); // A
                        Assert.IsTrue(tree.RootNode.LeftNode.RightNode.IsLeafNode); // B
                        Assert.AreEqual('C', ((HuffmanTreeLeafNodeData<char>)tree.RootNode.RightNode.Data).Content);
                        Assert.AreEqual('A', ((HuffmanTreeLeafNodeData<char>)tree.RootNode.LeftNode.LeftNode.Data).Content);
                        Assert.AreEqual('B', ((HuffmanTreeLeafNodeData<char>)tree.RootNode.LeftNode.RightNode.Data).Content);
                        break;
                    default:
                        break;
                }
            });
        }

        [TestMethod]
        public void TestCodeBook()
        {
            TestHuffmanTrees((treeName, tree) =>
            {
                switch (treeName)
                {
                    case "EmptyTree":

                        break;
                    case "HuffmanTreeInstance1":
                        Assert.AreEqual(new BitArray(new bool[] { false, false }), tree.CodeBook['A']); // 00
                        Assert.AreEqual(new BitArray(new bool[] { false, true }), tree.CodeBook['B']); // 01
                        Assert.AreEqual(new BitArray(new bool[] { true }), tree.CodeBook['C']);  // 1
                        break;
                    default:
                        break;
                }
            });
        }
        /// <summary>
        /// 测试用哈夫曼树字典
        /// </summary>
        private void TestHuffmanTrees(Action<string, HuffmanTree<char>> tester)
        {
            tester("EmptyTree", EmptyHuffmanTree);
            tester("HuffmanTreeInstance1", HuffmanTreeInstance_1);
        }
    }
}
