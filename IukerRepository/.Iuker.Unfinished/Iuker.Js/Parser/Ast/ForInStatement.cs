namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// For in 语句
    /// </summary>
    public class ForInStatement : Statement
    {
        public SyntaxNode Left;
        public Expression Right;
        public Statement Body;
        public bool Each;


    }
}