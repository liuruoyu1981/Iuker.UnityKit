/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 20:32
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
    /// 参数默认值=>引号活默认基础类型值检查
    /// </summary>
    public class Argument_Defualt_Quate_Or_BaseValue : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            // 单引号或者双引号
            if (next.TokenType == TokenType.SingleQuote || next.TokenType == TokenType.DoubleQuote)
            {
                // 字符串默认值
                analyzer.TempFunctionDefineContext.SetTempArgumentDefaultValue(next);
                return analyzer.CallChekcer(SyntaxSituation.Argument_Default_String);
            }

            if (next.TryAsBaseType())
            {
                // 非字符串默认值
                analyzer.TempFunctionDefineContext.SetTempArgumentDefaultValue(next);
                return analyzer.CallChekcer(SyntaxSituation.ArgumentSplitComma);
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.Argument_Defualt_Quate_Or_BaseValue;
    }
}
