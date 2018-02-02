/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/17/2017 15:27
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
using System.Collections.Generic;
using Iuker.UnityKit.Run.Base;

namespace Iuker.Common.Module.Socket
{
    /// <summary>
    /// Unity消息交换机
    /// 用于如Unity3d这样的单线程应用模型中
    /// 在Unity3d中，为了防止由于socket通信出现延时、卡顿导致UI卡死的情况，会考虑将socket通信单独启用一个线程
    /// 该类则用于unity3d主线程和socket通信线程之间交换socket网络消息使用
    /// </summary>
    public static class U3dSocketInterchanger
    {
        /// <summary>
        /// 编码解码器
        /// </summary>
        private static IEncoder mSocketEncoder;

        /// <summary>
        /// 初始化socket消息交换机
        /// </summary>
        /// <param name="frame"></param>
        public static void Init(IU3dFrame frame)
        {
            mSocketEncoder = frame.Encoder;
        }

        public static void Init(IEncoder encoder)
        {
            mSocketEncoder = encoder;
        }

        /// <summary>
        /// 到达消息队列
        /// </summary>
        public static readonly Queue<byte[]> ReceiveQueue = new Queue<byte[]>();

        /// <summary>
        /// 发送消息队列
        /// </summary>
        public static readonly Queue<byte[]> SendQueue = new Queue<byte[]>();

        /// <summary>
        /// 等待接收解析的消息条数
        /// </summary>
        public static int ReceiveCount { get { return ReceiveQueue.Count; } }

        /// <summary>
        /// 等待发送的消息条数
        /// </summary>
        public static int SendCount
        {
            get
            {
                return SendQueue.Count;
            }
        }

        /// <summary>
        /// 压入一条发送消息
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="message"></param>
        /// <param name="type">消息类型</param>
        public static void PushSendMessage(string typeName, object message, int type = 5)
        {
            var bytes = mSocketEncoder.Encode(typeName, message, type);

            if (bytes.Length > 1024)
            {
                Debuger.Log(string.Format("压入了一个大长度数据包，长度为：{0}", bytes.Length));
                SplitSendData(bytes);
            }
            else
            {
                SendQueue.Enqueue(bytes);
            }
        }

        /// <summary>
        /// 压入一条发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type">消息类型</param>
        public static void PushSendMessage<T>(T message, int type = 5)
        {
            var bytes = mSocketEncoder.Encode(message, type);

            if (bytes.Length > 1024)
            {
                Debuger.Log(string.Format("压入了一个大长度数据包，长度为：{0}", bytes.Length));
                SplitSendData(bytes);
            }
            else
            {
                SendQueue.Enqueue(bytes);
            }
        }

        private static void SplitSendData(byte[] data)
        {
            Debuger.Log("大数据包拆分发送");
            var index = 0;
            while (true)
            {
                if (index == data.Length) return;

                var l = data.Length - index;
                if (l >= 1024)
                {
                    var bytes = new byte[1024];
                    Buffer.BlockCopy(data, index, bytes, 0, 1024);
                    SendQueue.Enqueue(bytes);
                    index += 1024;
                }
                else
                {
                    var bytes = new byte[l];
                    Buffer.BlockCopy(data, index, bytes, 0, l);
                    SendQueue.Enqueue(bytes);
                    index += l;
                }
            }
        }


        /// <summary>
        /// 获得一条到达消息
        /// 如果当前无到达消息则会返回null
        /// </summary>
        /// <returns></returns>
        public static byte[] GetReceiveMessage()
        {
            return ReceiveQueue.Count > 0 ? ReceiveQueue.Dequeue() : null;
        }

        /// <summary>
        /// 获得一条待发送的消息
        /// 如果当前无等待发送的消息则会返回null
        /// </summary>
        /// <returns></returns>
        public static byte[] GetSendMessage()
        {
            return SendQueue.Count > 0 ? SendQueue.Dequeue() : null;
        }

        /// <summary>
        /// 压入一条到达消息
        /// </summary>
        /// <param name="message"></param>
        public static void PushReceiveMessage(byte[] message)
        {
            ReceiveQueue.Enqueue(message);
        }

        #region Js

        private static readonly Dictionary<string, Dictionary<string, object>> JsNetMessages = new Dictionary<string, Dictionary<string, object>>();

        /// <summary>
        /// 将一个通信答复置入Js消息答复字典中以便Js环境操作
        /// </summary>
        /// <param name="sonProject"></param>
        /// <param name="protoType"></param>
        /// <param name="message"></param>
        public static void PushProtoToJs(string sonProject, string protoType, object message)
        {
            if (!JsNetMessages.ContainsKey(sonProject))
            {
                JsNetMessages.Add(sonProject, new Dictionary<string, object>());
                JsNetMessages[sonProject].Add(protoType, message);
            }
            else
            {
                var sonMessageDic = JsNetMessages[sonProject];
                if (sonMessageDic.ContainsKey(sonProject))
                {
                    sonMessageDic[protoType] = message;
                }
                else
                {
                    sonMessageDic.Add(protoType, message);
                }
            }
        }

        public static object GetProtoByJs(string sonProject, string protoType)
        {
            var sonMessageDic = JsNetMessages[sonProject];
            var message = sonMessageDic[protoType];
            return message;
        }


        #endregion
    }
}