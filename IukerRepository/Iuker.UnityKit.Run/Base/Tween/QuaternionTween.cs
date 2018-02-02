/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 12:07:01
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
    /// <summary>
    /// Object used to tween Quaternion values.
    /// </summary>
    public class QuaternionTween : Tween<Quaternion>
    {
        private static Quaternion LerpQuaternion(ITween<Quaternion> t, Quaternion start, Quaternion end, float progress) { return Quaternion.Lerp(start, end, progress); }
        private static readonly Func<ITween<Quaternion>, Quaternion, Quaternion, float, Quaternion> LerpFunc = LerpQuaternion;

        /// <summary>
        /// Initializes a new QuaternionTween instance.
        /// </summary>
        public QuaternionTween() : base(LerpFunc) { }
    }
}
