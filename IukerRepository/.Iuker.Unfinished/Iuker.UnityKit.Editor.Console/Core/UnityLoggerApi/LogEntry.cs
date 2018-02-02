/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/16/2017 19:34
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
    /// <summary>
    /// 日志实体
    /// </summary>
    public class LogEntry
    {
        private static object mCacheLogEntryInstance;

        /// <summary>
        /// UNITY日志实体类型对象
        /// </summary>
        private static Type LogEntryType
        {
            get
            {
                if (!CachedReflection.Has("LogEntry"))
                {
                    CachedReflection.Cache("LogEntry", Type.GetType("UnityEditorInternal.LogEntry,UnityEditor.dll"));
                }
                return CachedReflection.Get<Type>("LogEntry");
            }
        }

        /// <summary>
        /// 通过反射激活器获得一个UNITY日志实体实例
        /// </summary>
        public static object GetNewEmptyObject => mCacheLogEntryInstance ??
                                                  (mCacheLogEntryInstance = Activator.CreateInstance(LogEntryType));

        /// <summary>
        /// 通过反射获取一个字段数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static FieldInfo GetField(string key, Type type)
        {
            if (!CachedReflection.Has(key))
            {
                CachedReflection.Cache(key, type.GetField(key));
            }
            return CachedReflection.Get<FieldInfo>(key);
        }

        /// <summary>
        /// 通过反射创建一个新的日志实体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IukerLog GetBluLog(object obj)
        {
            var log = new IukerLog();

            var logEntryType = LogEntryType;

            var condition = (string)GetField("condition", logEntryType).GetValue(obj);
            log.SetMessage(condition);
            log.SetStackTrace(condition);

            var file = (string)GetField("file", logEntryType).GetValue(obj);
            log.SetFile(file);

            var line = (int)GetField("line", logEntryType).GetValue(obj);
            log.SetLine(line);

            var mode = (int)GetField("mode", logEntryType).GetValue(obj);
            log.SetMode(mode);

            var instanceID = (int)GetField("instanceID", logEntryType).GetValue(obj);
            log.SetInstanceID(instanceID);

            return log;
        }

        /// <summary>
        /// 默认反射绑定标志位
        /// </summary>
        private static BindingFlags DefaultFlgs => BindingFlags.Public;

        private static MethodInfo GetMethod(string key)
        {
            if (!CachedReflection.Has(key))
            {
                CachedReflection.Cache(key, LogEntryType.GetMethod(key, DefaultFlgs));
            }
            return CachedReflection.Get<MethodInfo>("LogEntry");
        }

        /// <summary>
        /// 通过反射获取一个属性实例
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static PropertyInfo GetProperty(string key)
        {
            if (!CachedReflection.Has(key))
            {
                CachedReflection.Cache(key, LogEntryType.GetProperty(key, DefaultFlgs));
            }
            return CachedReflection.Get<PropertyInfo>(key);
        }





    }
}