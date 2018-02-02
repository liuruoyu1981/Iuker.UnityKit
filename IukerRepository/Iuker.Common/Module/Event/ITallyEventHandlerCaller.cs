/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2/12/2017 19:52:49
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
    /// 可计数事件处理器调用器接口
    /// </summary>
    public interface ITallyEventHandlerCaller
    {
        /// <summary>
        /// 添加一个事件处理器
        /// </summary>
        /// <param name="eventAction">事件处理委托</param>
        /// <param name="executeCount">事件处理委托的执行次数，默认为-1即不限制次数</param>
        ITallyEventHandlerCaller AddHandler(Action eventAction, int executeCount = -1);

        /// <summary>
        /// 移除一个事件处理委托
        /// </summary>
        /// <param name="evenAction"></param>
        ITallyEventHandlerCaller RemoveHandler(Action evenAction);

        /// <summary>
        /// 事件处理器数量
        /// </summary>
        int EventHanlderCount { get; }

        /// <summary>
        /// 调用所有的事件处理器
        /// </summary>
        ITallyEventHandlerCaller CallEventHanlder();

        /// <summary>
        /// 移除执行次数为零的事件处理器
        /// </summary>
        ITallyEventHandlerCaller ClearZero();
    }


    #region 泛型

    /// <summary>
    /// 泛型可计数事件处理器调用者接口，该调用者可以调用需要一个泛型参数的事件处理器
    /// </summary>
    public interface IEventHandlerCaller<TData>
    {
        /// <summary>
        /// 添加一个事件处理器
        /// </summary>
        /// <param name="eventAction">事件处理委托</param>
        /// <param name="executeCount">事件处理委托的执行次数，默认为-1即不限制次数</param>
        IEventHandlerCaller<TData> AddHandler(Action<TData> eventAction, int executeCount = -1);

        /// <summary>
        /// 移除一个事件处理委托
        /// </summary>
        /// <param name="evenAction"></param>
        IEventHandlerCaller<TData> RemoveHandler(Action<TData> evenAction);

        /// <summary>
        /// 事件处理器数量
        /// </summary>
        int EventHanlderCount { get; }

        /// <summary>
        /// 调用所有的泛型事件处理器
        /// </summary>
        IEventHandlerCaller<TData> CallEventHanlder(TData td);

        /// <summary>
        /// 移除执行次数为零的事件处理器
        /// </summary>
        IEventHandlerCaller<TData> ClearZero();


    }

    #endregion

}