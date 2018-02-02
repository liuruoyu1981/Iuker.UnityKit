/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 09:26:34
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



namespace Iuker.MoonSharp.Interpreter.Diagnostics
{
    /// <summary>
    /// 性能计数器
    /// </summary>
    public enum PerformanceCounter
    {
        /// <summary>
        /// 解析源代码并创建抽象语法数所花费的时间
        /// Measures the time spent parsing the source creating the AST
        /// </summary>
        AstCreation,
        /// <summary>
        /// 将抽象语法树转换为字节码所花费的时间
        /// Measures the time spent converting ASTs in bytecode
        /// </summary>
        Compilation,
        /// <summary>
        /// 执行脚本花费的时间
        /// Measures the time spent in executing scripts
        /// </summary>
        Execution,
        /// <summary>
        /// 动态创建/编译用户函数所花费的时间
        /// Measures the on the fly creation/compilation of functions in userdata descriptors
        /// </summary>
        AdaptersCompilation,
        /// <summary>
        /// 标记值的枚举的大小
        /// Sentinel value to get the enum size
        /// </summary>
        LastValue
    }
}
