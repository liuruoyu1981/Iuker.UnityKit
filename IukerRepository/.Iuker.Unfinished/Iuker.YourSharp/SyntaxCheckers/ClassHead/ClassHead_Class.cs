/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/22 11:09
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

using Iuker.YourSharp.Asts.Semantic;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers.ClassHead
{
    public class ClassHead_Class : AbsSyntaxChecker
    {
        /// <summary>
        /// 当前单词是否是合法类名
        /// </summary>
        private bool mIsLegalClassName;
        public override SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer)
        {
            mIsLegalClassName = analyzer.IsLegalClassName(next);

            if (mIsLegalClassName)
            {
                ClassDefineContext classDefineContext = new ClassDefineContext(analyzer.CurrentNameSpace, next.StringLiterals);
                analyzer.InsertClassContext(classDefineContext);
                analyzer.SyntaxAnalyzeType = SyntaxAnalyzeType.ClassBodyDefine;    //  将语法分析机切换为类体定义状态

                return SyntaxAnalyzerResult.Completed;
            }


            return SyntaxAnalyzerResult.Failed;
        }

        public override string ProcessedSituation { get; } = SyntaxSituation.ClassHead_Class;
    }
}