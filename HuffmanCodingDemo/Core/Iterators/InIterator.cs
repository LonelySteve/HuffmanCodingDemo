using System.Collections;
using System.Collections.Generic;
using HuffmanCodingDemo.Core.BinaryTrees;

namespace HuffmanCodingDemo.Core.Iterators
{
    /// <summary>
    /// 中序遍历器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class InIterator<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public BinaryTreeNode<T> RootTreeNode { get; private set; }
        public InIterator(BinaryTreeNode<T> binaryTreeNode)
        {
            RootTreeNode = binaryTreeNode;
        }

        public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
        {
            if (RootTreeNode != null)
            {
                if (RootTreeNode.LeftNode != null)
                {
                    foreach (var node in RootTreeNode.LeftNode.InIterator)
                    {
                        yield return node;
                    }
                }
                yield return RootTreeNode;
                if (RootTreeNode.RightNode != null)
                {
                    foreach (var node in RootTreeNode.RightNode.InIterator)
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
