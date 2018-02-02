/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:14:47
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

namespace Iuker.MoonSharp.Interpreter.Interop.RegistrationPolicies
{
    /// <summary>
    /// MoonSharp使用默认注册策略除非被替换。
    /// 允许取消登记，但如何一个类型的类型描述符已经注册则不允许注册新的类型描述符。
    /// 
    /// 自动类型注册是禁用的。
    /// </summary>
    public class DefaultRegistrationPolicy : IRegistrationPolicy
    {
        /// <summary>
        /// Called to handle the registration or deregistration of a type descriptor. Must return the type descriptor to be registered, or null to remove the registration.
        /// </summary>
        /// <param name="newDescriptor">The new descriptor, or null if this is a deregistration.</param>
        /// <param name="oldDescriptor">The old descriptor, or null if no descriptor was previously registered for this type.</param>
        /// <returns></returns>
        public IUserDataDescriptor HandleRegistration(IUserDataDescriptor newDescriptor, IUserDataDescriptor oldDescriptor)
        {
            if (newDescriptor == null)
                return null;
            else
                return oldDescriptor ?? newDescriptor;
        }

        /// <summary>
        /// Allows type automatic registration for the specified type.
        /// NOTE: automatic type registration is NOT recommended.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// True to register the type automatically, false otherwise.
        /// </returns>
        public virtual bool AllowTypeAutoRegistration(Type type)
        {
            return false;
        }

    }
}
