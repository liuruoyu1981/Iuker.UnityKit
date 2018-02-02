///***********************************************************************************************
//Author：liuruoyu1981
//CreateDate: 2/12/2017 08:43:07
//Email: 35490136@qq.com
//QQCode: 35490136
//CreateNote: 通用调用工具
//***********************************************************************************************/


///****************************************修改日志***********************************************
//1. 修改日期： 修改人： 修改内容：
//2. 修改日期： 修改人： 修改内容：
//3. 修改日期： 修改人： 修改内容：
//4. 修改日期： 修改人： 修改内容：
//5. 修改日期： 修改人： 修改内容：
//****************************************修改日志***********************************************/

//using System;
//using Iuker.Common.Module.Debugger;

//namespace Iuker.Common
//{
//    /// <summary>
//    /// 通用调用工具
//    /// </summary>
//    public class Debuger : ILoger
//    {
//        public static bool Enable { get; private set; }
//        public static Action<string> LogHander { get; private set; }
//        public static Action<string> WarningHander { get; private set; }
//        public static Action<string> ErrorHander { get; private set; }
//        public static Action<Exception> ExceptionHander { get; private set; }

//        public static void Init(Action<string> logHander, Action<string> warningHander,
//            Action<string> errorHander, Action<Exception> exceptionHander = null)
//        {
//            Enable = true;
//            LogHander = logHander;
//            WarningHander = warningHander;
//            ErrorHander = errorHander;
//            ExceptionHander = exceptionHander;
//        }

//        private static readonly Debuger instance = new Debuger();

//        public static void Log(string message)
//        {
//            instance.Info(message);
//        }

//        public static void LogWarning(string message)
//        {
//            instance.Warning(message);
//        }

//        public static void LogError(string message)
//        {
//            instance.Error(message);
//        }

//        public static void LogException(string message)
//        {
//            instance.Exception(message);
//        }

//        private bool mIsInited;
//        public string Owner { get; private set; }
//        public DateTime CreatDate { get; private set; }
//        public void Info(string message) => Enable.TrueDo(() => LogHander?.Invoke("C#: " + message));

//        public void Warning(string message) => Enable.TrueDo(() => WarningHander?.Invoke("C#: " + message));

//        public void Error(string message) => Enable.TrueDo(() => ErrorHander?.Invoke("C#: " + message));

//        public void Exception(string exception) => Enable.TrueDo(() => ExceptionHander?.Invoke(new Exception($"C#: {exception}")));
//        public ILoger Init(string creater)
//        {
//            if (!mIsInited)
//            {
//                Owner = creater;
//                CreatDate = DateTime.Now;
//                mIsInited = true;
//            }

//            return this;
//        }
//    }
//}
