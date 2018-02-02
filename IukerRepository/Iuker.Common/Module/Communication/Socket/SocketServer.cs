using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Communication.Socket;

namespace Iuker.Common.Module.Socket
{
    public class SocketServer
    {
        private readonly System.Net.Sockets.Socket server;//服务器socket监听对象

        /// <summary>
        /// 当前允许的最大连接数量
        /// </summary>
        private readonly int mMaxConnectNum;//最大客户端连接数

        /// <summary>
        /// 连接信号灯对象
        /// </summary>
        private Semaphore mConnectSemaphore;

        /// <summary>
        /// 通信端点对象池
        /// </summary>
        private ObjectPool<NetPeer> mPeerPool;

        /// <summary>
        /// 消息处理中心，由外部应用传入
        /// </summary>
        private readonly ISocketHandler mCenter;

        /// <summary>
        /// 编码解码器
        /// </summary>
        private IEncoder mSocketEncoder;

        /// <summary>
        /// 初始化通信监听
        /// </summary>
        /// <param name="mMax"></param>
        /// <param name="center"></param>
        public SocketServer(int mMax, ISocketHandler center)
        {
            //实例化监听对象
            server = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //设定服务器最大连接人数
            mMaxConnectNum = mMax;
            mCenter = center;
        }

        private class TcpPeerFactory : IObjectFactory<NetPeer>
        {
            public NetPeer CreateObject()
            {
                return new NetPeer();
            }
        }

        public void Start(int port, IEncoder encoder)
        {
            mSocketEncoder = encoder;
            //创建连接池
            mPeerPool = new ObjectPool<NetPeer>(new TcpPeerFactory(), mMaxConnectNum);
            //连接信号量
            mConnectSemaphore = new Semaphore(mMaxConnectNum, mMaxConnectNum);
            //for (int i = 0; i < maxClient; i++)
            //{
            //    TcpPeer token = new TcpPeer(mCenter, encoder);
            //    //初始化token信息               
            //    //token.ReceiveSAEA.Completed += IO_Comleted;
            //    //token.SendSAEA.Completed += IO_Comleted;
            //    //token.SendHandler = ProcessSend;
            //    //token.closeProcess = ClientClose;
            //    //token.TcpCenter = mCenter;
            //    mPeerPool.(token);
            //}
            //监听当前服务器网卡所有可用IP地址的port端口
            // 外网IP  内网IP192.168.x.x 本机IP一个127.0.0.1
            try
            {
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                //置于监听状态
                server.Listen(10);
                StartAccept(null);
            }
            catch (Exception exception)
            {
                Debuger.Log(exception.Message);
            }
        }

        //private void TcpPeerInit(TcpPeer peer) => peer.Init(mCenter, mSocketEncoder);


        /// <summary>
        /// 开始客户端连接监听
        /// </summary>
        private void StartAccept(SocketAsyncEventArgs e)
        {
            //如果当前传入为空  说明调用新的客户端连接监听事件 否则的话 移除当前客户端连接
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += Accept_Comleted;
            }
            else
            {
                e.AcceptSocket = null;
            }
            //信号量-1
            mConnectSemaphore.WaitOne();
            bool result = server.AcceptAsync(e);
            //判断异步事件是否挂起  没挂起说明立刻执行完成  直接处理事件 否则会在处理完成后触发Accept_Comleted事件
            if (!result)
            {
                ProcessAccept(e);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            //从连接对象池取出连接对象 供新用户使用
            NetPeer peer = mPeerPool.Take();
            mActivePeers.Add(peer);
            peer.mSocket = e.AcceptSocket;
            //TODO 通知应用层 有客户端连接
            mCenter.ClientConnect(peer);
            //开启消息到达监听
            //StartReceive(peer);
            peer.StartReceive();
            //释放当前异步对象
            StartAccept(e);
        }

        private readonly List<NetPeer> mActivePeers = new List<NetPeer>();

        private void Accept_Comleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        //private void StartReceive(TcpPeer token)
        //{
        //    try
        //    {
        //        //用户连接对象 开启异步数据接收
        //        bool result = token.mSocket.ReceiveAsync(token.ReceiveSAEA);
        //        //异步事件是否挂起
        //        if (!result)
        //        {
        //            ProcessReceive(token.ReceiveSAEA);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}

        //private void IO_Comleted(object sender, SocketAsyncEventArgs e)
        //{
        //    if (e.LastOperation == SocketAsyncOperation.Receive)
        //    {
        //        ProcessReceive(e);
        //    }
        //    else
        //    {
        //        ProcessSend(e);
        //    }
        //}

        //private void ProcessReceive(SocketAsyncEventArgs e)
        //{
        //    TcpPeer token = e.UserToken as TcpPeer;
        //    //判断网络消息接收是否成功
        //    if (token.ReceiveSAEA.BytesTransferred > 0 && token.ReceiveSAEA.SocketError == SocketError.Success)
        //    {
        //        byte[] message = new byte[token.ReceiveSAEA.BytesTransferred];
        //        //将网络消息拷贝到自定义数组
        //        Buffer.BlockCopy(token.ReceiveSAEA.Buffer, 0, message, 0, token.ReceiveSAEA.BytesTransferred);
        //        //处理接收到的消息
        //        token.OnReceive(message);
        //        StartReceive(token);
        //    }
        //    else
        //    {
        //        ClientClose(token,
        //            token.ReceiveSAEA.SocketError != SocketError.Success
        //                ? token.ReceiveSAEA.SocketError.ToString()
        //                : "客户端主动断开连接");
        //    }
        //}

        //private void ProcessSend(SocketAsyncEventArgs e)
        //{
        //    TcpPeer token = e.UserToken as TcpPeer;
        //    if (e.SocketError != SocketError.Success)
        //    {
        //        ClientClose(token, e.SocketError.ToString());
        //    }
        //    else
        //    {
        //        //消息发送成功，回调成功
        //        token.writed();
        //    }
        //}

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="token"> 断开连接的用户对象</param>
        /// <param name="error">断开连接的错误编码</param>
        private void ClientClose(NetPeer token, string error)
        {
            if (token.mSocket != null)
            {
                lock (token)
                {
                    //通知应用层面 客户端断开连接了
                    mCenter.ClientClose(token, error);
                    token.Close();
                    //加回一个信号量，供其它用户使用
                    mPeerPool.Restore(token);
                    mConnectSemaphore.Release();
                }
            }
        }

        public void Close()
        {
            mActivePeers.ForEach(peer => peer.Close());
        }






    }
}
