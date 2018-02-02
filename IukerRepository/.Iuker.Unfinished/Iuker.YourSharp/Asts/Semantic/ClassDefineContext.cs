/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 00:43
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
    /// 类定义上下文
    /// </summary>
    public class ClassDefineContext
    {
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; private set; }

        public string TempNameSpace;

        /// <summary>
        /// 类所导入的命名空间
        /// </summary>
        public HashSet<string> NamespaceSet { get; private set; } = new HashSet<string>();

        /// <summary>
        /// 类所在命名空间
        /// </summary>
        public string NameSpace { get; private set; }

        /// <summary>
        /// 类全局唯一标识字符串
        /// </summary>
        public string SingleToken => NameSpace + "." + ClassName;

        /// <summary>
        /// 类静态函数语义上下文字典
        /// </summary>
        public Dictionary<string, FunctionDefineContext> StaticFunctions { get; private set; } = new Dictionary<string, FunctionDefineContext>();

        /// <summary>
        /// 类实例函数语义上下文字典
        /// </summary>
        public Dictionary<string, FunctionDefineContext> InstanceFunctions { get; private set; } = new Dictionary<string, FunctionDefineContext>();


        public ClassDefineContext() { }

        public void AddNameSpace()
        {
            NamespaceSet.Add(TempNameSpace);
            TempNameSpace = null;
        } 

        public void SetNamespace(string ns) => NameSpace = ns;

        public void SetClassName(string name) => ClassName = name;


        public ClassDefineContext(string nameSpace, string classname)
        {
            NameSpace = nameSpace;
            ClassName = classname;
        }


    }
}
