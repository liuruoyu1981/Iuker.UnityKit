/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 15:58
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
using System.Linq;

namespace Iuker.YourSharp.Parser
{
    /// <summary>
    /// 词法单词
    /// </summary>
    public class Token
    {
        /// <summary>
        /// 单词的类型
        /// </summary>
        public TokenType TokenType;

        /// <summary>
        /// 单词第一个字符所在的行号
        /// </summary>
        //public int BeginLine;

        /// <summary>
        /// 单词最后一个字符所在的行号
        /// </summary>
        public int BeginLine => Location.Start.Line;

        /// <summary>
        /// 单词第一个字符所在的列号
        /// </summary>
        public int BeginColumn => Location.Start.Column;

        /// <summary>
        /// 单词最后一个字符所在的列号
        /// </summary>
        //public int EndColumn;

        /// <summary>
        /// 单词的字符串字面量
        /// </summary>
        public string StringLiterals;

        /// <summary>
        /// 单词实例之后的一个单词
        /// </summary>
        public Token NexToken;

        /// <summary>
        /// 单词实例之前的一个单词
        /// </summary>
        public Token PrevToken;

        /// <summary>
        /// 单词在单词集合中的索引
        /// </summary>
        public int TokenIndex;

        public Token(int line, int column)
        {
            Location = new Location(line, column);

            // 改成扩展方法支持
            mCostBaseValueFuncs = new List<Func<bool>>();

            mCostBaseValueFuncs.Add(TryToDecimal);
            mCostBaseValueFuncs.Add(TryToFloat);
            mCostBaseValueFuncs.Add(TryToInt32);
            mCostBaseValueFuncs.Add(TryToDouble);
        }

        /// <summary>
        /// 尝试转换字面值为基础类型
        /// </summary>
        /// <returns></returns>
        public bool TryAsBaseType() => mCostBaseValueFuncs.Any(func => func());

        private bool TryToFloat()
        {
            float floatValue;
            return float.TryParse(StringLiterals, out floatValue);
        }

        private bool TryToInt32()
        {
            int intValue;
            return int.TryParse(StringLiterals, out intValue);
        }

        private bool TryToDouble()
        {
            double doubleValue;
            return double.TryParse(StringLiterals, out doubleValue);
        }


        private bool TryToDecimal()
        {
            decimal decimalValue;
            return decimal.TryParse(StringLiterals, out decimalValue);
        }

        private readonly List<Func<bool>> mCostBaseValueFuncs;

        private static readonly Dictionary<string, TokenType> s_KeywordDictionary = new Dictionary<string, TokenType>
        {
            {"void",TokenType.Void },
            {"public",TokenType.Public },
            {"abstract",TokenType.Abstract },
            {"as",TokenType.As },
            {"base",TokenType.Base },
            {"bool",TokenType.Bool },
            {"break",TokenType.Break },
            {"byte",TokenType.Byte },
            {"int",TokenType.Int },
            {"if",TokenType.If },
            {"else",TokenType.Else },
            {"end",TokenType.End },
            {"static",TokenType.Static },
            {"namespace",TokenType.NameSpace },
            {"class",TokenType.Class },
            {"import",TokenType.Import },
        };

        /// <summary>
        /// 单词是否符合函数、属性、命名空间命名规则
        /// 函数及属性不能以下划线和小写字母开头
        /// </summary>
        public bool IsFuncPropNsIdentifier => !StringLiterals.StartsWith("_") && !char.IsLower(StringLiterals.First());

        /// <summary>
        /// 单词是否符合函数参数命名规范
        /// 参数命名必须以小写字母开头
        /// </summary>
        public bool IsArgumentIdentifier => char.IsLower(StringLiterals.First());


        /// <summary>
        /// 尝试将单词的类型从标识符转为关键字或命名空间字符串
        /// </summary>
        public void TryAsKeywordOrNameSpace()
        {
            if (s_KeywordDictionary.ContainsKey(StringLiterals))
            {
                TokenType = s_KeywordDictionary[StringLiterals];
            }
        }

        public Location Location { get; private set; }

        /// <summary>
        /// 单词字面量是否是合法的返回类型
        /// </summary>
        //public bool IsReturnType => Global.IsReturnType(StringLiterals);

        /// <summary>
        /// 单词是否是字段或参数声明的合法类型。
        /// </summary>
        //public bool IsFiledOrArgumentType => Global.IsFiledType(StringLiterals);

        //public bool IsLegalClassName =>

        /// <summary>
        /// 单词是否以小写字母开头
        /// </summary>
        private bool IsLowerStart => char.IsLower(StringLiterals[0]);

        /// <summary>
        /// 之后单词的类型
        /// </summary>
        public TokenType NexType => NexToken.TokenType;

        /// <summary>
        /// 之前单词的类型
        /// </summary>
        public TokenType PrevType => PrevToken.TokenType;













    }
}
