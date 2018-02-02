/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/15 11:24:33
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
    /// 解析单词时抛出异常
    /// </summary>
#if !(PCL || ((!UNITY_EDITOR) && (ENABLE_DOTNET)) || NETFX_CORE)
    [Serializable]
#endif
    public class InternalErrorException : InterpreterException
    {
        internal InternalErrorException(string message)
    : base(message)
        {

        }

        internal InternalErrorException(string format, params object[] args)
            : base(format, args)
        {

        }
    }
}
