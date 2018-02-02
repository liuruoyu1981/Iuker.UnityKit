/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/01 11:10:29
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

namespace Iuker.UnityKit.Run.Tween
{
    /// <summary>
    /// 动画对象
    /// </summary>
    public interface ITween
    {
        /// <summary>
        /// 动画的标识对象，可以为空
        /// </summary>
        object Key { get; }

        /// <summary>
        /// 获得动画当前的运行状态
        /// </summary>
        TweenState State { get; }

        /// <summary>
        /// 暂停动画
        /// </summary>
        void Pause();

        /// <summary>
        /// 恢复运行暂停的动画
        /// </summary>
        void Resume();

        /// <summary>
        /// 反向播放动画
        /// </summary>
        void PlayReverse();

        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="stopBehavior">用于停止动画的停止行为参数</param>
        void Stop(TweenStopBehavior stopBehavior = TweenStopBehavior.Complete);

        /// <summary>
        /// 更新动画
        /// </summary>
        /// <param name="elpsedTime">更新动画的时间</param>
        /// <returns></returns>
        bool Update(float elpsedTime);

        /// <summary>
        /// 重置动画对象
        /// 将动画对象的局部变量恢复初始值
        /// </summary>
        void Reset();
    }

    /// <summary>
    /// 泛型动画接口
    /// 该接口继承ITween接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITween<T> : ITween where T : struct
    {
        /// <summary>
        /// 获取动画的当前值
        /// </summary>
        T CurrentValue { get; }

        /// <summary>
        /// 获取动画的当前进度
        /// </summary>
        float CurrentProgress { get; }

        /// <summary>
        /// 当前动画已经执行的次数
        /// </summary>
        int ExecuteCount { get; }

        /// <summary>
        /// 启动一个动画
        /// </summary>
        /// <param name="start">开始值</param>
        /// <param name="end">结束值</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="scaleFunc"></param>
        /// <param name="progress">进度处理委托</param>
        /// <param name="completion">完成处理委托</param>
        void Init(T start, T end, float duration, Func<float, float> scaleFunc, Action<ITween<T>> progress,
            Action<ITween<T>> completion);

        /// <summary>
        /// 设置动画完成时委托
        /// </summary>
        /// <param name="completion"></param>
        void OnCompleted(Action<ITween<T>> completion);

        /// <summary>
        /// 设置动画暂停时委托
        /// </summary>
        /// <param name="puaseDelegate"></param>
        ITween<T> OnPuase(Action<ITween<T>> puaseDelegate);

        /// <summary>
        /// 设置动画开始时委托
        /// </summary>
        /// <param name="startDelegate"></param>
        ITween<T> OnStart(Action<ITween<T>> startDelegate);

        /// <summary>
        /// 设置动画恢复时委托
        /// </summary>
        /// <param name="resumeDelg"></param>
        /// <returns></returns>
        ITween<T> OnResume(Action<ITween<T>> resumeDelg);

        /// <summary>
        /// 设置曲线类型
        /// </summary>
        /// <param name="esType"></param>
        /// <returns></returns>
        ITween<T> SetEaseType(EaseType esType);

        /// <summary>
        /// 设置动画的循环类型
        /// </summary>
        /// <param name="loopType"></param>
        /// <returns></returns>
        ITween<T> SetLoopType(TweenLoopType loopType);

        /// <summary>
        /// 设置循环次数
        /// 只有当循环类型是Restart或者Yoyo时有效
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        ITween<T> SetLoopCount(int count);

        /// <summary>
        /// 设置一个检查器
        /// 检查器会在每帧进行调用
        /// </summary>
        /// <param name="ckCondition"></param>
        /// <param name="checkeOk"></param>
        /// <param name="count">检查器委托的可执行次数，默认为一次</param>
        /// <returns></returns>
        ITween<T> SetChecker(Func<ITween<T>, bool> ckCondition, Action<ITween<T>> checkeOk, int count = 1);

        /// <summary>
        /// 设置动画的周期委托相关数据
        /// </summary>
        /// <param name="tickCount">动画周期次数</param>
        /// <param name="tickAction">周期委托</param>
        /// <returns></returns>
        ITween<T> SetTick(int tickCount, Action<ITween<T>> tickAction);


    }
}
