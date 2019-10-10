using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HuffmanCodingCore.Structs.HuffmanTrees;
using System.Collections.Generic;
using HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data;
using HuffmanCodingCore.Utils;

namespace UnitTest.CoreTest.HuffmanTreesTest
{
    [TestClass]
    public class TestHuffmanTree
    {
        /// <summary>
        /// 空哈夫曼树
        /// </summary>
        public static HuffmanTree<char> EmptyHuffmanTree => HuffmanTree<char>.CreateFromWeightDictionary();
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
                        // BitArray 对象自带的Equals方法有问题，这里使用扩展的 Equal 方法来比较
                        Assert.IsTrue( new BitArray(new[] { false, false }).Equal(tree.CodeBook['A']));// 00
                        Assert.IsTrue( new BitArray(new []{false,true}).Equal(tree.CodeBook['B']) ); // 01
                        Assert.IsTrue( new BitArray(new []{true}).Equal(tree.CodeBook['C'])); // 1
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
