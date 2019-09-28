using System;
using System.Collections.Generic;
using HuffmanCodingDemo.Core.Iterators;

namespace HuffmanCodingDemo.Core.BinaryTrees
{
    /// <summary>
    /// 二叉树
    /// </summary>
    public class BinaryTree<T>
    {
        /// <summary>
        /// 获取或设置根结点
        /// </summary>
        public BinaryTreeNode<T> RootNode { set; get; }
        /// <summary>
        /// 获取当前树是否为空
        /// </summary>
        public bool IsEmpty { get => RootNode == null; }

        #region Node 的迭代器属性包装
        /// <summary>
        /// 前序迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> PreIterator => RootNode.PreIterator;
        /// <summary>
        /// 中序迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> InIterator => RootNode.InIterator;
        /// <summary>
        /// 后序迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> PostIterator => RootNode.PostIterator;
        #endregion

        public BinaryTree(BinaryTreeNode<T> rootNode = null)
        {
            RootNode = rootNode;
        }

        public BinaryTreeNode<T> FindOne(Predicate<BinaryTreeNode<T>> match, IteratorMode searchIteratorMode = IteratorMode.Pre)
        {
            // 如果此二叉树实例为空树，则返回空
            if (IsEmpty)
                return null;

            foreach (var node in RootNode.GetIterator(searchIteratorMode))
            {
                if (match(node))
                {
                    return node;
                }
            }
            // 未找到匹配结点则返回空
            return null;
        }

        public List<BinaryTreeNode<T>> FindAll(Predicate<BinaryTreeNode<T>> match, IteratorMode searchIteratorMode = IteratorMode.Pre)
        {
            List<BinaryTreeNode<T>> retNodes = new List<BinaryTreeNode<T>>();
            // 如果此二叉树实例为空树，则返回空列表
            if (IsEmpty)
                return retNodes;

            foreach (var node in RootNode.GetIterator(searchIteratorMode))
            {
                if (match(node))
                {
                    retNodes.Add(node);
                }
            }

            return retNodes;
        }
    }
}
