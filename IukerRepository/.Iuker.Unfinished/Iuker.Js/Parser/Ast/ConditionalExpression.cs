namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    public class ConditionalExpression : Expression
    {
        /// <summary>
        /// 条件测试表达式
        /// </summary>
        public Expression Test;

        /// <summary>
        /// 结果表达式
        /// </summary>
        public Expression Consequent;

        /// <summary>
        /// 交替表达式
        /// </summary>
        public Expression Alternate;
    }
}