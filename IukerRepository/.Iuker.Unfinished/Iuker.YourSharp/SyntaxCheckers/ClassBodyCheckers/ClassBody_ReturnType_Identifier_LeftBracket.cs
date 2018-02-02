/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/24 08:25
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
    public class ClassBody_ReturnType_Identifier_LeftBracket : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (analyzer.IsFiledOrArgumentType(next))
            {

            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } =
            SyntaxSituation.ClassBody_ReturnType_Identifier_LeftBracket;
    }
}
