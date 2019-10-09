namespace HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data
{
    /// <summary>
    ///     提供了哈夫曼树结点数据一组属性
    /// </summary>
    public interface IHuffmanTreeNodeData
    {
        /// <summary>
        ///     获取此节点权重
        /// </summary>
        ulong Weight { get; }

        /// <summary>
        ///     获取或设置此节点的代码
        /// </summary>
        bool? Code { get; set; }
    }
}