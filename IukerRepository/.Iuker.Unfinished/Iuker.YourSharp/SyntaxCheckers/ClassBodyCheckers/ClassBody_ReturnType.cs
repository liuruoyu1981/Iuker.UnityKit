/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/22 12:03
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
    /// <summary>
    /// 类内部成员定义
    /// 类内部定义入口检查器
    /// </summary>
    public class ClassBody_ReturnType : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (next.TokenType == TokenType.Identifier)  //  标识符开头，可能是函数声明也可能是字段声明
            {
                return analyzer.CallChekcer(SyntaxSituation.ClassBody_ReturnType_Identifier);
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.ClassBody_ReturnType;
    }
}
