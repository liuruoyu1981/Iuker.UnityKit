/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 20:17:20
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
    public struct Enumerator_Descendants : IEnumerator<GameObject>
    {
        readonly int childCount; // childCount is fixed when GetEnumerator is called.

        readonly Transform originTransform;
        bool canRun;

        bool withSelf;
        int currentIndex;
        GameObject current;
        InternalUnsafeRefStack sharedStack;
        Func<Transform, bool> descendIntoChildren;

        internal Enumerator_Descendants(Transform originTransform, bool withSelf, bool canRun, InternalUnsafeRefStack sharedStack, Func<Transform, bool> descendIntoChildren)
        {
            this.originTransform = originTransform;
            this.withSelf = withSelf;
            this.childCount = canRun ? originTransform.childCount : 0;
            this.currentIndex = -1;
            this.canRun = canRun;
            this.current = null;
            this.sharedStack = sharedStack;
            this.descendIntoChildren = descendIntoChildren;
        }

        public bool MoveNext()
        {
            if (!canRun) return false;

            while (sharedStack.size != 0)
            {
                if (sharedStack.array[sharedStack.size - 1].MoveNextCore(true, out current))
                {
                    return true;
                }
            }

            if (!withSelf && !descendIntoChildren(originTransform))
            {
                // reuse
                canRun = false;
                InternalUnsafeRefStack.RefStackPool.Enqueue(sharedStack);
                return false;
            }

            if (MoveNextCore(false, out current))
            {
                return true;
            }
            else
            {
                // reuse
                canRun = false;
                InternalUnsafeRefStack.RefStackPool.Enqueue(sharedStack);
                return false;
            }
        }

        bool MoveNextCore(bool peek, out GameObject current)
        {
            if (withSelf)
            {
                current = originTransform.gameObject;
                withSelf = false;
                return true;
            }

            ++currentIndex;
            if (currentIndex < childCount)
            {
                var item = originTransform.GetChild(currentIndex);
                if (descendIntoChildren(item))
                {
                    var childEnumerator = new Enumerator_Descendants(item, true, true, sharedStack, descendIntoChildren);
                    sharedStack.Push(ref childEnumerator);
                    return sharedStack.array[sharedStack.size - 1].MoveNextCore(true, out current);
                }
                else
                {
                    current = item.gameObject;
                    return true;
                }
            }

            if (peek)
            {
                sharedStack.size--; // Pop
            }

            current = null;
            return false;
        }

        public GameObject Current { get { return current; } }
        object IEnumerator.Current { get { return current; } }

        public void Dispose()
        {
            if (canRun)
            {
                canRun = false;
                InternalUnsafeRefStack.RefStackPool.Enqueue(sharedStack);
            }
        }

        public void Reset() { throw new NotSupportedException(); }
    }
}
