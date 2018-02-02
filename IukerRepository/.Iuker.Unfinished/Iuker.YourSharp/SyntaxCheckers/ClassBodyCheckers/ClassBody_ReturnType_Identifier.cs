/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/22 12:07
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

namespace Iuker.YourSharp.Asts.SyntaxCheckers.ClassBodyCheckers
{
    public class ClassBody_ReturnType_Identifier : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            // 字段初始化并赋值
            if (next.TokenType == TokenType.Assignment && next.PrevToken.PrevToken.TokenType != TokenType.Void)
            {
                return analyzer.CallChekcer(SyntaxSituation.ClassBody_ReturnType_Identifier_Assignment);
            }

            // 左括号，函数声明
            if (next.TokenType == TokenType.LeftBracket)
            {

            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.ClassBody_ReturnType_Identifier;
    }
}
