using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingCore.Structs.HuffmanTrees.Nodes.Data
{
    public struct HuffmanTreeNodeData : IHuffmanTreeNodeData
    {
        public ulong Weight { get; private set; }

        public bool? Code { get; set; }

        public HuffmanTreeNodeData(ulong weight, bool? code = null)
        {
            Weight = weight;
            Code = code;
        }
    }
}
