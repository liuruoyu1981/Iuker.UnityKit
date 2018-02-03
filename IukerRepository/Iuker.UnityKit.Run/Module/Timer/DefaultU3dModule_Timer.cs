/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 21:41:05
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
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Timer;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.Timer
{
    public class DefaultU3dModule_Timer : AbsU3dModule, IU3dTimerModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Timer.ToString();
            }
        }

        /// <summary>
        /// 计时器模块内部维护的计时器对象池
        /// </summary>
        private IObjectPool<ITimer> mTimerPool;

        public override void Init(IFrame frame)
        {
            base.Init(frame);

            mTimerPool = new ObjectPoolByStack<ITimer>(new DefaultU3dTimerFactory(), 50
            );
        }

        #region MyRegion

        ///// <summary>
        ///// 线程安全计时器字典
        ///// </summary>
        //private static readonly Dictionary<object, ITimer> sThreadSafeTimerDictionary = new Dictionary<object, ITimer>();

        //private static readonly DefaultU3dTimerFactory sThreadSafeTimerFactory = new DefaultU3dTimerFactory();

        //private static readonly ObjectPool<ITimer> sThreadSafeTimerPool = new ObjectPool<ITimer>(sThreadSafeTimerFactory, 100);

        //public ITimer GetSafeTimer(object key, Action<ITimer> onTick, float frequency)
        //{
        //    var safeTimer = sThreadSafeTimerPool.Take();
        //    safeTimer.Init(onTick, frequency);
        //    safeTimer.SetKey(key);
        //    if (!sThreadSafeTimerDictionary.ContainsKey(key))
        //    {
        //        sThreadSafeTimerDictionary.Add(key, safeTimer);
        //    }

        //    return safeTimer;
        //}

        #endregion


        /// <summary>
        /// 创建一个指定运行次数的计时器实例
        /// </summary>
        /// <param name="onTick">周期回调委托</param>
        /// <param name="frequency">周期频率</param>
        /// <param name="delay">首次启动延时时间</param>
        /// <param name="isContineFunc">计时器继续运行检查器</param>
        /// <param name="data">计时器附带数据</param>
        /// <param name="onClose">计时器停止时委托</param>
        /// <returns></returns>
        public ITimer CreateRepeatTimer(Action<ITimer> onTick, float frequency, float delay = 0, Func<int, bool> isContineFunc = null, object data = null,
            Action<ITimer> onClose = null)
        {
            var timer = mTimerPool.Take();
            var u3dTimer = (U3dTimer)timer;
            u3dTimer.Init(U3DFrame, this);
            timer.Init(onTick, frequency, delay, isContineFunc, data, onClose);
            return timer;
        }

        /// <summary>
        /// 创建一个只执行一次的计时器
        /// </summary>
        /// <param name="onTick"></param>
        /// <param name="frequency"></param>
        /// <param name="delay"></param>
        /// <param name="isContineFunc"></param>
        /// <param name="data"></param>
        /// <param name="onClose"></param>
        /// <returns></returns>
        public ITimer CreateOnceTimer(Action<ITimer> onTick, float frequency, float delay = 0, Func<int, bool> isContineFunc = null, object data = null,
            Action<ITimer> onClose = null)
        {
            var timer = CreateRepeatTimer(onTick, frequency, delay, isContineFunc, data, onClose).SetRunCount(1);
            return timer;
        }

        /// <summary>
        /// 回收一个计时器
        /// </summary>
        /// <param name="ryTimer"></param>
        public void RecycleTimer(ITimer ryTimer)
        {
            ryTimer.Reset();
            mTimerPool.Restore(ryTimer);
        }

        private class DefaultU3dTimerFactory : IObjectFactory<ITimer>
        {
            public ITimer CreateObject()
            {
                return new U3dTimer();
            }
        }

    }
}
