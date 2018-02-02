/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:37:32
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

namespace Iuker.MoonSharp.Interpreter.Interop.BasicDescriptors
{
    /// <summary>
    /// Permissions for members access
    /// </summary>
    [Flags]
    public enum MemberDescriptorAccess
    {
        /// <summary>
        /// The member can be read from
        /// </summary>
        CanRead = 1,
        /// <summary>
        /// The member can be written to
        /// </summary>
        CanWrite = 2,
        /// <summary>
        /// The can be invoked
        /// </summary>
        CanExecute = 4
    }
}
