namespace Iuker.Common.Base.Enums
{
    /// <summary>
    /// 日志类型
    /// 用于实现自定义的日志调试器
    /// </summary>
#if DEBUG
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 11:54:06")]
    [EmumPurposeDesc("日志类型", "日志类型")]
#endif
    public enum LogType
    {
        /// <summary>
        /// 普通日志
        /// </summary>
        Log,

        /// <summary>
        /// 警告日志
        /// </summary>
        Warning,

        /// <summary>
        /// 错误日志
        /// </summary>
        Error,

        /// <summary>
        /// 异常日志
        /// </summary>
        Exception,

        /// <summary>
        /// 断言日志
        /// </summary>
        Assert,
    }
}
