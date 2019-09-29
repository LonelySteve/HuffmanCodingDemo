using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data
{
    public struct HuffmanTreeLeafNodeData<T> : IHuffmanTreeNodeData
    {
        public T Content { get; private set; }

        public ulong Weight { get; private set; }

        public bool? Code { get; set; }

        public HuffmanTreeLeafNodeData(T content, ulong weight, bool? code = null)
        {
            Content = content;
            Weight = weight;
            Code = code;
        }
    }
}
