/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/22 09:57
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

namespace Iuker.YourSharp.Asts.SyntaxCheckers.ClassHead
{
    /// <summary>
    /// 类头部定义=>当前单词为访问权限类单词（public internal）
    /// </summary>
    public class ClassHead_Access : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (next.TokenType == TokenType.Class)   // public class
            {

            }
            if (next.TokenType == TokenType.Static)  // internal static
            {

            }
            if (next.TokenType == TokenType.Abstract)    //   public abstract
            {

            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.ClassHead_Access;
    }
}
