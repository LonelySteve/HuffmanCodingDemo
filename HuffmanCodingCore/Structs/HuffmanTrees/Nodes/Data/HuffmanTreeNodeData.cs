namespace HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data
{
    public struct HuffmanTreeNodeData : IHuffmanTreeNodeData
    {
        public ulong Weight { get; }

        public bool? Code { get; set; }

        public HuffmanTreeNodeData(ulong weight, bool? code = null)
        {
            Weight = weight;
            Code = code;
        }
    }
}