/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 下午5:29:24
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


namespace Iuker.Common.ActionWorkflow
{

    /// <summary>
    /// 行为处理答复者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IActionResponser<T>
    {
        /// <summary>
        /// 检查行为处理结果
        /// </summary>
        /// <returns></returns>
        bool CheckProcessResult();

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex"></param>
        void ProcessException(Exception ex);
    }

}
