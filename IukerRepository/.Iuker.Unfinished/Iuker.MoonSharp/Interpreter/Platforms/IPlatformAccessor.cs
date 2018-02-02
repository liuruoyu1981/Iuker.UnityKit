/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:45:25
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

using System.IO;
using System.Text;
using Iuker.MoonSharp.Interpreter.Modules;

namespace Iuker.MoonSharp.Interpreter.Platforms
{
    /// <summary>
    /// 脚本引擎所有访问底层平台(操作系统,框架)的抽象接口
    /// 既可用于支持“非标准”平台(即posix、非windows)和/或沙箱的行为脚本引擎。
    /// </summary>
    public interface IPlatformAccessor
    {
        /// <summary>
        /// 过滤核心模块排除不支持的操作
        /// </summary>
        /// <param name="module"></param>
        /// <returns>所请求的模块，不支持的模块会被过滤掉</returns>
        CoreModules FilterSupportedCoreModules(CoreModules module);

        /// <summary>
        /// 得到一个环境变量。必须实现，但是如果一个更有意义的实现不能实现或不希望实现，则允许始终返回null。
        /// </summary>
        /// <param name="envvarname"></param>
        /// <returns>环境变量值，如果没有找到，则为null</returns>
        string GetEnvironmentVariable(string envvarname);

        /// <summary>
        /// 决定应用程序是否运行在AOT(提前)模式
        /// </summary>
        bool IsRunningOnAOT();

        /// <summary>
        /// 获得平台的名称(用于调试目的)。
        /// </summary>
        /// <returns>平台的名称 (用于调试目的)</returns>
        string GetPlatformName();

        /// <summary>
        /// print函数调用的默认处理程序，可以在ScriptOptions中定制。
        /// </summary>
        /// <param name="content">The content.</param>
        void DefaultPrint(string content);

        /// <summary>
        /// input函数调用的默认处理程序，可以在ScriptOptions中定制。
        /// Default handler for interactive line input calls. Can be customized in ScriptOptions.
        /// 如果不能提供有意义的实现，则该方法应该返回null。
        /// </summary>
        /// <returns></returns>
        string DefaultInput(string prompt);

        /// <summary>
        /// io模块用来打开文件的函数
        /// 如果可以有一个无效的实现的io模块是过滤掉。
        /// 它应该返回一个给定文件的正确地初始化访问流
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mode">The mode (as per Lua usage - e.g. 'w+', 'rb', etc.).</param>
        /// <returns></returns>
        Stream IO_OpenFile(Script script, string filename, Encoding encoding, string mode);

        /// <summary>
        /// 得到一个标准的流(stdin、stdout stderr)。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        Stream IO_GetStandardStream(StandardFileType type);

        /// <summary>
        /// 临时文件名。用于“输入输出”和“操作系统”模块。
        /// 如果可以有一个无效的实现“输入输出”和“操作系统”模块是过滤掉。
        /// Can have an invalid implementation if 'io' and 'os' modules are filtered out.
        /// </summary>
        /// <returns></returns>
        string IO_OS_GetTempFilename();

        /// <summary>
        /// 退出过程,返回指定的退出代码。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        void OS_ExitFast(int exitCode);

        /// <summary>
        /// 检查一个文件是否存在。由“os”模块使用。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        bool OS_FileExists(string file);

        /// <summary>
        /// 删除指定的文件。由“os”模块使用。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="file">The file.</param>
        void OS_FileDelete(string file);

        /// <summary>
        /// 移动指定的文件。由“os”模块使用。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dst">The DST.</param>
        void OS_FileMove(string src, string dst);

        /// <summary>
        /// 执行指定的命令行，同时返回子进程退出代码和阻塞。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="cmdline">The cmdline.</param>
        /// <returns></returns>
        int OS_Execute(string cmdline);
    }
}
