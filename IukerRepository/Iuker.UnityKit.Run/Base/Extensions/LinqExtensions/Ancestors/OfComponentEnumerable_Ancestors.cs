/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 19:57:54
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

namespace Iuker.UnityKit.Run.LinqExtensions.Ancestors
{
    public struct OfComponentEnumerable_Ancestors<T> : IEnumerable<T> where T : Component
    {
        AncestorsEnumerable parent;

        public OfComponentEnumerable_Ancestors(ref AncestorsEnumerable parent)
        {
            this.parent = parent;
        }

        public OfComponentEnumerator_Ancestors<T> GetEnumerator()
        {
            return new OfComponentEnumerator_Ancestors<T>(ref parent);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region LINQ

        public void ForEach(Action<T> action)
        {
            var e = this.GetEnumerator();
            while (e.MoveNext())
            {
                action(e.Current);
            }
        }

        public T First()
        {
            var e = this.GetEnumerator();
            if (e.MoveNext())
            {
                return e.Current;
            }
            else
            {
                throw new InvalidOperationException("sequence is empty.");
            }
        }

        public T FirstOrDefault()
        {
            var e = this.GetEnumerator();
            return (e.MoveNext())
                ? e.Current
                : null;
        }

        public T[] ToArray()
        {
            var array = new T[4];
            var len = ToArrayNonAlloc(ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc(ref T[] array)
        {
            var index = 0;
            var e = this.GetEnumerator();
            while (e.MoveNext())
            {
                if (array.Length == index)
                {
                    var newSize = (index == 0) ? 4 : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = e.Current;
            }

            return index;
        }

        #endregion

    }
}
