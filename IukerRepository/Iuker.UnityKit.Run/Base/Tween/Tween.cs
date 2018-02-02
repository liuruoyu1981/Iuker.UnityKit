/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 11:09:50
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
using UnityEngine;

namespace Iuker.UnityKit.Run.Tween
{
    /// <summary>
    /// 动画对象
    /// </summary>
    public class Tween<T> : ITween<T> where T : struct
    {
        /// <summary>
        /// 动画的插值计算委托
        /// </summary>
        private readonly Func<ITween<T>, T, T, float, T> lerpFunc;
        private float mCurrentTime;
        private float mDuration;
        private Func<float, float> scaleFunc;
        private Action<ITween<T>> progressCallback;
        private Action<ITween<T>> completeDelegate;
        private Action<ITween<T>> pauseDelegate;
        private Action<ITween<T>> startDelegate;
        private Action<ITween<T>> resumeDelegate;
        private Action<ITween<T>> checkeOkDelegate;
        private Func<ITween<T>, bool> checkCondition;

        private TweenState state;
        private TweenLoopType _loopTypeType = TweenLoopType.Once;

        private T mStart;
        private T mEnd;
        private T value;

        /// <summary>
        /// 对人友好的动画名
        /// </summary>
        public string FriendNme { get; set; }

        /// <summary>
        /// 剩余执行次数
        /// </summary>
        private int residueCount = -1;

        /// <summary>
        /// 当前已执行次数
        /// </summary>
        private int executeCount;

        /// <summary>
        /// 动画周期已执行次数
        /// </summary>
        private int mTickExexuteCount;

        /// <summary>
        /// 动画周期执行次数
        /// </summary>
        private int mTickCount = -9999;

        /// <summary>
        /// 周期动画回调委托
        /// </summary>
        private Action<ITween<T>> mTickAction;

        /// <summary>
        /// 检查器委托可执行次数
        /// </summary>
        private int checkOkExecuteCount;

        public object Key { get; set; }

        public float MCurrentTime { get { return mCurrentTime; } }

        public float MDuration { get { return mDuration; } }

        public TweenState State { get { return state; } }

        public T MStartValue { get { return mStart; } }

        public T MEndValue { get { return mEnd; } }

        public T CurrentValue { get { return value; } }


        public Renderer Renderer;

        /// <summary>
        /// 获取动画当前的执行进度（0 - 1）
        /// </summary>
        public float CurrentProgress { get; private set; }

