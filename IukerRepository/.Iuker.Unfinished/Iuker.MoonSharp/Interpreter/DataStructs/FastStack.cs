﻿/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:47:42
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

#if !USE_DYNAMIC_STACKS
using System;
using System.Collections.Generic;

namespace Iuker.MoonSharp.Interpreter.DataStructs
{
    /// <summary>
    /// 快速栈。
    /// 预先分配不可重新调整大小的栈。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class FastStack<T> : IList<T>
    {
        T[] m_Storage;
        int m_HeadIdx = 0;

        public FastStack(int maxCapacity)
        {
            m_Storage = new T[maxCapacity];
        }

        public T this[int index]
        {
            get { return m_Storage[index]; }
            set { m_Storage[index] = value; }
        }

        public T Push(T item)
        {
            m_Storage[m_HeadIdx++] = item;
            return item;
        }

        public void Expand(int size)
        {
            m_HeadIdx += size;
        }

        /// <summary>
        /// 清理栈中指定范围的元素
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void Zero(int from, int to)
        {
            Array.Clear(m_Storage, from, to - from + 1);
        }

        /// <summary>
        /// 清理指定索引位置的元素并将该元素重置为默认值
        /// </summary>
        /// <param name="index"></param>
        private void Zero(int index)
        {
            m_Storage[index] = default(T);
        }

        public T Peek(int idxofs = 0)
        {
            T item = m_Storage[m_HeadIdx - 1 - idxofs];
            return item;
        }

        public void Set(int idxofs, T item)
        {
            m_Storage[m_HeadIdx - 1 - idxofs] = item;
        }

        public void CropAtCount(int p)
        {
            RemoveLast(Count - p);
        }

        public void RemoveLast(int cnt = 1)
        {
            if (cnt == 1)
            {
                --m_HeadIdx;
                m_Storage[m_HeadIdx] = default(T);
            }
            else
            {
                int oldhead = m_HeadIdx;
                m_HeadIdx -= cnt;
                Zero(m_HeadIdx, oldhead);
            }
        }

        public T Pop()
        {
            --m_HeadIdx;
            T retval = m_Storage[m_HeadIdx];
            m_Storage[m_HeadIdx] = default(T);
            return retval;
        }

        public void Clear()
        {
            Array.Clear(m_Storage, 0, m_Storage.Length);
            m_HeadIdx = 0;
        }

        public int Count
        {
            get { return m_HeadIdx; }
        }

        #region IList<T> Impl.

        int IList<T>.IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }

        void ICollection<T>.Add(T item)
        {
            Push(item);
        }

        void ICollection<T>.Clear()
        {
            Clear();
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<T>.Count
        {
            get { return this.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion











    }
}
#endif