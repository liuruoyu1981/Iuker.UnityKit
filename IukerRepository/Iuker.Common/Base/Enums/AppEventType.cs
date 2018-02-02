namespace Iuker.Common.Base.Enums
{
    /// <summary>
    /// App事件类型
    /// 1. App生命周期循环事件，FixedUpdate Update LateUpdate OnGUI。
    /// 2. App生命周期基础事件，AppStart,AppQuit,AppPause。
    /// </summary>
#if DEBUG
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 11:10:47")]
    [EmumPurposeDesc("App事件类型，指各种循环模式及App生命周期事件。", "")]
#endif
    public enum AppEventType
    {
        /// <summary>
        /// 固定时间循环
        /// </summary>
        FixedUpdate,

        /// <summary>
        /// 帧循环
        /// </summary>
        Update,

        /// <summary>
        /// 延迟循环
        /// </summary>
        LateUpdate,

        /// <summary>
        /// UI绘制循环
        /// </summary>
        OnGUI,

        /// <summary>
        /// 应用开始
        /// </summary>
        AppStart,

        /// <summary>
        /// 应用退出
        /// </summary>
        AppQuit,

        /// <summary>
        /// 应用暂停
        /// </summary>
        AppPause,

        /// <summary>
        /// 失去或获得焦点
        /// </summary>
        AppFocus,
    }
}
