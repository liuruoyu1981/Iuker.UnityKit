using System;
using System.Collections.Generic;
using Iuker.Js.Native;
using Iuker.Js.Native.Array;
using Iuker.Js.Native.Date;
using Iuker.Js.Native.Error;
using Iuker.Js.Native.Function;
using Iuker.Js.Native.Object;
using Iuker.Js.Parser.Ast;
using Iuker.Js.Runtime;
using Iuker.Js.Runtime.CallStack;
using Iuker.Js.Runtime.Environments;
using Iuker.Js.Runtime.Interop;

namespace Iuker.Js
{
    /// <summary>
    /// Javascript脚本执行引擎
    /// </summary>
    public class Engine
    {
        private readonly ExpressionInterpreter _expressions;
        private readonly StatementInterpreter _statements;
        private readonly Stack<ExecutionContext> _executionContexts;
        private JsValue _completionValue = JsValue.Undefined;
        private int _statementsCount;
        private long _timeoutTicks;

        /// <summary>
        /// 当前语法树节点
        /// </summary>
        private SyntaxNode _lastSyntaxNode = null;

        /// <summary>
        /// 类型转换器实例
        /// </summary>
        public ITypeConverter ClrTypeConverter;














        public ErrorConstructor Error { get; private set; }

        /// <summary>
        /// 基础类型JsToCSharp转换函数映射字典
        /// </summary>
        internal static Dictionary<Type, Func<Engine, object, JsValue>> TypeMappers =
            new Dictionary<Type, Func<Engine, object, JsValue>>
            {
                { typeof(bool), (engine, o) => new JsValue((bool)o)},
                {typeof(byte), (engine, o) => new JsValue((byte)o) },
                {typeof(char), (engine, o) => new JsValue((char)o) },
                //{typeof(DateTime), (engine, o) => engine.d },
            };


        /// <summary>
        /// 函数调用栈
        /// </summary>
        internal JintCallStack CallStack = new JintCallStack();





















































































































        public ObjectConstructor Object { get; private set; }

        public FunctionConstructor Function { get; private set; }

        public ArrayConstructor Array { get; private set; }

        public ErrorConstructor TypeError { get; private set; }




        public DateConstructor Date { get; private set; }





























































































































































































































































        public object EvaluateExpression(Expression expression)
        {
            _lastSyntaxNode = expression;

            switch (expression.Type)
            {
                case SyntaxNodes.AssignmentExpression:
                    break;
                case SyntaxNodes.ArrayExpression:
                    break;
                case SyntaxNodes.BlockStatement:
                    break;
                case SyntaxNodes.BinaryExpression:
                    break;
                case SyntaxNodes.BreakStatement:
                    break;
                case SyntaxNodes.CallExpression:
                    break;
                case SyntaxNodes.CatchClause:
                    break;
                case SyntaxNodes.ConditionalExpression:
                    break;
                case SyntaxNodes.ContinueStatement:
                    break;
                case SyntaxNodes.DoWhileStatement:
                    break;
                case SyntaxNodes.DebuggerStatement:
                    break;
                case SyntaxNodes.EmptyStatement:
                    break;
                case SyntaxNodes.ExpressionStatement:
                    break;
                case SyntaxNodes.ForStatement:
                    break;
                case SyntaxNodes.ForInStatement:
                    break;
                case SyntaxNodes.FunctionDeclaration:
                    break;
                case SyntaxNodes.FunctionExpression:
                    break;
                case SyntaxNodes.Identifier:
                    break;
                case SyntaxNodes.IfStatement:
                    break;
                case SyntaxNodes.Literal:
                    break;
                case SyntaxNodes.RegularExpressionLiteral:
                    break;
                case SyntaxNodes.LabeledStatement:
                    break;
                case SyntaxNodes.LogicalExpression:
                    break;
                case SyntaxNodes.MemberExpression:
                    break;
                case SyntaxNodes.NewExpression:
                    break;
                case SyntaxNodes.ObjectExpression:
                    break;
                case SyntaxNodes.Program:
                    break;
                case SyntaxNodes.Property:
                    break;
                case SyntaxNodes.ReturnStatement:
                    break;
                case SyntaxNodes.SequenceExpression:
                    break;
                case SyntaxNodes.SwitchStatement:
                    break;
                case SyntaxNodes.SwitchCase:
                    break;
                case SyntaxNodes.ThisExpression:
                    break;
                case SyntaxNodes.ThrowStatement:
                    break;
                case SyntaxNodes.TryStatement:
                    break;
                case SyntaxNodes.UnaryExpression:
                    break;
                case SyntaxNodes.UpdateExpression:
                    break;
                case SyntaxNodes.VariableDeclaration:
                    break;
                case SyntaxNodes.VariableDeclarator:
                    break;
                case SyntaxNodes.WhileStatement:
                    break;
                case SyntaxNodes.WithStatement:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
































































































































































































    }
}
