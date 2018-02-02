/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/26 22:50:41
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
using System.Timers;
using Iuker.Common.Module.Timer;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Timer
{
    public class CSharpTimer : BaseTimer, ITimer
    {
        private System.Timers.Timer mTimer;

        public ITimer Start()
        {
            mStarTime = DateTime.Now;
            mTimer.Enabled = true;
            return this;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            mReduceCount--;
            mRunedCount++;
            var isContinue = mContinueFunc(mReduceCount);
            if (isContinue)
            {
#if UNITY_EDITOR
                Debug.Log("计时器已运行次数：" + mRunedCount);
#endif
                mOnTick(this);
            }
            else
            {
                Close();
            }
        }

        public ITimer Pause()
        {
            throw new NotImplementedException();
        }

        public ITimer SetRunCount(int runCount)
        {
            mReduceCount = runCount + 1;
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

        public void Close()
        {
            Reset();
        }

        public ITimer Init(Action<ITimer> onTick, float frequency, float delay = 0, Func<int, bool> isContineFunc = null, object data = null,
            Action<ITimer> onClose = null)
        {
            mTimer = new System.Timers.Timer(frequency * 1000);
            mTimer.Elapsed += Elapsed;
            mTimer.AutoReset = true;
            mDelayStartTime = delay;
            mContinueFunc = isContineFunc ?? DefaultContinueFunc;
            Data = data;
            mOnClose = onClose;
            mOnTick = onTick;

            return this;
        }

        /// <summary>
        /// 重置计时器以便回收
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            mTimer.Enabled = false;
            mTimer.AutoReset = false;
            mTimer.Elapsed -= Elapsed;
        }

        public ITimer Resume()
        {
            throw new NotImplementedException();
        }
    }
}