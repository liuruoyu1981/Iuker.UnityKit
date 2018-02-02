using System;
using Iuker.Common.Base;

namespace Iuker.Common.Module.Debugger
{
#if DEBUG
    /// <summary>
    ///日志输出器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 19:51:29")]
    [InterfacePurposeDesc("日志输出器", "日志输出器")]
#endif
    public interface ILoger
    {
        /// <summary>
        /// 这个日志输出器的拥有者
        /// </summary>
        string Owner { get; }

        /// <summary>
        /// 创建日期
        /// </summary>
        DateTime CreatDate { get; }

        /// <summary>
        /// 输出一个普通日志
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// 输出一个警告日志
        /// </summary>
        /// <param name="message"></param>
        void Warning(string message);

        /// <summary>
        /// 输出一个错误日志
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// 输出一个异常日志
        /// </summary>
        /// <param name="exception"></param>
        void Exception(string exception);

        /// <summary>
        /// 初始化日志输出器
        /// </summary>
        /// <param name="creater"></param>
        /// <returns></returns>
        ILoger Init(string creater);
    }
}
