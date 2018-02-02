/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:54:27
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

namespace Iuker.MoonSharp.Interpreter.Debugging
{
    /// <summary>
    /// Enumeration of capabilities for a debugger
    /// </summary>
    [Flags]
    public enum DebuggerCaps
    {
        /// <summary>
        /// Flag set if the debugger can debug source code
        /// </summary>
        CanDebugSourceCode = 0x1,
        /// <summary>
        /// Flag set if the can debug VM bytecode
        /// </summary>
        CanDebugByteCode = 0x2,
        /// <summary>
        /// Flag set if the debugger uses breakpoints based on lines instead of tokens
        /// </summary>
        HasLineBasedBreakpoints = 0x4
    }
}
