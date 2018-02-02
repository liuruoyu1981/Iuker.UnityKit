/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/19 22:30
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
    /// YourSharp基础值类型Int32
    /// </summary>
    public sealed class YourSharp_Int32 : YourSharpValue<int>
    {
        public override string Class => "Int32";
        public override YourSharpDataType DataType => YourSharpDataType.Int32;
    }
}