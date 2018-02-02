using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// New构建表达式
    /// </summary>
    public class NewExpression : Expression
    {
        public Expression Callee;
        public IEnumerable<Expression> Arguments;
    }
}