/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/15 10:26:24
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

namespace Iuker.MoonSharp.Interpreter.Errors
{
    /// <summary>
    /// Lua解释器异常
    /// </summary>
    public class InterpreterException : Exception
    {
        protected InterpreterException(Exception ex, string message)
            : base(message, ex)
        {

        }

        protected InterpreterException(Exception ex)
            : base(ex.Message, ex)
        {

        }

        protected InterpreterException(string message)
            : base(message)
        {

        }

        protected InterpreterException(string format, params object[] args)
            : base(string.Format(format, args))
        {

        }

        /// <summary>
        /// 被执行的指令指针(如果它是有意义的)
        /// </summary>
        public int InstructionPtr { get; internal set; }

        /// <summary>
        /// Gets the interpreter call stack.
        /// </summary>
        //public IList<MoonSharp.Interpreter.Debugging.WatchItem> CallStack { get; internal set; }


        /// <summary>
        /// Gets the decorated message (error message plus error location in script) if possible.
        /// 被装饰的消息(在脚本错误消息加上错误的位置),如果可能的话。
        /// </summary>
        public string DecoratedMessage { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message should not be decorated
        /// 获取或设置一个值,指出是否不应该布置的消息
        /// </summary>
        public bool DoNotDecorateMessage { get; set; }

        //internal void DecorateMessage(Script script, SourceRef sref, int ip = -1)
        //{
        //    if (string.IsNullOrEmpty(this.DecoratedMessage))
        //    {
        //        if (DoNotDecorateMessage)
        //        {
        //            this.DecoratedMessage = this.Message;
        //            return;
        //        }
        //        else if (sref != null)
        //        {
        //            this.DecoratedMessage = string.Format("{0}: {1}", sref.FormatLocation(script), this.Message);
        //        }
        //        else
        //        {
        //            this.DecoratedMessage = string.Format("bytecode:{0}: {1}", ip, this.Message);
        //        }
        //    }
        //}

        /// <summary>
        /// 重新抛出该异常
        /// </summary>
        /// <returns></returns>
        public virtual void Rethrow()
        {
        }
    }
}
