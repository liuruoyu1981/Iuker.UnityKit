/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 12:06:26
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
    public class ColorTween : Tween<Color>
    {
        private static Color LerpColor(ITween<Color> t, Color start, Color end, float progress) { return Color.Lerp(start, end, progress); }
        private static readonly Func<ITween<Color>, Color, Color, float, Color> LerpFunc = LerpColor;

        /// <summary>
        /// Initializes a new ColorTween instance.
        /// </summary>
        public ColorTween() : base(LerpFunc) { }
    }
}
