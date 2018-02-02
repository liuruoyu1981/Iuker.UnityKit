namespace Iuker.Js.Parser.Ast
{
    public class DoWhileStatement : Statement
    {
        /// <summary>
        /// 执行体语句
        /// </summary>
        public Statement Body;

        /// <summary>
        /// 条件测试表达式
        /// </summary>
        public Expression Test;

    }
}