﻿/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 20:30:15
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


namespace Iuker.UnityKit.Run.LinqExtensions.OPerate
{
    /// <summary>
    /// Transform组件移动类型
    /// </summary>
    public enum TransformMoveType
    {
        /// <summary>
        /// 跟随父物体移动
        /// </summary>
        FollowParent,

        /// <summary>
        /// 将位置设置为Vector3.Zero，缩放设置为Vector3.One,选择设置为identity
        /// </summary>
        Origin,

        /// <summary>
        /// 不做任何改变
        /// </summary>
        DoNothing
    }
}