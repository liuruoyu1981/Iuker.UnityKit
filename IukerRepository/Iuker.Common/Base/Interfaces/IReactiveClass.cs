using System;
using Iuker.Common.Base.Interfaces;

namespace Iuker.Common.Base
{
    public interface IReactiveClass<T>
    {
        /// <summary>
        /// 初始化一个主动数据实例
        /// </summary>
        /// <param name="data"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        IReactiveClass<T> Init(T data, IFrame frame);


        IReactiveClass<T> WatchData(Action<T> onDataChanged);


        IReactiveClass<T> UpdateData(T newData);


        /// <summary>
        /// 内部数据
        /// </summary>
        T Data { get; }
    }
}