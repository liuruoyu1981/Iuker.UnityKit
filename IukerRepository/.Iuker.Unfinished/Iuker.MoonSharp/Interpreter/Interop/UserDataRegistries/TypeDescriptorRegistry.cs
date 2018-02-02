/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:05:46
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
using System.Runtime.CompilerServices;
using Iuker.MoonSharp.Interpreter.Compatibility;
using Iuker.MoonSharp.Interpreter.DataTypes;
using Iuker.MoonSharp.Interpreter.Interop.Attributes;
using Iuker.MoonSharp.Interpreter.Interop.ProxyObjects;
using Iuker.MoonSharp.Interpreter.Interop.RegistrationPolicies;

namespace Iuker.MoonSharp.Interpreter.Interop.UserDataRegistries
{
    /// <summary>
    /// 所有类型描述符的注册表。使用UserData静态访问这些数据。
    /// </summary>
    internal static class TypeDescriptorRegistry
    {
        private static object s_Lock = new object();
        private static Dictionary<Type, IUserDataDescriptor> s_TypeRegistry = new Dictionary<Type, IUserDataDescriptor>();
        private static Dictionary<Type, IUserDataDescriptor> s_TypeRegistryHistory = new Dictionary<Type, IUserDataDescriptor>();
        private static InteropAccessMode s_DefaultAccessMode;

        /// <summary>
        /// 注册程序集中所有被标注了MoonSharpUserDataAttribute的类型
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="includeExtensionTypes"></param>
        internal static void RegisterAssembly(Assembly asm = null, bool includeExtensionTypes = false)
        {
            if (asm == null)
            {
#if NETFX_CORE || DOTNET_CORE
					throw new NotSupportedException("Assembly.GetCallingAssembly is not supported on target framework.");
#else
                asm = Assembly.GetCallingAssembly();
#endif
            }

            if (includeExtensionTypes)  //  注册用户类型的扩展方法
            {
                var extensionTypes = from t in asm.SafeGetTypes()
                                     let attributes = Framework.Do.GetCustomAttributes(t, typeof(ExtensionAttribute), true)
                                     where attributes != null && attributes.Length > 0
                                     select new { Attributes = attributes, DataType = t };

                foreach (var extType in extensionTypes)
                {
                    //UserData.RegisterExtensionType(extType.DataType);
                }
            }

            var userDataTypes = from t in asm.SafeGetTypes()
                                let attributes = Framework.Do.GetCustomAttributes(t, typeof(MoonSharpUserDataAttribute), true)
                                where attributes != null && attributes.Length > 0
                                select new { Attributes = attributes, DataType = t };

            foreach (var userDataType in userDataTypes)
            {
                UserData.RegisterType(userDataType.DataType, userDataType.Attributes
                    .OfType<MoonSharpUserDataAttribute>()
                    .First()
                    .AccessMode);
            }
        }






















































        /// <summary>
        /// 获取或设置在整个应用程序中使用的默认访问模式
        /// Gets or sets the default access mode to be used in the whole application
        /// </summary>
        /// <value>
        /// The default access mode.
        /// </value>
        /// <exception cref="System.ArgumentException">InteropAccessMode is InteropAccessMode.Default</exception>
        internal static InteropAccessMode DefaultAccessMode
        {
            get { return s_DefaultAccessMode; }
            set
            {
                if (value == InteropAccessMode.Default)
                    throw new ArgumentException("InteropAccessMode is InteropAccessMode.Default");

                s_DefaultAccessMode = value;
            }
        }


        internal static IUserDataDescriptor RegisterProxyType_Impl(IProxyFactory proxyFactory,
            InteropAccessMode accessMode, string friendlyName)
        {
            return null;
        }

        internal static IUserDataDescriptor RegisterType_Impl(Type type, InteropAccessMode accessMode,
            string friendlyName, IUserDataDescriptor descriptor)
        {
            return null;
        }


















        /// <summary>
        /// 解析给定类型的访问模式的默认类型
        /// </summary>
        /// <param name="accessMode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static InteropAccessMode ResolveDefaultAccessModeForType(InteropAccessMode accessMode, Type type)
        {
            if (accessMode == InteropAccessMode.Default)
            {
                MoonSharpUserDataAttribute attr = Framework.Do.GetCustomAttributes(type, true)
                    .OfType<MoonSharpUserDataAttribute>()
                    .SingleOrDefault();

                if (attr != null)
                    accessMode = attr.AccessMode;
            }

            if (accessMode == InteropAccessMode.Default)
                accessMode = s_DefaultAccessMode;

            return accessMode;
        }


