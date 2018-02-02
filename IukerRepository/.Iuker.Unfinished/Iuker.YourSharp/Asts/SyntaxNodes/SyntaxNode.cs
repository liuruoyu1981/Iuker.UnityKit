/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 22:48
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

namespace Iuker.YourSharp.Asts.SyntaxNotes
{
    /// <summary>
    /// 抽象语法树基节点
    /// </summary>
    public abstract class SyntaxNode
    {
        public string NodeName;
        public abstract SyntaxNodeType NodeType { get; }
        //public Location Location;

        public SyntaxNode(Token token)
        {

        }

        /// <summary>
        /// 将当期实例转换为指定类型的其他语法树节点。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T As<T>() where T : SyntaxNode => (T)this;


    }
}
