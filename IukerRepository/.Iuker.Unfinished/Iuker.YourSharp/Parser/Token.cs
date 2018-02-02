/***********************************************************************************************
Author��liuruoyu1981
CreateDate: 2017/05/20 15:58
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************�޸���־***********************************************
1. �޸����ڣ� �޸��ˣ� �޸����ݣ�
2. �޸����ڣ� �޸��ˣ� �޸����ݣ�
3. �޸����ڣ� �޸��ˣ� �޸����ݣ�
4. �޸����ڣ� �޸��ˣ� �޸����ݣ�
5. �޸����ڣ� �޸��ˣ� �޸����ݣ�
****************************************�޸���־***********************************************/


using System;
using System.Collections.Generic;
using System.Linq;

namespace Iuker.YourSharp.Parser
{
    /// <summary>
    /// �ʷ�����
    /// </summary>
    public class Token
    {
        /// <summary>
        /// ���ʵ�����
        /// </summary>
        public TokenType TokenType;

        /// <summary>
        /// ���ʵ�һ���ַ����ڵ��к�
        /// </summary>
        //public int BeginLine;

        /// <summary>
        /// �������һ���ַ����ڵ��к�
        /// </summary>
        public int BeginLine => Location.Start.Line;

        /// <summary>
        /// ���ʵ�һ���ַ����ڵ��к�
        /// </summary>
        public int BeginColumn => Location.Start.Column;

        /// <summary>
        /// �������һ���ַ����ڵ��к�
        /// </summary>
        //public int EndColumn;

        /// <summary>
        /// ���ʵ��ַ���������
        /// </summary>
        public string StringLiterals;

        /// <summary>
        /// ����ʵ��֮���һ������
        /// </summary>
        public Token NexToken;

        /// <summary>
        /// ����ʵ��֮ǰ��һ������
        /// </summary>
        public Token PrevToken;

        /// <summary>
        /// �����ڵ��ʼ����е�����
        /// </summary>
        public int TokenIndex;

        public Token(int line, int column)
        {
            Location = new Location(line, column);

            // �ĳ���չ����֧��
            mCostBaseValueFuncs = new List<Func<bool>>();

            mCostBaseValueFuncs.Add(TryToDecimal);
            mCostBaseValueFuncs.Add(TryToFloat);
            mCostBaseValueFuncs.Add(TryToInt32);
            mCostBaseValueFuncs.Add(TryToDouble);
        }

        /// <summary>
        /// ����ת������ֵΪ��������
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
        /// �����Ƿ���Ϻ��������ԡ������ռ���������
        /// ���������Բ������»��ߺ�Сд��ĸ��ͷ
        /// </summary>
        public bool IsFuncPropNsIdentifier => !StringLiterals.StartsWith("_") && !char.IsLower(StringLiterals.First());

        /// <summary>
        /// �����Ƿ���Ϻ������������淶
        /// ��������������Сд��ĸ��ͷ
        /// </summary>
        public bool IsArgumentIdentifier => char.IsLower(StringLiterals.First());


        /// <summary>
        /// ���Խ����ʵ����ʹӱ�ʶ��תΪ�ؼ��ֻ������ռ��ַ���
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
        /// �����������Ƿ��ǺϷ��ķ�������
        /// </summary>
        //public bool IsReturnType => Global.IsReturnType(StringLiterals);

        /// <summary>
        /// �����Ƿ����ֶλ���������ĺϷ����͡�
        /// </summary>
        //public bool IsFiledOrArgumentType => Global.IsFiledType(StringLiterals);

        //public bool IsLegalClassName =>

        /// <summary>
        /// �����Ƿ���Сд��ĸ��ͷ
        /// </summary>
        private bool IsLowerStart => char.IsLower(StringLiterals[0]);

        /// <summary>
        /// ֮�󵥴ʵ�����
        /// </summary>
        public TokenType NexType => NexToken.TokenType;

        /// <summary>
        /// ֮ǰ���ʵ�����
        /// </summary>
        public TokenType PrevType => PrevToken.TokenType;













    }
}
