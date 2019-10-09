using System.Collections;
using System.Collections.Generic;
using HuffmanCodingCore.Structs.BinaryTrees.Nodes;

namespace HuffmanCodingCore.Iterators.BinaryTreeIterators
{
    /// <summary>
    ///     后序迭代器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class PostIterator<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public PostIterator(BinaryTreeNode<T> binaryTreeNode = null)
        {
            RootTreeNode = binaryTreeNode;
        }

        public BinaryTreeNode<T> RootTreeNode { get; }

        public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
        {
            if (RootTreeNode != null)
            {
                if (RootTreeNode.LeftNode != null)
                    foreach (var node in RootTreeNode.LeftNode.PostIterator)
                        yield return node;
                if (RootTreeNode.RightNode != null)
                    foreach (var node in RootTreeNode.RightNode.PostIterator)
                        yield return node;
                yield return RootTreeNode;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}