/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/18/2017 15:15
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
    /// 控制台窗口
    /// </summary>
    public static class ConsoleWindow
    {
        public static bool IsDebugError(int mode)
        {
            var options = new ConsoleWindowMode[]
            {
                ConsoleWindowMode.Error,
                ConsoleWindowMode.Assert,
                ConsoleWindowMode.Fatal,
                ConsoleWindowMode.AssetImportError,
                ConsoleWindowMode.AssetImportWarning,
                ConsoleWindowMode.ScriptingError,
                ConsoleWindowMode.ScriptCompileError,
                ConsoleWindowMode.ScriptCompileWarning,
                ConsoleWindowMode.StickyError,
                ConsoleWindowMode.ScriptingException,
                ConsoleWindowMode.GraphCompileError,
                ConsoleWindowMode.ScriptingAssertion
            };

            int mask = 0;
            foreach (ConsoleWindowMode t in options)
            {
                mask |= (int)t;
            }
            return (mode & mask) != 0;
        }

        public static bool HasFlag(
            ConsoleWindowFlag flag)
        {
            var method = GetMethod("HasFlag");
            return (bool)method.Invoke(null, new object[] {
                (int)flag
            });
        }

        /// <summary>
        /// 通过反射获取的SetFalg方法设置Unity控制台的某个标志位
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="active"></param>
        public static void SetFlag(ConsoleWindowFlag flag, bool active)
        {
            var method = GetMethod("SetFlag");
            method.Invoke(null, new object[] {
                (int)flag,
                active
            });
        }


        public static bool HasMode(int mode, ConsoleWindowMode modeToCheck)
        {
            var mothed = GetMethod("HasMode");
            return (bool)mothed.Invoke(null, new object[] { mode, (int)modeToCheck });
        }

        /// <summary>
        /// 获得ConsoleWindow的指定方法的方法信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static MethodInfo GetMethod(string key)
        {
            if (!CachedReflection.Has(key))
            {
                CachedReflection.Cache(key, ConsoleWindowType.GetMethod(key, DefaultFlags));
            }
            return CachedReflection.Get<MethodInfo>(key);
        }

        /// <summary>
        /// 获得ConsoleWindow的指定属性的属性信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static PropertyInfo GetProperty(
            string key)
        {
            if (!CachedReflection.Has(key))
                CachedReflection.Cache(key, ConsoleWindowType.GetProperty(key, DefaultFlags));
            return CachedReflection.Get<PropertyInfo>(key);
        }

        /// <summary>
        /// 获取ConsoleWindow类的类型信息实例
        /// </summary>
        private static Type ConsoleWindowType
        {
            get
            {
                if (!CachedReflection.Has("ConsoleWindow"))
                {
                    CachedReflection.Cache("ConsoleWindow", Type.GetType("UnityEditor.ConsoleWindow,UnityEditor.dll"));
                }
                return CachedReflection.Get<Type>("ConsoleWindow");
            }
        }

        /// <summary>
        /// 默认反射绑定标志位
        /// </summary>
        private static BindingFlags DefaultFlags => BindingFlags.Static | BindingFlags.NonPublic;
    }
}