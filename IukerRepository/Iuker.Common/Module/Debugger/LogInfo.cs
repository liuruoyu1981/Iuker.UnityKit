using System;
using Iuker.Common.Base;
using Iuker.Common.Base.Enums;

namespace Iuker.Common.Module.Debugger
{
#if DEBUG
    /// <summary>
    ///日志信息
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 19:44:50")]
    [ClassPurposeDesc("日志信息", "日志信息")]
#endif
    public class Log
    {
        public Log(string logMessage, LogType logType, string stackTrace)
        {
            CreateTime = DateTime.Now;
            LogMessage = logMessage;
            LogType = logType;
            StackTrace = stackTrace;
        }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogMessage { get; private set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; private set; }

        /// <summary>
        /// 日志生成时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 日志栈跟踪信息
        /// </summary>
        public string StackTrace { get; private set; }
    }
}
