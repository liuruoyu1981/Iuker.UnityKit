/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 16:18
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
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.SyntaxNodes.Expressions
{
    /// <summary>
    /// 二元操作表达式
    /// 数组、列表
    /// </summary>
    public class BinaryExpression : SyntaxNode
    {
        public BinaryExpression(Token token) : base(token)
        {
        }

        public override SyntaxNodeType NodeType { get; }
    }
}
