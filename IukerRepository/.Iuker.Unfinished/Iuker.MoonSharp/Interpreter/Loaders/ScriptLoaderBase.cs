/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:42:05
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
using System.Linq;
using Iuker.MoonSharp.Interpreter.DataTypes;
using Iuker.MoonSharp.Loaders;

namespace Iuker.MoonSharp.Interpreter.Loaders
{
    /// <summary>
    /// IScriptLoader的基本实现,提供模块名解析
    /// </summary>
    public abstract class ScriptLoaderBase : IScriptLoader
    {
        /// <summary>
        /// 检查一个脚本文件是否存在。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool ScriptFileExists(string name);

        /// <summary>
        /// 打开一个用于读取脚本代码的文件。
        /// 文件应该返回一个字符串、字节数组或者流。
        /// 如果返回的是一个字节数组，则会被认为是序列化了的字节码，如果返回的是一个字符串，则会被假设为一个脚本或者一个字符串代码块的调用输出。
        /// 如果是一个流对象，则会开始自动检测。
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="globalContext">The global context.</param>
        /// <returns>
        /// 字符串、字节数组、或流
        /// </returns>
        public abstract object LoadFile(string file, Table globalContext);



        /// <summary>
        /// 在一组路径上解析模块的名称。
        /// lua模块解析规则实现
        /// </summary>
        /// <param name="modname">The modname.</param>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        protected virtual string ResolveModuleName(string modname, string[] paths)
        {
            if (paths == null)
                return null;

            modname = modname.Replace('.', '/');

            foreach (string path in paths)
            {
                string file = path.Replace("?", modname);

                if (ScriptFileExists(file))
                    return file;
            }

            return null;
        }

        /// <summary>
        /// Resolves the name of a module to a filename (which will later be passed to OpenScriptFile).
        /// The resolution happens first on paths included in the LUA_PATH global variable (if and only if
        /// the IgnoreLuaPathGlobal is false), and - if the variable does not exist - by consulting the
        /// ScriptOptions.ModulesPaths array. Override to provide a different behaviour.
        /// </summary>
        /// <param name="modname">The modname.</param>
        /// <param name="globalContext">The global context.</param>
        /// <returns></returns>
        public virtual string ResolveModuleName(string modname, Table globalContext)
        {
            if (!this.IgnoreLuaPathGlobal)
            {
                DynValue s = globalContext.RawGet("LUA_PATH");

                if (s != null && s.Type == DataType.String)
                    return ResolveModuleName(modname, UnpackStringPaths(s.String));
            }

            return ResolveModuleName(modname, this.ModulePaths);
        }

        /// <summary>
        /// 获取或设置使用“require”函数时使用的模块路径。如果使用null，则使用默认路径(使用环境变量等)。
        /// </summary>
        public string[] ModulePaths { get; set; }


        /// <summary>
        /// 解包路径字符串的形式像“?;?。lua"到一个数组
        /// Unpacks a string path in a form like "?;?.lua" to an array
        /// </summary>
        public static string[] UnpackStringPaths(string str)
        {
            return str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }

        /// <summary>
        /// 获取默认的环境路径。
        /// </summary>
        public static string[] GetDefaultEnvironmentPaths()
        {
            string[] modulePaths = null;

            if (modulePaths == null)
            {
                string env = Script.GlobalOptions.Platform.GetEnvironmentVariable("MOONSHARP_PATH");
                if (!string.IsNullOrEmpty(env)) modulePaths = UnpackStringPaths(env);

                if (modulePaths == null)
                {
                    env = Script.GlobalOptions.Platform.GetEnvironmentVariable("LUA_PATH");
                    if (!string.IsNullOrEmpty(env)) modulePaths = UnpackStringPaths(env);
                }

                if (modulePaths == null)
                    modulePaths = UnpackStringPaths("?;?.lua");
            }

            return modulePaths;
        }

        /// <summary>
        /// 解析一个文件名(应用路径等。)
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="globalContext">The global context.</param>
        /// <returns></returns>
        public virtual string ResolveFileName(string filename, Table globalContext)
        {
            return filename;
        }

        /// <summary>
        /// 获取或设置一个值指示是否LUA_PATH全球检查路径模块包含。
        /// Gets or sets a value indicating whether the LUA_PATH global is checked or not to get the path where modules are contained.
        /// 如果为真，则全局lua路径不检查
        /// If true, the LUA_PATH global is NOT checked.
        /// </summary>
        public bool IgnoreLuaPathGlobal { get; set; }





    }
}
