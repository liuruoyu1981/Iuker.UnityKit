/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:30:03
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
    /// Marks a method as the handler of metamethods of a userdata type
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class MoonSharpUserDataMetamethodAttribute : Attribute
    {
        /// <summary>
        /// The metamethod name (like '__div', '__ipairs', etc.)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoonSharpUserDataMetamethodAttribute"/> class.
        /// </summary>
        /// <param name="name">The metamethod name (like '__div', '__ipairs', etc.)</param>
        public MoonSharpUserDataMetamethodAttribute(string name)
        {
            Name = name;
        }
    }
}
