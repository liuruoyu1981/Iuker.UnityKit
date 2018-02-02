/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/8/4 7:26:07
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

using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.Effect
{
    /// <summary>
    /// 波纹效果数据
    /// </summary>
    public class RippleData
    {
        public enum SizeModeType
        {
            FillRect,
            MatchSize
        }

        public bool AutoSize = true;
        public float SizePercent = 105f;
        public float ManualSize;
        public SizeModeType SizeMode = SizeModeType.FillRect;
        public float Speed = 8f;
        public Color Color = Color.black;
        public float StartAlpha = 0.3f;
        public float EndAlpha = 0.1f;
        public bool MoveTowardCenter = true;
        public Transform RippleParent;
        public bool PlaceRippleBehind;

        /// <summary>
        /// 拷贝一个现有的波纹效果数据
        /// </summary>
        /// <returns></returns>
        public RippleData Copy()
        {
            RippleData data = new RippleData();
            data.AutoSize = AutoSize;
            data.SizePercent = SizePercent;
            data.ManualSize = ManualSize;
            data.SizeMode = SizeMode;
            data.Speed = Speed;
            data.Color = Color;
            data.StartAlpha = StartAlpha;
            data.EndAlpha = EndAlpha;
            data.MoveTowardCenter = MoveTowardCenter;
            data.RippleParent = RippleParent;
            data.PlaceRippleBehind = PlaceRippleBehind;
            return data;
        }
    }
}