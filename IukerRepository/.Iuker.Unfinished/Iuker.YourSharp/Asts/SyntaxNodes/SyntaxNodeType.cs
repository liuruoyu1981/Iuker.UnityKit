﻿/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 00:50
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

namespace Iuker.YourSharp.Asts.SyntaxNotes
{
    /// <summary>
    /// 语法树节点类型
    /// </summary>
    public enum SyntaxNodeType : byte
    {
        /// <summary>
        /// 访问权限声明
        /// </summary>
        AccessDefine,

        /// <summary>
        /// 返回类型
        /// </summary>
        ReturnType,

        /// <summary>
        /// 函数名
        /// </summary>
        FunctionName,

        /// <summary>
        /// 函数字段局部变量
        /// </summary>
        FunctionFiled,

        /// <summary>
        /// 函数参数
        /// </summary>
        Argument,
    }
}