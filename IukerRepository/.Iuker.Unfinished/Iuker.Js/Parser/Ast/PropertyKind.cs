namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 属性种类
    /// </summary>
    public enum PropertyKind : byte
    {
        /// <summary>
        /// 数据
        /// </summary>
        Data = 1,

        /// <summary>
        /// Get取值器
        /// </summary>
        Get = 2,

        /// <summary>
        /// Set赋值器
        /// </summary>
        Set = 4,


    }
}