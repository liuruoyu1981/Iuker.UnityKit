///***********************************************************************************************
//Author：liuruoyu1981
//CreateDate: 2017/04/23 19:31:12
//Email: 35490136@qq.com
//QQCode: 35490136
//CreateNote: 
//***********************************************************************************************/


///****************************************修改日志***********************************************
//1. 修改日期： 修改人： 修改内容：
//2. 修改日期： 修改人： 修改内容：
//3. 修改日期： 修改人： 修改内容：
//4. 修改日期： 修改人： 修改内容：
//5. 修改日期： 修改人： 修改内容：
//****************************************修改日志***********************************************/

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using Iuker.Common.Module.Communication;

//namespace Iuker.Common.Module.Socket
//{
//    /// <summary>
//    /// Tcp通信端点
//    /// </summary>
//    public sealed class TcpPeer
//    {
//        #region 字段属性

//        /// <summary>
//        /// socket连接对象
//        /// </summary>
//        public System.Net.Sockets.Socket mSocket;

//        /// <summary>
//        /// 用户异步接收网络数据对象
//        /// </summary>
//        private SocketAsyncEventArgs mReceiveArgs;

//        /// <summary>
//        /// 用户异步发送网络数据对象
//        /// </summary>
//        private readonly SocketAsyncEventArgs mSendArgs;

//        /// <summary>
//        /// 当前的远程服务器
//        /// </summary>
//        private IPEndPoint NextServer => mServers.Next;

//        /// <summary>
//        /// 通信端点状态
//        /// </summary>
//        public SocketStatus Status { get; private set; }

//        /// <summary>
//        /// 编码解码器
//        /// </summary>
//        private ISocketEncoder mEncoder;

//        /// <summary>
//        /// 消息缓存
//        /// 用于粘包处理
//        /// </summary>
//        private List<byte> mMessageCache = new List<byte>();

//        /// <summary>
//        /// 是否正在读取
//        /// </summary>
//        private bool isReading = false;

//        /// <summary>
//        /// 是否正在发送
//        /// </summary>
//        private bool isWriting = false;

//        /// <summary>
//        /// 发送消息队列
//        /// </summary>
//        private Queue<byte[]> mSendQueue;

//        /// <summary>
//        /// 到达消息队列
//        /// </summary>
//        private Queue<byte[]> mReceiveQueue;

//        /// <summary>
//        /// 连接完成回调
//        /// </summary>
//        private Action mOnConnected;

//        /// <summary>
//        /// 接收完成回调
//        /// </summary>
//        private Action<byte[]> mOnReceived;


//        /// <summary>
//        /// 发送完成回调
//        /// </summary>
//        private Action mSended;

//        /// <summary>
//        /// 连接超时处理时间
//        /// </summary>
//        private int mConnectTimeOut;

//        /// <summary>
//        /// 连接超时处理委托
//        /// </summary>
//        private Action mOnConnectTimeOut;

//        /// <summary>
//        /// 连接断开委托
//        /// </summary>
//        private Action mOnDisconnect;

//        /// <summary>
//        /// 处理失去连接
//        /// </summary>
//        /// <param name="onDisconnect"></param>
//        /// <returns></returns>
//        public TcpPeer OnDisconnect(Action onDisconnect)
//        {
//            mOnDisconnect = onDisconnect;
//            return this;
//        }

//        /// <summary>
//        /// 发送失败委托
//        /// </summary>
//        private Action mOnSendFail;

//        /// <summary>
//        /// 处理发送失败
//        /// </summary>
//        /// <param name="onSendFail"></param>
//        /// <returns></returns>
//        public TcpPeer OnSendFail(Action onSendFail)
//        {
//            mOnSendFail = onSendFail;
//            return this;
//        }

//        /// <summary>
//        /// 接收失败委托
//        /// </summary>
//        private Action mOnReceiveFail;

//        /// <summary>
//        /// 处理接收失败
//        /// </summary>
//        /// <param name="onReceiveFail"></param>
//        /// <returns></returns>
//        public TcpPeer OnReceiveFail(Action onReceiveFail)
//        {
//            mOnReceiveFail = onReceiveFail;
//            return this;
//        }

//        /// <summary>
//        /// 通信超时事件对象
//        /// </summary>

//        private static readonly AutoResetEvent autoEvent = new AutoResetEvent(false);

//        #region 重连

//        /// <summary>
//        /// 重连之前需要执行的委托
//        /// </summary>
//        private Action mBeforeReConnect;

//        /// <summary>
//        /// 重连成功之后需要执行的委托
//        /// </summary>
//        private Action mAfterReConnect;

//        /// <summary>
//        /// 重连次数已消耗完放弃重连时需要执行的委托
//        /// </summary>
//        private Action mGiveUpReConnect;

//        /// <summary>
//        /// 可以尝试的重连最大次数
//        /// </summary>
//        private int mReConnectMax;

//        /// <summary>
//        /// 已尝试的重连次数
//        /// </summary>
//        private int mAttemptedCount;

//        /// <summary>
//        /// 是否需要进行重连
//        /// </summary>
//        private bool misReConnect;

