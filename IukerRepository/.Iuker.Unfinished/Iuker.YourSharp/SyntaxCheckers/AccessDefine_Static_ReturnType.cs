/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 18:41
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


using Iuker.YourSharp.Asts.Semantic;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers
{
    /// <summary>
    /// 静态函数声明=>返回类型节点检查
    /// </summary>
    public class AccessDefine_Static_ReturnType : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            // 这一步已经可以确定当前是一个函数声明
            if (next.IsFuncPropNsIdentifier)
            {
                FunctionDefineContext functionDefineContext = new FunctionDefineContext();
                return analyzer.CallChekcer(SyntaxSituation.FunctionDefine_LeftBracket);
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.AccessDefine_Static_ReturnType;
    }
}
