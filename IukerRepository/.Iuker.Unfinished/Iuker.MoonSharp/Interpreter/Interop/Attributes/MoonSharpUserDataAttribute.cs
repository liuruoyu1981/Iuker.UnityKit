/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:28:31
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
    /// Marks a type of automatic registration as userdata (which happens only if UserData.RegisterAssembly is called).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class MoonSharpUserDataAttribute : Attribute
    {
        /// <summary>
        /// The interop access mode
        /// </summary>
        public InteropAccessMode AccessMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoonSharpUserDataAttribute"/> class.
        /// </summary>
        public MoonSharpUserDataAttribute()
        {
            AccessMode = InteropAccessMode.Default;
        }
    }
}
