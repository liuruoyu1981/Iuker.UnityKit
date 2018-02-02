using System.Collections.Generic;
using System.Net;
using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base.Config;

namespace Iuker.Common.Module.Communication
{
#if DEBUG
    /// <summary>
    /// 服务器列表，表示一组可用的远程服务器端点集合。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170928 10:52:47")]
    [ClassPurposeDesc("服务器列表，表示一组可用的远程服务器端点集合。", "服务器列表，表示一组可用的远程服务器端点集合。")]
#endif
    public class ServerList
    {
        private List<IPEndPoint> IpEndPoints { get; set; }
        private List<Server> mServers;
        private int Index = -1;
        private int mServerIndex = -1;

        public IPEndPoint Next
        {
            get
            {
                Index++;
                if (Index == IpEndPoints.Count)
                {
                    Index = 0;
                }
                return IpEndPoints[Index];
            }
        }

        public Server NextServer
        {
            get
            {
                mServerIndex++;
                if (mServerIndex == mServers.Count)
                {
                    mServerIndex = 0;
                }
                return mServers[mServerIndex];
            }
        }

        public ServerList(List<Server> servers = null)
        {
            //IpEndPoints = ipEndPoints.ToList();
            mServers = servers;
        }
    }
}
