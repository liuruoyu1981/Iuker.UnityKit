/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/22 11:55
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

namespace Iuker.YourSharp.Asts.SyntaxNodes
{
    /// <summary>
    /// 类字段
    /// </summary>
    public class ClassFieldNode : SyntaxNode
    {
        public string FiledType { get; private set; }


        public ClassFieldNode(Token token,string filedType) : base(token)
        {
            NodeName = token.StringLiterals;
            FiledType = filedType;
        }

        public override SyntaxNodeType NodeType { get; }
    }
}
