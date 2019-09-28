using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core.Iterators
{
    /// <summary>
    /// 迭代器模式
    /// </summary>
    public enum IteratorMode
    {
        /// <summary>
        /// 前序遍历
        /// </summary>
        Pre,
        /// <summary>
        /// 中序遍历
        /// </summary>
        In,
        /// <summary>
        /// 后序遍历
        /// </summary>
        Post
    }
}
