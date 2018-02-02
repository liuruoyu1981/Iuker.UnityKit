/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:53:09
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
using Iuker.MoonSharp.Interpreter.Interop;
using Iuker.MoonSharp.Interpreter.Interop.PredefinedUserData;
using Iuker.MoonSharp.Interpreter.Interop.RegistrationPolicies;
using Iuker.MoonSharp.Interpreter.Interop.StandardDescriptors;
using Iuker.MoonSharp.Interpreter.Interop.UserDataRegistries;
using Iuker.MoonSharp.Interpreter.Serialization.Json;

namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// 该类暴露C#对象作为lua用户数据
    /// 基于效率考虑，类型使用全局注册
    /// </summary>
    public class UserData : RefIdObject
    {
        private UserData()
        {
            // 这种类型只能用一个创建方法来实例化
        }

        /// <summary>
        /// 获取或设置“uservalue”。看到调试。getuservalue debug.setuservalue。
        /// Gets or sets the "uservalue". See debug.getuservalue and debug.setuservalue.
        /// http://www.lua.org/manual/5.2/manual.html#pdf-debug.setuservalue
        /// </summary>
        public DynValue UserValue { get; set; }

        /// <summary>
        /// 获得这个用户数据关联的对象(零静力学)
        /// </summary>
        public object Object { get; private set; }

        /// <summary>
        /// 获取该userdata的类型描述符
        /// </summary>
        public IUserDataDescriptor Descriptor { get; private set; }

        static UserData()
        {
            RegistrationPolicy = InteropRegistrationPolicy.Default;

            RegisterType<EventFacade>(InteropAccessMode.NoReflectionAllowed);
            RegisterType<AnonWrapper>(InteropAccessMode.HideMembers);
            RegisterType<EnumerableWrapper>(InteropAccessMode.NoReflectionAllowed);
            RegisterType<JsonNull>(InteropAccessMode.Reflection);

            DefaultAccessMode = InteropAccessMode.LazyOptimized;
        }

        /// <summary>
        /// 注册用户数据互操作的类型
        /// </summary>
        /// <typeparam name="T">The type to be registered</typeparam>
        /// <param name="accessMode">The access mode (optional).</param>
        /// <param name="friendlyName">Friendly name for the type (optional)</param>
        public static IUserDataDescriptor RegisterType<T>(InteropAccessMode accessMode = InteropAccessMode.Default, string friendlyName = null)
        {
            return TypeDescriptorRegistry.RegisterType_Impl(typeof(T), accessMode, friendlyName, null);
        }

        /// <summary>
        /// 注册用户数据互操作的类型
        /// </summary>
        /// <param name="type">The type to be registered</param>
        /// <param name="accessMode">The access mode (optional).</param>
        /// <param name="friendlyName">Friendly name for the type (optional)</param>
        public static IUserDataDescriptor RegisterType(Type type, InteropAccessMode accessMode = InteropAccessMode.Default, string friendlyName = null)
        {
            return TypeDescriptorRegistry.RegisterType_Impl(type, accessMode, friendlyName, null);
        }



        /// <summary>
        /// 获取或设置在整个应用程序中使用的注册策略
        /// Gets or sets the registration policy to be used in the whole application
        /// </summary>
        public static IRegistrationPolicy RegistrationPolicy
        {
            get { return TypeDescriptorRegistry.RegistrationPolicy; }
            set { TypeDescriptorRegistry.RegistrationPolicy = value; }
        }

        /// <summary>
        /// Gets or sets the default access mode to be used in the whole application
        /// </summary>
        /// <value>
        /// The default access mode.
        /// </value>
        /// <exception cref="System.ArgumentException">InteropAccessMode is InteropAccessMode.Default</exception>
        public static InteropAccessMode DefaultAccessMode
        {
            get { return TypeDescriptorRegistry.DefaultAccessMode; }
            set { TypeDescriptorRegistry.DefaultAccessMode = value; }
        }

    }
}
