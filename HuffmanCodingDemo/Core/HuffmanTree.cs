using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core
{

    class HuffmanTree : BinaryTree<int>
    {
        public static HuffmanTree CreateFromDictionary(Dictionary<string, int> keyValuePairs)
        {

            SortedList<HuffmanTreeNode, HuffmanTreeNode> sortedList = new SortedList<HuffmanTreeNode, HuffmanTreeNode>(keyValuePairs.Select(new Func<KeyValuePair<string, int>, HuffmanTreeNode>((kps) => new HuffmanTreeNode() { Name = kps.Key, Data = kps.Value })).ToDictionary(new Func<HuffmanTreeNode, HuffmanTreeNode>((node) => node)));

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
                return new HuffmanTree();
            }
            return new HuffmanTree(sortedList.First().Value);
        }

        public HuffmanTree() : base() { }
        public HuffmanTree(HuffmanTreeNode node) : base(node) { }
    }
}
