namespace HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data
{
    public struct HuffmanTreeLeafNodeData<T> : IHuffmanTreeNodeData
    {
        public T Content { get; }

        public ulong Weight { get; }

        public bool? Code { get; set; }

        public HuffmanTreeLeafNodeData(T content, ulong weight, bool? code = null)
        {
            Content = content;
            Weight = weight;
            Code = code;
        }
    }
}