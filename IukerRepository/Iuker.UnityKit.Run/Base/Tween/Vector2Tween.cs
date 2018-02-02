/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 12:03:51
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
    /// 使用Vector2的动画
    /// </summary>
    public class Vector2Tween : Tween<Vector2>
    {
        private static Vector2 LerpVector2(ITween<Vector2> t, Vector2 start, Vector2 end, float progress) { return Vector2.Lerp(start, end, progress); }
        private static readonly Func<ITween<Vector2>, Vector2, Vector2, float, Vector2> LerpFunc = LerpVector2;

        /// <summary>
        /// 初始化一个Vector2动画实例
        /// </summary>
        public Vector2Tween() : base(LerpFunc) { }
    }
}
