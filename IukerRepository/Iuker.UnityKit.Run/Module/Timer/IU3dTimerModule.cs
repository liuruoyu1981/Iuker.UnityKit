/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 16:58:37
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
using Iuker.Common.Module.Timer;

namespace Iuker.UnityKit.Run.Module.Timer
{
    /// <summary>
    /// 计时器模块
    /// </summary>
    public interface IU3dTimerModule : IModule
    {
        /// <summary>
        /// 创建一个重复执行的计时器
        /// </summary>
        /// <param name="onTick">周期处理委托</param>
        /// <param name="frequency">计时器频率</param>
        /// <param name="delay">延迟启动时间默认为零</param>
        /// <param name="isContineFunc">计时器继续运行检查器</param>
        /// <param name="data">计时器数据</param>
        /// <param name="onClose">计时器停止处理委托</param>
        /// <returns></returns>
        ITimer CreateRepeatTimer(Action<ITimer> onTick, float frequency, float delay = 0,
            Func<int, bool> isContineFunc = null, object data = null, Action<ITimer> onClose = null);

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
        ITimer CreateOnceTimer(Action<ITimer> onTick, float frequency, float delay = 0,
            Func<int, bool> isContineFunc = null, object data = null, Action<ITimer> onClose = null);

        /// <summary>
        /// 回收一个计时器
        /// </summary>
        /// <param name="ryTimer"></param>
        void RecycleTimer(ITimer ryTimer);

    }
}
