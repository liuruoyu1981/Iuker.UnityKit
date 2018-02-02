/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/12 10:35:53
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
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Event;
using Iuker.UnityKit.Run.Base;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Event
{
    public class DefaultU3dModule_Event : AbsU3dModule, IU3dAppEventModule
    {
        private readonly EventModuleProxy mEventProxy = new EventModuleProxy();

        private readonly AppEventSupporter appEventSupport = AppEventSupporter.Instance;

        public override string ModuleName
        {
            get
            {
                return ModuleType.Event.ToString();
            }
        }

        public override void Init(IFrame frame)
        {
            base.Init(frame);

            WatchU3dAppEvent(AppEventType.Update, () =>
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    IssueEvent(U3dEventCode.Input_Escape.Literals);
                }
            });
            WatchU3dAppEvent(AppEventType.Update.ToString(), mEventProxy.ExecuteIssuedEvent);
        }

        #region AppEvent

        /// <summary>
        /// 移除一个UnityApp原生事件处理委托
        /// </summary>
        /// <param name="appEventType"></param>
        /// <param name="callback"></param>
        public void RemoveAppEvent(string appEventType, Action callback)
        {
            var eventType = (AppEventType)Enum.Parse(typeof(AppEventType), appEventType);
            appEventSupport.InternalRemoveLoopEvent(eventType, callback);
        }

        /// <summary>
        /// 观察一个UnityApp原生事件委托
        /// </summary>
        /// <param name="appEventType"></param>
        /// <param name="callback"></param>
        /// <param name="num"></param>
        public void WatchU3dAppEvent(string appEventType, Action callback, int num = -1)
        {
            var eventType = (AppEventType)Enum.Parse(typeof(AppEventType), appEventType);
            appEventSupport.AddOrUpdateEventCaller(eventType, callback, num);
        }

        public void WatchU3dAppEvent(AppEventType appEventType, Action callback, int num = -1)
        {
            WatchU3dAppEvent(appEventType.ToString(), callback, num);
        }

        #endregion

        #region Normal Event

        public void WatchEvent(string ecode, Action<object> handler, int num = -1)
        {
            mEventProxy.WatchEvent(ecode, handler, num);
        }

        public void WatchEvent(string ecode, Action handler, int num = -1)
        {
            mEventProxy.WatchEvent(ecode, handler, num);
        }

        public void IssueEvent(string eventCode, Action completedCallback = null, object data = null)
        {
            mEventProxy.IssueEvent(eventCode, completedCallback, data);
        }

        public void RemoveEvent(string EventCode)
        {
            mEventProxy.RemoveEvent(EventCode);
        }

        #endregion

    }
}
