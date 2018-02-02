using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Iuker.Common.Base;
using Iuker.Common.Module.Socket;

namespace Iuker.Common.Module.Communication.Socket
{
#if DEBUG
    /// <summary>
    /// 网络通信端点，提供常见的Tcp、Udp等通信协议支持，封装客户端和服务器的常见功能。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170928 10:46:22")]
    [ClassPurposeDesc("网络通信端点，提供常见的Tcp、Udp等通信协议支持，封装客户端和服务器的常见功能。", "网络通信端点，提供常见的Tcp、Udp等通信协议支持，封装客户端和服务器的常见功能。")]
#endif
    public class NetPeer
    {
        #region Tcp

        public NetPeer()
        {
            mSendArgs = new SocketAsyncEventArgs();
            mSendArgs.Completed += IoCompleted;
        }

        /// <summary>
        /// socket连接对象
        /// </summary>
        public System.Net.Sockets.Socket mSocket;

        /// <summary>
        /// 用户异步接收网络数据对象
        /// </summary>
        private SocketAsyncEventArgs mReceiveArgs;

        /// <summary>
        /// 接收缓冲数组
        /// </summary>
        private byte[] mReceiveBuffer;

        /// <summary>
        /// 用户异步发送网络数据对象
        /// </summary>
        private readonly SocketAsyncEventArgs mSendArgs;

        /// <summary>
        /// 发送缓冲数组
        /// </summary>
        private byte[] mSendBuffer;

        /// <summary>
        /// 连接断开委托
        /// </summary>
        private Action mOnDisconnect;

        /// <summary>
        /// 处理失去连接
        /// </summary>
        /// <param name="onDisconnect"></param>
        /// <returns></returns>
        public NetPeer OnDisconnect(Action onDisconnect)
        {
            mOnDisconnect = onDisconnect;
            return this;
        }

        /// <summary>
        /// 接收完成回调
        /// </summary>
        //private Action<byte[]> mOnReceived;


        /// <summary>
        /// 发送完成回调
        /// </summary>
        private Action mSended;

        /// <summary>
        /// 发送消息队列
        /// </summary>
        private Queue<byte[]> mSendQueue;

        public NetPeer SendQueuq(Queue<byte[]> sendQueue)
        {
            mSendQueue = sendQueue;
            return this;
        }

        /// <summary>
        /// 到达消息队列
        /// </summary>
        private Queue<byte[]> mReceiveQueue;

        public NetPeer ReceiveQueue(Queue<byte[]> receiveQueue)
        {
            mReceiveQueue = receiveQueue;
            return this;
        }

        /// <summary>
        /// 消息缓存
        /// 用于粘包处理
        /// </summary>
        private List<byte> mMessageCache = new List<byte>();

        /// <summary>
        /// 是否正在读取
        /// </summary>
        private bool isReading = false;

        /// <summary>
        /// 是否正在发送
        /// </summary>
        private bool isWriting = false;

        /// <summary>
        /// 通信端点状态
        /// </summary>
        public SocketStatus Status { get; private set; }

        /// <summary>
        /// 编码解码器
        /// </summary>
        private IEncoder mEncoder;

        public NetPeer Encoder(IEncoder encoder)
        {
            mEncoder = encoder;
            return this;
        }

        /// <summary>
        /// 发送失败委托
        /// </summary>
        private Action mOnSendFail;


        /// <summary>
        /// 接收失败委托
        /// </summary>
        private Action mOnReceiveFail;

        /// <summary>
        /// 处理接收失败
        /// </summary>
        /// <param name="onReceiveFail"></param>
        /// <returns></returns>
        public NetPeer OnReceiveFail(Action onReceiveFail)
        {
            mOnReceiveFail = onReceiveFail;
            return this;
        }


        public NetPeer ReceiveArgs(int size)
        {
            if (mReceiveArgs == null)
            {
                mReceiveArgs = new SocketAsyncEventArgs();
                mReceiveBuffer = new byte[size];
                mReceiveArgs.SetBuffer(mReceiveBuffer, 0, size);
                mReceiveArgs.Completed += IoCompleted;
            }

            return this;
        }


        /// <summary>
        /// 发送完毕
        /// </summary>
        private void OnSended()
        {
            if (mSendArgs.SocketError != SocketError.Success)
            {
                Debuger.Log("发送失败");
                if (mOnSendFail != null)
                {
                    mOnSendFail();
                }
            }
            else
            {
                if (mSended != null)
                {
                    mSended();
                }
                TrySend();
            }
        }

        private void IoCompleted(object sender, SocketAsyncEventArgs asyncEventArgs)
        {
            if (asyncEventArgs.LastOperation == SocketAsyncOperation.Receive)
            {
                OnReceived();
            }
            else
            {
                OnSended();
            }
        }

        /// <summary>
        /// 网络消息到达
        /// </summary>
        /// <param name="buff"></param>
        private void OnReceive(byte[] buff)
        {
            //将消息写入缓存
            mMessageCache.AddRange(buff);
            if (isReading) return;
            isReading = true;
            MessageDecode();
        }

        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="value"></param>
        public void MessageEnqueue(byte[] value)
        {
            mSendQueue.Enqueue(value);
            if (isWriting) return;
            isWriting = true;
            TrySend();
        }

