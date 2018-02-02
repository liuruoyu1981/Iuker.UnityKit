using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Iuker.Js.Native.String;
using Iuker.Js.Parser.Ast;

namespace Iuker.Js.Parser
{
    /// <summary>
    /// Javascript脚本解释器
    /// </summary>
    public class JavaScriptParser
    {
        /// <summary>
        /// 关键字集合
        /// </summary>
        private static readonly HashSet<string> Keywords = new HashSet<string>
        {
            "if", "in", "do", "var", "for", "new", "try", "let",
            "this", "else", "case", "void", "with", "enum",
            "while", "break", "catch", "throw", "const", "yield",
            "class", "super", "return", "typeof", "delete",
            "switch", "export", "import", "default", "finally", "extends",
            "function", "continue", "debugger", "instanceof"
        };

        /// <summary>
        /// 严格模式关键字
        /// </summary>
        private static readonly HashSet<string> StrictModeReservedWords = new HashSet<string>
        {
            "implements",
            "interface",
            "package",
            "private",
            "protected",
            "public",
            "static",
            "yield",
            "let"
        };

        /// <summary>
        /// 保留关键字
        /// </summary>
        private static readonly HashSet<string> FutureReservedWords = new HashSet<string>
        {
            "class",
            "enum",
            "export",
            "extends",
            "import",
            "super"
        };

        private Extra _extra;

        /// <summary>
        /// 
        /// </summary>
        private int _index; // position in the stream

        /// <summary>
        /// 源码长度
        /// </summary>
        private int _length; // length of the stream

        /// <summary>
        /// 当前行号
        /// </summary>
        private int _lineNumber;

        /// <summary>
        /// 开始行号
        /// </summary>
        private int _lineStart;

        /// <summary>
        /// 位置
        /// </summary>
        private Location _location;

        /// <summary>
        /// 之前的单词（回溯）
        /// </summary>
        private Token _lookahead;

        /// <summary>
        /// 源码
        /// </summary>
        private string _source;


        private State _state;
        private bool _strict;

        /// <summary>
        /// 变量作用域栈
        /// </summary>
        private readonly Stack<IVariableScope> _variableScopes = new Stack<IVariableScope>();

        /// <summary>
        /// 函数作用域栈
        /// </summary>
        private readonly Stack<IFunctionScope> _functionScopes = new Stack<IFunctionScope>();

        public JavaScriptParser()
        {

        }

        public JavaScriptParser(bool strict)
        {
            _strict = strict;
        }

        /// <summary>
        /// 是否是十进制数字
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static bool IsDecimalDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        /// <summary>
        /// 是否是十六进制字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static bool IsHexDigit(char ch)
        {
            return
                ch >= '0' && ch <= '9' ||
                ch >= 'a' && ch <= 'f' ||
                ch >= 'A' && ch <= 'F'
                ;
        }

        /// <summary>
        /// 是否是八进制字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static bool IsOctalDigit(char ch)
        {
            return ch >= '0' && ch <= '7';
        }

        /// <summary>
        /// 是否为空白字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static bool IsWhiteSpace(char ch)
        {
            return (ch == 32) || // space
                   (ch == 9) || // tab
                   (ch == 0xB) ||
                   (ch == 0xC) ||
                   (ch == 0xA0) ||
                   (ch >= 0x1680 && (
                        ch == 0x1680 ||
                        ch == 0x180E ||
                        (ch >= 0x2000 && ch <= 0x200A) ||
                        ch == 0x202F ||
                        ch == 0x205F ||
                        ch == 0x3000 ||
                        ch == 0xFEFF));
        }

        /// <summary>
        /// 是否为终结字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static bool IsLineTerminator(char ch)
        {
            return (ch == 10)
                   || (ch == 13)
                   || (ch == 0x2028) // line separator
                   || (ch == 0x2029) // paragraph separator
                ;
        }

        /// <summary>
        /// 是否为标识符起始字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static bool IsIdentifierStart(char ch)
        {
            return (ch == '$') || (ch == '_') ||
                   (ch >= 'A' && ch <= 'Z') ||
                   (ch >= 'a' && ch <= 'z') ||
                   (ch == '\\') ||
                   ((ch >= 0x80) && Regexes.NonAsciiIdentifierStart.IsMatch(ch.ToString()));
        }

        /// <summary>
        /// 是否为标识符合法范围字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        private static bool IsIdentifierPart(char ch)
        {
            return (ch == '$') || (ch == '_') ||
                   (ch >= 'A' && ch <= 'Z') ||
                   (ch >= 'a' && ch <= 'z') ||
                   (ch >= '0' && ch <= '9') ||
                   (ch == '\\') ||
                   ((ch >= 0x80) && Regexes.NonAsciiIdentifierPart.IsMatch(ch.ToString()));
        }

        /// <summary>
        /// 是否为保留关键字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static bool IsFutureReservedWord(string id)
        {
            return FutureReservedWords.Contains(id);
        }

        /// <summary>
        /// 是否为严格模式关键字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static bool IsStrictModeReservedWord(string id)
        {
            return StrictModeReservedWords.Contains(id);
        }

        /// <summary>
        /// 是否为受限制的关键字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static bool IsRestrictedWord(string id)
        {
            return "eval".Equals(id) || "arguments".Equals(id);
        }

        /// <summary>
        /// 是否为关键字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsKeyword(string id)
        {
            if (_strict && IsStrictModeReservedWord(id))
            {
                return true;
            }

            // 'const' is specialized as Keyword in V8.
            // 'yield' and 'let' are for compatiblity with SpiderMonkey and ES.next.
            // Some others are from future reserved words.

            return Keywords.Contains(id);
        }

        /// <summary>
        /// 单词是否为标识符类型
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsIdentifierName(Token token)
        {
            return token.Type == Tokens.Identifier ||
                   token.Type == Tokens.Keyword ||
                   token.Type == Tokens.BooleanLiteral ||
                   token.Type == Tokens.NullLiteral;
        }

        private void AddComment(string type, string value, int start, int end, Location location)
        {
            // Because the way the actual token is scanned, often the comments
            // (if any) are skipped twice during the lexical analysis.
            // Thus, we need to skip adding a comment if the comment array already
            // handled it.
            if (_state.LastCommentStart >= start)
            {
                return;
            }
            _state.LastCommentStart = start;

            var comment = new Comment
            {
                Type = type,
                Value = value
            };

            if (_extra.Range != null)
            {
                comment.Range = new[] { start, end };
            }
            if (_extra.Loc.HasValue)
            {
                comment.Location = location;
            }
            _extra.Comments.Add(comment);
        }

        private void SkipSingleLineComment(int offset)
        {
            //var start, loc, ch, comment;

            int start = _index - offset;
            _location = new Location
            {
                Start = new Position
                {
                    Line = _lineNumber,
                    Column = _index - _lineStart - offset
                }
            };

            while (_index < _length)
            {
                char ch = _source.CharCodeAt(_index);
                ++_index;
                if (IsLineTerminator(ch))
                {
                    if (_extra.Comments != null)
                    {
                        var comment = _source.Slice(start + 2, _index - 1);
                        _location.End = new Position
                        {
                            Line = _lineNumber,
                            Column = _index - _lineStart - 1
                        };
                        AddComment("Line", comment, start, _index - 1, _location);
                    }
                    if (ch == 13 && _source.CharCodeAt(_index) == 10)
                    {
                        ++_index;
                    }
                    ++_lineNumber;
                    _lineStart = _index;
                    return;
                }
            }

            if (_extra.Comments != null)
            {
                var comment = _source.Slice(start + offset, _index);
                _location.End = new Position
                {
                    Line = _lineNumber,
                    Column = _index - _lineStart
                };
                AddComment("Line", comment, start, _index, _location);
            }
        }

