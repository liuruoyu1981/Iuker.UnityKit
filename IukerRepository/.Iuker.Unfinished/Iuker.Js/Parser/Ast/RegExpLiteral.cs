namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 正则字面量
    /// </summary>
    public class RegExpLiteral : Expression, IPropertyKeyExpression
    {
        public object Value;
        public string Raw;
        public string Flags;

        public string GetKey()
        {
            return Value.ToString();
        }


    }
}