/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 18:48
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
    /// 函数定义=>左括号检查
    /// </summary>
    public class FunctionDefine_LeftBracket : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (next.TokenType == TokenType.LeftBracket)
            {
                return analyzer.CallChekcer(SyntaxSituation.ArgumentTypeOrRightBracket);
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.FunctionDefine_LeftBracket;
    }
}
