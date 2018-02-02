/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 16:12
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


using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers
{
    /// <summary>
    /// 语法分析检测器
    /// </summary>
    public interface ISyntaxChecker
    {
        /// <summary>
        /// 判断输入的单词是否合法
        /// </summary>
        /// <param name="next"></param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer);

        /// <summary>
        /// 语法错误处理
        /// </summary>
        /// <param name="token"></param>
        void OnError(Token token);

        /// <summary>
        /// 语法检查器所处理的语法情境。
        /// </summary>
        string ProcessedSituation { get; }


    }
}
