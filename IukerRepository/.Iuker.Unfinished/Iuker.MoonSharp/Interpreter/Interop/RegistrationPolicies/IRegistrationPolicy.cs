/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:15:01
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
    /// 管理如何处理接口
    /// </summary>
    public interface IRegistrationPolicy
    {
        /// <summary>
        /// 称为处理注册或取消登记的类型描述符。必须返回要注册的类型描述符，或者是null来删除注册
        /// </summary>
        /// <param name="newDescriptor">The new descriptor, or null if this is a deregistration.</param>
        /// <param name="oldDescriptor">The old descriptor, or null if no descriptor was previously registered for this type.</param>
        /// <returns></returns>
        IUserDataDescriptor HandleRegistration(IUserDataDescriptor newDescriptor, IUserDataDescriptor oldDescriptor);

        /// <summary>
        /// 允许为指定类型的类型自动注册。
        /// 注意:不推荐自动类型注册。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True to register the type automatically, false otherwise.</returns>
        bool AllowTypeAutoRegistration(Type type);
    }
}
