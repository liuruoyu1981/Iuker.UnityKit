using System.Collections.Generic;
using Iuker.Js.Parser.Ast;

namespace Iuker.Js.Parser
{
    /// <summary>
    /// 变量作用域
    /// 用于在特定范围内对所有变量进行安全引用。
    /// </summary>
    public interface IVariableScope
    {
        IList<VariableDeclaration> VariableDeclarations { get; set; }
    }
}