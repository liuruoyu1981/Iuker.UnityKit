/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 08:46
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


using System;
using Iuker.YourSharp.Asts.Semantic;
using Iuker.YourSharp.Asts.SyntaxNodes;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers
{
    /// <summary>
    /// 函数返回类型或成员定义
    /// 成员可用是全局成员也可以是类成员
    /// </summary>
    public class ReturnTypeOrMemberDefine : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            switch (analyzer.SyntaxAnalyzeType)
            {
                case SyntaxAnalyzeType.FunctionDefine:
                    return SyntaxAnalyzerResult.Failed;
                case SyntaxAnalyzeType.FunctionBodyDefine:
                    return InFunctionBody(next, analyzer);
                case SyntaxAnalyzeType.ClassBodyDefine:
                    return analyzer.CallChekcer(SyntaxSituation.ClassBody_ReturnType);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SyntaxAnalyzerResult InFunctionBody(Token current, SyntaxAnalyzer analyzer)
        {
            // todo 暂时不考虑属性
            // 函数标识符
            if (current.IsFuncPropNsIdentifier)
            {
                // 预读单词
                if (current.NexType == TokenType.LeftBracket) //  函数
                {
                    // 如果当前不是处于函数体分析阶段则创建一个函数定义上下文对象
                    if (analyzer.SyntaxAnalyzeType != SyntaxAnalyzeType.FunctionBodyDefine)
                    {
                        return OnFunctionDefine(current, analyzer);
                    }
                }
            }
            else
            {
                // 全局成员定义
                if (analyzer.SyntaxAnalyzeType != SyntaxAnalyzeType.FunctionBodyDefine)
                {
                    return OnMemberDefine(current, analyzer);
                }
                // 函数体局部变量定义

                return OnFunctionFieldDefine(current, analyzer);
            }
            return SyntaxAnalyzerResult.Failed;
        }

        /// <summary>
        /// 当处于函数定义分析阶段时
        /// </summary>
        /// <param name="nextToken"></param>
        /// <param name="analyzer"></param>
        private SyntaxAnalyzerResult OnFunctionDefine(Token nextToken, SyntaxAnalyzer analyzer)
        {
            FunctionDefineContext functionDefineContext = new FunctionDefineContext();
            functionDefineContext.Insert(new ReturnTypeNode(nextToken))
                .Insert(new FunctionNameNode(nextToken));
            // 切换语法分析器的分析状态
            analyzer.SyntaxAnalyzeType = SyntaxAnalyzeType.FunctionDefine;

            if (!analyzer.IsInGlobal)    //  全局函数声明
            {
                functionDefineContext.SetScope(analyzer.CurrentClassScope);
            }

            // 将函数定义上下文实例赋值给语义分析器的临时函数上下文对象
            // 这里是为了支持函数重载机制
            analyzer.TempFunctionDefineContext = functionDefineContext;

            return analyzer.CallChekcer(SyntaxSituation.FunctionDefine_LeftBracket);
        }

        /// <summary>
        /// 当处于作用域成员定义阶段时
        /// </summary>
        /// <param name="nextToken"></param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        private SyntaxAnalyzerResult OnMemberDefine(Token nextToken, SyntaxAnalyzer analyzer)
        {

            return SyntaxAnalyzerResult.Failed;
        }


        /// <summary>
        /// 当处于函数体字段（局部变量）定义阶段时
        /// </summary>
        /// <param name="nextToken"></param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        private SyntaxAnalyzerResult OnFunctionFieldDefine(Token nextToken, SyntaxAnalyzer analyzer)
        {
            if (analyzer.IsReturnType(nextToken.PrevToken))
            {
                analyzer.CurrentFunctionContext.InsertFunctionFiled(new FunctionFiledNode(nextToken,
                    nextToken.PrevToken));
                return SyntaxAnalyzerResult.Succeed;
            }

            return SyntaxAnalyzerResult.Failed;
        }







        public override string ProcessedSituation { get; } = SyntaxSituation.ReturnType;
    }
}
