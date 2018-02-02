namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 一元操作符类型
    /// </summary>
    public enum UnaryOperator
    {
        Plus,
        Minus,
        BitwiseNot,
        LogicalNot,
        Delete,
        Void,
        TypeOf,
        Increment,
        Decrement,
    }
}