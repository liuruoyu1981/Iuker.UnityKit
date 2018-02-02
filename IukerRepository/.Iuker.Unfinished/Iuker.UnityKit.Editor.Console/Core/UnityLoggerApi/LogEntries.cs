/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/16/2017 19:24
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

using System;
using System.Reflection;

namespace Iuker.UnityKit.Editor.Console.Core.UnityLoggerApi
{
    public class LogEntries
    {
        private static Type LogEntriesType
        {
            get
            {
                if (!CachedReflection.Has("LogEntries"))
                {
                    CachedReflection.Cache("LogEntries", Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll"));
                }
                return CachedReflection.Get<Type>("LogEntries");
            }
        }

        private static BindingFlags DefaultFlags => BindingFlags.Static | BindingFlags.Public;

        /// <summary>
        /// 获取Unity下LogEntries类的某个方法信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static MethodInfo GetMethod(string key)
        {
            if (!CachedReflection.Has(key))
            {
                CachedReflection.Cache(key, LogEntriesType.GetMethod(key, DefaultFlags));
            }
            return CachedReflection.Get<MethodInfo>(key);
        }

        /// <summary>
        /// 清理日志
        /// </summary>
        public static void Clear() => GetMethod("Clear").Invoke(null, null);

        private static bool GetEntryInternal(int row, object output)
        {
            var method = GetMethod("GetEntryInternal");
            return (bool)method.Invoke(null, new[] { row, output });
        }

        public static IukerLog GetCompleteLog(int row)
        {
            var emptyLog = LogEntry.GetNewEmptyObject;
            GetEntryInternal(row, emptyLog);
            return LogEntry.GetBluLog(emptyLog);
        }

        private static void GetFirstTwoLinesEntryTextAndModeInternal(int row, ref int mode, ref string text)
        {
            var method = GetMethod("GetFirstTwoLinesEntryTextAndModeInternal");

            int m = 0;
            string t = "";
            var parameters = new object[] { row, m, t };
            method.Invoke(null, parameters);

            mode = (int)parameters[1];
            text = (string)parameters[2];
        }

        public static IukerLog GetSimpleLog(int row)
        {
            int mode = 0;
            string message = "";
            GetFirstTwoLinesEntryTextAndModeInternal(row, ref mode, ref message);

            IukerLog log = new IukerLog();
            log.SetMessage(message);
            log.SetMode(mode);
            return log;
        }

        public static int GetEntryCount(int row)
        {
            var method = GetMethod("GetEntryCount");
            return (int)method.Invoke(null, new object[] { row });
        }

        public static int GetCount() => (int)GetMethod("GetCount").Invoke(null, null);

        /// <summary>
        /// 通过反射获取普通、警告及错误的日志数量
        /// </summary>
        /// <param name="error"></param>
        /// <param name="warning"></param>
        /// <param name="normal"></param>
        public static void GetCountsByType(ref int error, ref int warning, ref int normal)
        {
            var method = GetMethod("GetCountsByType");

            int temError = 0, tempWarning = 0, tempNormal = 0;
            var parameters = new object[] { temError, tempWarning, tempNormal };
            method.Invoke(null, parameters);

            temError = (int)parameters[0];
            tempWarning = (int)parameters[1];
            tempNormal = (int)parameters[2];
        }

        /// <summary>
        /// 通过反射调用原生控制台的StartGettingEntries方法来获取当前日志列表并返回日志数量
        /// </summary>
        /// <returns></returns>
        public static int StartGettingEntries() => (int)GetMethod("StartGettingEntries").Invoke(null, null);

        public static void EndGettingEntries() => GetMethod("EndGettingEntries").Invoke(null, null);


















    }
}