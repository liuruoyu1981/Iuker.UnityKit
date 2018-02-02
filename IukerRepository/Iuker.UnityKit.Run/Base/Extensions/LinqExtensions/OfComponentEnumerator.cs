/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 16:54:47
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
    /// 用于Unity3d组件的枚举器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct OfComponentEnumerator<T> : IEnumerator<T> where T : Component
    {
        /// <summary>
        /// 游戏对象枚举器
        /// </summary>
        private Enumerator enumerator;
        T current;

#if UNITY_EDITOR
        static List<T> componentCache = new List<T>(); // for no allocate on UNITY_EDITOR
#endif

        public OfComponentEnumerator(ref ChildrenEnumerable parent)
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
                    return true; // 如果枚举器的当前游戏对象返回了一个空组件列表则返回假否则返回真
                }
#else
                var component = enumerator.Current.GetComponent<T>();
                if(component != null)
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
