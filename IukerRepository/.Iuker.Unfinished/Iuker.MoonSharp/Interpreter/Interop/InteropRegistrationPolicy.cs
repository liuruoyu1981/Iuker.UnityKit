/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:03:49
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
using Iuker.MoonSharp.Interpreter.Interop.RegistrationPolicies;

namespace Iuker.MoonSharp.Interpreter.Interop
{
    /// <summary>
    /// Collection of the standard policies to handle UserData type registrations.
    /// Provided mostly for compile-time backward compatibility with old code.
    /// See also : <see cref="IRegistrationPolicy"/> .
    /// </summary>
    public static class InteropRegistrationPolicy
    {
        /// <summary>
        /// The default registration policy used by MoonSharp unless explicitely replaced.
        /// Deregistrations are allowed, but registration of a new descriptor are not allowed
        /// if a descriptor is already registered for that type.
        /// 
        /// Types must be explicitly registered.
        /// </summary>
        public static IRegistrationPolicy Default
        {
            get { return new DefaultRegistrationPolicy(); }
        }

        /// <summary>
        /// The default registration policy used by MoonSharp unless explicitely replaced.
        /// Deregistrations are allowed, but registration of a new descriptor are not allowed
        /// if a descriptor is already registered for that type.
        /// 
        /// Types must be explicitly registered.
        /// </summary>
        [Obsolete("Please use InteropRegistrationPolicy.Default instead.")]
        public static IRegistrationPolicy Explicit
        {
            get { return new DefaultRegistrationPolicy(); }
        }

        /// <summary>
        /// Types are automatically registered if not found in the registry. This is easier to use but potentially unsafe.
        /// </summary>
        public static IRegistrationPolicy Automatic
        {
            get { return new AutomaticRegistrationPolicy(); }
        }
    }
}
