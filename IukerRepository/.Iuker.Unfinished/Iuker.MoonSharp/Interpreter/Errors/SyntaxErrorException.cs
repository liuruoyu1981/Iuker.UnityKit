/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/15 10:35:13
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
using Iuker.MoonSharp.Tree;

namespace Iuker.MoonSharp.Interpreter.Errors
{
    /// <summary>
    /// 所有源码解析和词法分析错误的异常。
    /// </summary>
#if !(PCL || ((!UNITY_EDITOR) && (ENABLE_DOTNET)) || NETFX_CORE)
    [Serializable]
#endif
    public class SyntaxErrorException : InterpreterException
    {
        internal Token Token { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this exception was caused by premature stream termination (that is, unexpected EOF).
        /// 获取或设置一个值指示是否这个异常是由于过早流终止(即意想不到的EOF)。
        /// This can be used in REPL interfaces to tell between unrecoverable errors and those which can be recovered by extra input.
        /// 这个可以用在REPL接口告诉之间的不可恢复的错误和那些可以恢复额外的输入。
        /// </summary>
        public bool IsPrematureStreamTermination { get; set; }

        internal SyntaxErrorException(Token t, string format, params object[] args)
    : base(format, args)
        {
            Token = t;
        }


    }
}
