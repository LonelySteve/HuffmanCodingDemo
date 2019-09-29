using HuffmanCodingCore.Structs.BinaryTrees.Nodes;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingCore.Iterators.BinaryTreeIterators
{
    /// <summary>
    /// 后序迭代器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PostIterator<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public BinaryTreeNode<T> RootTreeNode { get; private set; }
        public PostIterator(BinaryTreeNode<T> binaryTreeNode = null)
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
