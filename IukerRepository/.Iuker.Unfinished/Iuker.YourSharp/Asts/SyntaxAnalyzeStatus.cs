/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 13:43
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

namespace YourSharp.Asts
{
    /// <summary>
    /// 语法分析器状态
    /// 用于标识当前语法分析机在分析那种语法上下文
    /// </summary>
    public enum SyntaxAnalyzeStatus : byte
    {
        /// <summary>
        /// 默认状态即无状态
        /// </summary>
        Default,

        /// <summary>
        /// 函数声明
        /// </summary>
        FunctionDefine,

        /// <summary>
        /// 函数体声明
        /// </summary>
        FunctionBodyDefine,

        /// <summary>
        /// 函数定义结束
        /// </summary>
        FunctionDefineOver,

        /// <summary>
        /// 进入全局作用域
        /// </summary>
        EnterGlobal,

        /// <summary>
        /// 全局变量定义
        /// </summary>
        GlobalFiledDefine,

        /// <summary>
        /// 全局函数=>函数头定义
        /// </summary>
        GlobalFunctionHeadDefine,

        /// <summary>
        /// 全局函数=>函数体定义
        /// </summary>
        GlobalFunction_BodyDefine,

        /// <summary>
        /// 类函数=>函数头定义
        /// </summary>
        ClassFunctionHeadDefine,

        /// <summary>
        /// 类函数=>函数体定义
        /// </summary>
        ClassFunctionBodyDefine,

        /// <summary>
        /// 实例函数=>函数头定义
        /// </summary>
        InstanceFunction_HeadDefine,

        /// <summary>
        /// 实例函数=>函数体定义
        /// </summary>
        InstanceFunction_BodyDefine,














    }
}
