using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 序列表达式
    /// </summary>
    public class SequenceExpression : Expression
    {
        public IList<Expression> Expressions;
    }
}