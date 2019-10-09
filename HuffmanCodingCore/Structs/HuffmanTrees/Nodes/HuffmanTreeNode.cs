using System;
using HuffmanCodingCore.Structs.BinaryTrees.Nodes;
using HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data;

namespace HuffmanCodingCore.Structs.HuffmanTrees.Nodes
{
    /// <summary>
    ///     哈夫曼树结点
    ///     <para>
    ///         与普通的二叉树结点不同的是：
    ///     </para>
    ///     <para>
    ///         1. 重载了加法操作，允许将两个哈夫曼树结点进行相加获得一个新的哈夫曼树结点
    ///     </para>
    ///     <para>
    ///         2. 具有指示自身作为孩子的编码字段（通常是 true 或 false）
    ///     </para>
    /// </summary>
    public class HuffmanTreeNode : BinaryTreeNode<IHuffmanTreeNodeData>,
        IComparable<BinaryTreeNode<IHuffmanTreeNodeData>>
    {
        public HuffmanTreeNode()
        {
        }

        public HuffmanTreeNode(HuffmanTreeNode leftNode, HuffmanTreeNode rightNode) : base(leftNode, rightNode)
        {
        }

        public HuffmanTreeNode(IHuffmanTreeNodeData data, HuffmanTreeNode leftNode = null,
            HuffmanTreeNode rightNode = null) : base(data, leftNode, rightNode)
        {
        }

        public int CompareTo(BinaryTreeNode<IHuffmanTreeNodeData> other)
        {
            return Data.Weight.CompareTo(other.Data.Weight);
        }

        public static HuffmanTreeNode operator +(HuffmanTreeNode leftNode, HuffmanTreeNode rightNode)
        {
            var compareResult = leftNode.CompareTo(rightNode);
            var newNodeData = new HuffmanTreeNodeData(leftNode.Data.Weight + rightNode.Data.Weight);

            if (compareResult <= 0) // leftNode <= rightNode
            {
                leftNode.Data.Code = false;
                rightNode.Data.Code = true;

                return new HuffmanTreeNode(newNodeData, leftNode, rightNode);
            }

            // leftNode > rightNode -> 交换顺序
            leftNode.Data.Code = true;
            rightNode.Data.Code = false;

            return new HuffmanTreeNode(newNodeData, rightNode, leftNode);
        }

        /// <summary>
        ///     获取指定代码的结点，如果取得的结点为空则返回空
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public HuffmanTreeNode GetChild(bool code)
        {
            var node = code == LeftNode.Data.Code ? LeftNode : RightNode;
            return (HuffmanTreeNode) node;
        }
    }
}