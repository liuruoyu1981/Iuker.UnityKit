/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:27:22
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
    /// 所有标准库模块枚举
    /// Enumeration (combinable as flags) of all the standard library modules
    /// </summary>
    [Flags]
    public enum CoreModules
    {
        /// <summary>
        /// 用于指定没有加载模块(= 0)。
        /// Value used to specify no modules to be loaded (equals 0).
        /// </summary>
        None = 0,

        /// <summary>
        /// 基础方法，包括 "assert", "collectgarbage", "error", "print", "select", "type", "tonumber" and "tostring"。
        /// </summary>
        Basic = 0x40,
        /// <summary>
        /// 全局常量： "_G", "_VERSION" and "_MOONSHARP"。
        /// </summary>
        GlobalConsts = 0x1,
        /// <summary>
        /// 表迭代器： "next", "ipairs" and "pairs".
        /// </summary>
        TableIterators = 0x2,
        /// <summary>
        /// 元表函数 ： "setmetatable", "getmetatable", "rawset", "rawget", "rawequal" and "rawlen".
        /// </summary>
        Metatables = 0x4,
        /// <summary>
        /// 字符串包
        /// </summary>
        String = 0x8,
        /// <summary>
        /// 加载函数： "load", "loadsafe", "loadfile", "loadfilesafe", "dofile" and "require"
        /// </summary>
        LoadMethods = 0x10,
        /// <summary>
        /// 表包
        /// </summary>
        Table = 0x20,
        /// <summary>
        /// 错误处理函数： "pcall" and "xpcall"
        /// </summary>
        ErrorHandling = 0x80,
        /// <summary>
        /// 数值包
        /// </summary>
        Math = 0x100,
        /// <summary>
        /// 协程包
        /// </summary>
        Coroutine = 0x200,
        /// <summary>
        /// 32位包
        /// </summary>
        Bit32 = 0x400,
        /// <summary>
        /// 操作系统有关时间的函数包： "clock", "difftime", "date" and "time"
        /// </summary>
        OS_Time = 0x800,
        /// <summary>
        /// The methods of "os" package excluding those listed for OS_Time. These are not supported under Unity.
        /// </summary>
        OS_System = 0x1000,
        /// <summary>
        /// The methods of "io" and "file" packages. These are not supported under Unity.
        /// </summary>
        IO = 0x2000,
        /// <summary>
        /// 调试包（有限支持）
        /// </summary>
        Debug = 0x4000,
        /// <summary>
        /// “动态”包(MoonSharp引入)。
        /// The "dynamic" package (introduced by MoonSharp).
        /// </summary>
        Dynamic = 0x8000,
        /// <summary>
        /// “json”包(MoonSharp引入)。
        /// The "json" package (introduced by MoonSharp).
        /// </summary>
        Json = 0x10000,


        /// <summary>
        /// 一种“硬”沙箱的预设,包括字符串、数学、表、bit32包、常量和表迭代器。
        /// A sort of "hard" sandbox preset, including string, math, table, bit32 packages, constants and table iterators.
        /// </summary>
        Preset_HardSandbox = GlobalConsts | TableIterators | String | Table | Basic | Math | Bit32,
        /// <summary>
        /// A softer sandbox preset, adding metatables support, error handling, coroutine, time functions, json parsing and dynamic evaluations.
        /// </summary>
        Preset_SoftSandbox = Preset_HardSandbox | Metatables | ErrorHandling | Coroutine | OS_Time | Dynamic | Json,
        /// <summary>
        /// The default preset. Includes everything except "debug" as now.
        /// Beware that using this preset allows scripts unlimited access to the system.
        /// </summary>
        Preset_Default = Preset_SoftSandbox | LoadMethods | OS_System | IO,
        /// <summary>
        /// The complete package.
        /// Beware that using this preset allows scripts unlimited access to the system.
        /// </summary>
        Preset_Complete = Preset_Default | Debug,

    }

    internal static class CoreModules_ExtensionMethods
    {
        public static bool Has(this CoreModules val, CoreModules flag)
        {
            return (val & flag) == flag;
        }
    }
}
