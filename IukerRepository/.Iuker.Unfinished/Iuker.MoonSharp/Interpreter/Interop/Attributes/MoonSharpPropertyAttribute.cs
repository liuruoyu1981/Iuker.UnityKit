/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:26:21
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
    /// Marks a property as a configruation property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class MoonSharpPropertyAttribute : Attribute
    {
        /// <summary>
        /// The metamethod name (like '__div', '__ipairs', etc.)
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MoonSharpPropertyAttribute"/> class.
        /// </summary>
        public MoonSharpPropertyAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoonSharpPropertyAttribute"/> class.
        /// </summary>
        /// <param name="name">The name for this property</param>
        public MoonSharpPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
