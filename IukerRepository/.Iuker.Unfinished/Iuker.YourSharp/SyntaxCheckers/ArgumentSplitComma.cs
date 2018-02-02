/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 17:15
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
    /// 函数声明=>逗号分隔检查
    /// 若遇到右括号则说明当前单词流的语法检查已结束
    /// </summary>
    public class ArgumentSplitComma : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (next.TokenType == TokenType.Comma)
            {
                // 逗号，说明参数声明已结束。
                // 将当前的临时参数节点其插入临时函数上下文的参数列表。
                analyzer.TempFunctionDefineContext.InsertTempArgument();
                return analyzer.CallChekcer(SyntaxSituation.ArgumentTypeOrRightBracket);
            }

            // 函数参数默认值检查
            if (next.TokenType == TokenType.Assignment)
            {
                return analyzer.CallChekcer(SyntaxSituation.Argument_Defualt_Quate_Or_BaseValue);
            }

            // todo 函数声明检查结束
            // 当前不支持泛型函数声明

            if (next.TokenType == TokenType.RightBracket)
            {
                // 将临时参数节点插入临时函数上下文的参数节点字典
                analyzer.TempFunctionDefineContext.InsertTempArgument();
                // 解析临时函数上下文的全命名并尝试将其插入语义分析器的函数上下文字典中
                analyzer.InsertFunctionContext(analyzer.TempFunctionDefineContext);

                // 扫描正常结束，调用分析器进入下一行单词分析。
                if (next.NexToken.TokenIndex != next.TokenIndex)
                {
                    // 切换语法分析器状态为函数体分析
                    analyzer.SyntaxAnalyzeType = SyntaxAnalyzeType.FunctionBodyDefine;
                    return SyntaxAnalyzerResult.Completed;
                }
                return SyntaxAnalyzerResult.Failed;
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.ArgumentSplitComma;
    }
}
