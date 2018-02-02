/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 21:23:55
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

using System;

namespace Iuker.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EnumTypeNameAttribute : Attribute
    {
        /// <summary>
        /// 响应的控件名
        /// </summary>
        public string TypeName { get; private set; }

        public EnumTypeNameAttribute(string tyname)
        {
            TypeName = tyname;
        }
    }
}
