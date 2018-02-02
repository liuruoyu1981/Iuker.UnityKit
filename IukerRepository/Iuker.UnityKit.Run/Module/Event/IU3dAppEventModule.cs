/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/12 10:34:48
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
using Iuker.Common.Base.Enums;
using Iuker.Common.Module.Event;

namespace Iuker.UnityKit.Run.Module.Event
{
    public interface IU3dAppEventModule : IEventModule
    {
        /// <summary>
        /// 观察一个App事件
        /// </summary>
        /// <param name="appEventType"></param>
        /// <param name="callback"></param>
        /// <param name="num">APP事件委托执行次数默认为-1即不限制次数</param>
        void WatchU3dAppEvent(string appEventType, Action callback, int num = -1);

        /// <summary>
        /// 观察一个App事件
        /// </summary>
        /// <param name="appEventType"></param>
        /// <param name="callback"></param>
        /// <param name="num">APP事件委托执行次数默认为-1即不限制次数</param>
        void WatchU3dAppEvent(AppEventType appEventType, Action callback, int num = -1);

        /// <summary>
        /// 取消对一个App事件的观察
        /// </summary>
        /// <param name="appEventType"></param>
        /// <param name="callback"></param>
        void RemoveAppEvent(string appEventType, Action callback);


    }
}
