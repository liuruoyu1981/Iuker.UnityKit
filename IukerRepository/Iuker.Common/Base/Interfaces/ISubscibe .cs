using System;
using System.Collections.Generic;

namespace Iuker.Common.Base.Interfaces
{
#if DEBUG
    /// <summary>
    /// 可订阅对象
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170922 11:51:56")]
    [InterfacePurposeDesc("可订阅对象", "可订阅对象")]
#endif
    public interface ISubscibe<T> : IDisposable
    {
        /// <summary>
        /// 订阅对象出生
        /// </summary>
        /// <param name="birthHandler"></param>
        /// <returns></returns>
        ISubscibe<T> SubscibeBirth(Action<ISubscibe<T>> birthHandler);

        /// <summary>
        /// 订阅对象激活
        /// </summary>
        /// <param name="activeHandler"></param>
        /// <returns></returns>
        ISubscibe<T> SubscibeActive(Action<ISubscibe<T>> activeHandler);

        /// <summary>
        /// 订阅对象闲置
        /// </summary>
        /// <param name="unActiveHandler"></param>
        /// <returns></returns>
        ISubscibe<T> SubscibeUnAcive(Action<ISubscibe<T>> unActiveHandler);

        /// <summary>
        /// 订阅对象死亡
        /// </summary>
        /// <param name="dieHandler"></param>
        /// <returns></returns>
        ISubscibe<T> SubscibeDie(Action<ISubscibe<T>> dieHandler);


    }

    public abstract class AbsSubscibe<T> : ISubscibe<T>
    {
        public string Name { get; private set; }

        #region 订阅事件处理集合

        private List<Action<ISubscibe<T>>> mBirthHandlers = new List<Action<ISubscibe<T>>>();
        private List<Action<ISubscibe<T>>> mActiveHandlers = new List<Action<ISubscibe<T>>>();
        private List<Action<ISubscibe<T>>> mUnActiveHandlers = new List<Action<ISubscibe<T>>>();
        private List<Action<ISubscibe<T>>> mDieHandlers = new List<Action<ISubscibe<T>>>();

        #endregion

        public virtual ISubscibe<T> SubscibeBirth(Action<ISubscibe<T>> birthHandler)
        {
            return this;
        }

        public ISubscibe<T> SubscibeActive(Action<ISubscibe<T>> activeHandler)
        {
            return this;
        }

        public ISubscibe<T> SubscibeUnAcive(Action<ISubscibe<T>> unActiveHandler)
        {
            return this;
        }

        public ISubscibe<T> SubscibeDie(Action<ISubscibe<T>> dieHandler)
        {
            return this;
        }

        public abstract void Dispose();
    }








}
