/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/12/2017 18:36
Email: 35490136@qq.com
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
using Iuker.Common.Base.Interfaces;

namespace Iuker.Common.Module.Communication
{
    /// <summary>
    /// 通信答复处理器
    /// </summary>
    public interface ICommunicationResponser
    {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="socketMessage"></param>
        void ProcessMessage(byte[] socketMessage);

        /// <summary>
        /// 响应器唯一标识
        /// 对应其可以相应的通信协议校验码
        /// </summary>
        int CommandId { get; }

        /// <summary>
        /// 检查处理结果
        /// </summary>
        /// <returns></returns>
        bool CheckProcessResult();

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex"></param>
        void ProcessException(Exception ex);

        /// <summary>
        /// 初始化socket通信消息响应器，该方法只会被执行一次。
        /// </summary>
        void Init(IFrame frame);

    }
}