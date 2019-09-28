using System.Collections;
using System.Collections.Generic;
using HuffmanCodingDemo.Core.BinaryTrees;

namespace HuffmanCodingDemo.Core.Iterators
{
    /// <summary>
    /// 前序遍历器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PreIterator<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public BinaryTreeNode<T> RootTreeNode { get; private set; }
        public PreIterator(BinaryTreeNode<T> binaryTreeNode)
        {
            RootTreeNode = binaryTreeNode;
        }

        public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
        {
            if (RootTreeNode != null)
            {
                yield return RootTreeNode;
                if (RootTreeNode.LeftNode != null)
                {
                    foreach (var node in RootTreeNode.LeftNode.PreIterator)
                    {
                        yield return node;
                    }
                }
                if (RootTreeNode.RightNode != null)
                {
                    foreach (var node in RootTreeNode.RightNode.PreIterator)
                    {
                        yield return node;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
