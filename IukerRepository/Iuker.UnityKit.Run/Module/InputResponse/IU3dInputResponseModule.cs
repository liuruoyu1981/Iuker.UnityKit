/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/28 12:41:36
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
using Iuker.Common;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.InputResponse
{
    /// <summary>
    /// 输入响应模块
    /// </summary>
    public interface IU3dInputResponseModule : IModule
    {
        /// <summary>
        /// 当前点击的游戏对象
        /// </summary>
        GameObject CurrentClick { get; set; }

        /// <summary>
        /// 触发输入事件
        /// </summary>
        /// <param name="ryInputEventType"></param>
        void IssueInputEvent(InputEventType ryInputEventType);

        /// <summary>
        /// 注册一个输入响应处理委托
        /// </summary>
        /// <param name="ryInputEventType"></param>
        /// <param name="action"></param>
        /// <param name="executeCount"></param>
        void WatchInputEvent(InputEventType ryInputEventType, Action action, int executeCount = -1);

        /// <summary>
        /// 移除一个输入响应处理委托
        /// </summary>
        /// <param name="inputEventType"></param>
        /// <param name="action"></param>
        void RemoveInputEvent(InputEventType inputEventType, Action action);

        /// <summary>
        /// 输入输出控制状态
        /// </summary>
        InputStatus InputStatus { get; set; }
    }
}
