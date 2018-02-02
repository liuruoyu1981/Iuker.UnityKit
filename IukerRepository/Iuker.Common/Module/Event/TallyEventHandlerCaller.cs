using System;
using System.Collections.Generic;
using System.Linq;

namespace Iuker.Common.Event
{
    /// <summary>
    /// 事件处理器调用者
    /// </summary>
    public class TallyEventHandlerCaller : ITallyEventHandlerCaller
    {
        private List<TallyEventHandler> mHandlers = new List<TallyEventHandler>();

        public ITallyEventHandlerCaller AddHandler(Action eventAction, int executeCount = -1)
        {
            var tallyEventHandler = new TallyEventHandler(executeCount, eventAction);
            mHandlers.Add(tallyEventHandler);
            return this;
        }

        public ITallyEventHandlerCaller RemoveHandler(Action evenAction)
        {
            var tallyEventHandler = mHandlers.Find(t => t.EventAction == evenAction);
            if (tallyEventHandler != null) mHandlers.Remove(tallyEventHandler);
            return this;
        }

        public int EventHanlderCount { get { return mHandlers.Count; } }

        public ITallyEventHandlerCaller CallEventHanlder()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < mHandlers.Count; i++)
            {
                var handler = mHandlers[i];
                handler.HandleEvent();
            }
            return this;
        }

        public ITallyEventHandlerCaller ClearZero()
        {
            mHandlers = mHandlers.Where(h => h.ResidueCount != 0).ToList();
            return this;
        }

    }

    #region 泛型

    /// <summary>
    /// 泛型事件处理器调用者
    /// </summary>
    public sealed class TallyEventHandlerCaller<TData> : IEventHandlerCaller<TData>
    {
        private List<TallyEventHandler<TData>> mHandlers = new List<TallyEventHandler<TData>>();

        /// <summary>
        /// 事件处理器数量
        /// </summary>
        public int EventHanlderCount { get { return mHandlers.Count; } }

        public IEventHandlerCaller<TData> AddHandler(Action<TData> eventAction, int executeCount = -1)
        {
            var tallyEventHandler = new TallyEventHandler<TData>(executeCount, eventAction);
            mHandlers.Add(tallyEventHandler);
            return this;
        }

        public IEventHandlerCaller<TData> RemoveHandler(Action<TData> eventAction)
        {
            var tallyEventHandler = mHandlers.Find(t => t.EventAction == eventAction);
            if (tallyEventHandler != null) mHandlers.Remove(tallyEventHandler);
            return this;
        }

        public IEventHandlerCaller<TData> CallEventHanlder(TData data)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < mHandlers.Count; i++)
            {
                var handler = mHandlers[i];
                handler.HandleEvent(data);
            }
            return this;
        }

        public IEventHandlerCaller<TData> ClearZero()
        {
            mHandlers = mHandlers.Where(h => h.ResidueCount != 0).ToList();
            return this;
        }
    }


    #endregion
}