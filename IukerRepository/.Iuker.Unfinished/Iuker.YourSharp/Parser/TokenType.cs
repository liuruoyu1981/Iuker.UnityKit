/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 16:00
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



namespace Iuker.YourSharp.Parser
{
    /// <summary>
    /// 单词类型
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// 无类型默认值
        /// </summary>
        None,

        /// <summary>
        /// 开始符
        /// </summary>
        EStart,

        /// <summary>
        /// 终结符
        /// </summary>
        Eof,

        /// <summary>
        /// 标识符
        /// </summary>
        Identifier,

        Number,

        /// <summary>
        /// 单行注释文本
        /// </summary>
        SingleCommentText,

        /// <summary>
        /// 关键字
        /// </summary>
        Keyword,

        HashBang,


        /// <summary>
        /// 加
        /// </summary>
        Add,

        /// <summary>
        /// 自增
        /// </summary>
        SelfAdd,

        /// <summary>
        /// 加等
        /// </summary>
        AddAssignment,

        /// <summary>
        /// 减
        /// </summary>
        Minus,

        /// <summary>
        /// 自减
        /// </summary>
        SelfMinus,

        /// <summary>
        /// 减等
        /// </summary>
        MinusAssignment,

        /// <summary>
        /// 乘
        /// </summary>
        Mul,

        /// <summary>
        /// 乘等
        /// </summary>
        MulAssignment,

        /// <summary>
        /// 除
        /// </summary>
        Div,

        /// <summary>
        /// 除等
        /// </summary>
        DivAssignment,

        #region Logic

        /// <summary>
        /// 小于
        /// </summary>
        Less,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessThan,

        /// <summary>
        /// 大于
        /// </summary>
        Greater,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThan,

        /// <summary>
        /// =赋值
        /// </summary>
        Assignment,

        /// <summary>
        /// ==判等
        /// </summary>
        DoubleEqual,

        #endregion

        #region 标点符号

        /// <summary>
        /// 点符号
        /// </summary>
        Dot,

        /// <summary>
        /// 冒号
        /// </summary>
        Colon,

        /// <summary>
        /// 逗号
        /// </summary>
        Comma,

        /// <summary>
        /// 左小括号
        /// </summary>
        LeftBracket,

        /// <summary>
        /// 右小括号
        /// </summary>
        RightBracket,

        /// <summary>
        /// 单引号
        /// </summary>
        SingleQuote,

        /// <summary>
        /// 双引号
        /// </summary>
        DoubleQuote,

        #endregion

        #region keyword type

        Abstract,

        As,

        Base,

        Bool,

        Break,

        Byte,

        Case,

        Catch,

        Char,

        Checked,

        Class,

        Const,

        Continue,

        Decimal,

        Default,

        Delegate,

        Do,

        Double,

        Else,

        End,

        Enum,

        Event,

        Explicit,

        Extern,

        False,

        Finally,

        Fixed,

        Float,

        For,

        Foreach,

        Goto,

        If,

        Implicit,

        In,

        Int,

        Interface,

        /// <summary>
        /// 导入命名空间
        /// </summary>
        Import,

        Is,

        Lock,

        Long,

        NameSpace,

        New,

        Null,

        Object,

        Operator,

        Out,

        Override,

        Params,

        Private = 101,

        Protected,

        Public,

        Internal,


        Readonly,

        Ref,

        Return,

        Sbyte,

        Sealed,

        Short,

        Sizeof,

        Stackallock,

        Static,

        String,

        Struct,

        Switch,

        This,

        Throw,

        True,

        Try,

        Typeof,

        Uint,

        Ulong,

        Unchecked,

        Unsafe,

        Ushort,

        Using,

        UsingStatic,

        Void,

        Volatile,

        While,

































        #endregion

        /// <summary>
        /// 单行注释
        /// </summary>
        SingleComment,

        /// <summary>
        /// 多行注释
        /// </summary>
        MultiComment,
    }
}
