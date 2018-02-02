/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 09:06
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


using Iuker.YourSharp.Asts.SyntaxNotes;

namespace Iuker.YourSharp.Asts.Semantic
{
    /// <summary>
    /// 语义上下文环境
    /// </summary>
    public interface ISemanticContext
    {
        /// <summary>
        /// 插入一个语法树节点
        /// </summary>
        /// <param name="syntaxNode"></param>
        ISemanticContext Insert(SyntaxNode syntaxNode);

        /// <summary>
        /// 上下文环境的唯一Id
        /// </summary>
        string SingleToken { get; }

    }
}
