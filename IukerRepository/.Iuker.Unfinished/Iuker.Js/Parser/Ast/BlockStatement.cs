using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 语法块声明
    /// </summary>
    public class BlockStatement : Statement
    {
        public IEnumerable<Statement> Body;
    }
}