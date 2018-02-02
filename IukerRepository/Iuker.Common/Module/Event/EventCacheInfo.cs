using System;

namespace Iuker.Common.Event
{
    /// <summary>
    /// 用于缓存稍后进行分发处理的事件数据
    /// </summary>
    public class EventCacheInfo
    {
        public string EventCode { get; private set; }

        public Action EventAction { get; private set; }

        public object Data { get; private set; }

        public EventCacheInfo Init(string eventCode, Action action, object data = null)
        {
            EventCode = eventCode;
            EventAction = action;
            Data = data;

            return this;
        }
    }
}