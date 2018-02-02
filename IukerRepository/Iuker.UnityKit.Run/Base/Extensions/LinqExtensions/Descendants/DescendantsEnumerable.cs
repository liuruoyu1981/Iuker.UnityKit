/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 20:05:03
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
    /// <summary>
    /// 可枚举后代
    /// </summary>
    public struct DescendantsEnumerable : IEnumerable<GameObject>
    {
        private static readonly Func<Transform, bool> alwaysTrue = _ => true;

        internal readonly GameObject origin;
        internal readonly bool withSelf;
        internal readonly Func<Transform, bool> descendIntoChildren;

        public DescendantsEnumerable(GameObject origin, bool withSelf, Func<Transform, bool> descendIntoChildren)
        {
            this.origin = origin;
            this.withSelf = withSelf;
            this.descendIntoChildren = descendIntoChildren ?? alwaysTrue;
        }

        /// <summary>Returns a collection of specified component in the source collection.</summary>
        public OfComponentEnumerable_Descendants<T> OfComponent<T>()
            where T : Component
        {
            return new OfComponentEnumerable_Descendants<T>(ref this);
        }

        /// <summary>Destroy every GameObject in the source collection safety(check null).</summary>
        /// <param name="useDestroyImmediate">If in EditMode, should be true or pass !Application.isPlaying.</param>
        public void Destroy(bool useDestroyImmediate = false)
        {
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Destroy(useDestroyImmediate, false);
            }
        }

        /// <summary>Destroy every GameObject in the source collection safety(check null).</summary>
        /// <param name="useDestroyImmediate">If in EditMode, should be true or pass !Application.isPlaying.</param>
        public void Destroy(Func<GameObject, bool> predicate, bool useDestroyImmediate = false)
        {
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                var item = e.Current;
                if (predicate(item))
                {
                    item.Destroy(useDestroyImmediate, false);
                }
            }
        }

        public Enumerator_Descendants GetEnumerator()
        {
            // check GameObject is destroyed only on GetEnumerator timing
            if (origin == null)
            {
                return new Enumerator_Descendants(null, withSelf, false, null, descendIntoChildren);
            }

            InternalUnsafeRefStack refStack;
            if (InternalUnsafeRefStack.RefStackPool.Count != 0)
            {
                refStack = InternalUnsafeRefStack.RefStackPool.Dequeue();
                refStack.Reset();
            }
            else
            {
                refStack = new InternalUnsafeRefStack(6);
            }

            return new Enumerator_Descendants(origin.transform, withSelf, true, refStack, descendIntoChildren);
        }

        IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region LINQ

        internal void ResizeArray<T>(ref int index, ref T[] array)
        {
            if (array.Length == index)
            {
                var newSize = (index == 0) ? 4 : index * 2;
                Array.Resize(ref array, newSize);
            }
        }

        internal void DescendantsCore(ref Transform transform, ref Action<GameObject> action)
        {
            if (!descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                action(child.gameObject);
                DescendantsCore(ref child, ref action);
            }
        }

        internal void DescendantsCore(ref Transform transform, ref int index, ref GameObject[] array)
        {
            if (!descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                ResizeArray(ref index, ref array);
                array[index++] = child.gameObject;
                DescendantsCore(ref child, ref index, ref array);
            }
        }

        internal void DescendantsCore(ref Func<GameObject, bool> filter, ref Transform transform, ref int index, ref GameObject[] array)
        {
            if (!descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                var childGameObject = child.gameObject;
                if (filter(childGameObject))
                {
                    ResizeArray(ref index, ref array);
                    array[index++] = childGameObject;
                }
                DescendantsCore(ref filter, ref child, ref index, ref array);
            }
        }

        internal void DescendantsCore<T>(ref Func<GameObject, T> selector, ref Transform transform, ref int index, ref T[] array)
        {
            if (!descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                ResizeArray(ref index, ref array);
                array[index++] = selector(child.gameObject);
                DescendantsCore(ref selector, ref child, ref index, ref array);
            }
        }

        internal void DescendantsCore<T>(ref Func<GameObject, bool> filter, ref Func<GameObject, T> selector, ref Transform transform, ref int index, ref T[] array)
        {
            if (!descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                var childGameObject = child.gameObject;
                if (filter(childGameObject))
                {
                    ResizeArray(ref index, ref array);
                    array[index++] = selector(childGameObject);
                }
                DescendantsCore(ref filter, ref selector, ref child, ref index, ref array);
            }
        }

        internal void DescendantsCore<TState, T>(ref Func<GameObject, TState> let, ref Func<TState, bool> filter, ref Func<TState, T> selector, ref Transform transform, ref int index, ref T[] array)
        {
            if (!descendIntoChildren(transform)) return;

            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                var state = let(child.gameObject);
                if (filter(state))
                {
                    ResizeArray(ref index, ref array);
                    array[index++] = selector(state);
                }
                DescendantsCore(ref let, ref filter, ref selector, ref child, ref index, ref array);
            }
        }

        /// <summary>Use internal iterator for performance optimization.</summary>
        /// <param name="action"></param>
        public void ForEach(Action<GameObject> action)
        {
            if (withSelf)
            {
                action(origin);
            }
            var originTransform = origin.transform;
            DescendantsCore(ref originTransform, ref action);
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc(ref GameObject[] array)
        {
            var index = 0;
            if (withSelf)
            {
                ResizeArray(ref index, ref array);
                array[index++] = origin;
            }

            var originTransform = origin.transform;
            DescendantsCore(ref originTransform, ref index, ref array);

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc(Func<GameObject, bool> filter, ref GameObject[] array)
        {
            var index = 0;
            if (withSelf && filter(origin))
            {
                ResizeArray(ref index, ref array);
                array[index++] = origin;
            }
            var originTransform = origin.transform;
            DescendantsCore(ref filter, ref originTransform, ref index, ref array);

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc<T>(Func<GameObject, T> selector, ref T[] array)
        {
            var index = 0;
            if (withSelf)
            {
                ResizeArray(ref index, ref array);
                array[index++] = selector(origin);
            }
            var originTransform = origin.transform;
            DescendantsCore(ref selector, ref originTransform, ref index, ref array);

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc<T>(Func<GameObject, bool> filter, Func<GameObject, T> selector, ref T[] array)
        {
            var index = 0;
            if (withSelf && filter(origin))
            {
                ResizeArray(ref index, ref array);
                array[index++] = selector(origin);
            }
            var originTransform = origin.transform;
            DescendantsCore(ref filter, ref selector, ref originTransform, ref index, ref array);

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc<TState, T>(Func<GameObject, TState> let, Func<TState, bool> filter, Func<TState, T> selector, ref T[] array)
        {
            var index = 0;
            if (withSelf)
            {
                var state = let(origin);
                if (filter(state))
                {
                    ResizeArray(ref index, ref array);
                    array[index++] = selector(state);
                }
            }

            var originTransform = origin.transform;
            DescendantsCore(ref let, ref filter, ref selector, ref originTransform, ref index, ref array);

            return index;
        }

        public GameObject[] ToArray()
        {
            var array = new GameObject[4];
            var len = ToArrayNonAlloc(ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public GameObject[] ToArray(Func<GameObject, bool> filter)
        {
            var array = new GameObject[4];
            var len = ToArrayNonAlloc(filter, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public T[] ToArray<T>(Func<GameObject, T> selector)
        {
            var array = new T[4];
            var len = ToArrayNonAlloc<T>(selector, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public T[] ToArray<T>(Func<GameObject, bool> filter, Func<GameObject, T> selector)
        {
            var array = new T[4];
            var len = ToArrayNonAlloc(filter, selector, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public T[] ToArray<TState, T>(Func<GameObject, TState> let, Func<TState, bool> filter, Func<TState, T> selector)
        {
            var array = new T[4];
            var len = ToArrayNonAlloc(let, filter, selector, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public GameObject First()
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

        public GameObject FirstOrDefault()
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

        #endregion

    }
}
