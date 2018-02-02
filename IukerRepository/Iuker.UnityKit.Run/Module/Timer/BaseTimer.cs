using System;
using Iuker.Common.Module.Timer;

namespace Iuker.UnityKit.Run.Module.Timer
{
    /// <summary>
    /// 基础计时器提供计时器的基础数据结构
    /// </summary>
    public class BaseTimer
    {
        public object Data { get; protected set; }
        protected Action<ITimer> mOnTick;
        protected Action<ITimer> mOnStart;
        protected Action<ITimer> mOnPause;
        protected Action<ITimer> mOnClose;
        protected DateTime mStarTime;
        protected DateTime mCloseTime;
        protected bool runToggle;
        protected int TickCount { get; set; }

        protected double CurrentDelayTime { get; set; }

        protected float Frequency { get; set; }

        public string FriendName { get; protected set; }

        /// <summary>
        /// 计时器key对象
        /// </summary>
        protected object Key { get; set; }

        /// <summary>
        /// 默认的计时器可否继续运行判断检测器
        /// 剩余次数大于或者小于零则可继续运行
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        protected bool DefaultContinueFunc(int count)
        {
            var result = count > 0 || count < 0;
            return result;
        }

        /// <summary>
        /// 剩余可运行次数
        /// 默认规则为大于0或小于0则继续运行，等于0则停止运行
        /// </summary>
        protected int mReduceCount = -1;

        /// <summary>
        /// 计时器已运行次数
        /// </summary>
        protected int mRunedCount;

        /// <summary>
        /// 计时器继续运行检查器
        /// </summary>
        protected Func<int, bool> mContinueFunc;

        /// <summary>
        /// 延迟启动时间
        /// </summary>
        public float mDelayStartTime { get; protected set; }

        /// <summary>
        /// 计时器从启动到现在运行的总时间（秒数）
        /// </summary>
        public double RunTotalTime
        {
            get
            {
                return (DateTime.Now - mStarTime).TotalSeconds;
            }
        }

        /// <summary>
        /// 计时器当前计时周期内已运行时间
        /// </summary>
        protected double mTickRunTime;

        /// <summary>
        /// 重置计时器状态以便重复利用
        /// </summary>
        public virtual void Reset()
        {
            mOnStart = null;
            mOnClose = null;
            mOnPause = null;
            mOnTick = null;
            Data = null;
            mContinueFunc = null;
            Key = null;
            mDelayStartTime = 0f;
            mReduceCount = -1;
            mRunedCount = 0;
            Frequency = 0;
            FriendName = null;
            TickCount = 0;
        }





    }
}