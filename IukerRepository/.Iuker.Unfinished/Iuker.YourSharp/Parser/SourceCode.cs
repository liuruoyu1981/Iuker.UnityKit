/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 16:06
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



namespace Iuker.YourSharp.Parser
{
    /// <summary>
    /// 以一个脚本文件作为分割单位的源码
    /// </summary>
    public class SourceCode
    {
        /// <summary>
        /// 提供源码的源文件或代码块的名字
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 源代码
        /// </summary>
        public readonly string Codes;

        /// <summary>
        /// 按行分割之后的源代码数组
        /// </summary>
        public string[] Lines;

        /// <summary>
        /// 是否位于全局
        /// </summary>
        public bool IsInGlobal;

        /// <summary>
        /// 源代码身份Id
        /// </summary>
        public int SourceId;

        /// <summary>
        /// 构建一个源代码实例
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="sourceId"></param>
        /// <param name="isInGlobal"></param>
        public SourceCode(string name, string code, int sourceId, bool isInGlobal)
        {
            Name = name ?? "chunk " + sourceId;
            Codes = code;
            SourceId = sourceId;
            IsInGlobal = isInGlobal;

            Lines = code.Split('\n');
        }









    }
}
