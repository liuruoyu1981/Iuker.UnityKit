using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 函数表达式
    /// </summary>
    public class FunctionExpression : Expression, IFunctionDeclaration
    {
        public FunctionExpression()
        {
            VariableDeclarations = new List<VariableDeclaration>();
        }

        public Identifier Id { get; set; }
        public IEnumerable<Identifier> Parameters { get; set; }
        public Statement Body { get; set; }
        public bool Strict { get; set; }

        public IList<VariableDeclaration> VariableDeclarations { get; set; }
        public IList<FunctionDeclaration> FunctionDeclarations { get; set; }

        #region ECMA6
        public IEnumerable<Expression> Defaults;
        public SyntaxNode Rest;
        public bool Generator;
        public bool Expression;
        #endregion
    }
}