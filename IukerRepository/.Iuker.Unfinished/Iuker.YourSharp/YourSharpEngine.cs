/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 18:10
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
using Iuker.YourSharp.Asts;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp
{
    /// <summary>
    /// MySharp脚本语言执行引擎。
    /// </summary>
    public class YourSharpEngine
    {
        /// <summary>
        /// 词法分析器
        /// </summary>
        public Lexer Lexer { get; private set; }

        /// <summary>
        /// 语法分析器
        /// </summary>
        public SyntaxAnalyzer Analyzer { get; private set; }

        /// <summary>
        /// 传入执行代码次数的计数器
        /// </summary>
        private int mCodeCounter;

        /// <summary>
        /// 默认的代码命名字符串
        /// </summary>
        private readonly string mCodeName = "Block";

        /// <summary>
        /// 执行MySharp脚本语言执行环境的初始化工作。
        /// </summary>
        public YourSharpEngine Init()
        {
            Lexer = new Lexer().Init();
            Analyzer = new SyntaxAnalyzer().Init(this);

            return this;
        }

        /// <summary>
        /// 执行一段传入的脚本代码
        /// </summary>
        /// <param name="codeText"></param>
        /// <param name="friendlyName"></param>
        public void DoString(string codeText, string friendlyName = null)
        {
            mCodeCounter++;
            string codeName = null;

            if (friendlyName == null) { codeName = mCodeName + mCodeCounter; }
            SourceCode sourceCode = new SourceCode(codeName, codeText, mCodeCounter, true);
            Lexer.Scan(sourceCode);
            Analyzer.Analyze(sourceCode);
        }

        public void DoFile(string path)
        {
        }


        #region Stack

        private readonly Stack<int> mIntStack = new Stack<int>(1024);





        #endregion



        private int ExecutePlus_Int32() => mIntStack.Pop() + mIntStack.Pop();
        private int ExecuteMinus_Int32() => mIntStack.Pop() - mIntStack.Pop();
        private int ExecuteTimes_Int32() => mIntStack.Pop() * mIntStack.Pop();
        private int ExecuteDivide_Int32() => mIntStack.Pop() / mIntStack.Pop();


    }
}
