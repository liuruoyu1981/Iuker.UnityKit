/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 18:54
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
    /// 函数参数=>参数标识符检查
    /// </summary>
    public class ArgumentIdentifier : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (next.IsArgumentIdentifier)
            {
                analyzer.TempFunctionDefineContext.TempArgumentNode.SetArgumentName(next);

                return analyzer.CallChekcer(SyntaxSituation.ArgumentSplitComma);
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.ArgumentIdentifier;
    }
}
