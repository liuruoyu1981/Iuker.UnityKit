/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 19:53
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
    /// 赋值表达式
    /// </summary>
    public class AssignmentExpression : Expression
    {
        /// <summary>
        /// 赋值操作类型
        /// </summary>
        public AssignmentOperatorType OperatorType;

        /// <summary>
        /// 赋值左表达式
        /// </summary>
        public Expression Left;

        /// <summary>
        /// 赋值右表达式
        /// </summary>
        public Expression Right;

        /// <summary>
        /// 尝试解析传入的字符串为赋值操作类型
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static AssignmentOperatorType ParseAssignmentOperatorType(string op)
        {
            switch (op)
            {
                case "=":
                    return AssignmentOperatorType.Assign;
                case "+=":
                    return AssignmentOperatorType.PlusAssign;
                case "-=":
                    return AssignmentOperatorType.MinusAssign;
                case "*=":
                    return AssignmentOperatorType.TimesAssign;
                case "/=":
                    return AssignmentOperatorType.DivideAssign;
                case "%=":
                    return AssignmentOperatorType.ModuloAssign;
                case "&=":
                    return AssignmentOperatorType.BitwiseAndAssign;
                case "|=":
                    return AssignmentOperatorType.BitwiseOrAssign;
                case "^=":
                    return AssignmentOperatorType.BitwiseXOrAssign;
                //case "<<=":
                //    return AssignmentOperatorType.LeftShiftAssign;
                //case ">>=":
                //    return AssignmentOperatorType.RightShiftAssign;
                //case ">>>=":
                    //return AssignmentOperatorType.UnsignedRightShiftAssign;

                default:
                    throw new ArgumentOutOfRangeException("Invalid assignment operator: " + op);
            }
        }











    }
}