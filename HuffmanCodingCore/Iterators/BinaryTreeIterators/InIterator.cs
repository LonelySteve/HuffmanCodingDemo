using System.Collections;
using System.Collections.Generic;
using HuffmanCodingCore.Structs.BinaryTrees.Nodes;

namespace HuffmanCodingCore.Iterators.BinaryTreeIterators
{
    /// <summary>
    ///     中序遍历器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class InIterator<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public InIterator(BinaryTreeNode<T> binaryTreeNode = null)
        {
            RootTreeNode = binaryTreeNode;
        }

        public BinaryTreeNode<T> RootTreeNode { get; }

        public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
        {
            if (RootTreeNode != null)
            {
                if (RootTreeNode.LeftNode != null)
                    foreach (var node in RootTreeNode.LeftNode.InIterator)
                        yield return node;
                yield return RootTreeNode;
                if (RootTreeNode.RightNode != null)
                    foreach (var node in RootTreeNode.RightNode.InIterator)
                        yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}