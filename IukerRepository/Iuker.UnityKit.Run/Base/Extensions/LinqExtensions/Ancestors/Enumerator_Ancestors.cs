/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 19:52:38
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
    /// <summary>
    /// 祖先枚举器
    /// </summary>
    public struct Enumerator_Ancestors:IEnumerator<GameObject>
    {
        private bool canRun;
        GameObject current;
        Transform currentTransform;
        bool withSelf;

        internal Enumerator_Ancestors(GameObject origin,Transform originTransform,bool withSelf,bool canRun)
        {
            this.current = origin;
            this.currentTransform = originTransform;
            this.withSelf = withSelf;
            this.canRun = canRun;
        }

        public bool MoveNext()
        {
            if (!canRun) return false;

            if (withSelf)
            {
                // withSelf, use origin and originTransform
                withSelf = false;
                return true;
            }

            var parentTransform = currentTransform.parent;
            if (parentTransform != null)
            {
                current = parentTransform.gameObject;
                currentTransform = parentTransform;
                return true;
            }

            return false;
        }

        public GameObject Current { get { return current; } }
        object IEnumerator.Current { get { return current; } }
        public void Dispose() { }
        public void Reset() { throw new NotSupportedException(); }

    }
}