        /// <summary>
        /// Gets the best possible type descriptor for a specified CLR type.
        /// </summary>
        /// <param name="type">The CLR type for which the descriptor is desired.</param>
        /// <param name="searchInterfaces">if set to <c>true</c> interfaces are used in the search.</param>
        /// <returns></returns>
        internal static IUserDataDescriptor GetDescriptorForType(Type type, bool searchInterfaces)
        {
            lock (s_Lock)
            {
                IUserDataDescriptor typeDescriptor = null;

                // if the type has been explicitly registered, return its descriptor as it's complete
                if (s_TypeRegistry.ContainsKey(type))
                    return s_TypeRegistry[type];

                if (RegistrationPolicy.AllowTypeAutoRegistration(type))
                {
                    // no autoreg of delegates
                    if (!Framework.Do.IsAssignableFrom((typeof(Delegate)), type))
                    {
                        return RegisterType_Impl(type, DefaultAccessMode, type.FullName, null);
                    }
                }

                // search for the base object descriptors
                for (Type t = type; t != null; t = Framework.Do.GetBaseType(t))
                {
                    IUserDataDescriptor u;

                    if (s_TypeRegistry.TryGetValue(t, out u))
                    {
                        typeDescriptor = u;
                        break;
                    }
                    else if (Framework.Do.IsGenericType(t))
                    {
                        if (s_TypeRegistry.TryGetValue(t.GetGenericTypeDefinition(), out u))
                        {
                            typeDescriptor = u;
                            break;
                        }
                    }
                }

                if (typeDescriptor is IGeneratorUserDataDescriptor)
                    typeDescriptor = ((IGeneratorUserDataDescriptor)typeDescriptor).Generate(type);


                // we should not search interfaces (for example, it's just for statics..), no need to look further
                if (!searchInterfaces)
                    return typeDescriptor;

                List<IUserDataDescriptor> descriptors = new List<IUserDataDescriptor>();

                if (typeDescriptor != null)
                    descriptors.Add(typeDescriptor);


                if (searchInterfaces)
                {
                    foreach (Type interfaceType in Framework.Do.GetInterfaces(type))
                    {
                        IUserDataDescriptor interfaceDescriptor;

                        if (s_TypeRegistry.TryGetValue(interfaceType, out interfaceDescriptor))
                        {
                            if (interfaceDescriptor is IGeneratorUserDataDescriptor)
                                interfaceDescriptor = ((IGeneratorUserDataDescriptor)interfaceDescriptor).Generate(type);

                            if (interfaceDescriptor != null)
                                descriptors.Add(interfaceDescriptor);
                        }
                        else if (Framework.Do.IsGenericType(interfaceType))
                        {
                            if (s_TypeRegistry.TryGetValue(interfaceType.GetGenericTypeDefinition(), out interfaceDescriptor))
                            {
                                if (interfaceDescriptor is IGeneratorUserDataDescriptor)
                                    interfaceDescriptor = ((IGeneratorUserDataDescriptor)interfaceDescriptor).Generate(type);

                                if (interfaceDescriptor != null)
                                    descriptors.Add(interfaceDescriptor);
                            }
                        }
                    }
                }

                if (descriptors.Count == 1)
                    return descriptors[0];
                if (descriptors.Count == 0)
                    return null;
                //return new CompositeUserDataDescriptor(descriptors, type);
                    return null;
            }
        }




        /// <summary>
        /// 获取注册类型列表。
        /// </summary>
        /// <value>
        /// 已经注册的类型。
        /// </value>
        public static IEnumerable<KeyValuePair<Type, IUserDataDescriptor>> RegisteredTypes
        {
            get { lock (s_Lock) return s_TypeRegistry.ToArray(); }
        }

        /// <summary>
        /// 获取注册类型列表，包括未注册的类型。
        /// </summary>
        /// <value>
        /// 已经注册的类型。
        /// </value>
        public static IEnumerable<KeyValuePair<Type, IUserDataDescriptor>> RegisteredTypesHistory
        {
            get
            {
                lock (s_Lock)
                {
                    return s_TypeRegistryHistory.ToArray();
                }
            }
        }

        /// <summary>
        /// 获取或设置注册策略。
        /// </summary>
        internal static IRegistrationPolicy RegistrationPolicy { get; set; }
    }
}
