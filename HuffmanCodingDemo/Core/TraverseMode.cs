using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core
{
    /// <summary>
    /// 遍历模式
    /// </summary>
    public enum TraverseMode
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
        Post,
        /// <summary>
        /// 层序遍历
        /// </summary>
        Level,
        /// <summary>
        /// 未知遍历模式
        /// </summary>
        Unknown
    }
}
