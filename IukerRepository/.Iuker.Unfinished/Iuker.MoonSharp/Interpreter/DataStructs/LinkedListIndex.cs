/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 11:37:42
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


using System.Collections.Generic;

namespace Iuker.MoonSharp.Interpreter.DataStructs
{
    /// <summary>
    /// 用于加速对LinkedList操作的索引对象 name="TValue"/> using a single key of type <typeparamref name="TKey"/>
    /// 多个索引对象可以链接到同一个LinkedList,但每一个LinkedList中的节点有且只能有一个索引对象
    /// </summary>
    /// <typeparam name="TKey">键的类型。 必须适当地实现Equals和GetHashCode。</typeparam>
    /// <typeparam name="TValue">链接列表中包含的值的类型</typeparam>
    internal class LinkedListIndex<TKey, TValue>
    {
        private LinkedList<TValue> m_LinkedList;
        private Dictionary<TKey, LinkedListNode<TValue>> m_Map = null;

        public LinkedListIndex(LinkedList<TValue> linkedList)
        {
            m_LinkedList = linkedList;
        }

        /// <summary>
        /// 找到指定的节点索引的键,或null。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public LinkedListNode<TValue> Find(TKey key)
        {
            LinkedListNode<TValue> node;

            if (m_Map == null)
                return null;

            if (m_Map.TryGetValue(key, out node))
                return node;

            return null;
        }

        /// <summary>
        /// 使用制定的索引key更新或创建一个新链表节点。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TValue Set(TKey key, TValue value)
        {
            LinkedListNode<TValue> node = Find(key);

            if (node == null)
            {
                Add(key, value);
                return default(TValue);
            }
            TValue val = node.Value;
            node.Value = value;
            return val;
        }

        /// <summary>
        /// 使用制定的索引key创建一个新链表节点。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            var node = m_LinkedList.AddLast(value);

            if (m_Map == null)
                m_Map = new Dictionary<TKey, LinkedListNode<TValue>>();

            m_Map.Add(key, node);
        }

        /// <summary>
        /// 从索引中删除指定的键，以及从链接列表中键索引的节点。
        /// </summary>
        /// <param name="key">The key.</param>
        public bool Remove(TKey key)
        {
            LinkedListNode<TValue> node = Find(key);

            if (node != null)
            {
                m_LinkedList.Remove(node);
                return m_Map.Remove(key);
            }

            return false;
        }


        /// <summary>
        /// 确定索引是否包含指定的key。
        /// </summary>
        /// <param name="key">The key.</param>
        public bool ContainsKey(TKey key)
        {
            if (m_Map == null)
                return false;

            return m_Map.ContainsKey(key);
        }

        /// <summary>
        /// 清除这个实例(删除所有元素)
        /// </summary>
        public void Clear()
        {
            if (m_Map != null)
                m_Map.Clear();
        }
    }
}
