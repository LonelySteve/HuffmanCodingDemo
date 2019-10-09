using System;
using System.Collections.Generic;
using System.Linq;
using HuffmanCodingCore.Iterators.BinaryTreeIterators;
using HuffmanCodingCore.Structs.BinaryTrees.Nodes;

namespace HuffmanCodingCore.Structs.BinaryTrees
{
    /// <summary>
    ///     二叉树
    /// </summary>
    public class BinaryTree<T>
    {
        public BinaryTree(BinaryTreeNode<T> rootNode = null)
        {
            RootNode = rootNode;
        }

        /// <summary>
        ///     获取或设置根结点
        /// </summary>
        public BinaryTreeNode<T> RootNode { set; get; }

        /// <summary>
        ///     获取当前树是否为空
        /// </summary>
        public bool IsEmpty => RootNode == null;

        /// <summary>
        ///     获取树的深度
        /// </summary>
        public int Depth => RootNode?.Depth ?? 0;

        /// <summary>
        ///     获取叶子结点列表
        /// </summary>
        public List<BinaryTreeNode<T>> LeafNodes =>
            RootNode == null ? new List<BinaryTreeNode<T>>() : RootNode.LeafNodes;

        /// <summary>
        ///     获取叶子结点数量
        /// </summary>
        public int LeafNodeCount => RootNode?.LeafNodeCount ?? 0;

        public BinaryTreeNode<T> FindOne(Predicate<BinaryTreeNode<T>> match,
            IteratorMode searchIteratorMode = IteratorMode.Pre)
        {
            // 如果此二叉树实例为空树，则返回空                       否则 获取第一个被匹配的结点
            return IsEmpty ? null : RootNode.GetIterator(searchIteratorMode).FirstOrDefault(node => match(node));
        }

        public List<BinaryTreeNode<T>> FindAll(Predicate<BinaryTreeNode<T>> match,
            IteratorMode searchIteratorMode = IteratorMode.Pre)
        {
            var retNodes = new List<BinaryTreeNode<T>>();
            // 如果此二叉树实例为空树，则返回空列表
            if (IsEmpty)
                return retNodes;

            retNodes.AddRange(RootNode.GetIterator(searchIteratorMode).Where(node => match(node)));

            return retNodes;
        }

        #region Node 的迭代器属性包装

        /// <summary>
        ///     前序迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> PreIterator =>
            RootNode == null ? new PreIterator<T>() : RootNode.PreIterator;

        /// <summary>
        ///     中序迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> InIterator =>
            RootNode == null ? new InIterator<T>() : RootNode.InIterator;

        /// <summary>
        ///     后序迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> PostIterator =>
            RootNode == null ? new PostIterator<T>() : RootNode.PostIterator;

        #endregion
    }
}