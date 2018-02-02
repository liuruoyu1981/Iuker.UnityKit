using System.Collections.Generic;
using Iuker.Js.Parser.Ast;

namespace Iuker.Js.Parser
{
    public interface IFunctionScope:IVariableScope
    {
        IList<FunctionDeclaration> FunctionDeclarations { get; set; }
    }
}