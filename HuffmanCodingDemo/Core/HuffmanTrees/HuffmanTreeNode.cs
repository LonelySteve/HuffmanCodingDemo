using System;
using HuffmanCodingDemo.Core.BinaryTrees;

namespace HuffmanCodingDemo.Core.HuffmanTrees
{
    /// <summary>
    /// 哈夫曼树结点
    /// <para>
    /// 与普通的二叉树结点不同的是：
    /// </para>
    /// <para>
    /// 1. 重载了加法操作，允许将两个哈夫曼树结点进行相加获得一个新的哈夫曼树结点
    /// </para>
    /// <para>
    /// 2. 具有指示自身作为孩子的编码字段（通常是 0 和 1）
    /// </para>
    /// </summary>
    public class HuffmanTreeNode<T> : BinaryTreeNode<HuffmanTreeNodeData<T>>, IComparable<BinaryTreeNode<HuffmanTreeNodeData<T>>>
    {
        public int ChildCode { get; set; }
        public int CompareTo(BinaryTreeNode<HuffmanTreeNodeData<T>> other)
        {
            return Data.weight.CompareTo(other.Data.weight);
        }

        public HuffmanTreeNode() : base() { ChildCode = -1; }
        public HuffmanTreeNode(HuffmanTreeNode<T> leftNode, HuffmanTreeNode<T> rightNode) : base(leftNode, rightNode) { ChildCode = -1; }
        public HuffmanTreeNode(HuffmanTreeNodeData<T> data, HuffmanTreeNode<T> leftNode = null, HuffmanTreeNode<T> rightNode = null) : base(data, leftNode, rightNode) { ChildCode = -1; }
        public static HuffmanTreeNode<T> operator +(HuffmanTreeNode<T> leftNode, HuffmanTreeNode<T> rightNode)
        {
            int compareResult = leftNode.CompareTo(rightNode);
            // TODO 避免硬编码左右孩子设定逻辑
            if (compareResult <= 0) // leftNode <= rightNode
            {
                leftNode.ChildCode = 0;
                rightNode.ChildCode = 1;

                return new HuffmanTreeNode<T>(leftNode, rightNode);
            }
            // leftNode > rightNode
            leftNode.ChildCode = 1;
            rightNode.ChildCode = 0;

            return new HuffmanTreeNode<T>(rightNode, leftNode);
        }
    }
}
