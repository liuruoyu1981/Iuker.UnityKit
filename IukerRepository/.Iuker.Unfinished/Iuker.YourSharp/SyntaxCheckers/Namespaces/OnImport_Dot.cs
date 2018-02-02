
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers.Namespaces
{
    public class OnImport_Dot : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            if (next.IsFuncPropNsIdentifier && next.NexType != TokenType.Dot)   //  导入新的命名空间
            {
                return analyzer.CallChekcer(SyntaxSituation.OnImport);
            }

            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.OnImport_Dot;
    }
}