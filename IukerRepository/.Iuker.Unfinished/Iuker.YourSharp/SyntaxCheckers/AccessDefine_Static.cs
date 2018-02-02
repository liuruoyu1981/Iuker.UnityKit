/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 17:24
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
    public class AccessDefine_Static : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer syntaxAnalyzer)
        {
            if (next.TokenType == TokenType.Readonly)
                return syntaxAnalyzer.CallChekcer(SyntaxSituation.AccessDefind_Staic_Readonly);

            if (syntaxAnalyzer.IsFiledOrArgumentType(next))
            {
                return syntaxAnalyzer.CallChekcer(SyntaxSituation.AccessDefind_Staic_Field);
            }

            if (syntaxAnalyzer.IsReturnType(next))
            {
                return syntaxAnalyzer.CallChekcer(SyntaxSituation.AccessDefine_Static_ReturnType);
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.AccessDefind_Staic;
    }
}
