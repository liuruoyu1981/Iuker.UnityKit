/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 19:57
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

namespace Iuker.YourSharp.Parser.Ast
{
    /// <summary>
    /// 赋值操作类型
    /// </summary>
    public enum AssignmentOperatorType : byte
    {
        /// <summary>
        /// =
        /// </summary>
        Assign,

        /// <summary>
        /// +=
        /// </summary>
        PlusAssign,

        /// <summary>
        /// -=
        /// </summary>
        MinusAssign,


        TimesAssign,

        /// <summary>
        /// /=
        /// </summary>
        DivideAssign,


        ModuloAssign,
        BitwiseAndAssign,
        BitwiseOrAssign,
        BitwiseXOrAssign,
        //LeftShiftAssign,
        //RightShiftAssign,
        //UnsignedRightShiftAssign,
    }
}