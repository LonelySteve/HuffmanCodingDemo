using System.Collections;
using System.Collections.Generic;
using HuffmanCodingDemo.Core.BinaryTrees;

namespace HuffmanCodingDemo.Core.Iterators
{
    /// <summary>
    /// 后序迭代器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PostIterator<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public BinaryTreeNode<T> RootTreeNode { get; private set; }
        public PostIterator(BinaryTreeNode<T> binaryTreeNode)
        {
            RootTreeNode = binaryTreeNode;
        }

        public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
        {
            if (RootTreeNode != null)
            {
                if (RootTreeNode.LeftNode != null)
                {
                    foreach (var node in RootTreeNode.LeftNode.PostIterator)
                    {
                        yield return node;
                    }
                }
                if (RootTreeNode.RightNode != null)
                {
                    foreach (var node in RootTreeNode.RightNode.PostIterator)
                    {
                        yield return node;
                    }
                }
                yield return RootTreeNode;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
