/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 20:10:01
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

namespace Iuker.UnityKit.Run.LinqExtensions.Descendants
{
    public struct OfComponentEnumerable_Descendants<T> : IEnumerable<T> where T : Component
    {
        DescendantsEnumerable parent;


        public OfComponentEnumerable_Descendants(ref DescendantsEnumerable parent)
        {
            this.parent = parent;
        }

        public OfComponentEnumerator_Descendants<T> GetEnumerator()
        {
            return new OfComponentEnumerator_Descendants<T>(ref parent);
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

        public T First()
        {
            var e = this.GetEnumerator();
            try
            {
                if (e.MoveNext())
                {
                    return e.Current;
                }
                else
                {
                    throw new InvalidOperationException("sequence is empty.");
                }
            }
            finally
            {
                e.Dispose();
            }
        }

        public T FirstOrDefault()
        {
            var e = this.GetEnumerator();
            try
            {
                return (e.MoveNext())
                    ? e.Current
                    : null;
            }
            finally
            {
                e.Dispose();
            }
        }

        /// <summary>Use internal iterator for performance optimization.</summary>
        public void ForEach(Action<T> action)
        {
            if (parent.withSelf)
            {
                T component = default(T);
#if UNITY_EDITOR
                parent.origin.GetComponents<T>(componentCache);
                if (componentCache.Count != 0)
                {
                    component = componentCache[0];
                    componentCache.Clear();
                }
#else
                        component = parent.origin.GetComponent<T>();
#endif

                if (component != null)
                {
                    action(component);
                }
            }

            var originTransform = parent.origin.transform;
            OfComponentDescendantsCore(ref originTransform, ref action);
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

#if UNITY_EDITOR
        static List<T> componentCache = new List<T>(); // for no allocate on UNITY_EDITOR
#endif

        void OfComponentDescendantsCore(ref Transform transform, ref Action<T> action)
        {
            if (!parent.descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);

                T component = default(T);
#if UNITY_EDITOR
                child.GetComponents<T>(componentCache);
                if (componentCache.Count != 0)
                {
                    component = componentCache[0];
                    componentCache.Clear();
                }
#else
                        component = child.GetComponent<T>();
#endif

                if (component != null)
                {
                    action(component);
                }
                OfComponentDescendantsCore(ref child, ref action);
            }
        }

        void OfComponentDescendantsCore(ref Transform transform, ref int index, ref T[] array)
        {
            if (!parent.descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                T component = default(T);
#if UNITY_EDITOR
                child.GetComponents<T>(componentCache);
                if (componentCache.Count != 0)
                {
                    component = componentCache[0];
                    componentCache.Clear();
                }
#else
                        component = child.GetComponent<T>();
#endif

                if (component != null)
                {
                    if (array.Length == index)
                    {
                        var newSize = (index == 0) ? 4 : index * 2;
                        Array.Resize(ref array, newSize);
                    }

                    array[index++] = component;
                }
                OfComponentDescendantsCore(ref child, ref index, ref array);
            }
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc(ref T[] array)
        {
            var index = 0;
            if (parent.withSelf)
            {
                T component = default(T);
#if UNITY_EDITOR
                parent.origin.GetComponents<T>(componentCache);
                if (componentCache.Count != 0)
                {
                    component = componentCache[0];
                    componentCache.Clear();
                }
#else
                        component = parent.origin.GetComponent<T>();
#endif

                if (component != null)
                {
                    if (array.Length == index)
                    {
                        var newSize = (index == 0) ? 4 : index * 2;
                        Array.Resize(ref array, newSize);
                    }

                    array[index++] = component;
                }
            }

            var originTransform = parent.origin.transform;
            OfComponentDescendantsCore(ref originTransform, ref index, ref array);

            return index;
        }

        #endregion
    }
}
