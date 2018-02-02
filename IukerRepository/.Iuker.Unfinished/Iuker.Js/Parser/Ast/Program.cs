using System.Collections.Generic;

namespace Iuker.Js.Parser.Ast
{
    /// <summary>
    /// 程序语句
    /// </summary>
    public class Program : Statement, IVariableScope, IFunctionScope
    {
        public Program()
        {
            VariableDeclarations = new List<VariableDeclaration>();
        }

        /// <summary>
        /// 执行体
        /// </summary>
        public ICollection<Statement> Body;

        /// <summary>
        /// 注释列表
        /// </summary>
        public List<Comment> Comments;

        /// <summary>
        /// 单词列表
        /// </summary>
        public List<Token> Tokens;

        /// <summary>
        /// 解析异常列表
        /// </summary>
        public List<ParserException> Errors;

        /// <summary>
        /// 是否为严格模式
        /// </summary>
        public bool Strict;

        public IList<VariableDeclaration> VariableDeclarations { get; set; }
        public IList<FunctionDeclaration> FunctionDeclarations { get; set; }
    }
}