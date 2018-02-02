/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:26:28
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

namespace Iuker.MoonSharp.Interpreter.Modules
{
    /// <summary>
    /// In a module type, mark methods or fields with this attribute to have them exposed as module functions.
    /// Methods must have the signature "public static DynValue ...(ScriptExecutionContextCallbackArguments)".
    /// Fields must be static or const strings, with an anonymous Lua function inside.
    /// 
    /// See <see cref="MoonSharpModuleAttribute"/> for more information about modules.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MoonSharpModuleMethodAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the function in the module (defaults to member name)
        /// </summary>
        public string Name { get; set; }
    }
}
