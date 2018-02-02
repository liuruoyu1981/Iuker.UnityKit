using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    public class SwitchCase : SyntaxNode
    {
        public Expression Test;
        public IEnumerable<Statement> Consequent;

    }
}