/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 20:24:35
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

namespace Iuker.UnityKit.Run.LinqExtensions.AfterSelf
{
    public struct AfterSelfEnumerable : IEnumerable<GameObject>
    {
        readonly GameObject origin;
        readonly bool withSelf;

        public AfterSelfEnumerable(GameObject origin, bool withSelf)
        {
            this.origin = origin;
            this.withSelf = withSelf;
        }

        /// <summary>Returns a collection of specified component in the source collection.</summary>
        public OfComponentEnumerable<T> OfComponent<T>()
            where T : Component
        {
            return new OfComponentEnumerable<T>(ref this);
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

        public Enumerator GetEnumerator()
        {
            // check GameObject is destroyed only on GetEnumerator timing
            return (origin == null)
                ? new Enumerator(null, withSelf, false)
                : new Enumerator(origin.transform, withSelf, true);
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

        public void ForEach(Action<GameObject> action)
        {
            var e = this.GetEnumerator();
            while (e.MoveNext())
            {
                action(e.Current);
            }
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc(ref GameObject[] array)
        {
            var index = 0;

            var e = this.GetEnumerator(); // does not need to call Dispose.
            while (e.MoveNext())
            {
                var item = e.Current;
                if (array.Length == index)
                {
                    var newSize = (index == 0) ? 4 : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = item;
            }

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc(Func<GameObject, bool> filter, ref GameObject[] array)
        {
            var index = 0;
            var e = this.GetEnumerator(); // does not need to call Dispose.
            while (e.MoveNext())
            {
                var item = e.Current;
                if (!filter(item)) continue;

                if (array.Length == index)
                {
                    var newSize = (index == 0) ? 4 : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = item;
            }

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc<T>(Func<GameObject, T> selector, ref T[] array)
        {
            var index = 0;
            var e = this.GetEnumerator(); // does not need to call Dispose.
            while (e.MoveNext())
            {
                var item = e.Current;
                if (array.Length == index)
                {
                    var newSize = (index == 0) ? 4 : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = selector(item);
            }

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc<T>(Func<GameObject, bool> filter, Func<GameObject, T> selector, ref T[] array)
        {
            var index = 0;
            var e = this.GetEnumerator(); // does not need to call Dispose.
            while (e.MoveNext())
            {
                var item = e.Current;
                if (!filter(item)) continue;

                if (array.Length == index)
                {
                    var newSize = (index == 0) ? 4 : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = selector(item);
            }

            return index;
        }

        /// <summary>Store element into the buffer, return number is size. array is automaticaly expanded.</summary>
        public int ToArrayNonAlloc<TState, T>(Func<GameObject, TState> let, Func<TState, bool> filter, Func<TState, T> selector, ref T[] array)
        {
            var index = 0;
            var e = this.GetEnumerator(); // does not need to call Dispose.
            while (e.MoveNext())
            {
                var item = e.Current;
                var state = let(item);

                if (!filter(state)) continue;

                if (array.Length == index)
                {
                    var newSize = (index == 0) ? 4 : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = selector(state);
            }

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
            if (e.MoveNext())
            {
                return e.Current;
            }
            else
            {
                throw new InvalidOperationException("sequence is empty.");
            }
        }

        public GameObject FirstOrDefault()
        {
            var e = this.GetEnumerator();
            return (e.MoveNext())
                ? e.Current
                : null;
        }

        #endregion

        public struct Enumerator : IEnumerator<GameObject>
        {
            readonly int childCount; // childCount is fixed when GetEnumerator is called.
            readonly Transform originTransform;
            readonly bool canRun;

            bool withSelf;
            int currentIndex;
            GameObject current;
            Transform parent;

            internal Enumerator(Transform originTransform, bool withSelf, bool canRun)
            {
                this.originTransform = originTransform;
                this.withSelf = withSelf;
                this.currentIndex = (originTransform != null) ? originTransform.GetSiblingIndex() + 1 : 0;
                this.canRun = canRun;
                this.current = null;
                this.parent = originTransform.parent;
                this.childCount = (parent != null) ? parent.childCount : 0;
            }

            public bool MoveNext()
            {
                if (!canRun) return false;

                if (withSelf)
                {
                    current = originTransform.gameObject;
                    withSelf = false;
                    return true;
                }

                if (currentIndex < childCount)
                {
                    current = parent.GetChild(currentIndex).gameObject;
                    currentIndex++;
                    return true;
                }

                return false;
            }

            public GameObject Current { get { return current; } }
            object IEnumerator.Current { get { return current; } }
            public void Dispose() { }
            public void Reset() { throw new NotSupportedException(); }
        }

        public struct OfComponentEnumerable<T> : IEnumerable<T>
            where T : Component
        {
            AfterSelfEnumerable parent;

            public OfComponentEnumerable(ref AfterSelfEnumerable parent)
            {
                this.parent = parent;
            }

            public OfComponentEnumerator<T> GetEnumerator()
            {
                return new OfComponentEnumerator<T>(ref this.parent);
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

        public struct OfComponentEnumerator<T> : IEnumerator<T>
            where T : Component
        {
            Enumerator enumerator; // enumerator is mutable
            T current;

#if UNITY_EDITOR
            static List<T> componentCache = new List<T>(); // for no allocate on UNITY_EDITOR
#endif

            public OfComponentEnumerator(ref AfterSelfEnumerable parent)
            {
                this.enumerator = parent.GetEnumerator();
                this.current = default(T);
            }

            public bool MoveNext()
            {
                while (enumerator.MoveNext())
                {
#if UNITY_EDITOR
                    enumerator.Current.GetComponents<T>(componentCache);
                    if (componentCache.Count != 0)
                    {
                        current = componentCache[0];
                        componentCache.Clear();
                        return true;
                    }
#else
                        
                        var component = enumerator.Current.GetComponent<T>();
                        if (component != null)
                        {
                            current = component;
                            return true;
                        }
#endif
                }

                return false;
            }

            public T Current { get { return current; } }
            object IEnumerator.Current { get { return current; } }
            public void Dispose() { }
            public void Reset() { throw new NotSupportedException(); }
        }
    }
}
