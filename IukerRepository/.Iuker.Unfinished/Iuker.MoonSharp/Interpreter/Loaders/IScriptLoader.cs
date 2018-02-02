/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 10:17:52
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
using Iuker.MoonSharp.Interpreter;

namespace Iuker.MoonSharp.Loaders
{
    /// <summary>
    /// 该接口定义了从文件加载脚本的请求是如何处理的
    /// 建议不通过继承而是直接实现该接口， and rather extend ScriptLoaderBase.
    /// </summary>
    public interface IScriptLoader
    {
        /// <summary>
        /// 打开一个用于读取脚本代码的文件。
        /// 文件应该返回一个字符串、字节数组或者流。
        /// 如果返回的是一个字节数组，则会被认为是序列化了的字节码，如果返回的是一个字符串，则会被假设为一个脚本或者一个字符串代码块的调用输出。
        /// 如果是一个流对象，则会开始自动检测。
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="globalContext">The global context.</param>
        /// <returns>
        /// A string, a byte[] or a Stream.
        /// </returns>
        object LoadFile(string file, Table globalContext);
        /// <summary>
        /// 解析一个文件名(应用路径等。)
        /// Resolves a filename [applying paths, etc.]
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="globalContext">The global context.</param>
        /// <returns></returns>
        [Obsolete("这个方法几乎没有任何用途。保留它只是为了保持向后兼容性")]
        string ResolveFileName(string filename, Table globalContext);
        /// <summary>
        /// 将模块名解析为一个文件名（稍后会被传递给OpenScriptFile方法）
        /// </summary>
        /// <param name="modname">The modname.</param>
        /// <param name="globalContext">The global context.</param>
        /// <returns></returns>
        string ResolveModuleName(string modname, Table globalContext);

    }
}
