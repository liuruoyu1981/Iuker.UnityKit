/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 17:00
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


using Iuker.YourSharp.Asts.SyntaxNodes;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers
{
    /// <summary>
    /// 访问权限定义语法情境语法检查器
    /// </summary>
    public class AccessDefine : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer syntaxAnalyzer)
        {
            // 类头部声明
            if (syntaxAnalyzer.SyntaxAnalyzeType == SyntaxAnalyzeType.ClassHeadDefine)
            {
                if (next.TokenType == TokenType.Class) // public class 
                {

                }
                if (next.TokenType == TokenType.Interface)   // public protected interface
                {

                }
                if (next.TokenType == TokenType.Abstract)    // public abstract
                {

                }

                return SyntaxAnalyzerResult.Failed;
            }


            if (syntaxAnalyzer.SyntaxAnalyzeType == SyntaxAnalyzeType.ClassBodyDefine)
            {
                switch (next.TokenType)
                {
                    case TokenType.Static:  //  静态字段：public static
                        AccessDefineNode accessDefine = new AccessDefineNode(next);
                        return syntaxAnalyzer.CallChekcer(SyntaxSituation.AccessDefind_Staic);
                    case TokenType.Readonly:    // 实例只读字段：public readonly
                        return syntaxAnalyzer.CallChekcer(SyntaxSituation.AccessDefind_Readonly);
                }
            }

            return syntaxAnalyzer.CallChekcer(SyntaxSituation.AccessDefind_Field);  // 实例可写字段：public int
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.AccessDefine;
    }
}