        private void SkipMultiLineComment()
        {
            //var start, loc, ch, comment;
            int start = 0;

            if (_extra.Comments != null)
            {
                start = _index - 2;
                _location = new Location
                {
                    Start = new Position
                    {
                        Line = _lineNumber,
                        Column = _index - _lineStart - 2
                    }
                };
            }

            while (_index < _length)
            {
                char ch = _source.CharCodeAt(_index);
                if (IsLineTerminator(ch))
                {
                    if (ch == 13 && _source.CharCodeAt(_index + 1) == 10)
                    {
                        ++_index;
                    }
                    ++_lineNumber;
                    ++_index;
                    _lineStart = _index;
                    if (_index >= _length)
                    {
                        ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
                    }
                }
                else if (ch == 42)
                {
                    // Block comment ends with '*/' (char #42, char #47).
                    if (_source.CharCodeAt(_index + 1) == 47)
                    {
                        ++_index;
                        ++_index;
                        if (_extra.Comments != null)
                        {
                            string comment = _source.Slice(start + 2, _index - 2);
                            _location.End = new Position
                            {
                                Line = _lineNumber,
                                Column = _index - _lineStart
                            };
                            AddComment("Block", comment, start, _index, _location);
                        }
                        return;
                    }
                    ++_index;
                }
                else
                {
                    ++_index;
                }
            }

            ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
        }

