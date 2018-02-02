/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 07:54
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
    /// 函数参数节点
    /// </summary>
    public class ArgumentNode : SyntaxNode
    {
        /// <summary>
        /// 参数名
        /// </summary>
        //public string ClassName { get; private set; }

        /// <summary>
        /// 参数返回类型
        /// </summary>
        public string ReturnType { get; }

        /// <summary>
        /// 参数默认值
        /// </summary>
        public string DefaultValue { get; private set; }

        public ArgumentNode(Token token) : base(token)
        {
            // 参数节点的构造函数只能用来判定节点的类型

            ReturnType = token.StringLiterals;
        }

        /// <summary>
        /// 设置参数名
        /// </summary>
        /// <param name="name"></param>
        public void SetArgumentName(Token name)
        {
            NodeName = name.StringLiterals;
        }

        /// <summary>
        /// 设置参数的默认值
        /// 默认值只能是基础类型
        /// </summary>
        /// <param name="defaulToken"></param>
        public void SetArgumentDefaultValue(Token defaulToken)
        {
            DefaultValue = defaulToken.StringLiterals;
        }

        public override SyntaxNodeType NodeType { get; } = SyntaxNodeType.Argument;
    }
}
