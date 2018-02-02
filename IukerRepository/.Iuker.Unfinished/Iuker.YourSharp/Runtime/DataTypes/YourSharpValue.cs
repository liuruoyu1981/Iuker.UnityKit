/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/19 22:19
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

namespace Iuker.YourSharp.Runtime.DataTypes
{
    /// <summary>
    /// YourSharp脚本基础值类型
    /// </summary>
    public abstract class YourSharpValue<T> where T : struct
    {
        /// <summary>
        /// 内部CSharp基础值或对象实例
        /// </summary>
        public T Value { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string Class { get; }

        /// <summary>
        /// 基础值数据类型
        /// </summary>
        public abstract YourSharpDataType DataType { get; }




















    }
}