using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCodingDemo.Core
{
    /// <summary>
    /// 二叉树
    /// </summary>
    class BinaryTree<T>
    {
        /// <summary>
        /// 获取或设置根结点
        /// </summary>
        public BinaryTreeNode<T> RootNode { set; get; }
        /// <summary>
        /// 获取当前树是否为空
        /// </summary>
        public bool IsEmpty { get => RootNode == null; }

        public BinaryTree(BinaryTreeNode<T> rootNode = null)
        {
            RootNode = rootNode;
        }

        public BinaryTreeNode<T> FindOne(Predicate<BinaryTreeNode<T>> match, TraverseMode traverseSearchMode = TraverseMode.Pre)
        {
            // 如果此二叉树实例为空树，则返回空
            if (IsEmpty)
                return null;

            BinaryTreeNode<T> retNodeVal = null;
            void searchHandler(object o, TraverseEventArgs e)
            {
                if (match((BinaryTreeNode<T>)o))
                {
                    retNodeVal = (BinaryTreeNode<T>)o;
                    e.Cancel = true;
                }
            }
            RootNode.TraverseEvent += searchHandler;
            // 开始遍历进行搜索
            RootNode.StartTraverse(traverseSearchMode);
            // 搜索完毕后取消事件
            RootNode.TraverseEvent -= searchHandler;
            return retNodeVal;
        }

        public List<BinaryTreeNode<T>> FindAll(Predicate<BinaryTreeNode<T>> match, TraverseMode traverseSearchMode = TraverseMode.Pre)
        {
            // 如果此二叉树实例为空树，则返回空
            if (IsEmpty)
                return null;

            List<BinaryTreeNode<T>> retNodes = new List<BinaryTreeNode<T>>();
            void searchHandler(object o, TraverseEventArgs e)
            {
                if (match((BinaryTreeNode<T>)o))
                {
                    retNodes.Add((BinaryTreeNode<T>)o);
                }
            }
            RootNode.TraverseEvent += searchHandler;
            // 开始遍历进行搜索
            RootNode.StartTraverse(traverseSearchMode);
            // 搜索完毕后取消事件
            RootNode.TraverseEvent -= searchHandler;
            return retNodes;
        }

        public void StartTraverse(TraverseMode mode)
        {
            // 如果此二叉树实例为空树，则不进行遍历
            if (IsEmpty)
                return;

            RootNode.StartTraverse(mode);
        }
    }
}
