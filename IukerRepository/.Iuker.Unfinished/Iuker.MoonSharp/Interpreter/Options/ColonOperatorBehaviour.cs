/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:42:44
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



namespace Iuker.MoonSharp.Interpreter.Options
{
    /// <summary>
    /// Defines behaviour of the colon ':' operator in CLR callbacks.
    /// Default behaviour is for ':' being treated the same as a '.' if the functions is implemented on the CLR side (e.g. in C#).
    /// </summary>
    public enum ColonOperatorBehaviour
    {
        /// <summary>
        /// The colon is treated the same as the dot ('.') operator.
        /// </summary>
        TreatAsDot,
        /// <summary>
        /// The colon is treated the same as the dot ('.') operator if the first argument is userdata, as a Lua colon operator otherwise.
        /// </summary>
        TreatAsDotOnUserData,
        /// <summary>
        /// The colon is treated in the same as the Lua colon operator works.
        /// </summary>
        TreatAsColon
    }
}
