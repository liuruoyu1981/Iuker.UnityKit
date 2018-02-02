/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 17:36
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

namespace Iuker.YourSharp.Parser.Ast
{
    /// <summary>
    /// 语法树节点类型
    /// </summary>
    public enum SyntaxNodeType : byte
    {
        /// <summary>
        /// 赋值表达式
        /// </summary>
        AssignmentExpression,

        /// <summary>
        /// 数组表达式
        /// </summary>
        ArrayExpression,

        /// <summary>
        /// 块语句
        /// </summary>
        BlockStatement,

        /// <summary>
        /// 二元表达式
        /// </summary>
        BinaryExpression,

        /// <summary>
        /// break语句
        /// </summary>
        BreakStatement,

        /// <summary>
        /// 调用表达式
        /// </summary>
        CallExpression,

        /// <summary>
        /// Catch表达式
        /// </summary>
        CatchClause,

        /// <summary>
        /// 条件判断表达式
        /// </summary>
        ConditionalExpression,

        /// <summary>
        /// Continue表达式
        /// </summary>
        ContinueStatement,

        /// <summary>
        /// 
        /// </summary>
        DoWhileStatement,
        DebuggerStatement,
        EmptyStatement,
        ExpressionStatement,
        ForStatement,
        ForInStatement,
        FunctionDeclaration,
        FunctionExpression,
        Identifier,
        IfStatement,
        Literal,
        RegularExpressionLiteral,
        LabeledStatement,
        LogicalExpression,
        MemberExpression,
        NewExpression,
        ObjectExpression,
        Program,
        Property,

        /// <summary>
        /// 返回语句
        /// </summary>
        ReturnStatement,
        SequenceExpression,
        SwitchStatement,
        SwitchCase,
        ThisExpression,
        ThrowStatement,
        TryStatement,
        UnaryExpression,
        UpdateExpression,
        VariableDeclaration,
        VariableDeclarator,
        WhileStatement,
        WithStatement




    }
}