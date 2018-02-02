/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 下午1:56:21
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
using System.Collections.Generic;
using Iuker.Common.Base.Interfaces;

namespace Iuker.Common.Base
{
    /// <summary>
    /// 响应式引用数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReactiveClass<T> : IReactiveClass<T>
    {
        private IFrame mFrame;

        public ReactiveClass()
        {
            OnDataChanged = new List<Action<T>>();
        }

        public IReactiveClass<T> UpdateData(T newData)
        {
            if (!Data.Equals(newData))
            {
                Data = newData;
                OnDataChanged.ForEach(d => d(Data));
            }

            return this;
        }

        /// <summary>
        /// 主动数据对象内部数据
        /// </summary>
        public T Data { get; protected set; }

        /// <summary>
        /// 处理数据变化的委托
        /// </summary>
        public List<Action<T>> OnDataChanged { get; protected set; }

        /// <summary>
        /// 初始化一个主动数据实例
        /// </summary>
        /// <param name="data"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public IReactiveClass<T> Init(T data, IFrame frame)
        {
            Data = data;
            mFrame = frame;
            return this;
        }

        public IReactiveClass<T> WatchData(Action<T> onDataChanged)
        {
            if (onDataChanged != null)
            {
                OnDataChanged.Add(onDataChanged);
            }

            return this;
        }

    }
}
