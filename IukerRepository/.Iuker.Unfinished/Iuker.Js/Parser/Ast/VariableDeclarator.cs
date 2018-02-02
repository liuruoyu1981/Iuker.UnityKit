namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 变量说明符表达式
    /// </summary>
    public class VariableDeclarator : Expression
    {

        public Identifier Id;
        public Expression Init;

    }
}