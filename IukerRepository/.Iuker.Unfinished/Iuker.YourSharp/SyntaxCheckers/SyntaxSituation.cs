/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 18:34
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

namespace Iuker.YourSharp.Asts.SyntaxCheckers
{
    /// <summary>
    /// 语法情境类型
    /// </summary>
    public static class SyntaxSituation
    {
        /// <summary>
        /// 导入命名空间声明
        /// </summary>
        public static readonly string OnImport = "OnImport";

        public static readonly string OnImport_Dot = "OnImport_Dot";


        /// <summary>
        /// 命名空间声明
        /// </summary>
        public static readonly string NameSpace = "NameSpace";

        /// <summary>
        /// 访问权限定义。
        /// </summary>
        public static readonly string AccessDefine = "AccessDefine";

        /// <summary>
        /// 返回类型声明
        /// 该情境需要依据当前分析环境决定这是一个全局函数还是一个实例函数
        /// </summary>
        public static readonly string ReturnType = "ReturnTypeOrMemberDefine";

        /// <summary>
        /// 静态声明
        /// </summary>
        public static readonly string Static = "Staic";

        /// <summary>
        /// 访问权限及静态字段声明。
        /// </summary>
        public static readonly string AccessDefind_Staic = "AccessDefind_Staic";

        /// <summary>
        /// 访问权限定义及静态只读声明。
        /// </summary>
        public static readonly string AccessDefind_Staic_Readonly = "AccessDefind_Staic_Readonly";

        /// <summary>
        /// 静态只读字段声明
        /// </summary>
        public static readonly string AccessDefind_Staic_Field = "AccessDefind_Staic_Field";

        /// <summary>
        /// 实例只读
        /// </summary>
        public static readonly string AccessDefind_Readonly = "AccessDefind_Readonly";

        /// <summary>
        /// 实例字段
        /// </summary>
        public static readonly string AccessDefind_Field = "AccessDefind_Field";

        /// <summary>
        /// 静态函数声明—返回类型节点
        /// </summary>
        public static readonly string AccessDefine_Static_ReturnType =
            "AccessDefine_Static_ReturnType";

        /// <summary>
        /// 函数定义左括号
        /// </summary>
        public static readonly string FunctionDefine_LeftBracket = "FunctionDefine_LeftBracket";

        /// <summary>
        /// 参数类型声明或右括号
        /// </summary>
        public static readonly string ArgumentTypeOrRightBracket = "ArgumentTypeOrRightBracket";

        /// <summary>
        /// 参数标识符
        /// </summary>
        public static readonly string ArgumentIdentifier = "ArgumentIdentifier";

        /// <summary>
        /// 函数声明逗号分隔
        /// </summary>
        public static readonly string ArgumentSplitComma = "ArgumentSplitComma";

        /// <summary>
        /// 参数默认值等号
        /// </summary>
        public static readonly string Argument_Default_Assignment = "Argument_Default_Assignment";

        /// <summary>
        /// 参数基础类型值
        /// </summary>
        //public static readonly string Argument_BaseTypeValue = "Argument_BaseTypeValue";

        /// <summary>
        /// 参数默认值引号或基础类型
        /// </summary>
        public static readonly string Argument_Defualt_Quate_Or_BaseValue = "Argument_Defualt_Quate_Or_BaseValue";

        /// <summary>
        /// 参数默认字符串值
        /// </summary>
        public static readonly string Argument_Default_String = "Argument_Default_String";


        #region 类定义

        #region 类头部定义

        /// <summary>
        /// 访问权限
        /// </summary>
        public static readonly string ClassHead_Access = "ClassHead_Access";


        public static readonly string ClassHead_Class = "ClassHead_Class";


        #endregion


        #region 类体定义

        public static readonly string ClassBody_ReturnType = "ClassBody_ReturnType";


        public static readonly string ClassBody_ReturnType_Identifier = "ClassBody_ReturnType_Identifier";


        public static readonly string ClassBody_ReturnType_Identifier_Assignment =
            "ClassBody_ReturnType_Identifier_Assignment";

        /// <summary>
        /// void MyTest(
        /// </summary>
        public static readonly string ClassBody_ReturnType_Identifier_LeftBracket =
            "ClassBody_ReturnType_Identifier_LeftBracket";

        /// <summary>
        /// void MyTest( int 
        /// </summary>
        public static readonly string ClassBody_ReturnType_Identifier_LeftBracket_ArgumentType =
            "ClassBody_ReturnType_Identifier_LeftBracket_ArgumentType";

        /// <summary>
        ///  void MyTest( int a
        /// </summary>
        public static readonly string ClassBody_ReturnType_Identifier_LeftBracket_ArgumentType_ArgumentIdentifier =
            "ClassBody_ReturnType_Identifier_LeftBracket_ArgumentType_ArgumentIdentifier";

        #endregion

        #endregion















































    }
}
