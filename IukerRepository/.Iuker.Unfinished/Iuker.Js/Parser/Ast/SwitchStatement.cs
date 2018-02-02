using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    public class SwitchStatement : Statement
    {
        /// <summary>
        /// 条件判断表达式
        /// </summary>
        public Expression Discriminant;


        public IEnumerable<SwitchCase> Cases;
    }
}