/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/18/2017 15:15
Email: liuruoyu1981@gmail.com
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

namespace Iuker.UnityKit.Editor.Console.Core.UnityLoggerApi
{
    /// <summary>
    /// 控制台窗口标志
    /// </summary>
    public enum ConsoleWindowFlag
    {
        Collapse = 1,
        ClearOnPlay = 2,
        ErrorPause = 4,
        Verbose = 8,
        StopForAssert = 16,
        StopForError = 32,
        Autoscroll = 64,
        LogLevelLog = 128,
        LogLevelWarning = 256,
        LogLevelError = 512,
        Remote = 1024,

    }
}