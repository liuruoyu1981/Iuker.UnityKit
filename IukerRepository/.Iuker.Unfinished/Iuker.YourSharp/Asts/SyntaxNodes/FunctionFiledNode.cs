/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 14:56
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
    public class FunctionFiledNode : SyntaxNode
    {
        public FunctionFiledNode(Token token, Token prevToken) : base(token)
        {
            NodeName = token.StringLiterals;
            FiledType = prevToken.StringLiterals;
        }

        /// <summary>
        /// 局部变量类型
        /// </summary>
        public string FiledType { get; private set; }

        public override SyntaxNodeType NodeType { get; } = SyntaxNodeType.FunctionFiled;



    }
}
