/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 12:14:13
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
    /// Similar to <see cref="DefaultRegistrationPolicy"/>, but with automatic type registration is disabled.
    /// </summary>
    public class AutomaticRegistrationPolicy : DefaultRegistrationPolicy
    {
        /// <summary>
        /// Allows type automatic registration for the specified type.
        /// NOTE: automatic type registration is NOT recommended.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// True to register the type automatically, false otherwise.
        /// </returns>
        public override bool AllowTypeAutoRegistration(Type type)
        {
            return true;
        }
    }
}
