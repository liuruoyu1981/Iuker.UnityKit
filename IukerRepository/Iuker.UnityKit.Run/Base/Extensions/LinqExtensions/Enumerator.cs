/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 16:50:35
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
    /// 游戏对象枚举器
    /// </summary>
    public struct Enumerator : IEnumerator<GameObject>
    {
        /// <summary>
        /// 枚举器被调用后，子对象数量固定
        /// </summary>
        private readonly int childCount;

        private readonly Transform originTransform;

        private bool canRun;

        private bool withSelf;
        private int currentIndex;
        private GameObject current;

        internal Enumerator(Transform originTransform, bool withSelf, bool canRun)
        {
            this.originTransform = originTransform;
            this.withSelf = withSelf;
            this.childCount = canRun ? originTransform.childCount : 0;
            this.currentIndex = -1;
            this.canRun = canRun;
            this.current = null;
        }

        public bool MoveNext()
        {
            if (!canRun) return false;

            if (withSelf) // 如果包含自身开关为真则首先移动到自身上
            {
                current = originTransform.gameObject;
                withSelf = false;   // 将包含自身开关置为假
                return true;
            }

            currentIndex++; // 更新当前索引
            if (currentIndex < childCount)
            {
                var child = originTransform.GetChild(currentIndex); //　获取当前索引上的子物体
                current = child.gameObject;
                return true;
            }

            return false;       // 如果当前索引已超出子物体数量则退出
        }

        /// <summary>
        /// 当前的游戏对象
        /// </summary>
        public GameObject Current { get { return current; } }

        /// <summary>
        /// 通过枚举器获得当前对象
        /// </summary>
        object IEnumerator.Current { get { return current; } }

        public void Dispose() { }

        public void Reset() { throw new NotSupportedException(); }
    }
}
