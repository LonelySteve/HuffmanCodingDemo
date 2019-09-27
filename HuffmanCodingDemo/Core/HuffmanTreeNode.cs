using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core
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
    class HuffmanTreeNode : BinaryTreeNode<int>, IComparable<BinaryTreeNode<int>>
    {
        public int ChildCode { get; set; }

        public int CompareTo(BinaryTreeNode<int> other)
        {
            return Data.CompareTo(other);
        }

        public HuffmanTreeNode() : base() { ChildCode = -1; }
        public HuffmanTreeNode(HuffmanTreeNode leftNode, HuffmanTreeNode rightNode) : base(leftNode, rightNode) { ChildCode = -1; }
        public HuffmanTreeNode(string name, HuffmanTreeNode leftNode, HuffmanTreeNode rightNode) : base(name, leftNode, rightNode) { ChildCode = -1; }
        public static HuffmanTreeNode operator +(HuffmanTreeNode leftNode, HuffmanTreeNode rightNode)
        {
            int compareResult = leftNode.CompareTo(rightNode);
            // TODO 避免硬编码左右孩子设定逻辑
            if (compareResult <= 0) // leftNode <= rightNode
            {
                leftNode.ChildCode = 0;
                rightNode.ChildCode = 1;

                return new HuffmanTreeNode(leftNode, rightNode);
            }
            // leftNode > rightNode
            leftNode.ChildCode = 1;
            rightNode.ChildCode = 0;

            return new HuffmanTreeNode(rightNode, leftNode);
        }
    }
}
