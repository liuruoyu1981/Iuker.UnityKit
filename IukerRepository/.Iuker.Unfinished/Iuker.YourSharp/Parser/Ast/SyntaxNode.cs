/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 17:35
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

using System.Diagnostics;

namespace Iuker.YourSharp.Parser.Ast
{
    /// <summary>
    /// 语法树节点
    /// </summary>
    public class SyntaxNode
    {
        /// <summary>
        /// 语法树节点类型
        /// </summary>
        public SyntaxNodeType NodeType;

        public int[] Range;

        /// <summary>
        /// 语法树节点位置信息
        /// 开始位置（所在行，所在列）
        /// 结束位置（所在行，所在列）
        /// </summary>
        public Location Location;

        /// <summary>
        /// 转为具体的语法树子类类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T As<T>() where T : SyntaxNode => (T)this;


    }
}