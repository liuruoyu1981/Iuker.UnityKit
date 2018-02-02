using System;
using System.Collections.Generic;
using System.Linq;

namespace Iuker.Common.Event
{
    /// <summary>
    /// 事件处理单元
    /// </summary>
    public class EventHandleUnit
    {
        /// <summary>
        /// 带有参数的事件处理器调用者列表字典
        /// </summary>
        private readonly Dictionary<string, IEventHandlerCaller<object>> dataEventDic = new Dictionary<string, IEventHandlerCaller<object>>();

        public void WatchEvent(string eventName, Action<object> action, int execeuteCount = -1)
        {
            if (dataEventDic.ContainsKey(eventName)) dataEventDic[eventName].AddHandler(action, execeuteCount);
            else
            {
                var newCaller = new TallyEventHandlerCaller<object>().AddHandler(action, execeuteCount);
                dataEventDic.Add(eventName, newCaller);
            }
        }

        /// <summary>
        /// 无参数事件处理器调用者字典
        /// </summary>
        private readonly Dictionary<string, ITallyEventHandlerCaller> nodataEventDic = new Dictionary<string, ITallyEventHandlerCaller>();

        public void WatchEvent(string eventName, Action action, int executeCount = -1)
        {
            if (nodataEventDic.ContainsKey(eventName)) nodataEventDic[eventName].AddHandler(action, executeCount);
            else
            {
                var newCaller = new TallyEventHandlerCaller().AddHandler(action, executeCount);
                nodataEventDic.Add(eventName, newCaller);
            }
        }

        public void RemoveEvent(string eventName)
        {
            RemoveDataEvent(eventName);
            RemoveNoDataEvent(eventName);
        }

        private void RemoveNoDataEvent(string eventName)
        {
            if (nodataEventDic.ContainsKey(eventName))
            {
                nodataEventDic.Remove(eventName);
            }
        }

        private void RemoveDataEvent(string eventName)
        {
            if (dataEventDic.ContainsKey(eventName))
            {
                dataEventDic.Remove(eventName);
            }
        }

        public void DoEvent(string eventName, object data)
        {
            if (dataEventDic.ContainsKey(eventName))
            {
                dataEventDic[eventName].CallEventHanlder(data);
                ClearZeroTallyAction();
            }
            if (nodataEventDic.ContainsKey(eventName))
            {
                nodataEventDic[eventName].CallEventHanlder();
                ClearZeroTallyAction();
            }
        }

        private int doEventCallCont;
        private const int zeroTallyActionClearMax = 10;

        private void ClearZero()
        {
            dataEventDic.Values.ToList().ForEach(c => c.ClearZero());
            nodataEventDic.Values.ToList().ForEach(c => c.ClearZero());
        }

        /// <summary>
        ///  累计DoEvent调用次数，达到阈值则执行一次无效事件处理器清理
        /// </summary>
        private void ClearZeroTallyAction()
        {
            doEventCallCont++;
            if (doEventCallCont <= zeroTallyActionClearMax) return;

            ClearZero();
            doEventCallCont = 0;
        }
    }
}