/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 18:52
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
    /// 函数参数=>类型检查或右括号结束
    /// </summary>
    public class ArgumentTypeOrRightBracket : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (analyzer.IsFiledOrArgumentType(next))    // 字段或参数类型
            {
                analyzer.TempFunctionDefineContext.TempArgumentNode = new ArgumentNode(next);
                return analyzer.CallChekcer(SyntaxSituation.ArgumentIdentifier);
            }


            // todo 无需对右括号进行检查，因为在逗号分隔检查器里已经在右括号分之下返回已完成状态。


            //if (next.TokenType == TokenType.RightBracket)  //  右括号
            //{
            //    if (next.TokenIndex != next.NexToken.TokenIndex)  //  当前行单词流已结束
            //    {
            //        return SyntaxAnalyzerResult.Completed;  //  返回分析完成
            //    }
            //    return SyntaxAnalyzerResult.Failed; // 返回失败存在语法错误
            //}

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.ArgumentTypeOrRightBracket;
    }
}
