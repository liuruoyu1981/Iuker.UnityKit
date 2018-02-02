/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/18 15:04:05
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

namespace Iuker.UnityKit.Run.Base.Config
{
    /// <summary>
    /// 配置节点-服务器地址
    /// </summary>
    [Serializable]
    public class Server
    {
        /// <summary>
        /// 服务器名
        /// </summary>
        public string Name;

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port;

        /// <summary>
        /// 服务器地址
        /// 例如：192.18.1.102
        /// </summary>
        public string Url;

        /// <summary>
        /// 获得服务器配置所表示的IP端点
        /// </summary>
        //public IPEndPoint IpEndPoint => new IPEndPoint(IPAddress.Parse(Url), Port);

    }
}
