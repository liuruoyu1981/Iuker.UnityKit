using System;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 逻辑操作表达式
    /// </summary>
    public class LogicalExpression : Expression
    {
        public LogicalOperator Operator;
        public Expression Left;
        public Expression Right;

        /// <summary>
        /// 尝试解析给定的字符串为逻辑操作类型
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static LogicalOperator ParseLogicalOperator(string op)
        {
            switch (op)
            {
                case "&&":
                    return LogicalOperator.LogicalAnd;
                case "||":
                    return LogicalOperator.LogicalOr;

                default:
                    throw new ArgumentOutOfRangeException("Invalid binary operator: " + op);
            }
        }


    }
}