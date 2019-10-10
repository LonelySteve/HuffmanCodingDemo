using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HuffmanCodingCore.Structs.BinaryTrees;
using HuffmanCodingCore.Structs.HuffmanTrees.Nodes;
using HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data;

namespace HuffmanCodingCore.Structs.HuffmanTrees
{
    /// <summary>
    /// 哈夫曼树
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HuffmanTree<T> : BinaryTree<IHuffmanTreeNodeData>
    {
        protected IEqualityComparer<T> KeyComparer { get; }
        protected HuffmanTree(IEqualityComparer<T> keyComparer=null) => this.KeyComparer = keyComparer;

        protected HuffmanTree(HuffmanTreeNode rootNode, IEqualityComparer<T> keyComparer=null) : base(rootNode) => this.KeyComparer = keyComparer;

        /// <summary>
        ///     获取编码本
        /// </summary>
        public Dictionary<T, BitArray> CodeBook
        {
            get
            {
                // 新建一个空的编码字典
                var codeBook = new Dictionary<T, BitArray>(0, KeyComparer);
                // 遍历每一个叶子结点，自下而上地构建编码
                foreach (var node in LeafNodes)
                {
                    var bits = new List<bool>();

                    var parentNode = (HuffmanTreeNode) node;
                    do
                    {
                        Debug.Assert(parentNode.Data.Code != null, "parentNode.Data.Code != null");
                        bits.Add(parentNode.Data.Code.Value);
                        parentNode = (HuffmanTreeNode) parentNode.ParentNode;
                    } while (parentNode.Data?.Code != null);
                    // 反转编码
                    bits.Reverse();
                    codeBook.Add(((HuffmanTreeLeafNodeData<T>) node.Data).Content, new BitArray(bits.ToArray()));
                }

                return codeBook;
            }
        }

        /// <summary>
        /// 以指定权重字典构造哈夫曼树
        /// </summary>
        /// <param name="weightDict">指定权重字典</param>
        /// <param name="keyComparer">键比较器</param>
        /// <returns></returns>
        public static HuffmanTree<T> CreateFromWeightDictionary(Dictionary<T, ulong> weightDict = null, IEqualityComparer<T> keyComparer = null)
        {
            // 如果权重字典为空，则直接返回一个空树
            if (weightDict == null || weightDict.Count == 0)
                return new HuffmanTree<T>(keyComparer);
            // 使用权重字典的数据构造一系列哈夫曼叶子结点
            var huffmanNodes = weightDict.Select(kps =>
                new HuffmanTreeNode(new HuffmanTreeLeafNodeData<T>(kps.Key, kps.Value))).ToList();
            // 使用自定义可重复键的键比较器来建立空的 SortedList 对象
            var sortedList =
                new SortedList<ulong, HuffmanTreeNode>(new DuplicateKeyComparser<ulong>());
            // 遍历可迭代的哈夫曼叶子结点向 SortedList 对象中添加键值对
            foreach (var node in huffmanNodes) sortedList.Add(node.Data.Weight, node);
            // 循环直到 SortedList 对象中对象不足两个
            while (sortedList.Count >= 2)
            {
                // 依次从 SortedList 对象中弹出两个权重最小的结点
                var minNode1 = sortedList.ElementAt(0);
                sortedList.RemoveAt(0);
                var minNode2 = sortedList.ElementAt(0);
                sortedList.RemoveAt(0);
                // 将弹出的两个结点按照哈夫曼结点相加规则合并为一个哈夫曼结点
                var newNode = minNode1.Value + minNode2.Value;
                // 将合并好的新结点重新加回 SortedList 对象中
                sortedList.Add(newNode.Data.Weight, newNode);
            }
            var onlyOneNode = sortedList.First().Value;
            // 不幸的是，有可能只有一个叶子结点，这是相当特殊的情况，这个时候需要手动构造一个根结点
            // ReSharper disable once InvertIf
            if (huffmanNodes.Count() == 1)
            {
                onlyOneNode.Data.Code = false;
                onlyOneNode = new HuffmanTreeNode(onlyOneNode, null);
            }
            // 存在权重字典为空的情况，在这种情况下构造的 SortedListed 对象内键值对数量会为 0 。此时应返回空树
            return sortedList.Count == 0 ? new HuffmanTree<T>(keyComparer) : new HuffmanTree<T>(onlyOneNode, keyComparer);
        }
    }

    /// <summary>
    ///     <para>重复键比较器，可以用于处理相等的值</para>
    ///     <para>在那些不会自动处理重复键的类可以使用这个比较器，例如：SortedList 或者 SortedDictionaries</para>
    ///     <para>这个比较器在遇到重复键时，会将 <see cref="Compare(T, T)" /> 中的参数 x 判定为大于 y </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DuplicateKeyComparser<T> : IComparer<T> where T : IComparable
    {
        public int Compare(T x, T y)
        {
            var result = x.CompareTo(y);
            return result == 0 ? 1 : result;
        }
    }
}