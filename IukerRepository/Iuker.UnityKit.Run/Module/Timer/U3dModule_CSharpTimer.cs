///***********************************************************************************************
//Author：liuruoyu1981
//CreateDate: 2017/07/26 22:49:11
//Email: 35490136@qq.com
//QQCode: 35490136
//CreateNote: 
//***********************************************************************************************/


///****************************************修改日志***********************************************
//1. 修改日期： 修改人： 修改内容：
//2. 修改日期： 修改人： 修改内容：
//3. 修改日期： 修改人： 修改内容：
//4. 修改日期： 修改人： 修改内容：
//5. 修改日期： 修改人： 修改内容：
//****************************************修改日志***********************************************/

//using System;
//using Iuker.Common;
//using Iuker.Common.Base.Enums;
//using Iuker.Common.Base.Interfaces;
//using Iuker.Common.Module.Timer;
//using Iuker.UnityKit.Run.Base;

//namespace Iuker.UnityKit.Run.Module.Timer
//{
//    /// <summary>
//    /// 使用C#原生计时器实现的计时器模块
//    /// </summary>
//    public class U3dModule_CSharpTimer : AbsU3dModule, ITimerModule
//    {
//        public override string ModuleName => ModuleType.Timer.ToString();

//        public ITimer CreateRepeatTimer(Action<ITimer> onTick, float frequency, float delay = 0,
//            Func<int, bool> isContineFunc = null, object data = null, Action<ITimer> onClose = null)
//        {
//            var timer = sTimerPool.Take();
//            timer.Init(onTick, frequency, delay, isContineFunc, data, onClose);

//            return timer;
//        }

//        public ITimer CreateOnceTimer(Action<ITimer> onTick, float frequency, float delay = 0, Func<int, bool> isContineFunc = null, object data = null,
//            Action<ITimer> onClose = null)
//        {
//            var timer = CreateRepeatTimer(onTick, frequency, delay, isContineFunc, data, onClose);
//            return timer;
//        }

//        public void RecycleTimer(ITimer ryTimer) => sTimerPool.Restore(ryTimer);

//        [ThreadStatic]
//        private static readonly ObjectPool<ITimer> sTimerPool = new ObjectPool<ITimer>(new CSharpTimerFactory(), 20);

//        private class CSharpTimerFactory : IObjectFactory<ITimer>
//        {
//            public ITimer CreateObject()
//            {
//                CSharpTimer timer = new CSharpTimer();
//                return timer;
//            }
//        }
//    }
//}