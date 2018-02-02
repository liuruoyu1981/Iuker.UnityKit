using System.Collections.Generic;
using Iuker.Js.Parser.Ast;

namespace Iuker.Js.Parser
{
    /// <summary>
    /// 函数作用域
    /// </summary>
    public class FunctionScope : IFunctionScope
    {
        public FunctionScope()
        {
            FunctionDeclarations = new List<FunctionDeclaration>();
        }

        public IList<FunctionDeclaration> FunctionDeclarations { get; set; }
        public IList<VariableDeclaration> VariableDeclarations { get; set; }
    }
}