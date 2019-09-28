using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core.HuffmanTrees
{
    public struct HuffmanTreeNodeData<T>
    {
        /// <summary>
        /// 数据内容
        /// </summary>
        public T content;
        /// <summary>
        /// 权重
        /// </summary>
        public int weight;

        public HuffmanTreeNodeData(T content, int weight)
        {
            this.content = content;
            this.weight = weight;
        }
    }
}
