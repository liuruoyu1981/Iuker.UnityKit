/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/18/2017 15:02
Email: liuruoyu1981@gmail.com
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

namespace Iuker.UnityKit.Editor.Console.Core.UnityLoggerApi
{
    public static class UnityLoggerServer
    {
        public static void Clear() => LogEntries.Clear();
        public static IukerLog GetCompleteLog(int row) => LogEntries.GetCompleteLog(row);

        public static IukerLog GetSimpleLog(int row) => LogEntries.GetSimpleLog(row);

        public static int GetLogCount(int row) => LogEntries.GetEntryCount(row);

        /// <summary>
        /// 通过反射调用原生控制台的StartGettingEntries方法来获取当前日志列表并返回日志数量
        /// </summary>
        /// <returns></returns>
        public static int StartGettingLogs() => LogEntries.StartGettingEntries();

        public static void StopGettingsLogs() => LogEntries.EndGettingEntries();


        public static int GetCount() => LogEntries.GetCount();

        /// <summary>
        /// 通过反射获取普通、警告及错误的日志数量
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="warning"></param>
        /// <param name="error"></param>
        public static void GetCount(ref int normal, ref int warning, ref int error) => LogEntries.GetCountsByType(
            ref error, ref warning, ref normal);

        public static bool IsDebugError(int mode) => ConsoleWindow.IsDebugError(mode);

        public static bool HasFlag(ConsoleWindowFlag flag) => ConsoleWindow.HasFlag(flag);

        /// <summary>
        /// 通过反射获取的SetFalg方法设置Unity控制台的某个标志位
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="active"></param>
        public static void SetFlag(ConsoleWindowFlag flag, bool active) => ConsoleWindow.SetFlag(flag, active);

        public static bool HasMode(int mode, ConsoleWindowMode modeToCheck) => ConsoleWindow.HasMode(mode, modeToCheck);

    }
}