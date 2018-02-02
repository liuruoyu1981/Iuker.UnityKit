/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:45:51
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
using System.IO;
using System.Text;
using Iuker.MoonSharp.Interpreter.Modules;

namespace Iuker.MoonSharp.Interpreter.Platforms
{
    /// <summary>
    /// 抽象类提供基本服务的IPlatformAccessor提供更容易的实现平台。
    /// </summary>
    public abstract class PlatformAccessorBase : IPlatformAccessor
    {
        /// <summary>
        /// 获得平台名称前缀
        /// </summary>
        /// <returns></returns>
        public abstract string GetPlatformNamePrefix();

        public string GetPlatformName()
        {
            string suffix = null;

            if (PlatformAutoDetector.IsRunningOnUnity)
            {
                if (PlatformAutoDetector.IsUnityNative)
                {
                    suffix = "unity." + GetUnityPlatformName().ToLower() + "." + GetUnityRuntimeName();
                }
                else
                {
                    if (PlatformAutoDetector.IsRunningOnMono)
                        suffix = "unity.dll.mono";
                    else
                        suffix = "unity.dll.unknown";
                }
            }
            else if (PlatformAutoDetector.IsRunningOnMono)
                suffix = "mono";
            else
                suffix = "dotnet";

            if (PlatformAutoDetector.IsPortableFramework)
                suffix = suffix + ".portable";

            if (PlatformAutoDetector.IsRunningOnClr4)
                suffix = suffix + ".clr4";
            else
                suffix = suffix + ".clr2";

#if DOTNET_CORE
			suffix += ".netcore";
#endif

            if (PlatformAutoDetector.IsRunningOnAOT)
                suffix = suffix + ".aot";

            return GetPlatformNamePrefix() + "." + suffix;
        }

        private string GetUnityRuntimeName()
        {
#if ENABLE_MONO
	return "mono";
#elif ENABLE_IL2CPP
	return "il2cpp";
#elif ENABLE_DOTNET
	return "dotnet";
#else
            return "unknown";
#endif
        }

        private string GetUnityPlatformName()
        {
#if UNITY_STANDALONE_OSX
			return "OSX";
#elif UNITY_STANDALONE_WIN
			return "WIN";
#elif UNITY_STANDALONE_LINUX
			return "LINUX";
#elif UNITY_STANDALONE
			return "STANDALONE";
#elif UNITY_WII
			return "WII";
#elif UNITY_IOS
			return "IOS";
#elif UNITY_IPHONE
			return "IPHONE";
#elif UNITY_ANDROID
			return "ANDROID";
#elif UNITY_PS3
			return "PS3";
#elif UNITY_PS4
			return "PS4";
#elif UNITY_SAMSUNGTV
			return "SAMSUNGTV";
#elif UNITY_XBOX360
			return "XBOX360";
#elif UNITY_XBOXONE
			return "XBOXONE";
#elif UNITY_TIZEN
			return "TIZEN";
#elif UNITY_TVOS
			return "TVOS";
#elif UNITY_WP_8_1
			return "WP_8_1";
#elif UNITY_WSA_10_0
			return "WSA_10_0";
#elif UNITY_WSA_8_1
			return "WSA_8_1";
#elif UNITY_WSA
			return "WSA";
#elif UNITY_WINRT_10_0
			return "WINRT_10_0";
#elif UNITY_WINRT_8_1
			return "WINRT_8_1";
#elif UNITY_WINRT
			return "WINRT";
#elif UNITY_WEBGL
			return "WEBGL";
#else
            return "UNKNOWNHW";
#endif
        }

        /// <summary>
        /// print函数调用的默认处理程序，可以在ScriptOptions中定制。
        /// </summary>
        /// <param name="content"></param>
        public abstract void DefaultPrint(string content);

        /// <summary>
        /// DEPRECATED.
        /// This is kept for backward compatibility, see the overload taking a prompt as an input parameter.
        /// 
        /// Default handler for interactive line input calls. Can be customized in ScriptOptions.
        /// If an inheriting class whants to give a meaningful implementation, this method MUST be overridden.
        /// </summary>
        /// <returns>null</returns>
        [Obsolete("Replace with DefaultInput(string)")]
        public virtual string DefaultInput()
        {
            return null;
        }

        /// <summary>
        /// 交互式命令行输入调用的缺省处理程序。可以在ScriptOptions中设置。
        /// 如果继承的类提供了一个有意义的实现，那么这个方法必须被重写。
        /// </summary>
        /// <returns>null</returns>
        public virtual string DefaultInput(string prompt)
        {
#pragma warning disable 618
            return DefaultInput();
#pragma warning restore 618
        }

        /// <summary>
        /// io模块用来打开文件的函数
        /// 如果可以有一个无效的实现的io模块是过滤掉。
        /// 它应该返回一个给定文件的正确地初始化访问流
        /// </summary>
        /// <param name="script"></param>
        /// <param name="filename">The filename.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mode">The mode (as per Lua usage - e.g. 'w+', 'rb', etc.).</param>
        /// <returns></returns>
        public abstract Stream IO_OpenFile(Script script, string filename, Encoding encoding, string mode);


        /// <summary>
        /// 得到一个标准的流(stdin、stdout stderr)。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public abstract Stream IO_GetStandardStream(StandardFileType type);


        /// <summary>
        /// 临时文件名。用于“输入输出”和“操作系统”模块。
        /// 如果可以有一个无效的实现“输入输出”和“操作系统”模块是过滤掉。
        /// </summary>
        /// <returns></returns>
        public abstract string IO_OS_GetTempFilename();

        /// <summary>
        /// 退出过程,返回指定的退出代码。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        public abstract void OS_ExitFast(int exitCode);


        /// <summary>
        /// 检查一个文件是否存在。由“os”模块使用。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        public abstract bool OS_FileExists(string file);


        /// <summary>
        /// 删除指定的文件。由“os”模块使用。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="file">The file.</param>
        public abstract void OS_FileDelete(string file);


        /// <summary>
        /// 移动指定的文件。由“os”模块使用。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dst">The DST.</param>
        public abstract void OS_FileMove(string src, string dst);


        /// <summary>
        /// 执行指定的命令行，同时返回子进程退出代码和阻塞。
        /// Can have an invalid implementation if the 'os' module is filtered out.
        /// </summary>
        /// <param name="cmdline">The cmdline.</param>
        /// <returns></returns>
        public abstract int OS_Execute(string cmdline);

        /// <summary>
        /// Filters the CoreModules enumeration to exclude non-supported operations
        /// </summary>
        /// <param name="module">The requested modules.</param>
        /// <returns>
        /// The requested modules, with unsupported modules filtered out.
        /// </returns>
        public abstract CoreModules FilterSupportedCoreModules(CoreModules module);

        /// <summary>
        /// 得到一个环境变量。必须实现，但是如果一个更有意义的实现不能实现或不希望实现，则允许始终返回null。
        /// </summary>
        /// <param name="envvarname"></param>
        /// <returns>环境变量值，如果没有找到，则为null</returns>
        public abstract string GetEnvironmentVariable(string envvarname);

        /// <summary>
        /// 决定应用程序是否运行在AOT(提前)模式
        /// </summary>
        /// <returns></returns>
        public virtual bool IsRunningOnAOT()
        {
            return PlatformAutoDetector.IsRunningOnAOT;
        }
    }
}
