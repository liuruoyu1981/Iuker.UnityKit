using System;

namespace Iuker.Js.Parser
{
    /// <summary>
    /// 解析异常
    /// </summary>
    public class ParserException : Exception
    {
        public int Column;
        public string Description;
        public int Index;
        public int LineNumber;

        //#if PORTABLE
        public string Source;
        //#endif

        public ParserException(string message) : base(message)
        {
        }



    }
}
