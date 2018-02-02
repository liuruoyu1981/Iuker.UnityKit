/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 21:36:32
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

namespace Iuker.Common.Module.Timer
{
    /// <summary>
    /// 计时器接口
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// 启动计时器
        /// </summary>
        ITimer Start();

        /// <summary>
        /// 停止计时器运行
        /// </summary>
        void Close();

        /// <summary>
        /// 暂停计时器运行
        /// </summary>
        ITimer Pause();

        /// <summary>
        /// 恢复计时器运行
        /// </summary>
        ITimer Resume();

        /// <summary>
        /// 计时器从启动到现在运行的总时间
        /// </summary>
        double RunTotalTime { get; }

        /// <summary>
        /// 初始化计时器
        /// </summary>
        /// <param name="onTick"></param>
        /// <param name="frequency"></param>
        /// <param name="delay"></param>
        /// <param name="isContineFunc">用于决定计时器是否可以继续运行的检测方法</param>
        /// <param name="data"></param>
        /// <param name="onClose"></param>
        ITimer Init(Action<ITimer> onTick, float frequency, float delay = 0f, Func<int, bool> isContineFunc = null, object data = null, Action<ITimer> onClose = null);

        ITimer OnStart(Action<ITimer> onStart);

        ITimer OnClose(Action<ITimer> onClose);

        ITimer OnPause(Action<ITimer> onPause);

        ITimer SetRunCount(int runCount);

        /// <summary>
        /// 给计时器设定一个友好的名字
        /// </summary>
        /// <param name="friendName"></param>
        /// <returns></returns>
        ITimer SetFriendName(string friendName);

        /// <summary>
        /// 给计时器设定一个key对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ITimer SetKey(object key);

        /// <summary>
        /// 计时器数据
        /// </summary>
        object Data { get; }

        /// <summary>
        /// 将计时器重置为初始值以便重新利用
        /// </summary>
        void Reset();


    }
}
