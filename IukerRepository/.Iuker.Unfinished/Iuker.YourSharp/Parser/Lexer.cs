/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 16:07
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


using System;
using System.Collections.Generic;
using System.Text;
using Iuker.YourSharp.Parser;
using Iuker.YourSharp.Common;

namespace Iuker.YourSharp
{
    /// <summary>
    /// 单词解析器
    /// </summary>
    public class Lexer
    {
        /// <summary>
        /// 已进入单词解析器的所有源代码
        /// </summary>
        private readonly Dictionary<string, SourceCode> m_SourceCodes = new Dictionary<string, SourceCode>();

        /// <summary>
        /// 单词集合字典
        /// </summary>
        public Dictionary<string, TokenSet> TokenSetDictionary { get; private set; } = new Dictionary<string, TokenSet>();

        private SourceCode m_CurrentSource;

        /// <summary>
        /// 字符读取光标
        /// </summary>
        private ushort m_Cursor = 0;

        /// <summary>
        /// 当前解析的源码内容
        /// </summary>
        private string m_Code;

        /// <summary>
        /// 当前字符所在行
        /// </summary>
        public ushort CurrentLine { get; private set; } = 1;

        /// <summary>
        /// 当前字符所在列
        /// </summary>
        public ushort CurrentColumnn { get; private set; }


        public Lexer Init()
        {
            return this;
        }

        public void Scan(SourceCode sourceCode)
        {
            if (!m_SourceCodes.ContainsKey(sourceCode.Name))
            {
                m_TokenInndex = 0;
                m_SourceCodes.Add(sourceCode.Name, sourceCode);
                m_CurrentSource = sourceCode;
                m_Code = sourceCode.Codes;
                TokenSetDictionary.Add(m_CurrentSource.Name, new TokenSet(m_CurrentSource.Name));
            }
            else
            {
            }

            ReadToken();
        }

        /// <summary>
        /// 当前是否已读取湾源码所有字符
        /// </summary>
        /// <returns></returns>
        private bool CursorNotEof => m_Cursor < m_Code.Length - 1;

        private bool mIsRn;

        private bool mIsWhiteSpace;

        /// <summary>
        /// 跳过空白字符
        /// </summary>
        private void SkipWhiteSpace()
        {
            mIsRn = IsRn();
            mIsWhiteSpace = CurrentChar == ' ';

            if (mIsRn)
            {
                CurrentLine++;
                CurrentColumnn = 0;
                m_Cursor += 2;
                SkipWhiteSpace();
            }
            if (mIsWhiteSpace)
            {
                CurrentColumnn++;
                m_Cursor++;
                SkipWhiteSpace();
            }
        }

        /// <summary>
        /// 指向一下个字符并跳过无用字符
        /// </summary>
        private void CursorNextAndSkipSpace()
        {
            // 如果当前未到达源码终点
            if (CursorNotEof)
            {
                if (IsRn())
                {
                    SkipRn();
                }
                if (CurrentChar == ' ')
                {
                    SkipOne();
                }
                else
                {
                    CurrentColumnn++;
                }

                m_Cursor++;
            }
        }

        private void CursorNext()
        {
            if (CursorNotEof)
            {
                CurrentColumnn++;
                m_Cursor++;
            }
        }

        /// <summary>
        /// 获得当前的字符
        /// </summary>
        /// <returns></returns>
        public char CurrentChar => CursorNotEof ? m_Code[m_Cursor] : '\0';

        public char NextChar => CursorNotEof ? m_Code[m_Cursor + 1] : '\0';



        private void SkipOne()
        {
            CurrentColumnn++;
            m_Cursor++;
        }

        private void SkipRn()
        {
            CurrentLine++;
            CurrentColumnn = 0;
            m_Cursor += 2;
        }

        private void SkipBr()
        {
            CurrentLine++;
            CurrentColumnn = 0;
            m_Cursor++;
        }

        private bool IsRn()
        {
            if (CurrentChar == '\r' && NextChar == '\n')
            {
                return true;
            }
            return false;
        }

        private Token mToken;