        /// <summary>
        /// 消息解码
        /// </summary>
        private void MessageDecode()
        {
            while (true)
            {
                if (mMessageCache.Count == 0)
                {
                    isReading = false;
                    return;
                }

                var bytes = mEncoder.Decode(ref mMessageCache);
                if (bytes == null)
                {
                    isReading = false;
                    return;
                }

                mReceiveQueue.Enqueue(bytes);
            }
        }

        /// <summary>
        /// 接收完毕
        /// </summary>
        private void OnReceived()
        {
            //判断网络消息接收是否成功
            if (mReceiveArgs.BytesTransferred > 0 && mReceiveArgs.SocketError == SocketError.Success)
            {
                byte[] message = new byte[mReceiveArgs.BytesTransferred];
                //将网络消息拷贝到自定义数组
                Buffer.BlockCopy(mReceiveArgs.Buffer, 0, message, 0, mReceiveArgs.BytesTransferred);
                //处理接收到的消息
                //mOnReceived?.Invoke(message);
                OnReceive(message);
                StartReceive();
            }
            else
            {
                if (mReceiveArgs.SocketError != SocketError.Success)
                {
                    Debuger.Log("接收数据失败");
                    if (mOnReceiveFail != null)
                    {
                        mOnReceiveFail();
                    }
                }
                else
                {
                    if (mOnDisconnect != null)
                    {
                        mOnDisconnect();
                    }
                    Status = SocketStatus.Disconnect;
                    Debuger.Log("连接已断开");
                }
            }
        }

        /// <summary>
        /// 开始发送数据
        /// </summary>
        public void StartReceive()
        {
            var result = mSocket.ReceiveAsync(mReceiveArgs);
            if (!result)
            {
                OnReceived();
            }
        }

        /// <summary>
        /// 尝试发送数据
        /// </summary>
        public void TrySend()
        {
            if (mSocket == null) return;
            if (mSendQueue.Count == 0) { isWriting = false; return; }   //  判断发送消息队列是否有消息
            var buffer = mSendQueue.Dequeue();                         //  取出第一条待发消息
            if (buffer.Length > 100)
            {
                Debuger.Log(buffer.Length.ToString());
            }

            mSendArgs.SetBuffer(buffer, 0, buffer.Length);
            bool result = mSocket.SendAsync(mSendArgs);                 //  开启异步发送
            if (!result)                                                //  是否挂起
            {
                OnSended();
            }
        }

        public void Close()
        {
            try
            {
                isReading = false;
                isWriting = false;
                mSocket.Close();
                mSocket = null;
                Status = SocketStatus.NotConnect;
            }
            catch (Exception e)
            {
                Debuger.Log(e.Message);
            }
        }

        #region 服务器

        public void Bind(IPEndPoint local = null)
        {



        }



        #endregion


        #region 客户端

        /// <summary>
        /// 连接超时事件对象
        /// </summary>

        private static readonly AutoResetEvent mConnectTimeOutEvent = new AutoResetEvent(false);

        /// <summary>
        /// 当前可用的服务器列表
        /// 客户端模式下使用
        /// </summary>
        private ServerList mServers;

        /// <summary>
        /// 当前的远程服务器
        /// </summary>
        private IPEndPoint NextServer { get { return mServers.Next; } }

        /// <summary>
        /// 连接超时处理时间
        /// </summary>
        private int mConnectTimeOut;

        /// <summary>
        /// 连接超时处理委托
        /// </summary>
        private Action mOnConnectTimeOut;

        /// <summary>
        /// 处理连接超时
        /// 超时时间单位为毫秒
        /// </summary>
        /// <param name="timeOut"></param>
        /// <param name="onConnectTimeOut"></param>
        /// <returns></returns>
        public NetPeer ConnectTimeOut(int timeOut, Action onConnectTimeOut)
        {
            mConnectTimeOut = timeOut;
            mOnConnectTimeOut = onConnectTimeOut;
            return this;
        }

        /// <summary>
        /// 连接完成回调
        /// </summary>
        private Action mOnConnected;

        public NetPeer OnConnected(Action onConnected)
        {
            mOnConnected = onConnected;
            return this;
        }

        public void ConnectAsync()
        {
            OnConnectingOrConnected();
            StartConnect();
            ProcessConnectFail();
        }

        void OnConnectingOrConnected()
        {
            switch (Status)
            {
                case SocketStatus.Connecting:
                    Debuger.Log("socket正在连接......！");
                    return;
                case SocketStatus.Connected:
                    Debuger.Log("socket当前已连接！");
                    return;
            }
        }

        void StartConnect()
        {
            Status = SocketStatus.Connecting;
            mSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mSocket.BeginConnect(mServers.Next, ConnectCallback, mSocket);
        }

        void ProcessConnectFail()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Debuger.LogError("连接线程id为：" + id);

            if (mConnectTimeOutEvent.WaitOne(mConnectTimeOut)) return;
            Close();
            if (mOnConnectTimeOut != null)
            {
                mOnConnectTimeOut();
            }
            Status = SocketStatus.NotConnect;
        }

        /// <summary>
        /// 连接完成回调
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            mSocket.EndConnect(ar);
            mConnectTimeOutEvent.Set();
            if (mOnConnected != null)
            {
                mOnConnected();
            }
            Status = SocketStatus.Connected;
        }


        #endregion


        #endregion







    }
}
