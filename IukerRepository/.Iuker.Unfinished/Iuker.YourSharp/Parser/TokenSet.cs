/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 16:03
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


using System.Collections.Generic;

namespace Iuker.YourSharp.Parser
{
    /// <summary>
    /// 单词集合
    /// 一个单词集合代表来自一个文件或一个字符串的单词数据集。
    /// </summary>
    public class TokenSet
    {
        /// <summary>
        /// 是否处于全局作用域
        /// </summary>
        public bool IsInGlobal = true;

        /// <summary>
        /// 所处的完整命名空间
        /// </summary>
        public string FullNameSpace;

        /// <summary>
        /// 单词集合的代码名
        /// </summary>
        public string CodeName { get; private set; }

        public TokenSet()
        {

        }

        public TokenSet(string codeName)
        {
            CodeName = codeName;
        }

        private readonly Dictionary<int, List<Token>> m_TokensDictionary = new Dictionary<int, List<Token>>();

        private readonly Queue<int> m_IndexStack = new Queue<int>();

        public List<Token> CurrentTokens => m_TokensDictionary[m_IndexStack.Peek()];

        public void AddToken(Token token)
        {
            int locLine = token.BeginLine;

            if (m_TokensDictionary.ContainsKey(locLine))
            {
                m_TokensDictionary[locLine].Add(token);
            }
            else
            {
                m_IndexStack.Enqueue(locLine);
                m_TokensDictionary.Add(locLine, new List<Token> { token });
            }
        }

        /// <summary>
        /// 获得一行单词流若当前已无剩余的单词流则返回空
        /// </summary>
        /// <returns></returns>
        public List<Token> GetTokens()
        {
            if (m_IndexStack.Count > 1)
            {
                m_IndexStack.Dequeue();
                return CurrentTokens;
            }
            return null;
        }



    }
}
