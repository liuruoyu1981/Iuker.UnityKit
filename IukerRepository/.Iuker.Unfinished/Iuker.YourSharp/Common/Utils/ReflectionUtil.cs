/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 15:53
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Iuker.YourSharp.Common.Utils
{
    /// <summary>
    /// 反射程序集工具
    /// </summary>
    public static class ReflectionUtil
    {
        /// <summary>
        /// 在目标程序集中查找实现了指定类型以类型名作为Key的类型字典，字典不包括接口和抽象类。
        /// </summary>
        /// <typeparam name="T">要获得的指定类型</typeparam>
        /// <param name="assembly">目标程序集</param>
        /// <returns></returns>
        public static Dictionary<string, Type> GetTypeDictionary<T>(Assembly assembly = null)
        {
            var result = (assembly ?? Assembly.GetExecutingAssembly()).GetTypes().Where(t => typeof(T).IsAssignableFrom(t)).
                Where(t => !t.IsInterface && !t.IsAbstract).ToDictionary(t => t.Name);
            return result;
        }

        /// <summary>
        /// 在指定的目标程序集列表中查找实现了指定类型以类型名作为Key的类型字典，字典不包括接口和抽象类。
        /// </summary>
        /// <typeparam name="T">查找的目标类型</typeparam>
        /// <param name="asmList">需要查找的程序集列表</param>
        /// <returns></returns>
        public static Dictionary<string, Type> GetTypeDictionary<T>(params Assembly[] asmList)
        {
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in asmList)
            {
                var tempTypes = GetTypeList<T>(assembly);
                types.AddRange(tempTypes);
            }
            var result = types.ToDictionary(t => t.Name);
            return result;
        }

        /// <summary>
        /// 获得指定程序集中给定类型的类型列表。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<Type> GetTypeList<T>(Assembly assembly = null)
        {
            var result = (assembly ?? Assembly.GetExecutingAssembly()).GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();
            return result;
        }

        /// <summary>
        /// 调用一个私有静态方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        public static void CallPrivateStaticMethod(Type type, string methodName, object[] param)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            if (method == null)
            {
                throw new Exception($"目标类型{type}找不到指定的私有静态函数{methodName}。");
            }

            try
            {
                method.Invoke(null, param);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null) throw ex.InnerException;
            }
        }

    }
}
