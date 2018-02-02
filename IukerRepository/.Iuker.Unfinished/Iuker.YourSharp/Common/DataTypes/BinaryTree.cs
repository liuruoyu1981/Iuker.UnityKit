/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 16:32
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;

namespace Iuker.YourSharp.Common.DataTypes
{
    /// <summary>
    /// 二叉树
    /// </summary>
    public class BinaryTree<T> where T : IComparable<T>
    {
        //private T mData;
        private BinaryTree<T> mLeft;
        private BinaryTree<T> mRight;

        public BinaryTree(T node)
        {
            NodeData = node;
        }

        public T NodeData { get; private set; }

        public BinaryTree<T> Left { get; set; }

        public BinaryTree<T> Right { get; set; }


        public void Insert(T newItem)
        {
            T current = NodeData;

            if (current.CompareTo(newItem) > 0)
            {
                if (Left == null)
                {
                    Left = new BinaryTree<T>(newItem);
                }
                else
                {
                    Left.Insert(newItem);
                }
            }
            else
            {
                if (Right == null)
                {
                    Right = new BinaryTree<T>(newItem);
                }
                else
                {
                    Right.Insert(newItem);
                }
            }
        }


        public static void PreOrderTree(BinaryTree<T> root)
        {
            while (true)
            {
                if (root != null)
                {
                    PreOrderTree(root.Left);
                    root = root.Right;
                    continue;
                }
                break;
            }
        }


        public void InOrderTree(BinaryTree<T> root)
        {
            if (root != null)
            {
                InOrderTree(root.Left);
                InOrderTree(root.Right);
            }
        }

        








    }
}
