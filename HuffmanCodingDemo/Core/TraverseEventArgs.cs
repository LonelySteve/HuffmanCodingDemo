using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core
{
    /// <summary>
    /// 遍历事件参数
    /// </summary>
    public class TraverseEventArgs : EventArgs
    {
        /// <summary>
        /// 当前遍历的索引号
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// 获取遍历模式
        /// </summary>
        public TraverseMode Mode { get; private set; }
        /// <summary>
        /// 指示是否取消遍历
        /// </summary>
        public bool Cancel { get; set; }

        public TraverseEventArgs(TraverseMode mode, int index = 0) { Mode = mode; Index = index; }
    }
}
