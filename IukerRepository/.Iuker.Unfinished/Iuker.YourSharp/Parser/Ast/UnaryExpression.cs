/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 17:51
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
    /// 一元操作表达式
    /// </summary>
    public class UnaryExpression : Expression
    {
        /// <summary>
        /// 一元操作类型
        /// </summary>
        public UnaryOperatorType OperatorType;

        /// <summary>
        /// 操作参数表达式
        /// </summary>
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
        public static UnaryOperatorType ParseUnaryOperatorType(string op)
        {
            switch (op)
            {
                case "+":
                    return UnaryOperatorType.Plus;
                case "-":
                    return UnaryOperatorType.Minus;
                case "++":
                    return UnaryOperatorType.Increment;
                case "--":
                    return UnaryOperatorType.Decrement;
                //case "~":
                //    return UnaryOperatorType.BitwiseNot;
                //case "!":
                //    return UnaryOperatorType.LogicalNot;
                //case "delete":
                //    return UnaryOperatorType.Delete;
                //case "void":
                //    return UnaryOperatorType.Void;
                //case "typeof":
                //    return UnaryOperatorType.TypeOf;

                default:
                    throw new ArgumentOutOfRangeException("Invalid unary operator: " + op);

            }
        }


    }
}