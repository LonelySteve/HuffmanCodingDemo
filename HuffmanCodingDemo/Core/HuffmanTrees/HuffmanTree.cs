using System;
using System.Collections.Generic;
using System.Linq;
using HuffmanCodingDemo.Core.BinaryTrees;

namespace HuffmanCodingDemo.Core.HuffmanTrees
{

    public class HuffmanTree<T> : BinaryTree<HuffmanTreeNodeData<T>>
    {
        public static HuffmanTree<T> CreateFromDictionary(Dictionary<T, int> keyValuePairs)
        {
            // 用参数字典的数据构造一系列哈夫曼结点，并建立与自身的键值对字典，然后使用这个新的字典来构造 SortedList 对象
            var huffmanNodes = keyValuePairs.Select(new Func<KeyValuePair<T, int>, HuffmanTreeNode<T>>(kps => new HuffmanTreeNode<T>(new HuffmanTreeNodeData<T>(kps.Key, kps.Value))));
            var newKeyValuePairs = huffmanNodes.ToDictionary(new Func<HuffmanTreeNode<T>, HuffmanTreeNode<T>>(node => node));
            var sortedList = new SortedList<HuffmanTreeNode<T>, HuffmanTreeNode<T>>(newKeyValuePairs);

            while (sortedList.Count >= 2)
            {
                var minNode_1 = sortedList.ElementAt(0);
                sortedList.RemoveAt(0);
                var minNode_2 = sortedList.ElementAt(0);
                sortedList.RemoveAt(0);
                var newNode = minNode_1.Value + minNode_2.Value;
                sortedList.Add(newNode, newNode);
            }

            if (sortedList.Count == 0)
            {
                return new HuffmanTree<T>();
            }
            return new HuffmanTree<T>(sortedList.First().Value);
        }

        public HuffmanTree() : base() { }
        public HuffmanTree(HuffmanTreeNode<T> rootNode) : base(rootNode) { }
    }
}