        private void ReadToken()
        {
            while (true)
            {
                SkipWhiteSpace(); //  跳过空白字符

                if (!CursorNotEof)
                {
                    mToken = CreateToken(TokenType.Eof, CurrentLine, CurrentColumnn, "<EOF>");
                    TokenSetDictionary[m_CurrentSource.Name].AddToken(mToken);
                    return;
                }

                switch (CurrentChar)
                {
                    case '+':
                        mToken = PotentiallyDoubleCharOperator(TokenType.Add, '+', TokenType.SelfAdd, '=', TokenType.AddAssignment);
                        break;
                    case '-':
                        mToken = PotentiallyDoubleCharOperator(TokenType.Minus, '-', TokenType.SelfMinus, '=', TokenType.MinusAssignment);
                        break;
                    case '/':
                        TryCreateDivToken();
                        break;
                    case '*':
                        mToken = PotentiallyDoubleCharOperator(TokenType.Mul, '=', TokenType.MulAssignment, char.MaxValue, TokenType.None);
                        break;
                    case '<':
                        mToken = PotentiallyDoubleCharOperator(TokenType.Less, '=', TokenType.LessThan, char.MaxValue, TokenType.None);
                        break;
                    case '>':
                        mToken = PotentiallyDoubleCharOperator(TokenType.Greater, '=', TokenType.GreaterThan, char.MaxValue, TokenType.None);
                        break;
                    case '=':
                        mToken = PotentiallyDoubleCharOperator(TokenType.Assignment, '=', TokenType.DoubleEqual, Char.MaxValue, TokenType.None);
                        break;
                    case '(':
                        mToken = CreateSingleToken(TokenType.LeftBracket, "(");
                        break;
                    case ')':
                        mToken = CreateSingleToken(TokenType.RightBracket, ")");
                        break;
                    case ',':
                        mToken = CreateSingleToken(TokenType.Comma, ",");
                        break;
                    case ';':
                        mToken = CreateSingleToken(TokenType.Colon, ";");
                        break;
                    case '\'':
                        mToken = CreateSingleToken(TokenType.SingleQuote, "\'");
                        break;
                    case '\"':
                        mToken = CreateSingleToken(TokenType.DoubleQuote, "\"");
                        break;
                    case '.':
                        mToken = CreateSingleToken(TokenType.Dot, ".");
                        break;

                    default:
                        if (char.IsLetter(CurrentChar) || CurrentChar == '_')
                        {
                            mToken = CreateIdentifier(CurrentChar);
                            mToken.TryAsKeywordOrNameSpace();
                        }
                        if (char.IsDigit(CurrentChar))
                        {
                            mToken = CreateNumber(CurrentChar);
                        }
                        break;
                }

                TokenSetDictionary[m_CurrentSource.Name].AddToken(mToken);
                if (m_CommentToken != null)
                {
                    TokenSetDictionary[m_CurrentSource.Name].AddToken(m_CommentToken);
                    m_CommentToken = null;
                }
            }
        }

        /// <summary>
        /// 当前单词
        /// </summary>
        private Token m_CommentToken;

        /// <summary>
        /// 尝试创建一个除相关单词或单行注释单词
        /// </summary>
        private void TryCreateDivToken()
        {
            if (NextChar == '=')
            {
                mToken = CreateSingleToken(TokenType.DivAssignment, "=");
                return;
            }
            if (NextChar == '/')
            {
                mToken = CreateSingleToken(TokenType.SingleComment, "/");
                m_CommentSb.Clear();
                CursorNextAndSkipSpace();
                m_CommentSb.Append(CurrentChar);
                var tempLine = CurrentLine;
                var tempCol = CurrentColumnn;
                ReadComment();
                //  ReadComment执行完毕后会更新行号和列号，所以使用局部变量保存状态
                m_CommentToken = CreateToken(TokenType.SingleCommentText, tempLine, tempCol, m_CommentSb.ToString());
            }
        }

        /// <summary>
        /// 循环读取注释文本直到遇到终止符（\r\n）
        /// </summary>
        private void ReadComment()
        {
            while (true)
            {
                if (NextChar != '\r')
                {
                    m_CommentSb.Append(NextChar);
                    CursorNext();
                    continue;
                }
                CursorNext();
                CursorNextAndSkipSpace();
                break;
            }
        }

