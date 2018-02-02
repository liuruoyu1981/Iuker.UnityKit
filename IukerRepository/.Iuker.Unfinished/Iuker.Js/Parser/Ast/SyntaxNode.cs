using System.Diagnostics;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 语法树节点
    /// </summary>
    public class SyntaxNode
    {
        public SyntaxNodes Type;
        public int[] Range;
        public Location Location;

        /// <summary>
        /// 转为具体的语法树子类类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T As<T>() where T : SyntaxNode => (T)this;

    }
}
