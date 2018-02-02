using System.Collections.Generic;
using Iuker.Js.Native;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 调用表达式
    /// </summary>
    public class CallExpression : Expression
    {
        /// <summary>
        /// 被调用的表达式
        /// </summary>
        public Expression Callee;

        /// <summary>
        /// 被调用的表达式（函数）的参数列表
        /// </summary>
        public IList<Expression> Arguments;

        /// <summary>
        /// 是否已缓存
        /// </summary>
        public bool Cached;

        /// <summary>
        /// 能否缓存
        /// 默认为真
        /// </summary>
        public bool CanBeCached = true;

        /// <summary>
        /// 已缓存的参数值
        /// </summary>
        public JsValue[] CachedArguments;


    }
}