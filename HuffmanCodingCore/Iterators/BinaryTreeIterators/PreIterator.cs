using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HuffmanCodingCore.Structs.BinaryTrees.Nodes;

namespace HuffmanCodingCore.Iterators.BinaryTreeIterators
{
    /// <summary>
    /// 前序遍历器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PreIterator<T> : IEnumerable<BinaryTreeNode<T>>
    {
        public BinaryTreeNode<T> RootTreeNode { get; private set; }
        public PreIterator(BinaryTreeNode<T> binaryTreeNode = null)
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
