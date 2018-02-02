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
    /// 控制台窗口模式
    /// </summary>
    public enum ConsoleWindowMode
    {
        Error = 1,
        Assert = 2,
        Log = 4,
        Fatal = 16,
        DontPreprocessCondition = 32,
        AssetImportError = 64,
        AssetImportWarning = 128,
        ScriptingError = 256,
        ScriptingWarning = 512,
        ScriptingLog = 1024,
        ScriptCompileError = 2048,
        ScriptCompileWarning = 4096,
        StickyError = 8192,
        MayIgnoreLineNumber = 16384,
        ReportBug = 32768,
        DisplayPreviousErrorInStatusBar = 65536,
        ScriptingException = 131072,
        DontExtractStacktrace = 262144,
        ShouldClearOnPlay = 524288,
        GraphCompileError = 1048576,
        ScriptingAssertion = 2097152
    }
}