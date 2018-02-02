
using Iuker.YourSharp.Asts.Semantic;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers.Namespaces
{
    public class OnImport : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (analyzer.CurrentClassContext == null)
            {
                ClassDefineContext classDefineContext = new ClassDefineContext();
                analyzer.InsertClassContext(classDefineContext);
            }

            if (next.BeginLine != next.NexToken.BeginLine)  //  命名空间导入结束
            {
                if (analyzer.CurrentClassContext.NamespaceSet.Count == 0)
                {
                    return SyntaxAnalyzerResult.Failed; //  没有导入任何命名空间返回错误
                }
                return SyntaxAnalyzerResult.Completed;
            }

            if (next.IsFuncPropNsIdentifier)
            {
                if (next.NexType != TokenType.Dot)  //  导入了一个命名空间
                {
                    analyzer.CurrentClassContext.TempNameSpace += next.StringLiterals;
                    analyzer.CurrentClassContext.AddNameSpace();
                    return analyzer.CallChekcer(SyntaxSituation.OnImport);
                }
                else
                {
                    analyzer.CurrentClassContext.TempNameSpace += next.StringLiterals;
                }
            }


            analyzer.CurrentClassContext.TempNameSpace = next.StringLiterals;
            return analyzer.CallChekcer(SyntaxSituation.OnImport_Dot);
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.OnImport;
    }
}