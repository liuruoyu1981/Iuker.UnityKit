/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 12:05:41
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
    public class Vector4Tween : Tween<Vector4>
    {
        private static Vector4 LerpVector4(ITween<Vector4> t, Vector4 start, Vector4 end, float progress) { return Vector4.Lerp(start, end, progress); }
        private static readonly Func<ITween<Vector4>, Vector4, Vector4, float, Vector4> LerpFunc = LerpVector4;

        /// <summary>
        /// Initializes a new Vector4Tween instance.
        /// </summary>
        public Vector4Tween() : base(LerpFunc) { }
    }
}
