using System;
using System.Collections.Generic;
using HuffmanCodingCore.Iterators.BinaryTreeIterators;

namespace HuffmanCodingCore.Structs.BinaryTrees.Nodes
{
    public class BinaryTreeNode<T>
    {
        private BinaryTreeNode<T> _leftNode;
        private BinaryTreeNode<T> _rightNode;

        /// <summary>
        ///     获取或设置数据字段
        /// </summary>
        public T Data { set; get; }

        /// <summary>
        ///     获取或设置左结点
        /// </summary>
        public BinaryTreeNode<T> LeftNode
        {
            get => _leftNode;
            set
            {
                if (_leftNode != null)
                    // 解除原左结点对自己的引用
                    _leftNode.ParentNode = null;
                if (value != null)
                    // 建立新左结点对自己的引用
                    value.ParentNode = this;
                // 保存新左结点的引用
                _leftNode = value;
            }
        }

        /// <summary>
        ///     获取或设置右结点
        /// </summary>
        public BinaryTreeNode<T> RightNode
        {
            get => _rightNode;
            set
            {
                if (_rightNode != null)
                    // 解除原右结点对自己的引用
                    _rightNode.ParentNode = null;
                if (value != null)
                    // 建立新右结点对自己的引用
                    value.ParentNode = this;
                // 保存新右结点的引用
                _rightNode = value;
            }
        }

        /// <summary>
        ///     获取该结点的深度
        /// </summary>
        public int Depth
        {
            get
            {
                var leftNodeDepth = LeftNode?.Depth ?? 0;
                var rightNodeDepth = RightNode?.Depth ?? 0;

                return Math.Max(leftNodeDepth, rightNodeDepth) + 1; // 加 1 表示自身所占用一个深度
            }
        }

        /// <summary>
        ///     指示该结点是否为叶子结点
        /// </summary>
        public bool IsLeafNode => Depth == 1;

        /// <summary>
        ///     获取当前结点的叶子结点数量
        /// </summary>
        public int LeafNodeCount
        {
            get
            {
                var leftEndNodeCount = LeftNode?.LeafNodeCount ?? 0;
                var rightEndNodeCount = RightNode?.LeafNodeCount ?? 0;
                // 如果左右结点没有任何一个具有终端结点，则返回1（即当前结点为终端结点），否则返回左右结点终端结点数之和
                if (leftEndNodeCount == 0 && rightEndNodeCount == 0)
                    return 1;
                return leftEndNodeCount + rightEndNodeCount;
            }
        }

        /// <summary>
        ///     获取叶子结点列表
        /// </summary>
        public List<BinaryTreeNode<T>> LeafNodes
        {
            get
            {
                var nodes = new List<BinaryTreeNode<T>>();
                if (IsLeafNode)
                {
                    nodes.Add(this);
                    return nodes;
                }

                if (LeftNode != null) nodes.AddRange(LeftNode.LeafNodes);
                if (RightNode != null) nodes.AddRange(RightNode.LeafNodes);
                return nodes;
            }
        }

        /// <summary>
        ///     获取父二叉树结点
        /// </summary>
        public BinaryTreeNode<T> ParentNode { get; private set; }

        public IEnumerable<BinaryTreeNode<T>> GetIterator(IteratorMode mode)
        {
            switch (mode)
            {
                case IteratorMode.Pre:
                    return PreIterator;
                case IteratorMode.In:
                    return InIterator;
                case IteratorMode.Post:
                    return PostIterator;
                default:
                    throw new NotImplementedException();
            }
        }

        #region 构造函数

        public BinaryTreeNode()
        {
        }

        public BinaryTreeNode(BinaryTreeNode<T> leftNode, BinaryTreeNode<T> rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public BinaryTreeNode(T data, BinaryTreeNode<T> leftNode = null, BinaryTreeNode<T> rightNode = null)
        {
            Data = data;
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        #endregion

        #region 迭代器属性

        /// <summary>
        ///     获取前序遍历迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> PreIterator => new PreIterator<T>(this);

        /// <summary>
        ///     获取中序遍历迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> InIterator => new InIterator<T>(this);

        /// <summary>
        ///     获取后序遍历迭代器
        /// </summary>
        public IEnumerable<BinaryTreeNode<T>> PostIterator => new PostIterator<T>(this);

        #endregion
    }
}