//        /// <summary>
//        /// 设置重连
//        /// </summary>
//        /// <param name="giveUp">进行最大次数尝试之后放弃重连时的回调委托</param>
//        /// <param name="tryMax">允许的最大重连尝试次数</param>
//        /// <param name="before">重连之前委托</param>
//        /// <param name="after">重连之后委托</param>
//        /// <returns></returns>
//        public TcpPeer ReConnect(Action giveUp, int tryMax, Action before = null, Action after = null)
//        {
//            mGiveUpReConnect = giveUp;
//            mReConnectMax = tryMax;
//            mBeforeReConnect = before;
//            mAfterReConnect = after;
//            misReConnect = true;

//            return this;
//        }

//        /// <summary>
//        /// 尝试进行重连操作
//        /// 前提条件，允许重连被开启
//        /// </summary>
//        public void TryReConnect()
//        {
//            var id = Thread.CurrentThread.ManagedThreadId;
//            Debuger.LogError("Tcp线程Id：" + id);

//            if (!misReConnect) return;
//            if (Status == SocketStatus.Connecting)
//            {
//                Debuger.Log("正在连接，重连取消");
//                return;
//            }

//            mAttemptedCount++;
//            mBeforeReConnect?.Invoke();
//            Debuger.Log($"尝试重连，当前已经尝试了{mAttemptedCount}次。");
//            if (mAttemptedCount == mReConnectMax)
//            {
//                Debuger.Log("已到达最大重连次数，将放弃重连");
//                mGiveUpReConnect?.Invoke();
//                return;
//            }

//            Close();
//            mSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            Status = SocketStatus.Connecting;

//            mSocket.BeginConnect(NextServer, ConnectCallback, mSocket);
//        }

//        #endregion

//        #endregion

//        public TcpPeer()
//        {
//            var id = Thread.CurrentThread.ManagedThreadId;
//            Debuger.LogError("创建TcpPeer，线程Id为：" + id);

//            mSendArgs = new SocketAsyncEventArgs();
//            mSendArgs.Completed += IoCompleted;
//        }


//        #region 链式方法

//        public TcpPeer ReceiveArgs(int size)
//        {
//            if (mReceiveArgs == null)
//            {
//                mReceiveArgs = new SocketAsyncEventArgs();
//                mReceiveArgs.SetBuffer(new byte[size], 0, size);
//                mReceiveArgs.Completed += IoCompleted;
//            }

//            return this;
//        }

//        public TcpPeer Encoder(ISocketEncoder encoder)
//        {
//            mEncoder = encoder;
//            return this;
//        }

//        /// <summary>
//        /// 当前可用的服务器列表
//        /// 客户端模式下使用
//        /// </summary>
//        private ServerList mServers;

//        /// <summary>
//        /// 设置可用的远程服务器列表
//        /// </summary>
//        /// <param name="ipEndPoints"></param>
//        /// <returns></returns>
//        public TcpPeer RemoteEnpoints(List<IPEndPoint> ipEndPoints)
//        {
//            mServers = new ServerList(ipEndPoints);
//            return this;
//        }

//        public TcpPeer SendQueuq(Queue<byte[]> sendQueue)
//        {
//            mSendQueue = sendQueue;
//            return this;
//        }

//        public TcpPeer ReceiveQueue(Queue<byte[]> receiveQueue)
//        {
//            mReceiveQueue = receiveQueue;
//            return this;
//        }

//        public TcpPeer OnConnected(Action onConnected)
//        {
//            mOnConnected = onConnected;
//            return this;
//        }

//        /// <summary>
//        /// 处理发送完毕
//        /// </summary>
//        /// <param name="sended"></param>
//        /// <returns></returns>
//        public TcpPeer OnSended(Action sended)
//        {
//            mSended = sended;
//            return this;
//        }

//        /// <summary>
//        /// 处理接收完成
//        /// </summary>
//        /// <param name="received"></param>
//        /// <returns></returns>
//        public TcpPeer OnReceived(Action<byte[]> received)
//        {
//            mOnReceived = received;
//            return this;
//        }

//        /// <summary>
//        /// 处理连接超时
//        /// 超时时间单位为毫秒
//        /// </summary>
//        /// <param name="timeOut"></param>
//        /// <param name="onConnectTimeOut"></param>
//        /// <returns></returns>
//        public TcpPeer ConnectTimeOut(int timeOut, Action onConnectTimeOut)
//        {
//            mConnectTimeOut = timeOut;
//            mOnConnectTimeOut = onConnectTimeOut;
//            return this;
//        }

//        #endregion





//        public void ConnectAsync()
//        {
//            OnConnectingOrConnected();
//            StartConnect();
//            ProcessConnectFail();

//            void OnConnectingOrConnected()
//            {
//                switch (Status)
//                {
//                    case SocketStatus.Connecting:
//                        Debuger.Log("socket正在连接......！");
//                        return;
//                    case SocketStatus.Connected:
//                        Debuger.Log("socket当前已连接！");
//                        return;
//                }
//            }

//            void StartConnect()
//            {
//                Status = SocketStatus.Connecting;
//                mSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                mSocket.BeginConnect(mServers.Next, ConnectCallback, mSocket);
//            }

