namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 成员表达式
    /// </summary>
    public class MemberExpression : Expression
    {
        public Expression Object;
        public Expression Property;

        public bool Computed;

    }
}