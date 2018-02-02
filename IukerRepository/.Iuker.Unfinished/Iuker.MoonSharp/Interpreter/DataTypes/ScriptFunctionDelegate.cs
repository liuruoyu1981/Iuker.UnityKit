/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:50:50
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



namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// 一个可以封装脚本函数的委托类型
    /// A Delegate type which can wrap a script function
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The return value of the script function</returns>
    public delegate object ScriptFunctionDelegate(params object[] args);

    /// <summary>
    /// 一种委托类型，它可以用泛型返回值来包装一个脚本函数。
    /// A Delegate type which can wrap a script function with a generic typed return value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args">The arguments.</param>
    /// <returns>The return value of the script function</returns>
    public delegate T ScriptFunctionDelegate<T>(params object[] args);
}
