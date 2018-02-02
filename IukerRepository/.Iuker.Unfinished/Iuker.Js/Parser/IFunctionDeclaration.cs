using System.Collections.Generic;
using Iuker.Js.Parser.Ast;

namespace Iuker.Js.Parser
{
    /// <summary>
    /// 函数声明
    /// </summary>
    interface IFunctionDeclaration
    {
        Identifier Id { get; }
        IEnumerable<Identifier> Parameters { get; }
        Statement Body { get; }
        bool Strict { get; }
    }
}
