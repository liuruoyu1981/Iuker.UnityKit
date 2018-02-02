/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:13:06
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
    /// 将一个CLR类型标记为MoonSharp module
    /// module是进行CLR和MoonSharp脚本互操作最快的方式，尽管这会消耗很多性能。module使用标准库来达到最高效率。
    /// module基本上只包含静态方法、和回调函数签名。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class MoonSharpModuleAttribute : Attribute
	{
        /// <summary>
        /// 获取或设置名称空间,表将包含的名称定义的函数。
        /// Gets or sets the namespace, that is the name of the table which will contain the defined functions.
        /// 在全局表中可以为空
        /// </summary>
        public string Namespace { get; set; }
	}
}
