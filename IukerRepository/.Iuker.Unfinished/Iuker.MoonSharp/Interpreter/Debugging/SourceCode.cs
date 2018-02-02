using System;
using System.Collections.Generic;
using System.Text;

namespace Iuker.MoonSharp.Interpreter.Debugging
{
    /// <summary>
    /// 表示给定脚本的源代码的类
    /// </summary>
    public class SourceCode : IScriptPrivateResource
    {
        public string Name { get; private set; }

        public string Code { get; private set; }

        /// <summary>
        /// 源码的所有行
        /// 源码以行表示的字符串数组
        /// </summary>
        public string[] Lines { get; private set; }

        public Script OwnerScript { get; private set; }

        public int SourceID { get; private set; }

        internal List<SourceRef> Refs { get; private set; }

        internal SourceCode(string name, string code, int sourceID, Script ownerScript)
        {
            Refs = new List<SourceRef>();

            List<string> lines = new List<string>();

            Name = name;
            Code = code;

            lines.Add($"-- Begin of chunk : {name} ");

            lines.AddRange(Code.Split('\n'));

            Lines = lines.ToArray();

            OwnerScript = ownerScript;
            SourceID = sourceID;
        }

        /// <summary>
        /// 获取一个脚本引用对象的代码片段
        /// </summary>
        /// <param name="sourceCodeRef"></param>
        /// <returns></returns>
        public string GetCodeSnippet(SourceRef sourceCodeRef)
        {
            if (sourceCodeRef.FromLine == sourceCodeRef.ToLine)
            {
                int from = AdjustStrIndex(Lines[sourceCodeRef.FromLine], sourceCodeRef.FromChar);
                int to = AdjustStrIndex(Lines[sourceCodeRef.FromLine], sourceCodeRef.ToChar);
                return Lines[sourceCodeRef.FromLine].Substring(from, to - from);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = sourceCodeRef.FromLine; i <= sourceCodeRef.ToLine; i++)
            {
                if (i == sourceCodeRef.FromLine)
                {
                    int from = AdjustStrIndex(Lines[i], sourceCodeRef.FromChar);
                    sb.Append(Lines[i].Substring(from));
                }
                else if (i == sourceCodeRef.ToLine)
                {
                    int to = AdjustStrIndex(Lines[i], sourceCodeRef.ToChar);
                    sb.Append(Lines[i].Substring(0, to + 1));
                }
                else
                {
                    sb.Append(Lines[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 调整字符串索引
        /// </summary>
        /// <param name="str"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        private int AdjustStrIndex(string str, int loc) => Math.Max(Math.Min(str.Length, loc), 0);





    }
}
