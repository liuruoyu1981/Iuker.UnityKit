/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:21:42
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
    /// 强制一个类成员变为不可见。可以用来隐藏公共成员。相当于MoonSharpVisible(假)。
    /// Forces a class member visibility to scripts. Can be used to hide public members. Equivalent to MoonSharpVisible(false).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field
        | AttributeTargets.Constructor | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public sealed class MoonSharpHiddenAttribute : Attribute
    {
    }
}
