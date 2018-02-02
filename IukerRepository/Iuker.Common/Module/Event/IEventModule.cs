/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/12 10:32:59
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

namespace Iuker.Common.Module.Event
{
    public interface IEventModule : IModule
    {
        /// <summary>
        /// 关注一个需要传递数据的事件
        /// </summary>
        /// <param name="ecode"></param>
        /// <param name="handler">事件处理委托</param>
        /// <param name="num">事件委托执行次数默认为-1即不限制次数</param>
        void WatchEvent(string ecode, Action<object> handler, int num = -1);

        /// <summary>
        /// 关注一个无需数据的事件
        /// </summary>
        /// <param name="ecode"></param>
        /// <param name="handler"></param>
        /// <param name="num"></param>
        void WatchEvent(string ecode, Action handler, int num = -1);

        /// <summary>
        /// 发起一个事件
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="data">事件数据</param>
        /// <param name="completedCallback">事件处理完毕回调委托</param>
        void IssueEvent(string eventCode, Action completedCallback = null, object data = null);

        /// <summary>
        /// 移除一个事件处理
        /// </summary>
        /// <param name="EventCode"></param>
        /// <param name="handler"></param>
        void RemoveEvent(string EventCode);
    }
}
