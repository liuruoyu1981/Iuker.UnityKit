/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:53:26
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
    /// 一个封装协同程序请求的类
    /// Class wrapping a request to yield a coroutine
    /// </summary>
    public class YieldRequest
    {
        /// <summary>
        /// 协同程序的返回值
        /// </summary>
        public DynValue[] ReturnValues;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="YieldRequest"/> is a forced yield.
        /// </summary>
        public bool Forced { get; internal set; }
    }
}
