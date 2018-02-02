namespace Iuker.Js.Parser
{
    /// <summary>
    /// 单词类型
    /// </summary>
    public enum Tokens
    {
        /// <summary>
        /// 布尔值字面量
        /// </summary>
        BooleanLiteral = 1,

        /// <summary>
        /// 结束哨兵字符
        /// </summary>
        EOF = 2,

        /// <summary>
        /// 标识符
        /// </summary>
        Identifier = 3,

        /// <summary>
        /// 关键字
        /// </summary>
        Keyword = 4,

        /// <summary>
        /// Null字面量
        /// </summary>
        NullLiteral = 5,

        /// <summary>
        /// 数字字面量
        /// </summary>
        NumericLiteral = 6,

        /// <summary>
        /// 
        /// </summary>
        Punctuator = 7,

        /// <summary>
        /// 字符串字面量
        /// </summary>
        StringLiteral = 8,

        /// <summary>
        /// 正则表达式
        /// </summary>
        RegularExpression = 9
    }
}