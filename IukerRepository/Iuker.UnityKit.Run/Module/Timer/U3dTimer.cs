/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 21:36:16
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
    （1） 2017/07/26 20：51
            <1> 增加RunTimeAtStop属性
            <2> 将Stop方法重构为Close，在Close方法中回收计时器实例自身
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using System;
using Iuker.Common;
using Iuker.Common.Base.Enums;
using Iuker.Common.Module.Timer;
using Iuker.UnityKit.Run.Base;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Timer
{
    /// <summary>
    /// U3d计时器
    /// </summary>
    public class U3dTimer : BaseTimer, ITimer
    {
        public ITimer Init(Action<ITimer> onTick, float frequency, float delay = 0, Func<int, bool> isContineFunc = null, object data = null,
            Action<ITimer> onClose = null)
        {
            Frequency = frequency;
            mDelayStartTime = delay;
            Data = data;
            mContinueFunc = isContineFunc ?? DefaultContinueFunc;
            mOnClose = onClose;
            mOnTick = onTick;

            return this;
        }

        public ITimer SetRunCount(int runCount)
        {
            mReduceCount = runCount;
            return this;
        }

        public ITimer SetFriendName(string friendName)
        {
            FriendName = friendName;
            return this;
        }

        public ITimer SetKey(object key)
        {
            Key = key;
            return this;
        }

        public ITimer OnStart(Action<ITimer> onStart)
        {
            mOnStart = onStart;
            return this;
        }

        public ITimer OnClose(Action<ITimer> onClose)
        {
            mOnClose = onClose;
            return this;
        }

        public ITimer OnPause(Action<ITimer> onPause)
        {
            mOnPause = onPause;
            return this;
        }

        private IU3dFrame mFrame;
        private IU3dTimerModule mTimerModule;

        public ITimer Init(IU3dFrame frame, IU3dTimerModule timerModule)
        {
            mFrame = frame;
            mTimerModule = timerModule;
            return this;
        }

        /// <summary>
        /// 延时启动计时器
        /// </summary>
        private void DelayStart()
        {
            CurrentDelayTime = CurrentDelayTime + Time.deltaTime;

            if (CurrentDelayTime >= mDelayStartTime)
            {
                CurrentDelayTime = 0f;
                mFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), UpdateTimer);
                mFrame.EventModule.RemoveAppEvent(AppEventType.Update.ToString(), DelayStart);
                mFrame.EventModule.WatchU3dAppEvent(AppEventType.LateUpdate.ToString(), LateUpdate);
            }
        }

        /// <summary>
        /// 计时器帧循环
        /// </summary>
        private void UpdateTimer()
        {
            if (!runToggle) return;
            mTickRunTime += Time.deltaTime;

            // 如果目前运行时长大于等于计时器周期频率时长
            if (!(mTickRunTime >= Frequency)) return;

            TickCount++; // 统计总的运行次数
            mReduceCount--;
            mTickRunTime = 0f;
            if (mOnTick != null)
            {
                mOnTick(this);
            }
        }

        private void LateUpdate()
        {
            if (mContinueFunc != null)
            {
                mContinueFunc(mReduceCount).FalseDo(Close);
            }
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        public ITimer Start()
        {
            runToggle = true;
            mStarTime = DateTime.Now;
            mFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), DelayStart);

            return this;
        }

        /// <summary>
        /// 恢复计时器运行
        /// </summary>
        public ITimer Resume()
        {
            runToggle = true;
            return this;
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public ITimer Pause()
        {
            runToggle = false;
            if (mOnPause != null)
            {
                mOnPause(this);
            }
            return this;
        }

        /// <summary>
        /// 关闭计时器并停止运行
        /// </summary>
        public void Close()
        {
            if (!runToggle) return; //  已经关闭了则直接退出
#if UNITY_EDITOR
            var runAtStopTime = (DateTime.Now - mStarTime).TotalSeconds;
            Debug.Log(string.Format("计时器{0}总共运行{1}秒，已停止！", FriendName, runAtStopTime));
#endif
            runToggle = false;
            mCloseTime = DateTime.Now;
            mFrame.EventModule.RemoveAppEvent(AppEventType.Update.ToString(), UpdateTimer);
            mFrame.EventModule.RemoveAppEvent(AppEventType.LateUpdate.ToString(), LateUpdate);
            if (mOnClose != null)
            {
                mOnClose(this);
            }
            mTimerModule.RecycleTimer(this);
        }
    }
}
