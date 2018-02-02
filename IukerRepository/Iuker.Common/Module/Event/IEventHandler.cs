/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2/12/2017 19:34:42
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 事件处理器接口
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;

namespace Iuker.Common.Event
{
    /// <summary>
    /// 事件处理器接口
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        void HandleEvent();

        Action EventAction { get; }
    }

    /// <summary>
    /// 事件处理器接口
    /// </summary>
    /// <typeparam name="TData">事件数据</typeparam>
    public interface IEventHandler<TData>
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventData"></param>
        void HandleEvent(TData eventData);

        Action<TData> EventAction { get; }
    }
}