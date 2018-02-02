/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/23 20:02:31
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


using Iuker.Common.Module.Communication.Socket;

namespace Iuker.Common.Module.Socket
{
    /// <summary>
    /// socket通信处理模块
    /// </summary>
    public interface ISocketHandler
    {
        /// <summary>
        /// 用户连接关闭
        /// </summary>
        /// <param name="peer"></param>
        /// <param name="error"></param>
        void ClientClose(NetPeer peer, string error);

        /// <summary>
        /// 用户连接
        /// </summary>
        /// <param name="peer"></param>
        void ClientConnect(NetPeer peer);

        /// <summary>
        /// 用户消息处理
        /// </summary>
        /// <param name="peer">通信端点对象</param>
        /// <param name="bytes">网络消息包字节数组</param>
        void MessageDispense(NetPeer peer, byte[] bytes);

    }
}
