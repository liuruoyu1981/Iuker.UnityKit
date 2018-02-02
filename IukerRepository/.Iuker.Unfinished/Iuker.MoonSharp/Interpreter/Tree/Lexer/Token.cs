/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/15 10:02:41
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

namespace Iuker.MoonSharp.Tree
{
    /// <summary>
    /// 词法单元
    /// </summary>
    public class Token
    {
        public readonly int SourceId;

        public readonly int FromCol, ToCol, FromLine, ToLine, PrevCol, PrevLine;

        public readonly TokenType Type;

        public string Text { get; set; }

        public Token(TokenType type, int sourceId, int fromLine, int fromCol, int toLine, int toCol, int prevLine,
            int prevCol)
        {
            Type = type;

            SourceId = sourceId;
            FromLine = fromLine;
            FromCol = fromCol;
            ToCol = toCol;
            ToLine = toLine;
            PrevCol = prevCol;
            PrevLine = prevLine;
        }

        public override string ToString()
        {
            string tokenTypeString = (Type + "                                                      ").Substring(0, 16);

            string location = string.Format("{0}:{1}-{2}:{3}", FromLine, FromCol, ToLine, ToCol);
            location = (location + "                                                      ").Substring(0, 10);
            return string.Format("{0}  - {1} - '{2}'", tokenTypeString, location, this.Text ?? "");
        }

        /// <summary>
        /// 解析字符串为词法单元类型
        /// </summary>
        /// <param name="reservedWord">待解析的字符串</param>
        /// <returns></returns>
        public static TokenType? GetReservedTokenType(string reservedWord)
        {
            switch (reservedWord)
            {
                case "and":
                    return TokenType.And;
                case "break":
                    return TokenType.Break;
                case "do":
                    return TokenType.Do;
                case "else":
                    return TokenType.Else;
                case "elseif":
                    return TokenType.ElseIf;
                case "end":
                    return TokenType.End;
                case "false":
                    return TokenType.False;
                case "for":
                    return TokenType.For;
                case "function":
                    return TokenType.Function;
                case "goto":
                    return TokenType.Goto;
                case "if":
                    return TokenType.If;
                case "in":
                    return TokenType.In;
                case "local":
                    return TokenType.Local;
                case "nil":
                    return TokenType.Nil;
                case "not":
                    return TokenType.Not;
                case "or":
                    return TokenType.Or;
                case "repeat":
                    return TokenType.Repeat;
                case "return":
                    return TokenType.Return;
                case "then":
                    return TokenType.Then;
                case "true":
                    return TokenType.True;
                case "until":
                    return TokenType.Until;
                case "while":
                    return TokenType.While;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获得一个单词的数字值
        /// </summary>
        /// <returns></returns>
        public double GetNumberValue()
        {
            switch (Type)
            {
                    case TokenType.Number:
                    return LexerUtils.ParNumber(this);
                    case TokenType.Number_Hex:
                    return LexerUtils.ParseHexInteger(this);
                    case TokenType.Number_HexFloat:
                    return LexerUtils.ParseHexFloat(this);
                default:
                    throw new NotSupportedException("GetNumberValue is supported only on numeric tokens");
            }
        }

        /// <summary>
        /// 是否是一个结束块单词
        /// </summary>
        /// <returns></returns>
        public bool IsEndOfBlock()
        {
            switch (Type)
            {
                case TokenType.Else:
                case TokenType.ElseIf:
                case TokenType.End:
                case TokenType.Until:
                case TokenType.Eof:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 是否是一个一元操作符单词
        /// </summary>
        /// <returns></returns>
        public bool IsUnaryOperator()
        {
            return Type == TokenType.Op_MinusOrSub || Type == TokenType.Not || Type == TokenType.Op_Len;
        }

        /// <summary>
        /// 是否是一个二元操作符单词
        /// </summary>
        public bool IsBinaryOperator()
        {
            switch (Type)
            {
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Op_Equal:
                case TokenType.Op_LessThan:
                case TokenType.Op_LessThanEqual:
                case TokenType.Op_GreaterThanEqual:
                case TokenType.Op_GreaterThan:
                case TokenType.Op_NotEqual:
                case TokenType.Op_Concat:
                case TokenType.Op_Pwr:
                case TokenType.Op_Mod:
                case TokenType.Op_Div:
                case TokenType.Op_Mul:
                case TokenType.Op_MinusOrSub:
                case TokenType.Op_Add:
                    return true;
                default:
                    return false;
            }
        }
    }
}
