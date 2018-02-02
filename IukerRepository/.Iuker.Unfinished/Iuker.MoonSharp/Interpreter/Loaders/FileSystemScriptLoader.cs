/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:41:40
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

namespace Iuker.MoonSharp.Interpreter.Loaders
{
    /// <summary>
    /// 直接从文件系统加载脚本的脚本加载器(不通过平台对象)
    /// </summary>
    public class FileSystemScriptLoader : ScriptLoaderBase
    {
        /// <summary>
		/// 检查一个脚本文件是否存在。
		/// </summary>
		/// <param name="name">The script filename.</param>
		/// <returns></returns>
		public override bool ScriptFileExists(string name)
        {
            return File.Exists(name);
        }

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
        public override object LoadFile(string file, Table globalContext)
        {
            return new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}
