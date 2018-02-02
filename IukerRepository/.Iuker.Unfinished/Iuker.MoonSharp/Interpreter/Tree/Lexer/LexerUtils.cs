/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/15 10:21:59
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
using System.Globalization;
using System.Text;
using Iuker.MoonSharp.Interpreter.Compatibility;
using Iuker.MoonSharp.Interpreter.Errors;

namespace Iuker.MoonSharp.Tree
{
    /// <summary>
    /// 词法单元工具
    /// </summary>
    internal static class LexerUtils
    {
        /// <summary>
        /// 尝试解析一个词法单词对象的数字值
        /// </summary>
        /// <param name="token">待解析的单词对象</param>
        /// <returns></returns>
        public static double ParNumber(Token token)
        {
            string text = token.Text;
            double res;
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out res))
                throw new SyntaxErrorException(token, "malformed number near '{0}'", text);

            return res;
        }

        /// <summary>
        /// 尝试解析一个词法单词的十六进制数值
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static double ParseHexInteger(Token token)
        {
            string text = token.Text;
            if ((text.Length < 2) || (text[0] != '0' && (char.ToUpper(text[1]) != 'X')))
                throw new InternalErrorException("hex numbers must start with '0x' near '{0}'.", text);

            ulong res;

            if (!ulong.TryParse(text.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out res))
                throw new SyntaxErrorException(token, "malformed number near '{0}'", text);

            return res;
        }

        public static string ReadHexProgressive(string s, ref double d, out int digits)
        {
            digits = 0;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if (CharIsHexDigit(c))
                {
                    int v = HexDigit2Value(c);
                    d *= 16.0;
                    d += v;
                    ++digits;
                }
                else
                {
                    return s.Substring(i);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 尝试解析一个词法单词的十六进制浮点值
        /// </summary>
        /// <param name="token">待解析的单词对象</param>
        /// <returns></returns>
        public static double ParseHexFloat(Token token)
        {
            string text = token.Text;

            try
            {
                if ((text.Length < 2) || (text[0] != '0' && (char.ToUpper(text[1]) != 'X')))
                    throw new InternalErrorException("hex float must start with '0x' near '{0}'", text);

                text = text.Substring(2);

                double value = 0.0;
                int dummy, exp = 0;

                text = ReadHexProgressive(text, ref value, out dummy);

                if (text.Length > 0 && text[0] == '.')
                {
                    text = text.Substring(1);
                    text = ReadHexProgressive(text, ref value, out exp);
                }

                exp *= -4;

                if (text.Length > 0 && char.ToUpper(text[0]) == 'p')
                {
                    if (text.Length == 1)
                        throw new SyntaxErrorException(token, "invalid hex float format near '{0}'", text);

                    text = text.Substring(text[1] == '+' ? 2 : 1);
                    int exp1 = int.Parse(text, CultureInfo.InvariantCulture);
                    exp += exp1;
                }

                double result = value * Math.Pow(2, exp);
                return result;
            }
            catch (FormatException)
            {
                throw new SyntaxErrorException(token, "malformed number near '{0}'", text);
            }
        }

        public static int HexDigit2Value(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'A' && c <= 'F')
                return 10 + (c - 'A');
            if (c >= 'a' && c <= 'f')
            {
                return 10 + (c - 'a');
            }
            throw new InternalErrorException("invalid hex digit near '{0}'", c);
        }

        /// <summary>
        /// 判断一个字符是否是十六进制表达式中的有效数字
        /// 即0—9
        /// </summary>
        /// <param name="c">待判断的字符</param>
        /// <returns></returns>
        public static bool CharIsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// 判断一个字符是否是十六进制表达式中的有效字符。
        /// 即0—9、a—z、A-Z。
        /// </summary>
        /// <param name="c">待判断的字符</param>
        /// <returns></returns>
        public static bool CharIsHexDigit(char c)
        {
            return CharIsDigit(c) ||
                    c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f' ||
                c == 'A' || c == 'B' || c == 'C' || c == 'D' || c == 'E' || c == 'F';
        }


        public static string AdjustLuaLongString(string str)
        {
            if (str.StartsWith("\r\n"))
                str = str.Substring(2);
            else if (str.StartsWith("\n"))
                str = str.Substring(1);

            return str;
        }

        public static string UnescapeLuaString(Token token, string str)
        {
            if (!Framework.Do.StringContainsChar(str, '\\'))
                return str;

            StringBuilder sb = new StringBuilder();

            bool escape = false;
            bool hex = false;
            int unicode_state = 0;
            string hexprefix = "";
            string val = "";
            bool zmode = false;

            foreach (char c in str)
            {
                redo:
                if (escape)
                {
                    if (val.Length == 0 && !hex && unicode_state == 0)
                    {
                        if (c == 'a') { sb.Append('\a'); escape = false; zmode = false; }
                        else if (c == '\r') { }  // this makes \\r\n -> \\n
                        else if (c == '\n') { sb.Append('\n'); escape = false; }
                        else if (c == 'b') { sb.Append('\b'); escape = false; }
                        else if (c == 'f') { sb.Append('\f'); escape = false; }
                        else if (c == 'n') { sb.Append('\n'); escape = false; }
                        else if (c == 'r') { sb.Append('\r'); escape = false; }
                        else if (c == 't') { sb.Append('\t'); escape = false; }
                        else if (c == 'v') { sb.Append('\v'); escape = false; }
                        else if (c == '\\') { sb.Append('\\'); escape = false; zmode = false; }
                        else if (c == '"') { sb.Append('\"'); escape = false; zmode = false; }
                        else if (c == '\'') { sb.Append('\''); escape = false; zmode = false; }
                        else if (c == '[') { sb.Append('['); escape = false; zmode = false; }
                        else if (c == ']') { sb.Append(']'); escape = false; zmode = false; }
                        else if (c == 'x') { hex = true; }
                        else if (c == 'u') { unicode_state = 1; }
                        else if (c == 'z') { zmode = true; escape = false; }
                        else if (CharIsDigit(c)) { val = val + c; }
                        else throw new SyntaxErrorException(token, "invalid escape sequence near '\\{0}'", c);
                    }
                    else
                    {
                        if (unicode_state == 1)
                        {
                            if (c != '{')
                                throw new SyntaxErrorException(token, "'{' expected near '\\u'");

                            unicode_state = 2;
                        }
                        else if (unicode_state == 2)
                        {
                            if (c == '}')
                            {
                                int i = int.Parse(val, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                                sb.Append(ConvertUtf32ToChar(i));
                                unicode_state = 0;
                                val = string.Empty;
                                escape = false;
                            }
                            else if (val.Length >= 8)
                            {
                                throw new SyntaxErrorException(token, "'}' missing, or unicode code point too large after '\\u'");
                            }
                            else
                            {
                                val += c;
                            }
                        }
                        else if (hex)
                        {
                            if (CharIsHexDigit(c))
                            {
                                val += c;
                                if (val.Length == 2)
                                {
                                    int i = int.Parse(val, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                                    sb.Append(ConvertUtf32ToChar(i));
                                    zmode = false;
                                    escape = false;
                                }
                            }
                            else
                            {
                                throw new SyntaxErrorException(token, "hexadecimal digit expected near '\\{0}{1}{2}'", hexprefix, val, c);
                            }
                        }
                        else if (val.Length > 0)
                        {
                            if (CharIsDigit(c))
                            {
                                val = val + c;
                            }

                            if (val.Length == 3 || !CharIsDigit(c))
                            {
                                int i = int.Parse(val, CultureInfo.InvariantCulture);

                                if (i > 255)
                                    throw new SyntaxErrorException(token, "decimal escape too large near '\\{0}'", val);

                                sb.Append(ConvertUtf32ToChar(i));

                                zmode = false;
                                escape = false;

                                if (!CharIsDigit(c))
                                    goto redo;
                            }
                        }
                    }
                }
                else
                {
                    if (c == '\\')
                    {
                        escape = true;
                        hex = false;
                        val = "";
                    }
                    else
                    {
                        if (!zmode || !char.IsWhiteSpace(c))
                        {
                            sb.Append(c);
                            zmode = false;
                        }
                    }
                }
            }

            if (escape && !hex && val.Length > 0)
            {
                int i = int.Parse(val, CultureInfo.InvariantCulture);
                sb.Append(ConvertUtf32ToChar(i));
                escape = false;
            }

            if (escape)
            {
                throw new SyntaxErrorException(token, "unfinished string near '\"{0}\"'", sb.ToString());
            }

            return sb.ToString();
        }

        private static string ConvertUtf32ToChar(int i)
        {
#if PCL || ENABLE_DOTNET
			return ((char)i).ToString();
#else
            return char.ConvertFromUtf32(i);
#endif
        }

    }
}
