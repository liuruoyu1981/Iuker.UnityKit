/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:24:04
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
    /// 模块类型,马克与这个属性字段作为一个模块公开常数。
    /// In a module type, mark fields with this attribute to have them exposed as a module constant.
    /// 
    /// See <see cref="MoonSharpModuleAttribute"/> for more information about modules.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MoonSharpModuleConstantAttribute : Attribute
    {
        /// <summary>
        /// get和set常数的名称——如果不同字段的名称本身
        /// Gets or sets the name of the constant - if different from the name of the field itself
        /// </summary>
        public string Name { get; set; }
    }
}
