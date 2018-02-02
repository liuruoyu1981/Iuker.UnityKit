/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:50:17
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



namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// State of coroutines
    /// </summary>
    public enum CoroutineState
    {
        /// <summary>
        /// This is the main coroutine
        /// </summary>
        Main,
        /// <summary>
        /// Coroutine has not started yet
        /// </summary>
        NotStarted,
        /// <summary>
        /// Coroutine is suspended
        /// </summary>
        Suspended,
        /// <summary>
        /// Coroutine has been forcefully suspended (i.e. auto-yielded)
        /// </summary>
        ForceSuspended,
        /// <summary>
        /// Coroutine is running
        /// </summary>
        Running,
        /// <summary>
        /// Coroutine has terminated
        /// </summary>
        Dead
    }
}
