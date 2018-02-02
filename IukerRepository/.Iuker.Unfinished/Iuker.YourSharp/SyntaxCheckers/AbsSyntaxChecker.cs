/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 17:00
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

using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxCheckers
{
    /// <summary>
    /// 语法检测器基类
    /// </summary>
    public abstract class AbsSyntaxChecker : ISyntaxChecker
    {
        public abstract SyntaxAnalyzerResult IsOK(Token next, SyntaxAnalyzer analyzer);
        public virtual void OnError(Token token)
        {
            throw new System.NotImplementedException();
        }

        public abstract string ProcessedSituation { get; }
    }
}
