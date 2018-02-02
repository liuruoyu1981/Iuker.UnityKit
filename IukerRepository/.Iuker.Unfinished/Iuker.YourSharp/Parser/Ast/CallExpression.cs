/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 22:02
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

namespace Iuker.YourSharp.Parser.Ast
{
    /// <summary>
    /// 调用表达式
    /// </summary>
    public class CallExpression : Expression
    {
        /// <summary>
        /// 被调用的表达式
        /// </summary>
        public Expression Callee;

        /// <summary>
        /// 被调用的表达式（函数）的参数列表
        /// </summary>
        public IList<Expression> Arguments;

        /// <summary>
        /// 是否已缓存
        /// </summary>
        public bool Cached;

        /// <summary>
        /// 能否缓存
        /// 默认为真
        /// </summary>
        public bool CanBeCached = true;

        /// <summary>
        /// 已缓存的参数值
        /// </summary>
        //public JsValue[] CachedArguments;

    }
}