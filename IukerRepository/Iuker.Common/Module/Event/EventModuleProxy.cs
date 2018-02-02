using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Event;

namespace Iuker.Common.Module.Event
{
#if DEBUG
    /// <summary>
    /// 事件模块基类
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170925 14:44:08")]
    [ClassPurposeDesc("事件模块代理", "事件模块代理")]
#endif
    public class EventModuleProxy
    {
        /// <summary>
        /// 事件处理单元字典
        /// </summary>
        private readonly Dictionary<string, EventHandleUnit> mEventHandleUnitDictionary = new Dictionary<string, EventHandleUnit>();

        /// <summary>
        /// 发布事件触发该事件所有相关的事件处理函数
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="data"></param>
        /// <param name="completedCallback"></param>
        public void IssueEvent(string eventCode, Action completedCallback = null, object data = null)
        {
            CacheEventCodeMap(eventCode);

            lock (mEventLock)
            {
                var eventCache = mEventCacheInfoPool.Take();
                eventCache.Init(eventCode, completedCallback, data);
                mEventInfoQueue.Enqueue(eventCache);
            }
        }

        /// <summary>
        /// 线程同步锁对象
        /// </summary>
        private readonly object mEventLock = new object();

        /// <summary>
        /// 以线程安全的方式执行事件分发
        /// </summary>
        public void ExecuteIssuedEvent()
        {
            lock (mEventLock)
            {
                if (mEventInfoQueue.Count == 0) return;

                var eventCount = mEventInfoQueue.Count;

                for (var i = 0; i < eventCount; i++)
                {
                    var eventInfo = mEventInfoQueue.Dequeue();
                    var eventCode = eventInfo.EventCode;
                    var data = eventInfo.Data;
                    var callback = eventInfo.EventAction;
                    var key = mEventCodeInfos[eventCode];

                    if (mEventHandleUnitDictionary.ContainsKey(key))
                    {
                        mEventHandleUnitDictionary[key].DoEvent(eventCode, data);
                    }
                    if (callback != null)
                    {
                        callback();
                    }
                    //  归还缓存事件数据
                    mEventCacheInfoPool.Restore(eventInfo);
                }
            }
        }

        /// <summary>
        /// 内部利用的缓存事件对象池
        /// </summary>
        private readonly ObjectPool<EventCacheInfo> mEventCacheInfoPool = new ObjectPool<EventCacheInfo>(
            new EventCacheInfoFactory(), 100);

        /// <summary>
        /// 缓存事件实例工厂
        /// </summary>
        private class EventCacheInfoFactory : IObjectFactory<EventCacheInfo>
        {
            public EventCacheInfo CreateObject()
            {
                return new EventCacheInfo();
            }
        }

        /// <summary>
        /// 用于事件分发的缓存事件数据列表
        /// </summary>
        private readonly Queue<EventCacheInfo> mEventInfoQueue = new Queue<EventCacheInfo>();
        //        private readonly List<EventCacheInfo> mCacheInfosBak = new List<EventCacheInfo>();

        /// <summary>
        /// 缓存事件码对应的事件类型
        /// 避免拆分字符串开销
        /// </summary>
        private readonly Dictionary<string, string> mEventCodeInfos = new Dictionary<string, string>();

        /// <summary>
        /// 当前事件类型
        /// </summary>
        private string mCurrentEventType;

        public void WatchEvent(string eventCode, Action handler, int executeCount = -1)
        {
            CacheEventCodeMap(eventCode);
            if (mEventHandleUnitDictionary.ContainsKey(mCurrentEventType))
            {
                mEventHandleUnitDictionary[mCurrentEventType].WatchEvent(eventCode, handler, executeCount);
            }
            else
            {
                var newUnit = new EventHandleUnit();
                newUnit.WatchEvent(eventCode, handler, executeCount);
                mEventHandleUnitDictionary.Add(mCurrentEventType, newUnit);
            }
        }

        /// <summary>
        /// 观察一个事件
        /// </summary>
        /// <param name="eventCode">事件名</param>
        /// <param name="handler">事件处理委托</param>
        /// <param name="executeCount">事件处理委托的执行次数</param>
        public void WatchEvent(string eventCode, Action<object> handler, int executeCount = -1)
        {
            CacheEventCodeMap(eventCode);
            if (mEventHandleUnitDictionary.ContainsKey(mCurrentEventType))
            {
                mEventHandleUnitDictionary[mCurrentEventType].WatchEvent(eventCode, handler, executeCount);
            }
            else
            {
                var newUnit = new EventHandleUnit();
                newUnit.WatchEvent(eventCode, handler, executeCount);
                mEventHandleUnitDictionary.Add(mCurrentEventType, newUnit);
            }
        }

        public void RemoveEvent(string eventCode)
        {
            CacheEventCodeMap(eventCode);
            mEventHandleUnitDictionary[mCurrentEventType].RemoveEvent(eventCode);
        }

        /// <summary>
        /// 缓存事件码类型的映射数据
        /// </summary>
        /// <param name="eventCode"></param>
        private void CacheEventCodeMap(string eventCode)
        {
            if (!mEventCodeInfos.ContainsKey(eventCode))
            {
                var evenArray = eventCode.Split('_');
                mCurrentEventType = evenArray[0];
                mEventCodeInfos.Add(eventCode, mCurrentEventType);
            }
            else
            {
                mCurrentEventType = mEventCodeInfos[eventCode];
            }
        }

    }
}
