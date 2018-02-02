/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/22 00:03
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

namespace Iuker.YourSharp.Asts.Semantic
{
    /// <summary>
    /// 全局定义上下文
    /// </summary>
    public class GlobalDefineContext
    {
        /// <summary>
        /// 全局函数上下文字典
        /// </summary>
        public Dictionary<string, FunctionDefineContext> GlobalFunctionContexts { get; } = new Dictionary<string, FunctionDefineContext>();

        /// <summary>
        /// 全局类定义上下文字典
        /// </summary>
        public Dictionary<string, ClassDefineContext> GlobalClassDefineContexts { get; } = new Dictionary<string, ClassDefineContext>();

        /// <summary>
        /// 全局命名空间集合
        /// </summary>
        public HashSet<string> Namespases { get; private set; } = new HashSet<string>();

        /// <summary>
        /// 指定的命名空间是否已存在
        /// </summary>
        /// <param name="space"></param>
        /// <returns></returns>
        public bool IsNameSpaceExist(string space) => Namespases.Contains(space);

        /// <summary>
        /// 全局可返回类型集合。
        /// 返回类型用于接口、属性、及函数声明。
        /// Dsharp语言中所有的类型都包含该集合中，包含基础类型和用户自定义类型。
        /// 类型为包含完整命名空间的类型唯一名称。
        /// </summary>
        protected static readonly HashSet<string> ReturnTypesDictionary = new HashSet<string>
        {
            "int",
            "uint",
            "byte",
            "ushort",
            "long",
            "ulong",
            "void",


        };

        protected static readonly HashSet<string> ClassesSet = new HashSet<string>()
        {



        };

        /// <summary>
        /// 允许用于声明字段的类型字符串集合。
        /// 该集合需要在启动时初始化，用于加载用户自定义类型。
        /// </summary>
        protected static readonly HashSet<string> GlobalFieldSet = new HashSet<string>()
        {
            "int","uint","ushot","long","ulong","float","double",

        };

        /// <summary>
        /// 是否为合法的返回类型字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsReturnType(string name)
        {
            return ReturnTypesDictionary.Contains(name);
        }

        /// <summary>
        /// 给定单词是否是声明字段的合法类型。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsFiledType(string name) => GlobalFieldSet.Contains(name);

        /// <summary>
        /// 是否为合法的类名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsLegalClassName(string name) => !GlobalClassDefineContexts.ContainsKey(name);








    }
}
