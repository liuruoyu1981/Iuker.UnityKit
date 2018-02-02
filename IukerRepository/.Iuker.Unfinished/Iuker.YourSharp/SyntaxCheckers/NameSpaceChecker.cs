/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 22:49
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

using System.Linq;
using System.Text;
using Iuker.YourSharp.Common;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers
{
    /// <summary>
    /// 命名空间检查器
    /// 1.命名空间字符串是否符合命名规范（MySharp.Common.Util）
    /// 2. 是否和已有命名空间重复
    /// </summary>
    public class NameSpaceChecker : AbsSyntaxChecker
    {
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            // 单词字面量不是以标识符结束则错误（MySharp.Common。）
            var lastToken = analyzer.CurrentTokens.Last();
            if (lastToken.TokenType != TokenType.Identifier) // 当前行单词流不是以标识符结尾则错误
            {
                return SyntaxAnalyzerResult.Failed;
            }

            return CheckNamespace(next, analyzer);
        }

        /// <summary>
        /// 判断给定的字符串是否符合字符串命名规范
        /// </summary>
        /// <param name="token"></param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        private SyntaxAnalyzerResult CheckNamespace(Token token, SyntaxAnalyzer analyzer)
        {
            while (true)
            {
                // 单独标识（Mysharp）
                if (token.TokenType == TokenType.Identifier && token.BeginLine != token.NexToken.BeginLine)
                {
                    // 命名空间解析完成
                    analyzer.CurrentNameSpace = ResolverNamespace(analyzer);
                    // 转换语法分析器当前状态为类头部声明
                    analyzer.SyntaxAnalyzeType = SyntaxAnalyzeType.ClassHeadDefine;
                    return SyntaxAnalyzerResult.Completed;
                }

                // 连续命名空间标识符（MySharp.Common.Test）
                if (token.TokenType == TokenType.Identifier && token.NexType == TokenType.Dot)
                {
                    token = token.NexToken.NexToken;
                    continue;
                }

                return SyntaxAnalyzerResult.Failed;
            }
        }

        /// <summary>
        /// 解析命名空间
        /// </summary>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        private string ResolverNamespace(SyntaxAnalyzer analyzer)
        {
            var currentTokens = analyzer.CurrentTokens;
            var sb = new StringBuilder();
            sb.Clear();

            for (int i = 1; i < currentTokens.Count; i++)
            {
                var token = currentTokens[i];
                sb.Append(token.StringLiterals);
            }

            var result = sb.ToString();
            //CacheFactory.StringBuilderPool.Restore(sb);
            return result;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.NameSpace;
    }
}
