/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 09:43
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

namespace Iuker.YourSharp.Runtime.MetaDatas
{
    /// <summary>
    /// 函数元数据
    /// </summary>
    public class FunctionMetaData
    {
        /// <summary>
        /// 函数名
        /// </summary>
        public string FunctionName { get; private set; }

        /// <summary>
        /// 函数作用域
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// 函数返回值类型
        /// </summary>
        public string ReturnType { get; private set; }

        /// <summary>
        /// 函数参数元数据列表
        /// </summary>
        public List<ArgumentMetaData> ArgumentMetaDatas { get; private set; }



    }
}
