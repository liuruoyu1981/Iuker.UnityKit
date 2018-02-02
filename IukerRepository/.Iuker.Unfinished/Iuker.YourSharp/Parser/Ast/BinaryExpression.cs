/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 20:05
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;

namespace Iuker.YourSharp.Parser.Ast
{
    /// <summary>
    /// 二元操作表达式
    /// </summary>
    public class BinaryExpression : Expression
    {

        /// <summary>
        /// 二元操作类型
        /// </summary>
        public BinaryOperatorType OperatorType;

        /// <summary>
        /// 二元表达式左子树
        /// </summary>
        public Expression Left;

        /// <summary>
        /// 二元表达式右子树
        /// </summary>
        public Expression Right;

        public static BinaryOperatorType ParseBinaryOperator(string op)
        {
            switch (op)
            {
                case "+":
                    return BinaryOperatorType.Plus;
                case "-":
                    return BinaryOperatorType.Minus;
                case "*":
                    return BinaryOperatorType.Times;
                case "/":
                    return BinaryOperatorType.Divide;
                case "%":
                    return BinaryOperatorType.Modulo;
                case "==":
                    return BinaryOperatorType.Equal;
                case "!=":
                    return BinaryOperatorType.NotEqual;
                case ">":
                    return BinaryOperatorType.Greater;
                case ">=":
                    return BinaryOperatorType.GreaterOrEqual;
                case "<":
                    return BinaryOperatorType.Less;
                case "<=":
                    return BinaryOperatorType.LessOrEqual;
                case "===":
                    return BinaryOperatorType.StrictlyEqual;
                case "!==":
                    return BinaryOperatorType.StricltyNotEqual;
                case "&":
                    return BinaryOperatorType.BitwiseAnd;
                case "|":
                    return BinaryOperatorType.BitwiseOr;
                case "^":
                    return BinaryOperatorType.BitwiseXOr;
                case "<<":
                    return BinaryOperatorType.LeftShift;
                case ">>":
                    return BinaryOperatorType.RightShift;
                case ">>>":
                    return BinaryOperatorType.UnsignedRightShift;
                case "instanceof":
                    return BinaryOperatorType.InstanceOf;
                case "in":
                    return BinaryOperatorType.In;

                default:
                    throw new ArgumentOutOfRangeException("Invalid binary operator: " + op);
            }
        }






    }
}