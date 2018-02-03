using System;
using System.Collections.Generic;
using Iuker.Common.Base.Interfaces;

namespace Iuker.Common.Base
{
    /// <summary>
    /// 使用栈的对象池。
    /// </summary>
    public class ObjectPoolByStack<T> : IObjectPool<T> where T : class
    {
        private readonly Stack<T> m_Stack = new Stack<T>();
        private readonly IObjectFactory<T> m_Factory;
        private readonly Action<T> m_OnCreated;

        public ObjectPoolByStack(IObjectFactory<T> factory, int count, Action<T> onCreated = null)
        {
            m_Factory = factory;
            m_OnCreated = onCreated;

            AppendObj(factory, count, onCreated);
        }

        private void AppendObj(IObjectFactory<T> factory, int count, Action<T> onCreated = null)
        {
            for (var i = 0; i < count; i++)
            {
                var obj = factory.CreateObject();
                m_Stack.Push(obj);
                if (onCreated == null) continue;

                onCreated(obj);
            }
        }

        public T Take()
        {
            if (m_Stack.Count == 0)
            {
                AppendObj(m_Factory, m_Stack.Count, m_OnCreated);
            }

            var element = m_Stack.Pop();
            return element;
        }

        public void Restore(T t)
        {
            m_Stack.Push(t);
        }

        public int UseCount { get; set; }
        public void Clear()
        {
            m_Stack.Clear();
        }
    }
}