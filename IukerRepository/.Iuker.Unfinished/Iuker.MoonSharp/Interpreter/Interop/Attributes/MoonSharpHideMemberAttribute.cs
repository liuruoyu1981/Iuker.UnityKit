/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:24:47
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

namespace Iuker.MoonSharp.Interpreter.Interop.Attributes
{
    /// <summary>
    /// 列出了用户数据成员不被暴露在脚本中引用它的名字。
    /// Lists a userdata member not to be exposed to scripts referencing it by name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
    public sealed class MoonSharpHideMemberAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the member to be hidden.
        /// </summary>
        public string MemberName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoonSharpHideMemberAttribute"/> class.
        /// </summary>
        /// <param name="memberName">Name of the member to hide.</param>
        public MoonSharpHideMemberAttribute(string memberName)
        {
            MemberName = memberName;
        }
    }
}
