using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using Iuker.Common.Base.Enums;
using Iuker.Common.Event;

namespace Iuker.UnityKit.Run.Module.Event
{
#if DEBUG
    /// <summary>
    /// App事件支持器组件，mono组件通过其分发Unity的原生事件。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170925 14:47:42")]
    [ClassPurposeDesc("App事件支持器组件，mono组件通过其分发Unity的原生事件。", "App事件支持器组件，mono组件通过其分发Unity的原生事件。")]
#endif
    public sealed class AppEventSupporter : MonoSingleton<AppEventSupporter>
    {
        private void FixedUpdate() { InvokeCaller(AppEventType.FixedUpdate); }

        private void Update() { InvokeCaller(AppEventType.Update); }

        private void LateUpdate() { InvokeCaller(AppEventType.LateUpdate); }

        private void OnGUI() { InvokeCaller(AppEventType.OnGUI); }

        private void OnApplicationQuit()
        {
            InvokeCaller(AppEventType.AppQuit);
        }

        private void OnApplicationPause()
        {
            InvokeCaller(AppEventType.AppPause);
        }

        private void OnApplicationFocus()
        {
            InvokeCaller(AppEventType.AppFocus);
        }

        private readonly Dictionary<AppEventType, ITallyEventHandlerCaller> eventCallerDic =
            new Dictionary<AppEventType, ITallyEventHandlerCaller>();

        private void InvokeCaller(AppEventType t)
        {
            if (eventCallerDic.ContainsKey(t))
            {
                eventCallerDic[t].CallEventHanlder();
            }
        }

        /// <summary>
        /// 增加或更新事件处理器调用器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action"></param>
        /// <param name="count"></param>
        public void AddOrUpdateEventCaller(AppEventType type, Action action, int count = -1)
        {
            if (eventCallerDic.ContainsKey(type))
            {
                eventCallerDic[type].AddHandler(action);
            }
            else
            {
                var caller = GetEventCaller().AddHandler(action, count);
                eventCallerDic.Add(type, caller);
            }
        }

        private ITallyEventHandlerCaller GetEventCaller()
        {
            return new TallyEventHandlerCaller();
        }

        public void InternalRemoveLoopEvent(AppEventType type, Action action)
        {
            if (eventCallerDic.ContainsKey(type))
            {
                var caller = eventCallerDic[type];
                caller.RemoveHandler(action);
            }
        }
    }
}
