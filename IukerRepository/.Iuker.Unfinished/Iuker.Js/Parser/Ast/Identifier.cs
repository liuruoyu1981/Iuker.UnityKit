namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 标识符
    /// </summary>
    public class Identifier : Expression, IPropertyKeyExpression
    {
        public string Name;
        public string GetKey() => Name;
    }
}