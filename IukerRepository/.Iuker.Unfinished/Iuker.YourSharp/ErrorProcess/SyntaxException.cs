using System;
using Iuker.YourSharp.Asts;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.ErrorProcess
{
    /// <summary>
    /// 语法异常
    /// </summary>
    public class SyntaxException : Exception
    {
        public SyntaxException(SyntaxAnalyzer analyzer, Token errorToken)
            : base($"代码id为{analyzer.BuildingSet.CodeName}中第{errorToken.BeginLine}行第{errorToken.BeginColumn}列有语法错误, 请检查！")
        {

        }

    }
}
