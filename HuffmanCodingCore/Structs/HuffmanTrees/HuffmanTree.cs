using HuffmanCodingCore.Structs.BinaryTrees;
using HuffmanCodingCore.Structs.HuffmanTrees.Nodes;
using HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingCore.Structs.HuffmanTrees
{
    public class HuffmanTree<T> : BinaryTree<IHuffmanTreeNodeData>
    {
        public static HuffmanTree<T> CreateFromWeightDictionary(Dictionary<T, ulong> weightDict)
        {
            // 用参数字典的数据构造一系列哈夫曼叶子结点，并建立与自身的键值对字典，然后使用这个新的字典来构造 SortedList 对象
            var huffmanNodes = weightDict.Select(new Func<KeyValuePair<T, ulong>, HuffmanTreeNode>(kps => new HuffmanTreeNode(new HuffmanTreeLeafNodeData<T>(kps.Key, kps.Value))));
            var newKeyValuePairs = huffmanNodes.ToDictionary(new Func<HuffmanTreeNode, ulong>(node => node.Data.Weight));
            var sortedList = new SortedList<ulong, HuffmanTreeNode>(newKeyValuePairs, new DuplicateKeyComparser<ulong>());

            while (sortedList.Count >= 2)
            {
                var minNode_1 = sortedList.ElementAt(0);
                sortedList.RemoveAt(0);
                var minNode_2 = sortedList.ElementAt(0);
                sortedList.RemoveAt(0);
                var newNode = minNode_1.Value + minNode_2.Value;
                sortedList.Add(newNode.Data.Weight, newNode);
            }

            if (sortedList.Count == 0)
            {
                return new HuffmanTree<T>();
            }
            return new HuffmanTree<T>(sortedList.First().Value);
        }

        public HuffmanTree() : base() { }
        public HuffmanTree(HuffmanTreeNode rootNode) : base(rootNode) { }
        /// <summary>
        /// 获取编码本
        /// </summary>
        public Dictionary<T, BitArray> CodeBook
        {
            get
            {
                Dictionary<T, BitArray> codeBook = new Dictionary<T, BitArray>();
                var leafNodes = LeafNodes;
                foreach (var node in leafNodes)
                {
                    var bits = new List<bool>();

                    HuffmanTreeNode parentNode = (HuffmanTreeNode)node;
                    do
                    {
                        bits.Add(parentNode.Data.Code.Value);
                        parentNode = (HuffmanTreeNode)parentNode.ParentNode;
                    } while (parentNode.Data.Code.HasValue);
                    bits.Reverse();
                    codeBook.Add(((HuffmanTreeLeafNodeData<T>)node.Data).Content, new BitArray(bits.ToArray()));
                }
                return codeBook;
            }
        }
    }
    /// <summary>
    /// <para>重复键比较器，可以用于处理相等的值</para>
    /// <para>在那些不会自动处理重复键的类可以使用这个比较器，例如：SortedList 或者 SortedDictionaries</para>
    /// <para>这个比较器在遇到重复键时，会将 <see cref="Compare(T, T)"/> 中的参数 x 判定为大于 y </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DuplicateKeyComparser<T> : IComparer<T> where T : IComparable
    {
        public int Compare(T x, T y)
        {
            int result = x.CompareTo(y);
            return result == 0 ? 1 : result;
        }
    }
}
