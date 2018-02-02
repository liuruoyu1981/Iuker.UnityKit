/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/20 06:11:54
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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BaseProto;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module;
using Iuker.Common.Module.Communication;
using Iuker.Common.Module.Socket;
using Iuker.Common.Module.Timer;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Module.Event;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Communication.Socket
{
    /// <inheritdoc cref="IU3dSocketModule" />
    public sealed class DefaultU3dModule_Socket : AbsU3dModule, IU3dSocketModule
    {
        #region 字段

        public override string ModuleName
        {
            get
            {
                return ModuleType.Socket.ToString();
            }
        }

        /// <summary>
        /// socket通信对象
        /// </summary>
        private System.Net.Sockets.Socket mSocket;

        /// <summary>
        /// socket读写缓存
        /// </summary>
        private readonly byte[] mReadBuff = new byte[1024 * 64];

        /// <summary>
        /// 缓存数据
        /// </summary>
        private List<byte> mCacheData = new List<byte>();

        /// <summary>
        /// 当前是否处于读取状态
        /// </summary>
        private bool mIsReading;

        /// <summary>
        /// 编码解码器
        /// </summary>
        private IEncoder mSocketEncoder;

        /// <summary>
        /// 连接超时事件对象
        /// </summary>
        private readonly AutoResetEvent autoEvent = new AutoResetEvent(false);

        /// <summary>
        /// socket连接线程
        /// </summary>
        private Thread mThread;

        /// <summary>
        /// 发送消息缓存字节数组
        /// </summary>
        private byte[] mSendBytes;

        /// <summary>
        /// 服务器列表
        /// </summary>
        private ServerList mServerList;

        /// <summary>
        /// 服务器端点列表
        /// </summary>
        //private List<IPEndPoint> mRemotePoints;

        /// <summary>
        /// 事件模块
        /// </summary>
        private IU3dAppEventModule mEventModule;

        /// <summary>
        /// 当前连接次数
        /// 即应用启动到现在一共成功建立了多少次连接
        /// </summary>
        public int ConnectCount { get; private set; }

        public int HeartLossCount { get; set; }

        /// <summary>
        /// socket通信状态
        /// </summary>
        public SocketStatus Status { get; private set; }

        #region 心跳

        /// <summary>
        /// 心跳计时器
        /// </summary>
        private ITimer mHearTimer;

        /// <summary>
        /// 重连计时器
        /// </summary>
        private ITimer mReConnectTimer;

        /// <summary>
        /// 心跳发送频率
        /// </summary>
        private int HEART_FREQUENCY = 5;

        /// <summary>
        /// 可以尝试的重连最大次数
        /// </summary>
        private const int RECONNECT_MAX = 5;

        /// <summary>
        /// 已尝试的重连次数
        /// </summary>
        private int mAttemptedCount;

        #endregion

        #endregion

        #region 基类方法

        public override void Init(IFrame frame)
        {
            base.Init(frame);

            mSocketEncoder = U3DFrame.Encoder;
            HEART_FREQUENCY = ProjectBaseConfig.Instance.HeartFrequency;
            U3dSocketInterchanger.Init(mSocketEncoder);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();

            mEventModule = U3DFrame.EventModule;
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.AppQuit, ProcessAppQuit);
        }

        void ProcessAppQuit()
        {
            Close();
        }

        #endregion

        private void ConnectAsyncByThread()
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
            var addresses = Dns.GetHostAddresses(mServerList.NextServer.Url);
            mSocket = addresses[0].AddressFamily == AddressFamily.InterNetworkV6 ? new System.Net.Sockets.Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp) : new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var url = mServerList.NextServer.Url;
            var port = mServerList.NextServer.Port;
            mSocket.BeginConnect(url, port, ConnectCallback, mSocket);
        }

        void ProcessConnectFail()
        {
            if (autoEvent.WaitOne(3000)) return;
            Close();
            mEventModule.IssueEvent(U3dEventCode.Net_ConnectTimeOut.Literals);
            Status = SocketStatus.NotConnect;
            Debug.Log("连接失败");
        }

        public void ConnectAsync(string ip = null, int port = -9999)
        {
            if (mServerList == null)
            {
                if (ip != null && port != -9999)
                {
                    var server = new Server { Url = ip, Port = port };
                    mServerList = new ServerList(new List<Server> { server });
                }
                else
                {
                    var baseConfig = U3DFrame.UnityAppConfig.ProjectBaseConfig;
                    mServerList = new ServerList(baseConfig.Servers);
                }
            }

            StartThreadAndConnect();
        }

        void StartThreadAndConnect()    //  开启新线程并启动异步连接
        {
            if (mThread != null)
            {
                mThread.Abort();
            }
            mThread = null;
            mThread = new Thread(ConnectAsyncByThread);
            mThread.Start();
        }

        public void Close()
        {
            if (Status != SocketStatus.Connected) return;
            mSocket.Close();
            mEventModule.RemoveAppEvent(AppEventType.Update.ToString(), TrySend);
            Status = SocketStatus.NotConnect;
            mSocket = null;
            if (mHearTimer != null)
            {
                mHearTimer.Close();
            }
            mHearTimer = null;
            Debug.Log("心跳计时器已关闭");
        }

        /// <summary>
        /// 发起连接回调
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ConnectCallback(IAsyncResult asyncResult)
        {
            mSocket.EndConnect(asyncResult);
            autoEvent.Set();
            CleanReConnect();
            EnterConnectState();
            EventBroadcast();
        }

        void EnterConnectState()
        {
            ConnectCount++;
            Status = SocketStatus.Connected;
            mEventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), TrySend);
            mSocket.BeginReceive(mReadBuff, 0, mReadBuff.Length, SocketFlags.None, ReceiveCallback, mReadBuff);
            mHearTimer = U3DFrame.TimerModule.CreateRepeatTimer(SendHeart, HEART_FREQUENCY).Start();
        }

        void CleanReConnect()
        {
            mAttemptedCount = 0;
            HeartLossCount = 0;
            if (mReConnectTimer != null)
            {
                mReConnectTimer.Close();
            }
        }

        void EventBroadcast()
        {
            Debuger.Log(ConnectCount == 1 ? "连接建立成功" : "连接重新建立成功");
            mEventModule.IssueEvent(ConnectCount == 1
                ? U3dEventCode.Net_OnFirstConnected.Literals  // 抛出连接建立成功事件
                : U3dEventCode.Net_ReConnected.Literals); // 抛出连接重新建立成功事件

            U3DFrame.EventModule.IssueEvent(U3dEventCode.Net_ConnectSucceed.Literals);
        }

        private void SendHeart(ITimer timer)
        {
            Send();
            OnDisconnect();
        }

        void Send()
        {
            var heartPackge = new IukMessageBase();  //  用-1的协议号来标识这是一个心跳消息
            U3dSocketInterchanger.PushSendMessage(heartPackge, -1);
            HeartLossCount++;
            //Debug.LogError("心跳已发送");
        }

        void OnDisconnect()
        {
            if (HeartLossCount != 2) return;
            mEventModule.IssueEvent(U3dEventCode.Net_TryReConnect.Literals);
            Close();
            mReConnectTimer = U3DFrame.TimerModule.CreateRepeatTimer(TryReConnect, 3f).Start();
        }

        private void TryReConnect(ITimer timer)
        {
            if (mAttemptedCount == RECONNECT_MAX)
            {
                mEventModule.IssueEvent(U3dEventCode.Net_GiveUpReConnect.Literals);
                Status = SocketStatus.NotConnect;
                mReConnectTimer.Close();
                return;
            }

            Status = SocketStatus.NotConnect;
            mAttemptedCount++;
            ConnectAsync();
        }

        private void OnDataReceive()
        {
            while (true)
            {
                var bytes = mSocketEncoder.Decode(ref mCacheData);
                if (bytes == null)
                {
                    mIsReading = false;
                    return;
                }
                U3dSocketInterchanger.PushReceiveMessage(bytes);
            }
        }

        private void TrySend()
        {
            if (U3dSocketInterchanger.SendCount <= 0) return;
            try
            {
                mSendBytes = U3dSocketInterchanger.GetSendMessage();
                mSocket.BeginSend(mSendBytes, 0, mSendBytes.Length, SocketFlags.None, OnSended, null);
            }
            catch (SocketException socketException)
            {
                ProcessError(socketException);
            }
        }

        /// <summary>
        /// 发送成功回调
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnSended(IAsyncResult asyncResult)
        {
            mSocket.EndSend(asyncResult);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            // 如果当前连接已断开或者失去连接则退出数据接收
            if (Status == SocketStatus.NotConnect || Status == SocketStatus.Disconnect) return;

            try
            {
                //获取当前收到的消息长度()
                var length = mSocket.EndReceive(asyncResult);
                if (length == 0)
                {
                    mEventModule.IssueEvent(U3dEventCode.Net_TryReConnect.Literals);
                    Close();
                    mReConnectTimer = U3DFrame.TimerModule.CreateRepeatTimer(TryReConnect, 3f).Start();
                    return;
                }

                //Debug.LogError($"接收到长度为{length}的数据");
                var message = new byte[length];
                Buffer.BlockCopy(mReadBuff, 0, message, 0, length);
                mCacheData.AddRange(message);
                if (!mIsReading)
                {
                    mIsReading = true;
                    OnDataReceive();
                }
                //尾递归 再次开启异步消息接收 消息到达后会直接写入 缓冲区 readbuff
                mSocket.BeginReceive(mReadBuff, 0, mReadBuff.Length, SocketFlags.None, ReceiveCallback, mSocket);
            }
            catch (SocketException socketException)
            {
                ProcessError(socketException);
            }
        }

        private void ProcessError(SocketException socketException)
        {
            Debuger.LogException(socketException.Message);
            Debuger.Log("发生网络错误！");
            mEventModule.IssueEvent(U3dEventCode.Net_ErrorBreak.ToString());    //  抛出网络错误事件
        }


    }
}
