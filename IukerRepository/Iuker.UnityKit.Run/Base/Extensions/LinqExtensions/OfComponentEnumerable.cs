/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 16:52:58
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.LinqExtensions
{
    /// <summary>
    /// 用于unity3d组件的可枚举对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct OfComponentEnumerable<T> : IEnumerable<T> where T : Component
    {
        private ChildrenEnumerable parent;

        public OfComponentEnumerable(ref ChildrenEnumerable parent)
        {
            this.parent = parent;
        }

        public OfComponentEnumerator<T> GetEnumerator()
        {
            return new OfComponentEnumerator<T>(ref this.parent);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() { return GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        #region LINQ

        /// <summary>
        /// 对每个组件调用传入委托
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action)
        {
            var e = this.GetEnumerator();
            while (e.MoveNext())
            {
                action(e.Current);
            }
        }

        /// <summary>
        /// 获取枚举器内部组件集合中的第一个组件
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            var e = GetEnumerator();
            if (e.MoveNext())
            {
                return e.Current;
            }
            else
            {
                throw new InvalidOperationException("sequence is empty.");
            }
        }

        /// <summary>
        /// 获取枚举器内部组件集合中的第一个组件
        /// 若枚举器内部组件集合为空则返回空
        /// </summary>
        /// <returns></returns>
        public T FirstOrDefault()
        {
            var e = GetEnumerator();
            return (e.MoveNext())
                ? e.Current
                : null;
        }

        public int ToArrayNonAlloc(ref T[] array)
        {
            var index = 0;
            var e = this.GetEnumerator();
            while (e.MoveNext())
            {
                if (array.Length == index)
                {
                    var newSize = (index == 0) ? parent.GetChildrenSize() : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = e.Current;
            }

            return index;
        }

        public T[] ToArray()
        {
            var array = new T[parent.GetChildrenSize()];
            var len = ToArrayNonAlloc(ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array; ;
        }



        #endregion

    }
}
