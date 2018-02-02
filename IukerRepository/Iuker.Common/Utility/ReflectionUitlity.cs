using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// 反射工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 18:10:06")]
    [ClassPurposeDesc("", "")]
#endif
    public static class ReflectionUitlity
    {
        /// <summary>
        /// 程序集公开类型缓存字典
        /// 将当前已经查找过的程序集的公开类型列表缓存在该字典中
        /// 后续查找直接使用缓存列表提高反射效率
        /// </summary>
        private static readonly Dictionary<string, List<Type>> sAssemblyDictionary = new Dictionary<string, List<Type>>();

        private static List<Type> TryGetAssemblyTypes(string assemblyName)
        {
            if (sAssemblyDictionary.ContainsKey(assemblyName))
            {
                var result = sAssemblyDictionary[assemblyName];
                return result;
            }
            return null;
        }



        /// <summary>
        /// 获得目标程序集中的所有公开类型列表
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<Type> GetAllType(Assembly assembly)
        {
            if (sAssemblyDictionary.ContainsKey(assembly.FullName))
            {
                return sAssemblyDictionary[assembly.FullName];
            }

            var types = assembly.GetTypes().ToList();
            sAssemblyDictionary.Add(assembly.FullName, types);
            return types;
        }

        public static List<Type> GetAllType(List<Assembly> assemblies)
        {
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(GetAllType(assembly));
            }

            return types;
        }

        /// <summary>
        /// 在目标程序集中查找实现了指定类型以类型名作为Key的类型字典，字典不包括接口和抽象类。
        /// </summary>
        /// <typeparam name="T">要获得的指定类型</typeparam>
        /// <param name="assembly">目标程序集</param>
        /// <param name="whereFunc">过滤委托</param>
        /// <returns></returns>
        public static Dictionary<string, Type> GetTypeDictionary<T>(Assembly assembly = null, Func<Type, bool> whereFunc = null)
        {
            var targetAssembly = assembly ?? Assembly.GetEntryAssembly();
            var assemblyName = targetAssembly.FullName;
            var targetTypes = TryGetAssemblyTypes(assemblyName);
            if (targetTypes == null)
            {
                targetTypes = (assembly ?? Assembly.GetExecutingAssembly()).GetTypes().ToList();
                sAssemblyDictionary.Add(assemblyName, targetTypes);
            }

            var result = targetTypes.Where(t => typeof(T).IsAssignableFrom(t))
                .Where(t => !t.IsInterface && !t.IsAbstract);
            if (whereFunc == null) return result.ToDictionary(t => t.Name);

            var otherResult = result.Where(whereFunc).ToDictionary(t => t.Name);
            return otherResult;
        }

        public static Dictionary<string, Type> GetTypeDictionary<T>(List<Assembly> assemblies, Func<Type, bool> whereFunc = null)
        {
            var typeDictionary = new Dictionary<string, Type>();

            foreach (var assembly in assemblies)
            {
                typeDictionary.Combin(GetTypeDictionary<T>(assembly, whereFunc));
            }

            return typeDictionary;
        }

        /// <summary>
        /// 在目标程序集中查找实现了指定类型以类型名作为Key的类型字典
        /// 字典不包括接口和抽象类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assembly"></param>
        /// <param name="whereFunc"></param>
        /// <returns></returns>
        public static Dictionary<string, Type> GetTypeDictionary(Type type, Assembly assembly = null, Func<Type, bool> whereFunc = null)
        {
            var targetTypes = TryCacheTypes(assembly);

            var result = targetTypes.Where(type.IsAssignableFrom)
                .Where(t => !t.IsInterface && !t.IsAbstract);
            if (whereFunc != null)
            {
                var otherResult = result.Where(whereFunc).ToDictionary(t => t.Name);
                return otherResult;
            }

            return result.ToDictionary(t => t.Name);
        }

        private static List<Type> TryCacheTypes(Assembly assembly)
        {
            var targetAssembly = assembly ?? Assembly.GetEntryAssembly();
            var assemblyName = targetAssembly.FullName;
            var targetTypes = TryGetAssemblyTypes(assemblyName);
            if (targetTypes == null)
            {
                targetTypes = (assembly ?? Assembly.GetExecutingAssembly()).GetTypes().ToList();
                sAssemblyDictionary.Add(assemblyName, targetTypes);
            }

            return targetTypes;
        }

        /// <summary>
        /// 在目标程序集中查找目标接口类型的所有子接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<Type> GetInterfaces(Type type, Assembly assembly = null, Func<Type, bool> wherer = null)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException("查找子接口必须传入一个接口类型！");
            }

            var targetTypes = TryCacheTypes(assembly);

            var result = targetTypes.Where(type.IsAssignableFrom)
                .Where(t => t.IsInterface).ToList();

            if (wherer != null)
            {
                result = result.Where(wherer).ToList();
            }

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
            var types = new List<Type>();
            foreach (var assembly in asmList)
            {
                var tempTypes = GetTypeList<T>(assembly);
                types.AddRange(tempTypes);
            }
            var result = types.ToDictionary(t => t.Name);
            return result;
        }

        public static Dictionary<string, Type> GetTypeDictionary<T>(List<Assembly> assemblies)
        {
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var tempTypes = GetTypeList<T>(assembly);
                types.AddRange(tempTypes);
            }
            var result = types.ToDictionary(t => t.Name);
            return result;
        }

        public static List<Type> GetTypeList<T>(Assembly assembly = null, bool isInterface = false, bool isAbstract = false)
        {
            var targetAssembly = assembly ?? Assembly.GetEntryAssembly();
            var assemblyName = targetAssembly.FullName;
            var targetTypes = TryGetAssemblyTypes(assemblyName);
            if (targetTypes == null)
            {
                targetTypes = (assembly ?? Assembly.GetExecutingAssembly()).GetTypes().ToList();
                sAssemblyDictionary.Add(assemblyName, targetTypes);
            }
            var result = targetTypes.Where(t => typeof(T).IsAssignableFrom(t) && t.IsInterface == isInterface && t.IsAbstract == isAbstract).ToList();
            return result;
        }

        public static List<Type> GetTypeList<T>(List<Assembly> assemblies)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(GetTypeList<T>(assembly));
            }

            return types;
        }

        public static List<Type> GetAllTypes(List<Assembly> assemblies)
        {
            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes());
            }

            return types;
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
                throw new Exception(string.Format("目标类型{0}找不到指定的私有静态函数{1}。", type, methodName));
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

        /// <summary>
        /// 获得指定枚举类型的所有字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<FieldInfo> GetEnumAllField(Type type)
        {
            var fieldInfos = type.GetFields().ToList();
            fieldInfos.RemoveAt(0);
            return fieldInfos;
        }


    }
}
