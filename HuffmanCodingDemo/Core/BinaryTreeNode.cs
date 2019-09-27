using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core
{
    class BinaryTreeNode<T>
    {
        public event EventHandler<TraverseEventArgs> TraverseEvent;
        private BinaryTreeNode<T> leftNode;
        private BinaryTreeNode<T> rightNode;
        /// <summary>
        /// 获取或设置二叉树结点的名字（可以不唯一）
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置数据字段
        /// </summary>
        public T Data { set; get; }
        /// <summary>
        /// 获取或设置左结点
        /// </summary>
        public BinaryTreeNode<T> LeftNode
        {
            get => leftNode; set
            {
                // 解除原左结点对自己的引用
                leftNode.ParentNode = null;
                // 建立新左结点对自己的引用
                value.ParentNode = this;
                // 保存新左结点的引用
                leftNode = value;
            }
        }
        /// <summary>
        /// 获取或设置右结点
        /// </summary>
        public BinaryTreeNode<T> RightNode
        {
            get => rightNode; set
            {
                // 解除原右结点对自己的引用
                rightNode.ParentNode = null;
                // 建立新右结点对自己的引用
                value.ParentNode = this;
                // 保存新右结点的引用
                rightNode = value;
            }
        }
        /// <summary>
        /// 获取该结点的深度
        /// </summary>
        public int Depth
        {
            get
            {
                int leftNodeDepth = LeftNode == null ? 0 : LeftNode.Depth;
                int rightNodeDepth = RightNode == null ? 0 : RightNode.Depth;

                return Math.Max(leftNodeDepth, rightNodeDepth) + 1; // 加 1 表示自身所占用一个深度
            }
        }
        /// <summary>
        /// 获取当前结点的终端结点数量
        /// </summary>
        public int EndNodeCount
        {
            get
            {
                int leftEndNodeCount = LeftNode == null ? 0 : LeftNode.EndNodeCount;
                int rightEndNodeCount = RightNode == null ? 0 : RightNode.EndNodeCount;
                // 如果左右结点没有任何一个具有终端结点，则返回1（即当前结点为终端结点），否则返回左右结点终端结点数之和
                if (leftEndNodeCount == 0 && rightEndNodeCount == 0)
                    return 1;
                else
                    return leftEndNodeCount + rightEndNodeCount;
            }
        }

        /// <summary>
        /// 获取父二叉树结点
        /// </summary>
        public BinaryTreeNode<T> ParentNode { get; private set; }

        public BinaryTreeNode() { }

        public BinaryTreeNode(BinaryTreeNode<T> leftNode, BinaryTreeNode<T> rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public BinaryTreeNode(string name, BinaryTreeNode<T> leftNode, BinaryTreeNode<T> rightNode)
        {
            Name = name;
            LeftNode = leftNode;
            RightNode = rightNode;
        }

        public void StartTraverse(TraverseMode mode)
        {
            TraverseEventArgs traverseEventArgs = new TraverseEventArgs(TraverseMode.Unknown);
            switch (mode)
            {
                case TraverseMode.Pre:
                    PreTraverseNode(ref traverseEventArgs);
                    break;
                case TraverseMode.In:
                    InTraverseNode(ref traverseEventArgs);
                    break;
                case TraverseMode.Post:
                    PostTraverseNode(ref traverseEventArgs);
                    break;
                case TraverseMode.Level:
                    LevelTraverseNode(ref traverseEventArgs);
                    break;
                default:
                    break;
            }
        }

        protected void PreTraverseNode(ref TraverseEventArgs traverseEventArgs)
        {
            if (traverseEventArgs.Cancel)
                return;

            TraverseEventArgs newTraverseEventArgs = new TraverseEventArgs(TraverseMode.Pre, traverseEventArgs.Index + 1);
            // 触发遍历事件
            TraverseEvent.Invoke(this, newTraverseEventArgs);
            if (LeftNode != null)
                LeftNode.PreTraverseNode(ref newTraverseEventArgs);
            if (RightNode != null)
                RightNode.PreTraverseNode(ref newTraverseEventArgs);
        }
        protected void InTraverseNode(ref TraverseEventArgs traverseEventArgs)
        {
            if (traverseEventArgs.Cancel)
                return;

            TraverseEventArgs newTraverseEventArgs = new TraverseEventArgs(TraverseMode.In, traverseEventArgs.Index + 1);
            if (LeftNode != null)
                LeftNode.PreTraverseNode(ref newTraverseEventArgs);
            // 触发遍历事件
            TraverseEvent.Invoke(this, newTraverseEventArgs);
            if (RightNode != null)
                RightNode.PreTraverseNode(ref newTraverseEventArgs);
        }

        protected void PostTraverseNode(ref TraverseEventArgs traverseEventArgs)
        {
            if (traverseEventArgs.Cancel)
                return;

            TraverseEventArgs newTraverseEventArgs = new TraverseEventArgs(TraverseMode.Post, traverseEventArgs.Index + 1);
            if (LeftNode != null)
                LeftNode.PreTraverseNode(ref newTraverseEventArgs);
            if (RightNode != null)
                RightNode.PreTraverseNode(ref newTraverseEventArgs);
            // 触发遍历事件
            TraverseEvent.Invoke(this, newTraverseEventArgs);
        }

        protected void LevelTraverseNode(ref TraverseEventArgs traverseEventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
