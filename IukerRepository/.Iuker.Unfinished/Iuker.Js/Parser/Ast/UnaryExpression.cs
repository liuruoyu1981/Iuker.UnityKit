using System;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 一元操作表达式
    /// </summary>
    public class UnaryExpression : Expression
    {
        public UnaryOperator Operator;
        public Expression Argument;

        /// <summary>
        /// 前缀
        /// </summary>
        public bool Prefix;

        /// <summary>
        /// 尝试解析指定的字符串为一元操作符类型
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static UnaryOperator ParseUnaryOperator(string op)
        {
            switch (op)
            {
                case "+":
                    return UnaryOperator.Plus;
                case "-":
                    return UnaryOperator.Minus;
                case "++":
                    return UnaryOperator.Increment;
                case "--":
                    return UnaryOperator.Decrement;
                case "~":
                    return UnaryOperator.BitwiseNot;
                case "!":
                    return UnaryOperator.LogicalNot;
                case "delete":
                    return UnaryOperator.Delete;
                case "void":
                    return UnaryOperator.Void;
                case "typeof":
                    return UnaryOperator.TypeOf;

                default:
                    throw new ArgumentOutOfRangeException("Invalid unary operator: " + op);

            }
        }






    }
}