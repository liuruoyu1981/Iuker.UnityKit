namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 属性表达式
    /// </summary>
    public class Property : Expression
    {
        public PropertyKind Kind;
        public IPropertyKeyExpression Key;
        public Expression Value;
    }
}