        private void SkipComment()
        {
            bool start = _index == 0;

            while (_index < _length)
            {
                char ch = _source.CharCodeAt(_index);

                if (IsWhiteSpace(ch))
                {
                    ++_index;
                }
                else if (IsLineTerminator(ch))
                {
                    ++_index;
                    if (ch == 13 && _source.CharCodeAt(_index) == 10)
                    {
                        ++_index;
                    }
                    ++_lineNumber;
                    _lineStart = _index;
                    start = true;
                }
                else if (ch == '/')
                {
                    ch = _source.CharCodeAt(_index + 1);
                    if (ch == '/')
                    {
                        ++_index;
                        ++_index;
                        SkipSingleLineComment(2);
                        start = true;
                    }
                    else if (ch == '*')
                    {
                        ++_index;
                        ++_index;
                        SkipMultiLineComment();
                    }
                    else
                    {
                        break;
                    }
                }
                else if (start && ch == '-')
                {
                    if (_source.CharCodeAt(_index + 1) == '-' && _source.CharCodeAt(_index + 2) == '>')
                    {
                        // '-->' is a single line comment
                        _index += 3;
                        SkipSingleLineComment(3);
                    }
                    else
                    {
                        break;
                    }
                }
                else if (ch == '<')
                {
                    if (_source.Slice(_index + 1, _index + 4) == "!--")
                    {
                        ++_index; // '<'
                        ++_index; // '!'
                        ++_index; // '-'
                        ++_index; // '-'
                        SkipSingleLineComment(4);

                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 跳过扫描十六进制
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool ScanHexEscape(char prefix, out char result)
        {
            int code = char.MinValue;

            int len = (prefix == 'u') ? 4 : 2;
            for (int i = 0; i < len; ++i)
            {
                if (_index < _length && IsHexDigit(_source.CharCodeAt(_index)))
                {
                    char ch = _source.CharCodeAt(_index++);
                    code = code * 16 +
                           "0123456789abcdef".IndexOf(ch.ToString(),
                               StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    result = char.MinValue;
                    return false;
                }
            }

            result = (char)code;
            return true;
        }

        /// <summary>
        /// 如果在下一个令牌前有行终止符，则返回true。
        /// </summary>
        /// <returns></returns>
        private bool PeekLineTerminator()
        {
            int pos = _index;
            int line = _lineNumber;
            int start = _lineStart;
            SkipComment();
            bool found = _lineNumber != line;
            _index = pos;
            _lineNumber = line;
            _lineStart = start;

            return found;
        }

        private string GetEscapedIdentifier()
        {
            char ch = _source.CharCodeAt(_index++);
            var id = new StringBuilder(ch.ToString());

            // '\u' (char #92, char #117) denotes an escaped character.
            if (ch == 92)
            {
                if (_source.CharCodeAt(_index) != 117)
                {
                    ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
                }
                ++_index;

                if (!ScanHexEscape('u', out ch) || ch == '\\' || !IsIdentifierStart(ch))
                {
                    ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
                }
                id = new StringBuilder(ch.ToString());
            }

            while (_index < _length)
            {
                ch = _source.CharCodeAt(_index);
                if (!IsIdentifierPart(ch))
                {
                    break;
                }
                ++_index;


                // '\u' (char #92, char #117) denotes an escaped character.
                if (ch == 92)
                {
                    if (_source.CharCodeAt(_index) != 117)
                    {
                        ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
                    }
                    ++_index;

                    if (!ScanHexEscape('u', out ch) || ch == '\\' || !IsIdentifierPart(ch))
                    {
                        ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
                    }
                    id.Append(ch);
                }
                else
                {
                    id.Append(ch);
                }
            }

            return id.ToString();
        }

        private string GetIdentifier()
        {
            int start = _index++;
            while (_index < _length)
            {
                char ch = _source.CharCodeAt(_index);
                if (ch == 92)
                {
                    // Blackslash (char #92) marks Unicode escape sequence.
                    _index = start;
                    return GetEscapedIdentifier();
                }
                if (IsIdentifierPart(ch))
                {
                    ++_index;
                }
                else
                {
                    break;
                }
            }

            return _source.Slice(start, _index);
        }

        private Token ScanIdentifier()
        {
            int start = _index;
            Tokens type;

            // Backslash (char #92) starts an escaped character.
            string id = (_source.CharCodeAt(_index) == 92) ? GetEscapedIdentifier() : GetIdentifier();

            // There is no keyword or literal with only one character.
            // Thus, it must be an identifier.
            if (id.Length == 1)
            {
                type = Tokens.Identifier;
            }
            else if (IsKeyword(id))
            {
                type = Tokens.Keyword;
            }
            else if ("null".Equals(id))
            {
                type = Tokens.NullLiteral;
            }
            else if ("true".Equals(id) || "false".Equals(id))
            {
                type = Tokens.BooleanLiteral;
            }
            else
            {
                type = Tokens.Identifier;
            }

            return new Token
            {
                Type = type,
                Value = id,
                LineNumber = _lineNumber,
                LineStart = _lineStart,
                Range = new[] { start, _index }
            };
        }

        private void MarkStart()
        {
            SkipComment();
            if (_extra.Loc.HasValue)
            {
                _state.MarkerStack.Push(_index - _lineStart);
                _state.MarkerStack.Push(_lineNumber);
            }
            if (_extra.Range != null)
            {
                _state.MarkerStack.Push(_index);
            }
        }

        private T MarkEnd<T>(T node) where T : SyntaxNode
        {
            if (_extra.Range != null)
            {
                node.Range = new[] { _state.MarkerStack.Pop(), _index };
            }
            if (_extra.Loc.HasValue)
            {
                node.Location = new Location
                {
                    Start = new Position
                    {
                        Line = _state.MarkerStack.Pop(),
                        Column = _state.MarkerStack.Pop()
                    },
                    End = new Position
                    {
                        Line = _lineNumber,
                        Column = _index - _lineStart
                    }
                };
                PostProcess(node);
            }
            return node;
        }

        public SyntaxNode PostProcess(SyntaxNode node)
        {
            if (_extra.Source != null)
            {
                node.Location.Source = _extra.Source;
            }
            return node;
        }

        #region 表达式构建

        public ArrayExpression CreateArrayExpression(IEnumerable<Expression> elements)
        {
            return new ArrayExpression
            {
                Type = SyntaxNodes.ArrayExpression,
                Elements = elements
            };
        }

        public AssignmentExpression CreateAssignmentExpression(string op, Expression left, Expression right)
        {
            return new AssignmentExpression
            {
                Type = SyntaxNodes.AssignmentExpression,
                Operator = AssignmentExpression.ParseAssignmentOperator(op),
                Left = left,
                Right = right
            };
        }

        public Expression CreateBinaryExpression(string op, Expression left, Expression right)
        {

            return (op == "||" || op == "&&")
                ? (Expression)new LogicalExpression
                {
                    Type = SyntaxNodes.LogicalExpression,
                    Operator = LogicalExpression.ParseLogicalOperator(op),
                    Left = left,
                    Right = right
                }
                : new BinaryExpression
                {
                    Type = SyntaxNodes.BinaryExpression,
                    Operator = BinaryExpression.ParseBinaryOperator(op),
                    Left = left,
                    Right = right
                };
        }

        public BlockStatement CreateBlockStatement(IEnumerable<Statement> body)
        {
            return new BlockStatement
            {
                Type = SyntaxNodes.BlockStatement,
                Body = body
            };
        }

        public BreakStatement CreateBreakStatement(Identifier label)
        {
            return new BreakStatement
            {
                Type = SyntaxNodes.BreakStatement,
                Label = label
            };
        }

        public CallExpression CreateCallExpression(Expression callee, IList<Expression> args)
        {
            return new CallExpression
            {
                Type = SyntaxNodes.CallExpression,
                Callee = callee,
                Arguments = args
            };
        }

        public CatchClause CreateCatchClause(Identifier param, BlockStatement body)
        {
            return new CatchClause
            {
                Type = SyntaxNodes.CatchClause,
                Param = param,
                Body = body
            };
        }

        public ConditionalExpression CreateConditionalExpression(Expression test, Expression consequent,
            Expression alternate)
        {
            return new ConditionalExpression
            {
                Type = SyntaxNodes.ConditionalExpression,
                Test = test,
                Consequent = consequent,
                Alternate = alternate
            };
        }

        public ContinueStatement CreateContinueStatement(Identifier label)
        {
            return new ContinueStatement
            {
                Type = SyntaxNodes.ContinueStatement,
                Label = label
            };
        }

        public DebuggerStatement CreateDebuggerStatement()
        {
            return new DebuggerStatement
            {
                Type = SyntaxNodes.DebuggerStatement
            };
        }

        public DoWhileStatement CreateDoWhileStatement(Statement body, Expression test)
        {
            return new DoWhileStatement
            {
                Type = SyntaxNodes.DoWhileStatement,
                Body = body,
                Test = test
            };
        }

        public EmptyStatement CreateEmptyStatement()
        {
            return new EmptyStatement
            {
                Type = SyntaxNodes.EmptyStatement
            };
        }

        public ExpressionStatement CreateExpressionStatement(Expression expression)
        {
            return new ExpressionStatement
            {
                Type = SyntaxNodes.ExpressionStatement,
                Expression = expression
            };
        }

        public ForStatement CreateForStatement(SyntaxNode init, Expression test, Expression update, Statement body)
        {
            return new ForStatement
            {
                Type = SyntaxNodes.ForStatement,
                Init = init,
                Test = test,
                Update = update,
                Body = body
            };
        }

        public ForInStatement CreateForInStatement(SyntaxNode left, Expression right, Statement body)
        {
            return new ForInStatement
            {
                Type = SyntaxNodes.ForInStatement,
                Left = left,
                Right = right,
                Body = body,
                Each = false
            };
        }

        public FunctionDeclaration CreateFunctionDeclaration(Identifier id, IEnumerable<Identifier> parameters,
            IEnumerable<Expression> defaults, Statement body, bool strict)
        {
            var functionDeclaration = new FunctionDeclaration
            {
                Type = SyntaxNodes.FunctionDeclaration,
                Id = id,
                Parameters = parameters,
                Defaults = defaults,
                Body = body,
                Strict = strict,
                Rest = null,
                Generator = false,
                Expression = false,
                VariableDeclarations = LeaveVariableScope(),
                FunctionDeclarations = LeaveFunctionScope()
            };

            _functionScopes.Peek().FunctionDeclarations.Add(functionDeclaration);

            return functionDeclaration;
        }

        public FunctionExpression CreateFunctionExpression(Identifier id, IEnumerable<Identifier> parameters,
            IEnumerable<Expression> defaults, Statement body, bool strict)
        {
            return new FunctionExpression
            {
                Type = SyntaxNodes.FunctionExpression,
                Id = id,
                Parameters = parameters,
                Defaults = defaults,
                Body = body,
                Strict = strict,
                Rest = null,
                Generator = false,
                Expression = false,
                VariableDeclarations = LeaveVariableScope(),
                FunctionDeclarations = LeaveFunctionScope()
            };
        }

        public Identifier CreateIdentifier(string name)
        {
            return new Identifier
            {
                Type = SyntaxNodes.Identifier,
                Name = name
            };
        }

        public IfStatement CreateIfStatement(Expression test, Statement consequent, Statement alternate)
        {
            return new IfStatement
            {
                Type = SyntaxNodes.IfStatement,
                Test = test,
                Consequent = consequent,
                Alternate = alternate
            };
        }

        public LabelledStatement CreateLabeledStatement(Identifier label, Statement body)
        {
            return new LabelledStatement
            {
                Type = SyntaxNodes.LabeledStatement,
                Label = label,
                Body = body
            };
        }

        public Literal CreateLiteral(Token token)
        {
            if (token.Type == Tokens.RegularExpression)
            {
                return new Literal
                {
                    Type = SyntaxNodes.RegularExpressionLiteral,
                    Value = token.Value,
                    Raw = _source.Slice(token.Range[0], token.Range[1])
                };
            }

            return new Literal
            {
                Type = SyntaxNodes.Literal,
                Value = token.Value,
                Raw = _source.Slice(token.Range[0], token.Range[1])
            };
        }

        public MemberExpression CreateMemberExpression(char accessor, Expression obj, Expression property)
        {
            return new MemberExpression
            {
                Type = SyntaxNodes.MemberExpression,
                Computed = accessor == '[',
                Object = obj,
                Property = property
            };
        }

        public NewExpression CreateNewExpression(Expression callee, IEnumerable<Expression> args)
        {
            return new NewExpression
            {
                Type = SyntaxNodes.NewExpression,
                Callee = callee,
                Arguments = args
            };
        }

        public ObjectExpression CreateObjectExpression(IEnumerable<Property> properties)
        {
            return new ObjectExpression
            {
                Type = SyntaxNodes.ObjectExpression,
                Properties = properties
            };
        }

        public UpdateExpression CreatePostfixExpression(string op, Expression argument)
        {
            return new UpdateExpression
            {
                Type = SyntaxNodes.UpdateExpression,
                Operator = UnaryExpression.ParseUnaryOperator(op),
                Argument = argument,
                Prefix = false
            };
        }

        public Program CreateProgram(ICollection<Statement> body, bool strict)
        {
            return new Program
            {
                Type = SyntaxNodes.Program,
                Body = body,
                Strict = strict,
                VariableDeclarations = LeaveVariableScope(),
                FunctionDeclarations = LeaveFunctionScope()
            };
        }

        #region 作用域

        private void EnterVariableScope()
        {
            _variableScopes.Push(new VariableScope());
        }

        private IList<VariableDeclaration> LeaveVariableScope()
        {
            return _variableScopes.Pop().VariableDeclarations;
        }

        private void EnterFunctionScope()
        {
            _functionScopes.Push(new FunctionScope());
        }

        private IList<FunctionDeclaration> LeaveFunctionScope()
        {
            return _functionScopes.Pop().FunctionDeclarations;
        }


        #endregion

        public Property CreateProperty(PropertyKind kind, IPropertyKeyExpression key, Expression value)
        {
            return new Property
            {
                Type = SyntaxNodes.Property,
                Key = key,
                Value = value,
                Kind = kind
            };
        }

        public ReturnStatement CreateReturnStatement(Expression argument)
        {
            return new ReturnStatement
            {
                Type = SyntaxNodes.ReturnStatement,
                Argument = argument
            };
        }

        public SequenceExpression CreateSequenceExpression(IList<Expression> expressions)
        {
            return new SequenceExpression
            {
                Type = SyntaxNodes.SequenceExpression,
                Expressions = expressions
            };
        }

        public SwitchCase CreateSwitchCase(Expression test, IEnumerable<Statement> consequent)
        {
            return new SwitchCase
            {
                Type = SyntaxNodes.SwitchCase,
                Test = test,
                Consequent = consequent
            };
        }

        public SwitchStatement CreateSwitchStatement(Expression discriminant, IEnumerable<SwitchCase> cases)
        {
            return new SwitchStatement
            {
                Type = SyntaxNodes.SwitchStatement,
                Discriminant = discriminant,
                Cases = cases
            };
        }

        public ThisExpression CreateThisExpression()
        {
            return new ThisExpression
            {
                Type = SyntaxNodes.ThisExpression
            };
        }

        public ThrowStatement CreateThrowStatement(Expression argument)
        {
            return new ThrowStatement
            {
                Type = SyntaxNodes.ThrowStatement,
                Argument = argument
            };
        }

        public TryStatement CreateTryStatement(Statement block, IEnumerable<Statement> guardedHandlers,
            IEnumerable<CatchClause> handlers, Statement finalizer)
        {
            return new TryStatement
            {
                Type = SyntaxNodes.TryStatement,
                Block = block,
                GuardedHandlers = guardedHandlers,
                Handlers = handlers,
                Finalizer = finalizer
            };
        }

        public UnaryExpression CreateUnaryExpression(string op, Expression argument)
        {
            if (op == "++" || op == "--")
            {
                return new UpdateExpression
                {
                    Type = SyntaxNodes.UpdateExpression,
                    Operator = UnaryExpression.ParseUnaryOperator(op),
                    Argument = argument,
                    Prefix = true
                };
            }

            return new UnaryExpression
            {
                Type = SyntaxNodes.UnaryExpression,
                Operator = UnaryExpression.ParseUnaryOperator(op),
                Argument = argument,
                Prefix = true
            };
        }


        public VariableDeclaration CreateVariableDeclaration(IEnumerable<VariableDeclarator> declarations, string kind)
        {
            var variableDeclaration = new VariableDeclaration
            {
                Type = SyntaxNodes.VariableDeclaration,
                Declarations = declarations,
                Kind = kind
            };

            _variableScopes.Peek().VariableDeclarations.Add(variableDeclaration);

            return variableDeclaration;
        }

        public VariableDeclarator CreateVariableDeclarator(Identifier id, Expression init)
        {
            return new VariableDeclarator
            {
                Type = SyntaxNodes.VariableDeclarator,
                Id = id,
                Init = init
            };
        }

        public WhileStatement CreateWhileStatement(Expression test, Statement body)
        {
            return new WhileStatement
            {
                Type = SyntaxNodes.WhileStatement,
                Test = test,
                Body = body
            };
        }

        public WithStatement CreateWithStatement(Expression obj, Statement body)
        {
            return new WithStatement
            {
                Type = SyntaxNodes.WithStatement,
                Object = obj,
                Body = body
            };
        }



        #endregion

        #region 标点符号

        /// <summary>
        /// 扫描标点符号
        /// </summary>
        /// <returns></returns>
        private Token ScanPunctuator()
        {
            int start = _index;
            char code = _source.CharCodeAt(_index);
            char ch1 = _source.CharCodeAt(_index);

            switch ((int)code)
            {
                // Check for most common single-character punctuators.
                case 46: // . dot
                case 40: // ( open bracket
                case 41: // ) close bracket
                case 59: // ; semicolon
                case 44: // , comma
                case 123: // { open curly brace
                case 125: // } close curly brace
                case 91: // [
                case 93: // ]
                case 58: // :
                case 63: // ?
                case 126: // ~
                    ++_index;

                    return new Token
                    {
                        Type = Tokens.Punctuator,
                        Value = code.ToString(),
                        LineNumber = _lineNumber,
                        LineStart = _lineStart,
                        Range = new[] { start, _index }
                    };

                default:
                    char code2 = _source.CharCodeAt(_index + 1);

                    // '=' (char #61) marks an assignment or comparison operator.
                    if (code2 == 61)
                    {
                        switch ((int)code)
                        {
                            case 37: // %
                            case 38: // &
                            case 42: // *:
                            case 43: // +
                            case 45: // -
                            case 47: // /
                            case 60: // <
                            case 62: // >
                            case 94: // ^
                            case 124: // |
                                _index += 2;
                                return new Token
                                {
                                    Type = Tokens.Punctuator,
                                    Value =
                                        code.ToString() +
                                        code2.ToString(),
                                    LineNumber = _lineNumber,
                                    LineStart = _lineStart,
                                    Range = new[] { start, _index }
                                };

                            case 33: // !
                            case 61: // =
                                _index += 2;

                                // !== and ===
                                if (_source.CharCodeAt(_index) == 61)
                                {
                                    ++_index;
                                }
                                return new Token
                                {
                                    Type = Tokens.Punctuator,
                                    Value = _source.Slice(start, _index),
                                    LineNumber = _lineNumber,
                                    LineStart = _lineStart,
                                    Range = new[] { start, _index }
                                };
                        }
                    }
                    break;
            }

            // Peek more characters.

            char ch2 = _source.CharCodeAt(_index + 1);
            char ch3 = _source.CharCodeAt(_index + 2);
            char ch4 = _source.CharCodeAt(_index + 3);

            // 4-character punctuator: >>>=

            if (ch1 == '>' && ch2 == '>' && ch3 == '>')
            {
                if (ch4 == '=')
                {
                    _index += 4;
                    return new Token
                    {
                        Type = Tokens.Punctuator,
                        Value = ">>>=",
                        LineNumber = _lineNumber,
                        LineStart = _lineStart,
                        Range = new[] { start, _index }
                    };
                }
            }

            // 3-character punctuators: == !== >>> <<= >>=

            if (ch1 == '>' && ch2 == '>' && ch3 == '>')
            {
                _index += 3;
                return new Token
                {
                    Type = Tokens.Punctuator,
                    Value = ">>>",
                    LineNumber = _lineNumber,
                    LineStart = _lineStart,
                    Range = new[] { start, _index }
                };
            }

            if (ch1 == '<' && ch2 == '<' && ch3 == '=')
            {
                _index += 3;
                return new Token
                {
                    Type = Tokens.Punctuator,
                    Value = "<<=",
                    LineNumber = _lineNumber,
                    LineStart = _lineStart,
                    Range = new[] { start, _index }
                };
            }

            if (ch1 == '>' && ch2 == '>' && ch3 == '=')
            {
                _index += 3;
                return new Token
                {
                    Type = Tokens.Punctuator,
                    Value = ">>=",
                    LineNumber = _lineNumber,
                    LineStart = _lineStart,
                    Range = new[] { start, _index }
                };
            }

            // Other 2-character punctuators: ++ -- << >> && ||

            if (ch1 == ch2 && ("+-<>&|".IndexOf(ch1) >= 0))
            {
                _index += 2;
                return new Token
                {
                    Type = Tokens.Punctuator,
                    Value = ch1.ToString() + ch2.ToString(),
                    LineNumber = _lineNumber,
                    LineStart = _lineStart,
                    Range = new[] { start, _index }
                };
            }

            if ("<>=!+-*%&|^/".IndexOf(ch1) >= 0)
            {
                ++_index;
                return new Token
                {
                    Type = Tokens.Punctuator,
                    Value = ch1.ToString(),
                    LineNumber = _lineNumber,
                    LineStart = _lineStart,
                    Range = new[] { start, _index }
                };
            }

            ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");

            return null;
        }




        #endregion

        #region 数字以及字面量

        private Token ScanHexLiteral(int start)
        {
            string number = "";

            while (_index < _length)
            {
                if (!IsHexDigit(_source.CharCodeAt(_index)))
                {
                    break;
                }
                number += _source.CharCodeAt(_index++).ToString();
            }

            if (number.Length == 0)
            {
                ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
            }

            if (IsIdentifierStart(_source.CharCodeAt(_index)))
            {
                ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
            }

            return new Token
            {
                Type = Tokens.NumericLiteral,
                Value = Convert.ToInt64(number, 16),
                LineNumber = _lineNumber,
                LineStart = _lineStart,
                Range = new[] { start, _index }
            };
        }

        private Token ScanOctalLiteral(int start)
        {
            string number = "0" + _source.CharCodeAt(_index++);
            while (_index < _length)
            {
                if (!IsOctalDigit(_source.CharCodeAt(_index)))
                {
                    break;
                }
                number += _source.CharCodeAt(_index++).ToString();
            }

            if (IsIdentifierStart(_source.CharCodeAt(_index)) || IsDecimalDigit(_source.CharCodeAt(_index)))
            {
                ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
            }

            return new Token
            {
                Type = Tokens.NumericLiteral,
                Value = Convert.ToInt32(number, 8),
                Octal = true,
                LineNumber = _lineNumber,
                LineStart = _lineStart,
                Range = new[] { start, _index }
            };
        }

        private Token ScanNumericLiteral()
        {
            char ch = _source.CharCodeAt(_index);

            int start = _index;
            string number = "";
            if (ch != '.')
            {
                number = _source.CharCodeAt(_index++).ToString();
                ch = _source.CharCodeAt(_index);

                // Hex number starts with '0x'.
                // Octal number starts with '0'.
                if (number == "0")
                {
                    if (ch == 'x' || ch == 'X')
                    {
                        ++_index;
                        return ScanHexLiteral(start);
                    }
                    if (IsOctalDigit(ch))
                    {
                        return ScanOctalLiteral(start);
                    }

                    // decimal number starts with '0' such as '09' is illegal.
                    if (ch > 0 && IsDecimalDigit(ch))
                    {
                        ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
                    }
                }

                while (IsDecimalDigit(_source.CharCodeAt(_index)))
                {
                    number += _source.CharCodeAt(_index++).ToString();
                }
                ch = _source.CharCodeAt(_index);
            }

            if (ch == '.')
            {
                number += _source.CharCodeAt(_index++).ToString();
                while (IsDecimalDigit(_source.CharCodeAt(_index)))
                {
                    number += _source.CharCodeAt(_index++).ToString();
                }
                ch = _source.CharCodeAt(_index);
            }

            if (ch == 'e' || ch == 'E')
            {
                number += _source.CharCodeAt(_index++).ToString();

                ch = _source.CharCodeAt(_index);
                if (ch == '+' || ch == '-')
                {
                    number += _source.CharCodeAt(_index++).ToString();
                }
                if (IsDecimalDigit(_source.CharCodeAt(_index)))
                {
                    while (IsDecimalDigit(_source.CharCodeAt(_index)))
                    {
                        number += _source.CharCodeAt(_index++).ToString();
                    }
                }
                else
                {
                    ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
                }
            }

            if (IsIdentifierStart(_source.CharCodeAt(_index)))
            {
                ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
            }

            double n;
            try
            {
                n = Double.Parse(number, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);

                if (n > double.MaxValue)
                {
                    n = double.PositiveInfinity;
                }
                else if (n < -double.MaxValue)
                {
                    n = double.NegativeInfinity;
                }
            }
            catch (OverflowException)
            {
                n = StringPrototype.TrimEx(number).StartsWith("-") ? double.NegativeInfinity : double.PositiveInfinity;
            }
            catch (Exception)
            {
                n = double.NaN;
            }

            return new Token
            {
                Type = Tokens.NumericLiteral,
                Value = n,
                LineNumber = _lineNumber,
                LineStart = _lineStart,
                Range = new[] { start, _index }
            };
        }

        private Token ScanStringLiteral()
        {
            var str = new StringBuilder();
            bool octal = false;
            var startLineStart = _lineStart;
            var startLineNumber = _lineNumber;

            char quote = _source.CharCodeAt(_index);

            int start = _index;
            ++_index;

            while (_index < _length)
            {
                char ch = _source.CharCodeAt(_index++);

                if (ch == quote)
                {
                    quote = char.MinValue;
                    break;
                }

                if (ch == '\\')
                {
                    ch = _source.CharCodeAt(_index++);
                    if (ch == char.MinValue || !IsLineTerminator(ch))
                    {
                        switch (ch)
                        {
                            case 'n':
                                str.Append('\n');
                                break;
                            case 'r':
                                str.Append('\r');
                                break;
                            case 't':
                                str.Append('\t');
                                break;
                            case 'u':
                            case 'x':
                                int restore = _index;
                                char unescaped;
                                if (ScanHexEscape(ch, out unescaped))
                                {
                                    str.Append(unescaped);
                                }
                                else
                                {
                                    _index = restore;
                                    str.Append(ch);
                                }
                                break;
                            case 'b':
                                str.Append("\b");
                                break;
                            case 'f':
                                str.Append("\f");
                                break;
                            case 'v':
                                str.Append("\x0B");
                                break;

                            default:
                                if (IsOctalDigit(ch))
                                {
                                    int code = "01234567".IndexOf(ch);

                                    // \0 is not octal escape sequence
                                    if (code != 0)
                                    {
                                        octal = true;
                                    }

                                    if (_index < _length && IsOctalDigit(_source.CharCodeAt(_index)))
                                    {
                                        octal = true;
                                        code = code * 8 + "01234567".IndexOf(_source.CharCodeAt(_index++));

                                        // 3 digits are only allowed when string starts
                                        // with 0, 1, 2, 3
                                        if ("0123".IndexOf(ch) >= 0 &&
                                            _index < _length &&
                                            IsOctalDigit(_source.CharCodeAt(_index)))
                                        {
                                            code = code * 8 + "01234567".IndexOf(_source.CharCodeAt(_index++));
                                        }
                                    }
                                    str.Append((char)code);
                                }
                                else
                                {
                                    str.Append(ch);
                                }
                                break;
                        }
                    }
                    else
                    {
                        ++_lineNumber;
                        if (ch == '\r' && _source.CharCodeAt(_index) == '\n')
                        {
                            ++_index;
                        }
                        _lineStart = _index;
                    }
                }
                else if (IsLineTerminator(ch))
                {
                    break;
                }
                else
                {
                    str.Append(ch);
                }
            }

            if (quote != 0)
            {
                ThrowError(null, Messages.UnexpectedToken, "ILLEGAL");
            }

            return new Token
            {
                Type = Tokens.StringLiteral,
                Value = str.ToString(),
                Octal = octal,
                LineNumber = _lineNumber,
                LineStart = _lineStart,
                Range = new[] { start, _index }
            };
        }

        private Token ScanRegExp()
        {
            bool classMarker = false;
            bool terminated = false;

            SkipComment();

            int start = _index;
            char ch;

            var str = new StringBuilder(_source.CharCodeAt(_index++).ToString());

            while (_index < _length)
            {
                ch = _source.CharCodeAt(_index++);

                str.Append(ch);
                if (ch == '\\')
                {
                    ch = _source.CharCodeAt(_index++);
                    // ECMA-262 7.8.5
                    if (IsLineTerminator(ch))
                    {
                        ThrowError(null, Messages.UnterminatedRegExp);
                    }
                    str.Append(ch);
                }
                else if (IsLineTerminator(ch))
                {
                    ThrowError(null, Messages.UnterminatedRegExp);
                }
                else if (classMarker)
                {
                    if (ch == ']')
                    {
                        classMarker = false;
                    }
                }
                else
                {
                    if (ch == '/')
                    {
                        terminated = true;
                        break;
                    }

                    if (ch == '[')
                    {
                        classMarker = true;
                    }
                }
            }

            if (!terminated)
            {
                ThrowError(null, Messages.UnterminatedRegExp);
            }

            // Exclude leading and trailing slash.
            string pattern = str.ToString().Substring(1, str.Length - 2);

            string flags = "";
            while (_index < _length)
            {
                ch = _source.CharCodeAt(_index);
                if (!IsIdentifierPart(ch))
                {
                    break;
                }

                ++_index;
                if (ch == '\\' && _index < _length)
                {
                    ch = _source.CharCodeAt(_index);
                    if (ch == 'u')
                    {
                        ++_index;
                        int restore = _index;
                        if (ScanHexEscape('u', out ch))
                        {
                            flags += ch.ToString();
                            for (str.Append("\\u"); restore < _index; ++restore)
                            {
                                str.Append(_source.CharCodeAt(restore).ToString());
                            }
                        }
                        else
                        {
                            _index = restore;
                            flags += "u";
                            str.Append("\\u");
                        }
                    }
                    else
                    {
                        str.Append("\\");
                    }
                }
                else
                {
                    flags += ch.ToString();
                    str.Append(ch.ToString());
                }
            }

            Peek();

            return new Token
            {
                Type = Tokens.RegularExpression,
                Literal = str.ToString(),
                Value = pattern + flags,
                Range = new[] { start, _index }
            };
        }

        private void Peek()
        {
            int pos = _index;
            int line = _lineNumber;
            int start = _lineStart;
            _lookahead = (_extra.Tokens != null) ? CollectToken() : Advance();
            _index = pos;
            _lineNumber = line;
            _lineStart = start;
        }

        private Token Advance()
        {
            SkipComment();

            if (_index >= _length)
            {
                return new Token
                {
                    Type = Tokens.EOF,
                    LineNumber = _lineNumber,
                    LineStart = _lineStart,
                    Range = new[] { _index, _index }
                };
            }

            char ch = _source.CharCodeAt(_index);

            // Very common: ( and ) and ;
            if (ch == 40 || ch == 41 || ch == 59)
            {
                return ScanPunctuator();
            }

            // String literal starts with single quote (#39) or double quote (#34).
            if (ch == 39 || ch == 34)
            {
                return ScanStringLiteral();
            }

            if (IsIdentifierStart(ch))
            {
                return ScanIdentifier();
            }

            // Dot (.) char #46 can also start a floating-point number, hence the need
            // to check the next character.
            if (ch == 46)
            {
                if (IsDecimalDigit(_source.CharCodeAt(_index + 1)))
                {
                    return ScanNumericLiteral();
                }
                return ScanPunctuator();
            }

            if (IsDecimalDigit(ch))
            {
                return ScanNumericLiteral();
            }

            return ScanPunctuator();
        }

        private Token CollectToken()
        {
            SkipComment();
            _location = new Location
            {
                Start = new Position
                {
                    Line = _lineNumber,
                    Column = _index - _lineStart
                }
            };

            Token token = Advance();
            _location.End = new Position
            {
                Line = _lineNumber,
                Column = _index - _lineStart
            };

            if (token.Type != Tokens.EOF)
            {
                var range = new[] { token.Range[0], token.Range[1] };
                string value = _source.Slice(token.Range[0], token.Range[1]);
                _extra.Tokens.Add(new Token
                {
                    Type = token.Type,
                    Value = value,
                    Range = range,
                    Location = _location
                });
            }

            return token;
        }

        #endregion

        #region 正则表达式

        private Token CollectRegex()
        {
            SkipComment();

            int pos = _index;
            var loc = new Location
            {
                Start = new Position
                {
                    Line = _lineNumber,
                    Column = _index - _lineStart
                }
            };

            Token regex = ScanRegExp();
            loc.End = new Position
            {
                Line = _lineNumber,
                Column = _index - _lineStart
            };

            // Pop the previous token, which is likely '/' or '/='
            if (_extra.Tokens != null)
            {
                Token token = _extra.Tokens[_extra.Tokens.Count - 1];
                if (token.Range[0] == pos && token.Type == Tokens.Punctuator)
                {
                    if ("/".Equals(token.Value) || "/=".Equals(token.Value))
                    {
                        _extra.Tokens.RemoveAt(_extra.Tokens.Count - 1);
                    }
                }

                _extra.Tokens.Add(new Token
                {
                    Type = Tokens.RegularExpression,
                    Value = regex.Literal,
                    Range = new[] { pos, _index },
                    Location = loc
                });
            }

            return regex;
        }

        private Token Lex()
        {
            Token token = _lookahead;
            _index = token.Range[1];
            _lineNumber = token.LineNumber.HasValue ? token.LineNumber.Value : 0;
            _lineStart = token.LineStart;

            _lookahead = (_extra.Tokens != null) ? CollectToken() : Advance();

            _index = token.Range[1];
            _lineNumber = token.LineNumber.HasValue ? token.LineNumber.Value : 0;
            _lineStart = token.LineStart;

            return token;
        }

        #endregion

        #region 异常处理


        private void ThrowError(Token token, string messageFormat, params object[] arguments)
        {
            ParserException exception;
            string msg = String.Format(messageFormat, arguments);

            if (token != null && token.LineNumber.HasValue)
            {
                exception = new ParserException("Line " + token.LineNumber + ": " + msg)
                {
                    Index = token.Range[0],
                    LineNumber = token.LineNumber.Value,
                    Column = token.Range[0] - _lineStart + 1,
                    Source = _extra.Source
                };
            }
            else
            {
                exception = new ParserException("Line " + _lineNumber + ": " + msg)
                {
                    Index = _index,
                    LineNumber = _lineNumber,
                    Column = _index - _lineStart + 1,
                    Source = _extra.Source
                };
            }

            exception.Description = msg;
            throw exception;
        }



        #endregion










        /// <summary>
        /// 额外
        /// </summary>
        private class Extra
        {
            public int? Loc;
            public int[] Range;
            public string Source;

            public List<Comment> Comments;
            public List<Token> Tokens;
            public List<ParserException> Errors;
        }

        /// <summary>
        /// 位置标记器
        /// </summary>
        private class LocationMarker
        {
            private readonly int[] _marker;

            public LocationMarker(int index, int lineNumber, int lineStart)
            {
                _marker = new[] { index, lineNumber, index - lineStart, 0, 0, 0 };
            }

            public void End(int index, int lineNumber, int lineStart)
            {
                _marker[3] = index;
                _marker[4] = lineNumber;
                _marker[5] = index - lineStart;
            }

            public void Apply(SyntaxNode node, Extra extra, Func<SyntaxNode, SyntaxNode> postProcess)
            {
                if (extra.Range.Length > 0)
                {
                    node.Range = new[] { _marker[0], _marker[3] };
                }
                if (extra.Loc.HasValue)
                {
                    node.Location = new Location
                    {
                        Start = new Position
                        {
                            Line = _marker[1],
                            Column = _marker[2]
                        },
                        End = new Position
                        {
                            Line = _marker[4],
                            Column = _marker[5]
                        }
                    };
                }

                node = postProcess(node);
            }
        };

        /// <summary>
        /// 解析参数
        /// </summary>
        private struct ParsedParameters
        {
            public Token FirstRestricted;
            public string Message;
            public IEnumerable<Identifier> Parameters;
            public Token Stricted;
        }

        private static class Regexes
        {
            public static readonly Regex NonAsciiIdentifierStart = new Regex("[\xaa\xb5\xba\xc0-\xd6\xd8-\xf6\xf8-\u02c1\u02c6-\u02d1\u02e0-\u02e4\u02ec\u02ee\u0370-\u0374\u0376\u0377\u037a-\u037d\u0386\u0388-\u038a\u038c\u038e-\u03a1\u03a3-\u03f5\u03f7-\u0481\u048a-\u0527\u0531-\u0556\u0559\u0561-\u0587\u05d0-\u05ea\u05f0-\u05f2\u0620-\u064a\u066e\u066f\u0671-\u06d3\u06d5\u06e5\u06e6\u06ee\u06ef\u06fa-\u06fc\u06ff\u0710\u0712-\u072f\u074d-\u07a5\u07b1\u07ca-\u07ea\u07f4\u07f5\u07fa\u0800-\u0815\u081a\u0824\u0828\u0840-\u0858\u08a0\u08a2-\u08ac\u0904-\u0939\u093d\u0950\u0958-\u0961\u0971-\u0977\u0979-\u097f\u0985-\u098c\u098f\u0990\u0993-\u09a8\u09aa-\u09b0\u09b2\u09b6-\u09b9\u09bd\u09ce\u09dc\u09dd\u09df-\u09e1\u09f0\u09f1\u0a05-\u0a0a\u0a0f\u0a10\u0a13-\u0a28\u0a2a-\u0a30\u0a32\u0a33\u0a35\u0a36\u0a38\u0a39\u0a59-\u0a5c\u0a5e\u0a72-\u0a74\u0a85-\u0a8d\u0a8f-\u0a91\u0a93-\u0aa8\u0aaa-\u0ab0\u0ab2\u0ab3\u0ab5-\u0ab9\u0abd\u0ad0\u0ae0\u0ae1\u0b05-\u0b0c\u0b0f\u0b10\u0b13-\u0b28\u0b2a-\u0b30\u0b32\u0b33\u0b35-\u0b39\u0b3d\u0b5c\u0b5d\u0b5f-\u0b61\u0b71\u0b83\u0b85-\u0b8a\u0b8e-\u0b90\u0b92-\u0b95\u0b99\u0b9a\u0b9c\u0b9e\u0b9f\u0ba3\u0ba4\u0ba8-\u0baa\u0bae-\u0bb9\u0bd0\u0c05-\u0c0c\u0c0e-\u0c10\u0c12-\u0c28\u0c2a-\u0c33\u0c35-\u0c39\u0c3d\u0c58\u0c59\u0c60\u0c61\u0c85-\u0c8c\u0c8e-\u0c90\u0c92-\u0ca8\u0caa-\u0cb3\u0cb5-\u0cb9\u0cbd\u0cde\u0ce0\u0ce1\u0cf1\u0cf2\u0d05-\u0d0c\u0d0e-\u0d10\u0d12-\u0d3a\u0d3d\u0d4e\u0d60\u0d61\u0d7a-\u0d7f\u0d85-\u0d96\u0d9a-\u0db1\u0db3-\u0dbb\u0dbd\u0dc0-\u0dc6\u0e01-\u0e30\u0e32\u0e33\u0e40-\u0e46\u0e81\u0e82\u0e84\u0e87\u0e88\u0e8a\u0e8d\u0e94-\u0e97\u0e99-\u0e9f\u0ea1-\u0ea3\u0ea5\u0ea7\u0eaa\u0eab\u0ead-\u0eb0\u0eb2\u0eb3\u0ebd\u0ec0-\u0ec4\u0ec6\u0edc-\u0edf\u0f00\u0f40-\u0f47\u0f49-\u0f6c\u0f88-\u0f8c\u1000-\u102a\u103f\u1050-\u1055\u105a-\u105d\u1061\u1065\u1066\u106e-\u1070\u1075-\u1081\u108e\u10a0-\u10c5\u10c7\u10cd\u10d0-\u10fa\u10fc-\u1248\u124a-\u124d\u1250-\u1256\u1258\u125a-\u125d\u1260-\u1288\u128a-\u128d\u1290-\u12b0\u12b2-\u12b5\u12b8-\u12be\u12c0\u12c2-\u12c5\u12c8-\u12d6\u12d8-\u1310\u1312-\u1315\u1318-\u135a\u1380-\u138f\u13a0-\u13f4\u1401-\u166c\u166f-\u167f\u1681-\u169a\u16a0-\u16ea\u16ee-\u16f0\u1700-\u170c\u170e-\u1711\u1720-\u1731\u1740-\u1751\u1760-\u176c\u176e-\u1770\u1780-\u17b3\u17d7\u17dc\u1820-\u1877\u1880-\u18a8\u18aa\u18b0-\u18f5\u1900-\u191c\u1950-\u196d\u1970-\u1974\u1980-\u19ab\u19c1-\u19c7\u1a00-\u1a16\u1a20-\u1a54\u1aa7\u1b05-\u1b33\u1b45-\u1b4b\u1b83-\u1ba0\u1bae\u1baf\u1bba-\u1be5\u1c00-\u1c23\u1c4d-\u1c4f\u1c5a-\u1c7d\u1ce9-\u1cec\u1cee-\u1cf1\u1cf5\u1cf6\u1d00-\u1dbf\u1e00-\u1f15\u1f18-\u1f1d\u1f20-\u1f45\u1f48-\u1f4d\u1f50-\u1f57\u1f59\u1f5b\u1f5d\u1f5f-\u1f7d\u1f80-\u1fb4\u1fb6-\u1fbc\u1fbe\u1fc2-\u1fc4\u1fc6-\u1fcc\u1fd0-\u1fd3\u1fd6-\u1fdb\u1fe0-\u1fec\u1ff2-\u1ff4\u1ff6-\u1ffc\u2071\u207f\u2090-\u209c\u2102\u2107\u210a-\u2113\u2115\u2119-\u211d\u2124\u2126\u2128\u212a-\u212d\u212f-\u2139\u213c-\u213f\u2145-\u2149\u214e\u2160-\u2188\u2c00-\u2c2e\u2c30-\u2c5e\u2c60-\u2ce4\u2ceb-\u2cee\u2cf2\u2cf3\u2d00-\u2d25\u2d27\u2d2d\u2d30-\u2d67\u2d6f\u2d80-\u2d96\u2da0-\u2da6\u2da8-\u2dae\u2db0-\u2db6\u2db8-\u2dbe\u2dc0-\u2dc6\u2dc8-\u2dce\u2dd0-\u2dd6\u2dd8-\u2dde\u2e2f\u3005-\u3007\u3021-\u3029\u3031-\u3035\u3038-\u303c\u3041-\u3096\u309d-\u309f\u30a1-\u30fa\u30fc-\u30ff\u3105-\u312d\u3131-\u318e\u31a0-\u31ba\u31f0-\u31ff\u3400-\u4db5\u4e00-\u9fcc\ua000-\ua48c\ua4d0-\ua4fd\ua500-\ua60c\ua610-\ua61f\ua62a\ua62b\ua640-\ua66e\ua67f-\ua697\ua6a0-\ua6ef\ua717-\ua71f\ua722-\ua788\ua78b-\ua78e\ua790-\ua793\ua7a0-\ua7aa\ua7f8-\ua801\ua803-\ua805\ua807-\ua80a\ua80c-\ua822\ua840-\ua873\ua882-\ua8b3\ua8f2-\ua8f7\ua8fb\ua90a-\ua925\ua930-\ua946\ua960-\ua97c\ua984-\ua9b2\ua9cf\uaa00-\uaa28\uaa40-\uaa42\uaa44-\uaa4b\uaa60-\uaa76\uaa7a\uaa80-\uaaaf\uaab1\uaab5\uaab6\uaab9-\uaabd\uaac0\uaac2\uaadb-\uaadd\uaae0-\uaaea\uaaf2-\uaaf4\uab01-\uab06\uab09-\uab0e\uab11-\uab16\uab20-\uab26\uab28-\uab2e\uabc0-\uabe2\uac00-\ud7a3\ud7b0-\ud7c6\ud7cb-\ud7fb\uf900-\ufa6d\ufa70-\ufad9\ufb00-\ufb06\ufb13-\ufb17\ufb1d\ufb1f-\ufb28\ufb2a-\ufb36\ufb38-\ufb3c\ufb3e\ufb40\ufb41\ufb43\ufb44\ufb46-\ufbb1\ufbd3-\ufd3d\ufd50-\ufd8f\ufd92-\ufdc7\ufdf0-\ufdfb\ufe70-\ufe74\ufe76-\ufefc\uff21-\uff3a\uff41-\uff5a\uff66-\uffbe\uffc2-\uffc7\uffca-\uffcf\uffd2-\uffd7\uffda-\uffdc]");
            public static readonly Regex NonAsciiIdentifierPart = new Regex("[\xaa\xb5\xba\xc0-\xd6\xd8-\xf6\xf8-\u02c1\u02c6-\u02d1\u02e0-\u02e4\u02ec\u02ee\u0300-\u0374\u0376\u0377\u037a-\u037d\u0386\u0388-\u038a\u038c\u038e-\u03a1\u03a3-\u03f5\u03f7-\u0481\u0483-\u0487\u048a-\u0527\u0531-\u0556\u0559\u0561-\u0587\u0591-\u05bd\u05bf\u05c1\u05c2\u05c4\u05c5\u05c7\u05d0-\u05ea\u05f0-\u05f2\u0610-\u061a\u0620-\u0669\u066e-\u06d3\u06d5-\u06dc\u06df-\u06e8\u06ea-\u06fc\u06ff\u0710-\u074a\u074d-\u07b1\u07c0-\u07f5\u07fa\u0800-\u082d\u0840-\u085b\u08a0\u08a2-\u08ac\u08e4-\u08fe\u0900-\u0963\u0966-\u096f\u0971-\u0977\u0979-\u097f\u0981-\u0983\u0985-\u098c\u098f\u0990\u0993-\u09a8\u09aa-\u09b0\u09b2\u09b6-\u09b9\u09bc-\u09c4\u09c7\u09c8\u09cb-\u09ce\u09d7\u09dc\u09dd\u09df-\u09e3\u09e6-\u09f1\u0a01-\u0a03\u0a05-\u0a0a\u0a0f\u0a10\u0a13-\u0a28\u0a2a-\u0a30\u0a32\u0a33\u0a35\u0a36\u0a38\u0a39\u0a3c\u0a3e-\u0a42\u0a47\u0a48\u0a4b-\u0a4d\u0a51\u0a59-\u0a5c\u0a5e\u0a66-\u0a75\u0a81-\u0a83\u0a85-\u0a8d\u0a8f-\u0a91\u0a93-\u0aa8\u0aaa-\u0ab0\u0ab2\u0ab3\u0ab5-\u0ab9\u0abc-\u0ac5\u0ac7-\u0ac9\u0acb-\u0acd\u0ad0\u0ae0-\u0ae3\u0ae6-\u0aef\u0b01-\u0b03\u0b05-\u0b0c\u0b0f\u0b10\u0b13-\u0b28\u0b2a-\u0b30\u0b32\u0b33\u0b35-\u0b39\u0b3c-\u0b44\u0b47\u0b48\u0b4b-\u0b4d\u0b56\u0b57\u0b5c\u0b5d\u0b5f-\u0b63\u0b66-\u0b6f\u0b71\u0b82\u0b83\u0b85-\u0b8a\u0b8e-\u0b90\u0b92-\u0b95\u0b99\u0b9a\u0b9c\u0b9e\u0b9f\u0ba3\u0ba4\u0ba8-\u0baa\u0bae-\u0bb9\u0bbe-\u0bc2\u0bc6-\u0bc8\u0bca-\u0bcd\u0bd0\u0bd7\u0be6-\u0bef\u0c01-\u0c03\u0c05-\u0c0c\u0c0e-\u0c10\u0c12-\u0c28\u0c2a-\u0c33\u0c35-\u0c39\u0c3d-\u0c44\u0c46-\u0c48\u0c4a-\u0c4d\u0c55\u0c56\u0c58\u0c59\u0c60-\u0c63\u0c66-\u0c6f\u0c82\u0c83\u0c85-\u0c8c\u0c8e-\u0c90\u0c92-\u0ca8\u0caa-\u0cb3\u0cb5-\u0cb9\u0cbc-\u0cc4\u0cc6-\u0cc8\u0cca-\u0ccd\u0cd5\u0cd6\u0cde\u0ce0-\u0ce3\u0ce6-\u0cef\u0cf1\u0cf2\u0d02\u0d03\u0d05-\u0d0c\u0d0e-\u0d10\u0d12-\u0d3a\u0d3d-\u0d44\u0d46-\u0d48\u0d4a-\u0d4e\u0d57\u0d60-\u0d63\u0d66-\u0d6f\u0d7a-\u0d7f\u0d82\u0d83\u0d85-\u0d96\u0d9a-\u0db1\u0db3-\u0dbb\u0dbd\u0dc0-\u0dc6\u0dca\u0dcf-\u0dd4\u0dd6\u0dd8-\u0ddf\u0df2\u0df3\u0e01-\u0e3a\u0e40-\u0e4e\u0e50-\u0e59\u0e81\u0e82\u0e84\u0e87\u0e88\u0e8a\u0e8d\u0e94-\u0e97\u0e99-\u0e9f\u0ea1-\u0ea3\u0ea5\u0ea7\u0eaa\u0eab\u0ead-\u0eb9\u0ebb-\u0ebd\u0ec0-\u0ec4\u0ec6\u0ec8-\u0ecd\u0ed0-\u0ed9\u0edc-\u0edf\u0f00\u0f18\u0f19\u0f20-\u0f29\u0f35\u0f37\u0f39\u0f3e-\u0f47\u0f49-\u0f6c\u0f71-\u0f84\u0f86-\u0f97\u0f99-\u0fbc\u0fc6\u1000-\u1049\u1050-\u109d\u10a0-\u10c5\u10c7\u10cd\u10d0-\u10fa\u10fc-\u1248\u124a-\u124d\u1250-\u1256\u1258\u125a-\u125d\u1260-\u1288\u128a-\u128d\u1290-\u12b0\u12b2-\u12b5\u12b8-\u12be\u12c0\u12c2-\u12c5\u12c8-\u12d6\u12d8-\u1310\u1312-\u1315\u1318-\u135a\u135d-\u135f\u1380-\u138f\u13a0-\u13f4\u1401-\u166c\u166f-\u167f\u1681-\u169a\u16a0-\u16ea\u16ee-\u16f0\u1700-\u170c\u170e-\u1714\u1720-\u1734\u1740-\u1753\u1760-\u176c\u176e-\u1770\u1772\u1773\u1780-\u17d3\u17d7\u17dc\u17dd\u17e0-\u17e9\u180b-\u180d\u1810-\u1819\u1820-\u1877\u1880-\u18aa\u18b0-\u18f5\u1900-\u191c\u1920-\u192b\u1930-\u193b\u1946-\u196d\u1970-\u1974\u1980-\u19ab\u19b0-\u19c9\u19d0-\u19d9\u1a00-\u1a1b\u1a20-\u1a5e\u1a60-\u1a7c\u1a7f-\u1a89\u1a90-\u1a99\u1aa7\u1b00-\u1b4b\u1b50-\u1b59\u1b6b-\u1b73\u1b80-\u1bf3\u1c00-\u1c37\u1c40-\u1c49\u1c4d-\u1c7d\u1cd0-\u1cd2\u1cd4-\u1cf6\u1d00-\u1de6\u1dfc-\u1f15\u1f18-\u1f1d\u1f20-\u1f45\u1f48-\u1f4d\u1f50-\u1f57\u1f59\u1f5b\u1f5d\u1f5f-\u1f7d\u1f80-\u1fb4\u1fb6-\u1fbc\u1fbe\u1fc2-\u1fc4\u1fc6-\u1fcc\u1fd0-\u1fd3\u1fd6-\u1fdb\u1fe0-\u1fec\u1ff2-\u1ff4\u1ff6-\u1ffc\u200c\u200d\u203f\u2040\u2054\u2071\u207f\u2090-\u209c\u20d0-\u20dc\u20e1\u20e5-\u20f0\u2102\u2107\u210a-\u2113\u2115\u2119-\u211d\u2124\u2126\u2128\u212a-\u212d\u212f-\u2139\u213c-\u213f\u2145-\u2149\u214e\u2160-\u2188\u2c00-\u2c2e\u2c30-\u2c5e\u2c60-\u2ce4\u2ceb-\u2cf3\u2d00-\u2d25\u2d27\u2d2d\u2d30-\u2d67\u2d6f\u2d7f-\u2d96\u2da0-\u2da6\u2da8-\u2dae\u2db0-\u2db6\u2db8-\u2dbe\u2dc0-\u2dc6\u2dc8-\u2dce\u2dd0-\u2dd6\u2dd8-\u2dde\u2de0-\u2dff\u2e2f\u3005-\u3007\u3021-\u302f\u3031-\u3035\u3038-\u303c\u3041-\u3096\u3099\u309a\u309d-\u309f\u30a1-\u30fa\u30fc-\u30ff\u3105-\u312d\u3131-\u318e\u31a0-\u31ba\u31f0-\u31ff\u3400-\u4db5\u4e00-\u9fcc\ua000-\ua48c\ua4d0-\ua4fd\ua500-\ua60c\ua610-\ua62b\ua640-\ua66f\ua674-\ua67d\ua67f-\ua697\ua69f-\ua6f1\ua717-\ua71f\ua722-\ua788\ua78b-\ua78e\ua790-\ua793\ua7a0-\ua7aa\ua7f8-\ua827\ua840-\ua873\ua880-\ua8c4\ua8d0-\ua8d9\ua8e0-\ua8f7\ua8fb\ua900-\ua92d\ua930-\ua953\ua960-\ua97c\ua980-\ua9c0\ua9cf-\ua9d9\uaa00-\uaa36\uaa40-\uaa4d\uaa50-\uaa59\uaa60-\uaa76\uaa7a\uaa7b\uaa80-\uaac2\uaadb-\uaadd\uaae0-\uaaef\uaaf2-\uaaf6\uab01-\uab06\uab09-\uab0e\uab11-\uab16\uab20-\uab26\uab28-\uab2e\uabc0-\uabea\uabec\uabed\uabf0-\uabf9\uac00-\ud7a3\ud7b0-\ud7c6\ud7cb-\ud7fb\uf900-\ufa6d\ufa70-\ufad9\ufb00-\ufb06\ufb13-\ufb17\ufb1d-\ufb28\ufb2a-\ufb36\ufb38-\ufb3c\ufb3e\ufb40\ufb41\ufb43\ufb44\ufb46-\ufbb1\ufbd3-\ufd3d\ufd50-\ufd8f\ufd92-\ufdc7\ufdf0-\ufdfb\ufe00-\ufe0f\ufe20-\ufe26\ufe33\ufe34\ufe4d-\ufe4f\ufe70-\ufe74\ufe76-\ufefc\uff10-\uff19\uff21-\uff3a\uff3f\uff41-\uff5a\uff66-\uffbe\uffc2-\uffc7\uffca-\uffcf\uffd2-\uffd7\uffda-\uffdc]");
        };



    }
}
