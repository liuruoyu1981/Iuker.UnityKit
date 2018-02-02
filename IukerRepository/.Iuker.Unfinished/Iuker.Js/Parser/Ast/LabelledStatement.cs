namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 标签语句
    /// </summary>
    public class LabelledStatement : Statement
    {
        public Identifier Labe;
        public Statement Body;
    }
}