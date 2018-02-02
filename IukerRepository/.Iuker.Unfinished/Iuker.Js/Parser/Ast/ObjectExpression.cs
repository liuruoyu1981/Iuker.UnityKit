using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 对象表达式
    /// </summary>
    public class ObjectExpression : Expression
    {
        /// <summary>
        /// 对象属性列表
        /// </summary>
        public IEnumerable<Property> Properties;
    }
}