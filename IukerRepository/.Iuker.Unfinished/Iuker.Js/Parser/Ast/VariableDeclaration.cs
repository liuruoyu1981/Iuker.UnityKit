using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 变量声明
    /// </summary>
    public class VariableDeclaration : Statement
    {
        public IEnumerable<VariableDeclarator> Declarations;
        public string Kind;
    }
}