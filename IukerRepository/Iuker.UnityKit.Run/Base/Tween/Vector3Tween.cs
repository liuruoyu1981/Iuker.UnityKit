/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 12:05:00
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
using UnityEngine;

namespace Iuker.UnityKit.Run.Tween
{
    public class Vector3Tween : Tween<Vector3>
    {
        private static Vector3 LerpVector3(ITween<Vector3> t, Vector3 start, Vector3 end, float progress)
        {
            return Vector3.Lerp(start, end, progress);
        }

        private static readonly Func<ITween<Vector3>, Vector3, Vector3, float, Vector3> LerpFunc = LerpVector3;

        /// <summary>
        /// Initializes a new QuaternionTween instance.
        /// </summary>
        public Vector3Tween() : base(LerpFunc) { }
    }
}