//            void ProcessConnectFail()
//            {
//                if (autoEvent.WaitOne(3000)) return;
//                Close();
//                mOnConnectTimeOut?.Invoke();
//                Status = SocketStatus.NotConnect;
//            }
//        }

//        /// <summary>
//        /// 连接完成回调
//        /// </summary>
//        /// <param name="ar"></param>
//        private void ConnectCallback(IAsyncResult ar)
//        {
//            mSocket.EndConnect(ar);
//            autoEvent.Set();
//            mOnConnected?.Invoke();
//            Status = SocketStatus.Connected;
//            mAttemptedCount = 0;
//        }

//        /// <summary>
//        /// 网络消息到达
//        /// </summary>
//        /// <param name="buff"></param>
//        private void OnReceive(byte[] buff)
//        {
//            //将消息写入缓存
//            mMessageCache.AddRange(buff);
//            if (isReading) return;
//            isReading = true;
//            MessageDecode();
//        }

//        /// <summary>
//        /// 消息解码
//        /// </summary>
//        private void MessageDecode()
//        {
//            while (true)
//            {
//                if (mMessageCache.Count == 0)
//                {
//                    isReading = false;
//                    return;
//                }

//                var bytes = mEncoder.Decode(ref mMessageCache);
//                if (bytes == null)
//                {
//                    isReading = false;
//                    return;
//                }

//                mReceiveQueue.Enqueue(bytes);
//            }
//        }

//        /// <summary>
//        /// 消息入队
//        /// </summary>
//        /// <param name="value"></param>
//        public void MessageEnqueue(byte[] value)
//        {
//            mSendQueue.Enqueue(value);
//            if (isWriting) return;
//            isWriting = true;
//            TrySend();
//        }

//        /// <summary>
//        /// 尝试发送数据
//        /// </summary>
//        public void TrySend()
//        {
//            var id = Thread.CurrentThread.ManagedThreadId;
//            Debuger.LogError("TcpPeer尝试发送数据，线程Id为：" + id);

//            if (mSocket == null) return;                                //  
//            if (mSendQueue.Count == 0) { isWriting = false; return; }   //  判断发送消息队列是否有消息
//            byte[] buff = mSendQueue.Dequeue();                         //  取出第一条待发消息
//            mSendArgs.SetBuffer(buff, 0, buff.Length);                  //  设置消息发送异步对象的发送数据缓冲区数据
//            bool result = mSocket.SendAsync(mSendArgs);                 //  开启异步发送
//            if (!result)                                                //  是否挂起
//            {
//                OnSended();
//            }
//        }

//        public void Close()
//        {
//            try
//            {
//                isReading = false;
//                isWriting = false;
//                if (Status == SocketStatus.Connected)
//                {
//                    mSocket.Shutdown(SocketShutdown.Both);
//                    mSocket.Disconnect(true);
//                }
//                mSocket.Close();
//                mSocket = null;
//                Status = SocketStatus.NotConnect;
//            }
//            catch (Exception e)
//            {
//                Debuger.Log(e.Message);
//            }
//        }

//        /// <summary>
//        /// 开始发送数据
//        /// </summary>
//        public void StartReceive()
//        {
//            var result = mSocket.ReceiveAsync(mReceiveArgs);
//            if (!result)
//            {
//                OnReceived();
//            }
//        }

//        /// <summary>
//        /// 接收完毕
//        /// </summary>
//        private void OnReceived()
//        {
//            //判断网络消息接收是否成功
//            if (mReceiveArgs.BytesTransferred > 0 && mReceiveArgs.SocketError == SocketError.Success)
//            {
//                byte[] message = new byte[mReceiveArgs.BytesTransferred];
//                //将网络消息拷贝到自定义数组
//                Buffer.BlockCopy(mReceiveArgs.Buffer, 0, message, 0, mReceiveArgs.BytesTransferred);
//                //处理接收到的消息
//                mOnReceived?.Invoke(message);
//                OnReceive(message);
//                StartReceive();
//            }
//            else
//            {
//                if (mReceiveArgs.SocketError != SocketError.Success)
//                {
//                    Debuger.Log("接收数据失败");
//                    mOnReceiveFail?.Invoke();
//                }
//                else
//                {
//                    mOnDisconnect?.Invoke();
//                    Status = SocketStatus.Disconnect;
//                    Debuger.Log("连接已断开");
//                }
//            }
//        }

//        /// <summary>
//        /// 发送完毕
//        /// </summary>
//        private void OnSended()
//        {
//            if (mSendArgs.SocketError != SocketError.Success)
//            {
//                Debuger.Log("发送失败");
//                mOnSendFail?.Invoke();
//            }
//            else
//            {
//                mSended?.Invoke();
//                TrySend();
//            }
//        }

//        private void IoCompleted(object sender, SocketAsyncEventArgs asyncEventArgs)
//        {
//            if (asyncEventArgs.LastOperation == SocketAsyncOperation.Receive)
//            {
//                OnReceived();
//            }
//            else
//            {
//                OnSended();
//            }
//        }
//    }


//}
