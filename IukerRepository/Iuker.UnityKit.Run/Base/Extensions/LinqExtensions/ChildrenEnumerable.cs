/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 16:48:53
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
    /// 可枚举的子游戏对象
    /// </summary>
    public struct ChildrenEnumerable : IEnumerable<GameObject>
    {
        private readonly GameObject origin;
        private readonly bool withSelf;

        public ChildrenEnumerable(GameObject origin, bool withSelf)
        {
            this.origin = origin;
            this.withSelf = withSelf;
        }

        public OfComponentEnumerable<T> OfComponent<T>() where T : Component
        {
            return new OfComponentEnumerable<T>(ref this);
        }

        public void Destroy(bool useDestroyImmediate = false, bool detachParent = false)
        {

        }

        /// <summary>
        /// 获得枚举器
        /// </summary>
        /// <returns></returns>
        public Enumerator GetEnumerator()
        {
            return (origin == null)
                ? new Enumerator(null, withSelf, false)
                : new Enumerator(origin.transform, withSelf, true);
        }

        IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        #region LINQ

        /// <summary>
        /// 获得子游戏对象数量
        /// 如果包含自身则为子游戏对象数量加上自身
        /// </summary>
        /// <returns></returns>
        public int GetChildrenSize() { return origin.transform.childCount + (withSelf ? 1 : 0); }

        /// <summary>
        /// 对枚举器内部游戏对象集合中每个游戏对象执行传入委托
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<GameObject> action)
        {
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                action(e.Current);
            }
        }

        /// <summary>
        /// 将枚举器内部集合中所有游戏对象存储到缓存数组中并返回内部集合的游戏对象数量
        /// 缓存数组的容量若小于内部集合元素数量则会进行自动扩容
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public int ToArrayNonAlloc(ref GameObject[] array)
        {
            var index = 0;

            var e = GetEnumerator();
            while (e.MoveNext())
            {
                var item = e.Current;
                if (array.Length == index)
                {
                    var newSize = (index == 0) ? GetChildrenSize() : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = item;
            }

            return index;
        }

        /// <summary>
        /// 将枚举器内部集合中所有游戏对象存储到缓存数组中并返回内部集合的游戏对象数量
        /// 缓存数组的容量若小于内部集合元素数量则会进行自动扩容
        /// 可以传递一个过滤器用于过滤内部集合中的游戏对象
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public int ToArrayNonAlloc(Func<GameObject, bool> filter, ref GameObject[] array)
        {
            var index = 0;

            var e = GetEnumerator();
            while (e.MoveNext())
            {
                var item = e.Current;
                if (!filter(item)) continue;

                if (array.Length == index)
                {
                    var newSize = (index == 0) ? GetChildrenSize() : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = item;
            }

            return index;
        }

        /// <summary>
        /// 使用选择器选择枚举器内部集合中游戏对象上的组件
        /// 并将组件投射到传入的组件数组中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public int ToArrayNonAlloc<T>(Func<GameObject, T> selector, ref T[] array)
        {
            var index = 0;
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                var item = e.Current;
                if (array.Length == index)
                {
                    var newSize = (index == 0) ? GetChildrenSize() : index * 2;
                    Array.Resize(ref array, newSize);
                }
            }

            return index;
        }


        /// <summary>
        /// 使用过滤器过滤枚举器内部集合中的游戏对象
        /// 使用选择器选择游戏对象上的组件并投射到传入的缓存组件数组中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="selector"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public int ToArrayNonAlloc<T>(Func<GameObject, bool> filter, Func<GameObject, T> selector, ref T[] array)
        {
            var index = 0;
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                var item = e.Current;
                if (!filter(item)) continue;

                if (array.Length == index)
                {
                    var newSize = (index == 0) ? GetChildrenSize() : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = selector(item);
            }

            return index;
        }

        public int ToArrayNonAlloc<TState, T>(Func<GameObject, TState> let, Func<TState, bool> filter, Func<TState, T> selector,
            ref T[] array)
        {
            var index = 0;
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                var item = e.Current;
                var state = let(item);

                if (!filter(state)) continue;

                if (array.Length == index)
                {
                    var newSize = (index == 0) ? GetChildrenSize() : index * 2;
                    Array.Resize(ref array, newSize);
                }
                array[index++] = selector(state);
            }

            return index;
        }

        public GameObject[] ToArray()
        {
            var array = new GameObject[GetChildrenSize()];
            var len = ToArrayNonAlloc(ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public GameObject[] ToArray(Func<GameObject, bool> filter)
        {
            var array = new GameObject[GetChildrenSize()];
            var len = ToArrayNonAlloc(filter, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public T[] ToArray<T>(Func<GameObject, T> selector)
        {
            var array = new T[GetChildrenSize()];
            var len = ToArrayNonAlloc(selector, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public T[] ToArray<T>(Func<GameObject, bool> filter, Func<GameObject, T> selector)
        {
            var array = new T[GetChildrenSize()];
            var len = ToArrayNonAlloc(filter, selector, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        public T[] ToArray<TState, T>(Func<GameObject, TState> let, Func<TState, bool> filter, Func<TState, T> selector)
        {
            var array = new T[GetChildrenSize()];
            var len = ToArrayNonAlloc(let, filter, selector, ref array);
            if (array.Length != len)
            {
                Array.Resize(ref array, len);
            }
            return array;
        }

        /// <summary>
        /// 获得枚举器内部游戏对象集合中的第一个游戏对象
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 尝试获得枚举器内部游戏对象集合中的第一个游戏对象
        /// 若枚举器内部集合为空则会返回空
        /// </summary>
        /// <returns></returns>
        public GameObject FirstOrDefault()
        {
            var e = this.GetEnumerator();
            return (e.MoveNext())
                ? e.Current
                : null;
        }


        #endregion
    }
}
