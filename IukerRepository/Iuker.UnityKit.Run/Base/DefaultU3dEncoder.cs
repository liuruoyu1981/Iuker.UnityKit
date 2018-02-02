using System;
using System.Collections.Generic;
using System.Linq;
using BaseProto;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module;
using Iuker.Common.Module.Socket;
using Iuker.Common.Serialize;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// 默认编码解码器
    /// </summary>
    public class DefaultU3dEncoder : IEncoder
    {
        /// <summary>
        /// 项目序列化器
        /// </summary>
        private ISerializer mSerializer;
        private IU3dFrame mU3DFrame;

        private IProtoIdResolver mProtoIdResolver;

        /// <summary>
        /// 消息编码
        /// </summary>
        /// <param name="bodyMessage">要发送的主体消息</param>
        /// <param name="type">服务器类型</param>
        /// <returns></returns>
        public byte[] Encode<T>(T bodyMessage, int type = 5)
        {
            var protoName = typeof(T).Name;
            return Encode(protoName, bodyMessage, type);
        }

        public byte[] Encode(string typeName, object message, int type = 5)
        {
            var protoCmdId = mProtoIdResolver.GetProtoId(typeName);
            var bodyBytes = mSerializer.Serialize(message); //  先拿到实际发送的主体消息二进制数组
            var messageBase = new IukMessageBase { dataContent = bodyBytes, type = type, cmdId = protoCmdId };
            var sendBytes = mSerializer.Serialize(messageBase);
            var binaryer = new Binaryer();
            var headArray = BitConverter.GetBytes(sendBytes.Length).Reverse().ToArray();
            binaryer.Write(headArray);
            binaryer.Write(sendBytes);
            var finalBytes = binaryer.GetBuff();
            binaryer.Close();
            return finalBytes;
        }

        public byte[] EncodeMessageBase(int type)
        {
            var messageBase = new IukMessageBase { type = type };
            var sendBytes = mSerializer.Serialize(messageBase);
            var binaryer = new Binaryer();
            var headArray = BitConverter.GetBytes(sendBytes.Length).Reverse().ToArray();

            binaryer.Write(headArray);
            binaryer.Write(sendBytes);
            var finalBytes = binaryer.GetBuff();

            return finalBytes;
        }

        /// <summary>
        /// 消息解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decode(ref List<byte> data)
        {
            if (data.Count < 4) return null;

            var headArray = ToBig(data);
            var length = BitConverter.ToInt32(headArray, 0);
            // 如果消息体长度 大于缓存中数据长度 说明消息没有读取完 等待下次消息到达后再次处理
            if (length > data.Count - 1) return null;

            var receiveBytes = data.ToArray();
            data.Clear();
            byte[] messageBytes = new byte[length];
            Buffer.BlockCopy(receiveBytes, 4, messageBytes, 0, length);
            var residueCount = receiveBytes.Length - 4 - length;
            if (residueCount > 0)
            {
                var residueBytes = new byte[residueCount];
                Buffer.BlockCopy(receiveBytes, 4 + length, residueBytes, 0, residueCount);
                data.AddRange(residueBytes);
            }

            return messageBytes;
        }

        /// <summary>
        /// 解析服务器答复消息的消息长度
        /// 大小端转换
        /// </summary>
        private byte[] ToBig(List<byte> bytes)
        {
            var mHeadLengthBytes = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                mHeadLengthBytes[i] = bytes[i];
            }

            mHeadLengthBytes = mHeadLengthBytes.Reverse().ToArray();
            return mHeadLengthBytes;
        }

        public IEncoder Init(IFrame frame)
        {
            mU3DFrame = (IU3dFrame)frame;
            mProtoIdResolver = mU3DFrame.ProtoIdResolver;
            mSerializer = mU3DFrame.Serializer;

            return this;
        }
    }
}