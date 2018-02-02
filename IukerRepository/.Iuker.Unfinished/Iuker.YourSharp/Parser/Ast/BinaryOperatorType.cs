/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 20:06
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
    /// 二元操作类型
    /// </summary>
    public enum BinaryOperatorType : byte
    {
        /// <summary>
        /// 加
        /// </summary>
        Plus,

        /// <summary>
        /// 减
        /// </summary>
        Minus,

        /// <summary>
        /// 乘
        /// </summary>
        Times,

        /// <summary>
        /// 除
        /// </summary>
        Divide,

        /// <summary>
        /// 取模
        /// </summary>
        Modulo,

        /// <summary>
        /// 等于
        /// </summary>
        Equal,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual,

        /// <summary>
        /// 大于
        /// </summary>
        Greater,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterOrEqual,

        /// <summary>
        /// 小于
        /// </summary>
        Less,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessOrEqual,

        /// <summary>
        /// 严格判等===
        /// </summary>
        StrictlyEqual,

        /// <summary>
        /// 严格不等===
        /// </summary>
        StricltyNotEqual,
        BitwiseAnd,
        BitwiseOr,
        BitwiseXOr,
        LeftShift,
        RightShift,
        UnsignedRightShift,
        InstanceOf,
        In,



    }
}