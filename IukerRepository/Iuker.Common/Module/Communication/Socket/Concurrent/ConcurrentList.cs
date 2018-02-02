/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/26 10:53:58
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


using System.Collections;
using System.Collections.Generic;

namespace Iuker.Common.Module.Socket.Concurrent
{
    /// <summary>
    /// 线程安全列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentList<T> : IList<T>
    {
        private readonly List<T> _sourceList = new List<T>();
        public IEnumerator<T> GetEnumerator()
        {
            lock (this)
            {
                return _sourceList.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (this)
            {
                return _sourceList.GetEnumerator();
            }
        }

        public void Add(T item)
        {
            lock (this)
            {
                _sourceList.Add(item);
            }
        }

        public void Clear()
        {
            lock (this)
            {
                _sourceList.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (this)
            {
                return _sourceList.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                _sourceList.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(T item)
        {
            lock (this)
            {
                return _sourceList.Remove(item);
            }
        }

        public int Count
        {
            get
            {
                lock (this)
                {
                    return _sourceList.Count;
                }
            }
        }

        public bool IsReadOnly { get; private set; }
        public int IndexOf(T item)
        {
            lock (this)
            {
                return _sourceList.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (this)
            {
                _sourceList.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (this)
            {
                _sourceList.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                lock (this)
                {
                    return _sourceList[index];
                }
            }
            set
            {
                lock (this)
                {
                    _sourceList[index] = value;
                }
            }
        }

    }
}