        public int ExecuteCount { get { return executeCount; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lerpFunc"></param>
        protected Tween(Func<ITween<T>, T, T, float, T> lerpFunc)
        {
            this.lerpFunc = lerpFunc;
            state = TweenState.Stopped;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <param name="scaleFunc"></param>
        /// <param name="progress"></param>
        /// <param name="completion"></param>
        public void Init(T start, T end, float duration, Func<float, float> scaleFunc, Action<ITween<T>> progress,
            Action<ITween<T>> completion)
        {
            if (duration <= 0)
            {
                throw new ArgumentException("duration must be greater than 0");
            }
            // ReSharper disable once JoinNullCheckWithUsage
            if (scaleFunc == null)
            {
                throw new ArgumentException("scaleFunc");
            }

            mCurrentTime = 0;
            this.mDuration = duration;
            this.scaleFunc = scaleFunc;
            progressCallback = progress;
            completeDelegate = completion;
            state = TweenState.Start;

            this.mStart = start;
            this.mEnd = end;

            //UpdateValue();
        }


        /// <summary>
        /// 重置动画对象
        /// 该方法负责清理动画实例的状态使动画对象恢复初始值
        /// </summary>
        /// <returns></returns>
        public void Reset()
        {
            FriendNme = null;
            residueCount = -1;
            executeCount = 0;
            mTickExexuteCount = 0;
            mTickCount = -9999;
            mTickAction = null;
            checkOkExecuteCount = 0;
        }

        public void Pause()
        {
            if (state != TweenState.Running) return;

            state = TweenState.Paused;
            if (pauseDelegate != null)
            {
                pauseDelegate(this);
            }
        }

        /// <summary>
        /// 恢复一个动画
        /// 如果动画处于Stop状态则无法恢复
        /// </summary>
        public void Resume()
        {
            if (state == TweenState.Paused)
            {
                state = TweenState.Running;
                if (resumeDelegate != null)
                {
                    resumeDelegate(this);
                }
            }
        }

        /// <summary>
        /// 反向播放
        /// </summary>
        public void PlayReverse()
        {
            Pause();
            var temp = value;
            var newDuration = CurrentProgress * mDuration;   //  计算出新的反向动画的过渡时间
            mDuration = newDuration;
            mEnd = mStart;
            mStart = temp;
            mCurrentTime = 0f;
            CurrentProgress = 0f;
            Resume();
        }

        public void Stop(TweenStopBehavior stopBehavior)
        {
            if (state != TweenState.Stopped)
            {
                state = TweenState.Stopped;
                if (stopBehavior == TweenStopBehavior.Complete)
                {
                    mCurrentTime = mDuration;
                    UpdateValue();
                    if (completeDelegate != null)
                    {
                        completeDelegate(this);
                    }
                    completeDelegate = null;
                }
            }
        }

        /// <summary>
        /// 更新动画
        /// 如果返回真则说明动画已经停止
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public bool Update(float elapsedTime)
        {
            if (state == TweenState.Start)
            {
                if (startDelegate != null)
                {
                    startDelegate(this);
                }
                state = TweenState.Running;
            }

            if (checkCondition != null && checkCondition(this) && checkOkExecuteCount > 0 || checkOkExecuteCount < 0)
            {
                checkOkExecuteCount--;
                if (checkeOkDelegate != null)
                {
                    checkeOkDelegate(this);
                }
            }

            if (state == TweenState.Running)
            {
                mCurrentTime += elapsedTime;
                if (mCurrentTime >= mDuration)    //  执行完一次动画
                {
                    residueCount--;                     //  剩余执行次数减一
                    executeCount++;                 //   总执行次数加一
                    mTickExexuteCount++;    //  周期执行次数加一

                    // 判断动画周期执行相关
                    if (mTickExexuteCount == mTickCount)
                    {
                        if (mTickAction != null)
                        {
                            mTickAction(this);
                        }
                        mTickExexuteCount = 0;
                    }

                    switch (_loopTypeType)
                    {
                        case TweenLoopType.Restart:
                            if (residueCount == 0) Stop(TweenStopBehavior.Complete);
                            Restart();
                            break;
                        case TweenLoopType.Yoyo:
                            if (residueCount == 0) Stop(TweenStopBehavior.Complete);
                            Yoyo();
                            break;
                        case TweenLoopType.Once:
                            Stop(TweenStopBehavior.Complete);
                            return true;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                UpdateValue();
                return false;
            }
            var result = state == TweenState.Stopped;
            return result;
        }

        private void Yoyo()
        {
            var temp = mEnd;
            mEnd = mStart;
            mStart = temp;
            mCurrentTime = 0f;
            CurrentProgress = 0f;
        }

        private void Restart()
        {
            value = mStart;
            mCurrentTime = 0f;
            CurrentProgress = 0f;
        }

        private void UpdateValue()
        {
#if UNITY || UNITY_5_3_OR_NEWER

            if (Renderer == null || Renderer.isVisible)
            {

#endif

                CurrentProgress = scaleFunc(mCurrentTime / mDuration);
                value = lerpFunc(this, mStart, mEnd, CurrentProgress);
                if (progressCallback != null)
                {
                    progressCallback(this);
                }

#if UNITY || UNITY_5_3_OR_NEWER

            }

#endif
        }

        public void OnCompleted(Action<ITween<T>> completedDelg)
        {
            completeDelegate = completedDelg;
        }

        public ITween<T> OnStart(Action<ITween<T>> startDelg)
        {
            startDelegate = startDelg;
            return this;
        }

        public ITween<T> OnPuase(Action<ITween<T>> pauseDelg)
        {
            pauseDelegate = pauseDelg;
            return this;
        }

        public ITween<T> OnResume(Action<ITween<T>> resumeDelg)
        {
            resumeDelegate = resumeDelg;
            return this;
        }

        public ITween<T> SetEaseType(EaseType esType)
        {
            scaleFunc = TweenManager.scaleFuncDictionary[esType];
            return this;
        }

        public ITween<T> SetLoopType(TweenLoopType loopType)
        {
            _loopTypeType = loopType;
            return this;
        }

        /// <summary>
        /// 设置动画循环次数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public ITween<T> SetLoopCount(int count)
        {
            residueCount = count;
            return this;
        }

        /// <summary>
        /// 设置动画的周期委托相关数据
        /// </summary>
        /// <param name="tickCount">动画周期次数</param>
        /// <param name="tickAction">周期委托</param>
        /// <returns></returns>
        public ITween<T> SetTick(int tickCount, Action<ITween<T>> tickAction)
        {
            mTickCount = tickCount;
            mTickAction = tickAction;
            return this;
        }

        public ITween<T> SetChecker(Func<ITween<T>, bool> ckCondition, Action<ITween<T>> checkeOk, int count = 1)
        {
            checkCondition = ckCondition;
            checkeOkDelegate = checkeOk;
            checkOkExecuteCount = count;
            return this;
        }


    }
}
