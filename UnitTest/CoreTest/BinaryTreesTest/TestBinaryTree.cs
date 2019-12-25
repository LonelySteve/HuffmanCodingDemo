using System;
using System.Text;
using HuffmanCodingCore.Iterators.BinaryTreeIterators;
using HuffmanCodingCore.Structs.BinaryTrees;
using HuffmanCodingCore.Structs.BinaryTrees.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.CoreTest.BinaryTreesTest
{
    [TestClass]
    public class TestBinaryTree
    {
        /// <summary>
        ///     空树
        /// </summary>
        public BinaryTree<char> EmptyBinaryTree => new BinaryTree<char>();

        /// <summary>
        ///     标准的二叉树对象
        ///     A
        ///     __|__
        ///     B   C
        /// </summary>
        public BinaryTree<char> StandardBinaryTree
        {
            get
            {
                // 构建根节点
                var rootNode =
                    new BinaryTreeNode<char>('A', new BinaryTreeNode<char>('B'), new BinaryTreeNode<char>('C'));
                return new BinaryTree<char>(rootNode);
            }
        }

        /// <summary>
        ///     一个普通的二叉树对象
        ///     A
        ///     ___|___
        ///     B     C
        ///     __|__ __|__
        ///     D         E
        /// </summary>
        public BinaryTree<char> NormalBinaryTree
        {
            get
            {
                var bNode = new BinaryTreeNode<char>('B', new BinaryTreeNode<char>('D'));
                var cNode = new BinaryTreeNode<char>('C', null, new BinaryTreeNode<char>('E'));
                // 构建根节点
                var rootNode = new BinaryTreeNode<char>('A', bNode, cNode);

                return new BinaryTree<char>(rootNode);
            }
        }

        /// <summary>
        ///     <para>测试二叉树的创建</para>
        ///     <para>创建的二叉树结构如下：</para>
        ///     <para>
        ///         A
        ///         B  C
        ///         D $ $ E
        ///         $ $    $ $
        ///     </para>
        ///     <para>其中 $ 表示空树</para>
        /// </summary>
        [TestMethod]
        public void TestCreateTree()
        {
            TestBinaryTrees((treeName, tree) =>
            {
                // 获取此二叉树的前序，中序以及后序遍历字符串以验证二叉树建立正确
                var preStringBuilder = new StringBuilder();
                foreach (var node in tree.PreIterator) preStringBuilder.Append(node.Data);
                var inStringBuilder = new StringBuilder();
                foreach (var node in tree.InIterator) inStringBuilder.Append(node.Data);
                var postStringBuilder = new StringBuilder();
                foreach (var node in tree.PostIterator) postStringBuilder.Append(node.Data);

                switch (treeName)
                {
                    case "EmptyTree":
                        Assert.AreEqual("", preStringBuilder.ToString());
                        Assert.AreEqual("", inStringBuilder.ToString());
                        Assert.AreEqual("", postStringBuilder.ToString());
                        break;
                    case "NormalBinaryTree":
                        Assert.AreEqual("ABDCE", preStringBuilder.ToString());
                        Assert.AreEqual("DBACE", inStringBuilder.ToString());
                        Assert.AreEqual("DBECA", postStringBuilder.ToString());
                        break;
                    case "StandardBinaryTree":
                        Assert.AreEqual("ABC", preStringBuilder.ToString());
                        Assert.AreEqual("BAC", inStringBuilder.ToString());
                        Assert.AreEqual("BCA", postStringBuilder.ToString());
                        break;
                }
            });
        }

        [TestMethod]
        public void TestTreeSearch()
        {
            TestBinaryTrees((treeName, tree) =>
            {
                // 使用三种遍历方法获取 数据段 为 'B' 的结点
                var preResult = tree.FindOne(node => node.Data == 'B');
                var inResult = tree.FindOne(node => node.Data == 'B', IteratorMode.In);
                var postResult = tree.FindOne(node => node.Data == 'B', IteratorMode.Post);
                switch (treeName)
                {
                    case "EmptyTree":
                        // 对于空树是无法找到 'B' 结点的，所以应当会得到 null 值
                        Assert.AreEqual((object) null, preResult);
                        Assert.AreEqual((object) null, inResult);
                        Assert.AreEqual((object) null, postResult);
                        break;
                    case "NormalBinaryTree":
                    case "StandardBinaryTree":
                        // 比较三种遍历方法获取的结点是否一致
                        Assert.AreEqual((object) preResult, inResult);
                        Assert.AreEqual((object) inResult, postResult);
                        Assert.AreEqual((object) preResult, postResult);
                        break;
                }
            });
        }

        [TestMethod]
        public void TestTreeDepth()
        {
            TestBinaryTrees((treeName, tree) =>
            {
                switch (treeName)
                {
                    case "EmptyTree":
                        Assert.AreEqual(0, tree.Depth);
                        break;
                    case "NormalBinaryTree":
                        Assert.AreEqual(3, tree.Depth);
                        break;
                    case "StandardBinaryTree":
                        Assert.AreEqual(2, tree.Depth);
                        break;
                }
            });
        }

        [TestMethod]
        public void TestTreeLeafNodes()
        {
            TestBinaryTrees((treeName, tree) =>
            {
                switch (treeName)
                {
                    case "EmptyTree":
                        Assert.AreEqual(0, tree.LeafNodeCount);
                        Assert.AreEqual(0, tree.LeafNodes.Count);
                        break;
                    case "NormalBinaryTree":
                        Assert.AreEqual(2, tree.LeafNodeCount);
                        Assert.AreEqual(2, tree.LeafNodes.Count);
                        tree.LeafNodes.Find(node => node.Data == 'D');
                        tree.LeafNodes.Find(node => node.Data == 'E');
                        break;
                    case "StandardBinaryTree":
                        Assert.AreEqual(2, tree.LeafNodeCount);
                        Assert.AreEqual(2, tree.LeafNodes.Count);
                        tree.LeafNodes.Find(node => node.Data == 'B');
                        tree.LeafNodes.Find(node => node.Data == 'C');
                        break;
                }
            });
        }

        /// <summary>
        ///     测试用二叉树字典
        /// </summary>
        private void TestBinaryTrees(Action<string, BinaryTree<char>> tester)
        {
            tester("EmptyTree", EmptyBinaryTree);
            tester("NormalBinaryTree", NormalBinaryTree);
            tester("StandardBinaryTree", StandardBinaryTree);
        }
    }
}