        /// <summary>
        /// 创建标识符单词
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private Token CreateIdentifier(char current)
        {
            m_IdentifierSb.Clear();
            m_IdentifierSb.Append(current);
            ReadIdentifier();
            return CreateToken(TokenType.Identifier, CurrentLine, CurrentColumnn, m_IdentifierSb.ToString());
        }

        /// <summary>
        /// 创建数字单词
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private Token CreateNumber(char current)
        {
            m_NumberSb.Clear();
            m_NumberSb.Append(current);
            ReadNumber();
            return CreateToken(TokenType.Number, CurrentLine, CurrentColumnn, m_NumberSb.ToString());
        }

        /// <summary>
        /// 标识符字符串处理器
        /// </summary>
        private readonly StringBuilder m_IdentifierSb = new StringBuilder();

        /// <summary>
        /// 数字常量字符串处理器
        /// </summary>
        private readonly StringBuilder m_NumberSb = new StringBuilder();

        /// <summary>
        /// 注释字符串处理器
        /// </summary>
        private readonly StringBuilder m_CommentSb = new StringBuilder();

        /// <summary>
        /// 循环读取标识符
        /// </summary>
        private void ReadIdentifier()
        {
            while (true)
            {
                if (char.IsLetter(NextChar) || char.IsDigit(NextChar))
                {
                    m_IdentifierSb.Append(NextChar);
                    CursorNextAndSkipSpace();
                    continue;
                }
                CursorNextAndSkipSpace();
                break;
            }
        }

        /// <summary>
        /// 循环读取数值
        /// </summary>
        private void ReadNumber()
        {
            while (true)
            {
                if (char.IsDigit(NextChar))
                {
                    m_NumberSb.Append(NextChar);
                    CursorNextAndSkipSpace();
                    continue;
                }
                CursorNextAndSkipSpace();
                break;
            }
        }

        /// <summary>
        /// 创建可能存在的二元操作符单词，如果不存在则创建一元操作符单词。
        /// </summary>
        /// <param name="singleType"></param>
        /// <param name="doubleChar1"></param>
        /// <param name="doubleType1"></param>
        /// <param name="doubleChar2"></param>
        /// <param name="doubleType2"></param>
        /// <returns></returns>
        private Token PotentiallyDoubleCharOperator
        (
            TokenType singleType,
            char doubleChar1,
            TokenType doubleType1,
            char doubleChar2,
            TokenType doubleType2
        )
        {
            if (NextChar == doubleChar1)
            {
                return CreateDoubleToken(doubleType1);
            }
            if (NextChar == doubleChar2)
            {
                return CreateDoubleToken(doubleType2);
            }
            return CreateSingleToken(singleType, null);
        }

        /// <summary>
        /// 创建一个可能的二元操作符单词
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        private Token CreateDoubleToken(TokenType tokenType)
        {
            CursorNextAndSkipSpace();
            CursorNextAndSkipSpace();
            return CreateToken(tokenType, CurrentLine, CurrentColumnn, null);
        }

        /// <summary>
        /// 创建一个一元单词
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="literal"></param>
        /// <returns></returns>
        private Token CreateSingleToken(TokenType tokenType, string literal)
        {
            CursorNextAndSkipSpace();
            return CreateToken(tokenType, CurrentLine, CurrentColumnn, literal);
        }

        private Token CreateToken(TokenType tokenType, int beginLine, int beginColumn, string literal)
        {
            var newToken = new Token(beginLine, beginColumn)
            {
                TokenType = tokenType,
                StringLiterals = literal,
                TokenIndex = m_TokenInndex
            };

            m_TokenInndex++;
            m_LastToken.NexToken = newToken;
            newToken.PrevToken = m_LastToken;
            m_LastToken = newToken;

            return newToken;
        }

        private int m_TokenInndex;

        /// <summary>
        /// 默认起始单词表示空
        /// </summary>
        private Token m_LastToken = new Token(0, 0)
        {
            TokenType = TokenType.EStart,
            StringLiterals = null
        };

        /// <summary>
        /// 创建单词集合
        /// 单词集合可以来自脚本文件或代码块
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        public TokenSet GetTokenSet(string setName) => TokenSetDictionary[setName];








    }
}
