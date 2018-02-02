/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 19:42:15
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

using Iuker.UnityKit.Run.LinqExtensions.Ancestors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.LinqExtensions
{
    /// <summary>
    /// 可枚举的祖先对象
    /// </summary>
    public struct AncestorsEnumerable : IEnumerable<GameObject>
    {
        private readonly GameObject origin;
        private readonly bool withSelf;

        public AncestorsEnumerable(GameObject origin, bool withSelf)
        {
            this.origin = origin;
            this.withSelf = withSelf;
        }

        IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Enumerator_Ancestors GetEnumerator()
        {
            // check GameObject is destroyed only on GetEnumerator timing
            return (origin == null)
                ? new Enumerator_Ancestors(null, null, withSelf, false)
                : new Enumerator_Ancestors(origin, origin.transform, withSelf, true);
        }

        public OfComponentEnumerable_Ancestors<T> OfComponent<T>()
              where T : Component
        {
            return new OfComponentEnumerable_Ancestors<T>(ref this);
        }

        public void Destroy(bool useDestroyImmediate = false)
        {
            var e = GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Destroy(useDestroyImmediate, false);
            }
        }

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




    }
}
