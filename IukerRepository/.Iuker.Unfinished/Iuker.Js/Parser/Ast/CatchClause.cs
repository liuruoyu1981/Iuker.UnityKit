namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// Catch子句
    /// </summary>
    public class CatchClause : Statement
    {
        public Identifier Param;

        public BlockStatement Body;
    }
}