using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// ���乤��
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 18:10:06")]
    [ClassPurposeDesc("", "")]
#endif
    public static class ReflectionUitlity
    {
        /// <summary>
        /// ���򼯹������ͻ����ֵ�
        /// ����ǰ�Ѿ����ҹ��ĳ��򼯵Ĺ��������б����ڸ��ֵ���
        /// ��������ֱ��ʹ�û����б���߷���Ч��
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
        /// ���Ŀ������е����й��������б�
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
        /// ��Ŀ������в���ʵ����ָ����������������ΪKey�������ֵ䣬�ֵ䲻�����ӿںͳ����ࡣ
        /// </summary>
        /// <typeparam name="T">Ҫ��õ�ָ������</typeparam>
        /// <param name="assembly">Ŀ�����</param>
        /// <param name="whereFunc">����ί��</param>
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
        /// ��Ŀ������в���ʵ����ָ����������������ΪKey�������ֵ�
        /// �ֵ䲻�����ӿںͳ�����
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
        /// ��Ŀ������в���Ŀ��ӿ����͵������ӽӿ�����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<Type> GetInterfaces(Type type, Assembly assembly = null, Func<Type, bool> wherer = null)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException("�����ӽӿڱ��봫��һ���ӿ����ͣ�");
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
        /// ��ָ����Ŀ������б��в���ʵ����ָ����������������ΪKey�������ֵ䣬�ֵ䲻�����ӿںͳ����ࡣ
        /// </summary>
        /// <typeparam name="T">���ҵ�Ŀ������</typeparam>
        /// <param name="asmList">��Ҫ���ҵĳ����б�</param>
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
        /// ����һ��˽�о�̬����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        public static void CallPrivateStaticMethod(Type type, string methodName, object[] param)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            if (method == null)
            {
                throw new Exception(string.Format("Ŀ������{0}�Ҳ���ָ����˽�о�̬����{1}��", type, methodName));
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
        /// ���ָ��ö�����͵������ֶ�
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
