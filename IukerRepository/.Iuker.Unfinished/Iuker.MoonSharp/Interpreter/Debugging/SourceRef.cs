using System;

namespace Iuker.MoonSharp.Interpreter.Debugging
{
    /// <summary>
    /// 代表脚本源代码的一个引用
    /// </summary>
    public class SourceRef
    {
        /// <summary>
        /// 获取表示此位置是否在CLR中的值。
        /// </summary>
        public bool IsClrLocation { get; private set; }

        /// <summary>
        /// 获取源码索引
        /// </summary>
        public int SourceIdx { get; private set; }

        /// <summary>
        /// 获取源码开始列号
        /// </summary>
        public int FromChar { get; private set; }

        /// <summary>
        /// 获取源码结束列号
        /// </summary>
        public int ToChar { get; private set; }

        /// <summary>
        /// 获取源码开始行号
        /// </summary>
        public int FromLine { get; private set; }

        /// <summary>
        /// 获取源码结束行号
        /// </summary>
        public int ToLine { get; private set; }

        /// <summary>
        /// 在源模式下获取一个值，指示此实例是否为停止“步骤”
        /// </summary>
        public bool IsStepStop { get; private set; }

        /// <summary>
        /// 获取一个值，指示此实例是否为断点
        /// </summary>
        public bool Breakpoint;

        /// <summary>
        /// 获取一个值，指示此实例是否不能设置为断点
        /// </summary>
        public bool CannotBreakpoint { get; private set; }

        internal static SourceRef GetClrLocation() => new SourceRef(0, 0, 0, 0, 0, false) { IsClrLocation = true };

        public SourceRef(SourceRef src, bool isStepStop)
        {
            SourceIdx = src.SourceIdx;
            FromChar = src.FromChar;
            ToChar = src.ToChar;
            FromLine = src.FromLine;
            ToLine = src.ToLine;
            IsStepStop = isStepStop;
        }

        public SourceRef(int sourceIdx, int from, int to, int fromline, int toline, bool isStepStop)
        {
            SourceIdx = sourceIdx;
            FromChar = from;
            ToChar = to;
            FromLine = fromline;
            ToLine = toline;
            IsStepStop = isStepStop;
        }

        public override string ToString()
        {
            return string.Format("[{0}]{1} ({2}, {3}) -> ({4}, {5})",
                SourceIdx, IsStepStop ? "*" : " ",
                FromLine, FromChar,
                ToLine, ToChar);
        }

        internal int GetLocationDistance(int sourceIdx, int line, int col)
        {
            const int PER_LINE_FACTOR = 1600; // we avoid computing real lines length and approximate with heuristics..

            if (sourceIdx != SourceIdx)
                return int.MaxValue;

            if (FromLine == ToLine)
            {
                if (line == FromLine)
                {
                    if (col >= FromChar && col <= ToChar)
                        return 0;
                    if (col < FromChar)
                        return FromChar - col;
                    return col - ToChar;
                }
                return Math.Abs(line - FromLine) * PER_LINE_FACTOR;
            }
            if (line == FromLine)
            {
                if (col < FromChar)
                    return FromChar - col;
                return 0;
            }
            if (line == ToLine)
            {
                if (col > ToChar)
                    return col - ToChar;
                return 0;
            }
            if (line > FromLine && line < ToLine)
            {
                return 0;
            }
            if (line < FromLine)
            {
                return (FromLine - line) * PER_LINE_FACTOR;
            }
            return (line - ToLine) * PER_LINE_FACTOR;
        }

        /// <summary>
        /// 获取源引用是否包含指定的位置
        /// </summary>
        /// <param name="sourceIdx">Index of the source.</param>
        /// <param name="line">The line.</param>
        /// <param name="col">The column.</param>
        /// <returns></returns>
        public bool IncludesLocation(int sourceIdx, int line, int col)
        {
            if (sourceIdx != SourceIdx || line < FromLine || line > ToLine)
                return false;

            if (FromLine == ToLine)
                return col >= FromChar && col <= ToChar;
            if (line == FromLine)
                return col >= FromChar;
            if (line == ToLine)
                return col <= ToChar;

            return true;
        }

        /// <summary>
        /// 设置不能断点的标志
        /// </summary>
        /// <returns></returns>
        public SourceRef SetNoBreakPoint()
        {
            CannotBreakpoint = true;
            return this;
        }

        /// <summary>
        /// 根据脚本首选项格式化位置
        /// </summary>
        /// <param name="script"></param>
        /// <param name="forceClassicFormat"></param>
        /// <returns></returns>
        public string FormatLocation(Script script, bool forceClassicFormat = false)
        {
            SourceCode sc = script.GetSourceCode(this.SourceIdx);

            if (this.IsClrLocation)
                return "[clr]";

            //if (script.Options.UseLuaErrorLocations || forceClassicFormat)
            //{
            //    return string.Format("{0}:{1}", sc.Name, this.FromLine);
            //}
            //else if (this.FromLine == this.ToLine)
            //{
            //    if (this.FromChar == this.ToChar)
            //    {
            //        return string.Format("{0}:({1},{2})", sc.Name, this.FromLine, this.FromChar, this.ToLine, this.ToChar);
            //    }
            //    else
            //    {
            //        return string.Format("{0}:({1},{2}-{4})", sc.Name, this.FromLine, this.FromChar, this.ToLine, this.ToChar);
            //    }
            //}
            //else
            //{
            //    return string.Format("{0}:({1},{2}-{3},{4})", sc.Name, this.FromLine, this.FromChar, this.ToLine, this.ToChar);
            //}

            return null;
        }



    }
}
