/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 12:00:21
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

namespace Iuker.UnityKit.Run.Tween
{
    /// <summary>
    /// 浮点动画
    /// </summary>
    public class FloatTween : Tween<float>
    {
        private static float LerpFloat(ITween<float> t, float start, float end, float progress) { return start + (end - start) * progress; }
        private static readonly Func<ITween<float>, float, float, float, float> LerpFunc = LerpFloat;

        /// <summary>
        /// 初始化一个浮点动画实例.
        /// </summary>
        public FloatTween() : base(LerpFunc) { }
    }
}
