namespace Iuker.Common.Event
{
    public class EventCodeInfo
    {
        /// <summary>
        /// 事件的类型
        /// 业务逻辑上区分
        /// </summary>
        public string EventType { get; private set; }

        /// <summary>
        /// 事件名
        /// 唯一
        /// </summary>
        public string EventName { get; private set; }

        public EventCodeInfo(string type, string name)
        {
            EventType = type;
            EventName = name;
        }
    }
}