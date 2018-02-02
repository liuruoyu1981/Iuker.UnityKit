/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 15:52:08
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

using Iuker.UnityKit.Run.LinqExtensions.AfterSelf;
using Iuker.UnityKit.Run.LinqExtensions.BeforeSelf;
using Iuker.UnityKit.Run.LinqExtensions.Descendants;
using System;
using UnityEngine;

namespace Iuker.UnityKit.Run.LinqExtensions
{
    public static partial class GameObjectLinq
    {
        public static GameObject Parent(this GameObject origin)
        {
            var parentTransform = origin == null ? null : origin.transform.parent;
            return parentTransform != null ? parentTransform.gameObject : null;
        }

        /// <summary>
        /// 获得游戏对象指定名字的子游戏对象
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject Child(this GameObject origin, string name)
        {
            if (origin == null) return null;

            var child = origin.transform.Find(name);
            if (child == null) return null;
            return child.gameObject;
        }

        public static ChildrenEnumerable Children(this GameObject origin)
        {
            return new ChildrenEnumerable(origin, false);
        }

        public static ChildrenEnumerable ChildrenAndSelf(this GameObject origin)
        {
            return new ChildrenEnumerable(origin, true);
        }

        /// <summary>Returns a collection of the ancestor GameObjects of this GameObject.</summary>
        public static AncestorsEnumerable Ancestors(this GameObject origin)
        {
            return new AncestorsEnumerable(origin, false);
        }

        /// <summary>Returns a collection of GameObjects that contain this element, and the ancestors of this GameObject.</summary>
        public static AncestorsEnumerable AncestorsAndSelf(this GameObject origin)
        {
            return new AncestorsEnumerable(origin, true);
        }

        /// <summary>Returns a collection of the descendant GameObjects.</summary>
        public static DescendantsEnumerable Descendants(this GameObject origin, Func<Transform, bool> descendIntoChildren = null)
        {
            return new DescendantsEnumerable(origin, false, descendIntoChildren);
        }

        /// <summary>Returns a collection of GameObjects that contain this GameObject, and all descendant GameObjects of this GameObject.</summary>
        public static DescendantsEnumerable DescendantsAndSelf(this GameObject origin, Func<Transform, bool> descendIntoChildren = null)
        {
            return new DescendantsEnumerable(origin, true, descendIntoChildren);
        }

        /// <summary>Returns a collection of the sibling GameObjects before this GameObject.</summary>
        public static BeforeSelfEnumerable BeforeSelf(this GameObject origin)
        {
            return new BeforeSelfEnumerable(origin, false);
        }

        /// <summary>Returns a collection of GameObjects that contain this GameObject, and the sibling GameObjects before this GameObject.</summary>
        public static BeforeSelfEnumerable BeforeSelfAndSelf(this GameObject origin)
        {
            return new BeforeSelfEnumerable(origin, true);
        }

        /// <summary>Returns a collection of the sibling GameObjects after this GameObject.</summary>
        public static AfterSelfEnumerable AfterSelf(this GameObject origin)
        {
            return new AfterSelfEnumerable(origin, false);
        }

        /// <summary>Returns a collection of GameObjects that contain this GameObject, and the sibling GameObjects after this GameObject.</summary>
        public static AfterSelfEnumerable AfterSelfAndSelf(this GameObject origin)
        {
            return new AfterSelfEnumerable(origin, true);
        }





    }
}
