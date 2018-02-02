using System;
using System.Collections;
using System.Collections.Generic;

namespace Iuker.Common.DataTypes.Collections
{
    /// <summary>
    /// 可观察列表
    /// </summary>
    public class ObservableList<T> : IList<T>
    {
        private IList<T> mNewList;
        private IList<T> mOldList;
        private readonly List<Action<IList<T>, T>> OnAdd = new List<Action<IList<T>, T>>();
        private readonly List<Action<IList<T>, T>> OnRemove = new List<Action<IList<T>, T>>();
        private readonly List<Action<IList<T>, T>> OnInsert = new List<Action<IList<T>, T>>();
        private readonly List<Action> OnClear = new List<Action>();
        private readonly List<Action<IList<T>, IList<T>>> OnInit = new List<Action<IList<T>, IList<T>>>();


        #region 事件订阅

        public void WatchOnAdd(Action<IList<T>, T> del)
        {
            if (del != null) OnAdd.Add(del);
        }

        public void WatchOnRemove(Action<IList<T>, T> del)
        {
            if (del != null) OnRemove.Add(del);
        }

        public void WatchOnInsert(Action<IList<T>, T> del)
        {
            if (del != null) OnInsert.Add(del);
        }

        public void WatchOnClear(Action del)
        {
            if (del != null) OnClear.Add(del);
        }

        public void WatchOnInit(Action<IList<T>, IList<T>> del)
        {
            if (del != null) OnInit.Add(del);
        }

        public void RemoveOnAdd(Action<IList<T>, T> del)
        {
            if (del != null) OnAdd.Remove(del);
        }

        public void RemoveOnRemove(Action<IList<T>, T> del)
        {
            if (del != null) OnRemove.Remove(del);
        }

        public void RemoveOnInsert(Action<IList<T>, T> del)
        {
            if (del != null) OnInsert.Remove(del);
        }

        public void RemoveOnClear(Action del)
        {
            if (del != null) OnClear.Remove(del);
        }

        public void RemoveOnUpdate(Action<IList<T>, IList<T>> del)
        {
            if (del != null) OnInit.Remove(del);
        }


        #endregion

        /// <summary>
        /// 初始化一个可观察列表
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public ObservableList<T> Init(IList<T> newValue)
        {
            mOldList = mNewList;
            mNewList = newValue;
            if (OnInit.Count > 0)
            {
                OnInit.ForEach(del => del(mNewList, mOldList));
            }

            return this;
        }


        public IEnumerator<T> GetEnumerator()
        {
            return mNewList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            mNewList.Add(item);
            if (OnAdd.Count > 0)
            {
                OnAdd.ForEach(del => del(mNewList, item));
            }
        }

        public void Clear()
        {
            mNewList.Clear();
            if (OnClear.Count > 0)
            {
                OnClear.ForEach(del => del());
            }
        }

        public bool Contains(T item)
        {
            return mNewList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            mNewList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (mNewList.Remove(item))
            {
                if (OnRemove.Count > 0)
                {
                    OnRemove.ForEach(del => del(mNewList, item));
                }
                return true;
            }
            return false;
        }

        public int Count
        {
            get
            {
                return mNewList.Count;
            }
        }

        public bool IsReadOnly { get; set; }

        public int IndexOf(T item)
        {
            return mNewList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            mNewList.Insert(index, item);
            if (OnInsert.Count > 0)
            {
                OnInsert.ForEach(del => del(mNewList, item));
            }
        }

        public void RemoveAt(int index)
        {
            if (index < mNewList.Count)
            {
                var item = mNewList[index];
                mNewList.RemoveAt(index);
                if (OnRemove.Count > 0)
                {
                    OnRemove.ForEach(del => del(mNewList, item));
                }
            }
        }

        public T this[int index]
        {
            get { return mNewList[index]; }
            set { mNewList[index] = value; }
        }
    }
}