/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/20 06:11:09
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

using Iuker.Common;
using Iuker.Common.Module.Socket;

namespace Iuker.UnityKit.Run.Module.Communication.Socket
{
    /// <summary>
    /// socket通信模块
    /// </summary>
    public interface IU3dSocketModule : IModule
    {
        /// <summary>
        /// socket通信模块的运行状态
        /// </summary>
        SocketStatus Status { get; }

        /// <summary>
        /// 异步连接至远目标程服务器
        /// 如果没有指定目标地址和端口，则会使用服务器配置项连接。
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void ConnectAsync(string ip = null, int port = -9999);

        /// <summary>
        /// 关闭与远程服务器的连接
        /// </summary>
        void Close();

        /// <summary>
        /// 当前连接次数
        /// </summary>
        int ConnectCount { get; }

        /// <summary>
        /// 心跳失联次数
        /// </summary>
        int HeartLossCount { get; set; }

    }
}
