using System;
using System.Text;
using HuffmanCodingDemo.Core.BinaryTrees;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.CoreTest.BinaryTreesTest
{
    [TestClass]
    public class TestBinaryTree
    {
        public BinaryTree<char> testTree1;
        public TestBinaryTree()
        {
            // 构建根节点
            BinaryTreeNode<char> rootNode =
                new BinaryTreeNode<char>('A',
                new BinaryTreeNode<char>('B',
                new BinaryTreeNode<char>('D')),
                new BinaryTreeNode<char>('C', null,
                new BinaryTreeNode<char>('E')));
            testTree1 = new BinaryTree<char>(rootNode);
        }
        /// <summary>
        /// <para>测试二叉树的创建</para>
        /// <para>创建的二叉树结构如下：</para>
        /// <para>
        ///   A
        ///  B  C
        /// D $ $ E
        ///$ $    $ $
        /// </para>
        /// <para>其中 $ 表示空树</para>
        /// </summary>
        [TestMethod]
        public void TestCreateTree()
        {
            // 获取此二叉树的前序，中序以及后序遍历字符串以验证二叉树建立正确
            var preStringBuilder = new StringBuilder();
            foreach (var node in testTree1.PreIterator)
            {
                preStringBuilder.Append(node.Data);
            }
            var inStringBuilder = new StringBuilder();
            foreach (var node in testTree1.InIterator)
            {
                inStringBuilder.Append(node.Data);
            }
            var postStringBuilder = new StringBuilder();
            foreach (var node in testTree1.PostIterator)
            {
                postStringBuilder.Append(node.Data);
            }
            Assert.AreEqual("ABDCE", preStringBuilder.ToString());
            Assert.AreEqual("DBACE", inStringBuilder.ToString());
            Assert.AreEqual("DBECA", postStringBuilder.ToString());
        }

        [TestMethod]
        public void TestTreeSearch()
        {

        }
    }
}
