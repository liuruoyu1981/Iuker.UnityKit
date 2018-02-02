/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 10:25:24
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
using Iuker.MoonSharp.Interpreter.DataTypes;

namespace Iuker.MoonSharp.Interpreter.Errors
{
    /// <summary>
    /// 脚本运行时异常，提供静态构造函数
    /// 产生更多的lua标准错误
    /// </summary>
#if !(PCL || ((!UNITY_EDITOR) && (ENABLE_DOTNET)) || NETFX_CORE)
    [Serializable]
#endif
    public class ScriptRuntimeException : InterpreterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRuntimeException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ScriptRuntimeException(Exception ex)
    : base(ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRuntimeException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ScriptRuntimeException(ScriptRuntimeException ex)
            : base(ex, ex.DecoratedMessage)
        {
            this.DecoratedMessage = Message;
            this.DoNotDecorateMessage = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRuntimeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ScriptRuntimeException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRuntimeException"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public ScriptRuntimeException(string format, params object[] args)
            : base(format, args)
        {

        }

        /// <summary>
        /// Creates a ScriptRuntimeException with a predefined error message specifying that
        /// an arithmetic operation was attempted on non-numbers
        /// </summary>
        /// <param name="l">The left operand.</param>
        /// <param name="r">The right operand (or null).</param>
        /// <returns>The exception to be raised.</returns>
        /// <exception cref="InternalErrorException">If both are numbers</exception>
        public static ScriptRuntimeException ArithmeticOnNonNumber(DynValue l, DynValue r = null)
        {
            if (l.Type != DataType.Number && l.Type != DataType.String)
                return new ScriptRuntimeException("attempt to perform arithmetic on a {0} value", l.Type.ToLuaTypeString());
            else if (r != null && r.Type != DataType.Number && r.Type != DataType.String)
                return new ScriptRuntimeException("attempt to perform arithmetic on a {0} value", r.Type.ToLuaTypeString());
            else if (l.Type == DataType.String || (r != null && r.Type == DataType.String))
                return new ScriptRuntimeException("attempt to perform arithmetic on a string value");
            else
                throw new InternalErrorException("ArithmeticOnNonNumber - both are numbers");
        }

        /// <summary>
        /// Creates a ScriptRuntimeException with a predefined error message specifying that
        /// a constrained conversion of a Lua type to a CLR type has failed.
        /// </summary>
        /// <param name="t">The Lua type.</param>
        /// <param name="t2">The expected CLR type.</param>
        /// <returns>
        /// The exception to be raised.
        /// </returns>
        public static ScriptRuntimeException ConvertObjectFailed(DataType t, Type t2)
        {
            return new ScriptRuntimeException("cannot convert a {0} to a clr type {1}", t.ToString().ToLowerInvariant(), t2.FullName);
        }



















































































































































































































































































































































        /// <summary>
        /// Creates a ScriptRuntimeException with a predefined error message specifying that
        /// a conversion of a Lua type to a CLR type has failed.
        /// </summary>
        /// <param name="t">The Lua type.</param>
        /// <returns>
        /// The exception to be raised.
        /// </returns>
        public static ScriptRuntimeException ConvertObjectFailed(DataType t)
        {
            return new ScriptRuntimeException("cannot convert a {0} to a clr type", t.ToString().ToLowerInvariant());
        }













































































































































    }
}
