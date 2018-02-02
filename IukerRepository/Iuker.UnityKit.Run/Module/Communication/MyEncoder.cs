using Iuker.Common.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Communication;

namespace Iuker.UnityKit.Run.Module.Communication
{
    public class MyEncoder : IEncoder
    {
        private static readonly int MessageHeadLength = 4 + 1 + 1 + 8 + 2 + 2 + 2;
        private static readonly int BodyHeadLength = 4;

        public byte[] Decode(ref List<byte> data)
        {
            if (data.Count < 4) return null;

            var headArray = ToBig(data);
            var bodyLength = BitConverter.ToInt32(headArray, 0);
            if (bodyLength > data.Count - 1 + MessageHeadLength) return null;

            var receiveBytes = data.ToArray();
            data.Clear();

            var byteBuf = new ByteBuf(receiveBytes, BodyHeadLength);
            var bodyBytes = new byte[bodyLength];
            var protoType = byteBuf.ReadByte();
            var directionId = byteBuf.ReadByte();
            var sessionId = byteBuf.ReadLong();
            var moduleId = byteBuf.ReadShort();
            var businessId = byteBuf.ReadShort();
            var logicId = byteBuf.ReadShort();

            var bodyMessageBytes = byteBuf.ReadResidual();
            return bodyMessageBytes;
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

        public byte[] Encode<T>(T message, int type = 5)
        {
            throw new NotImplementedException();
        }

        public byte[] Encode(string typeName, object message, int type = 5)
        {
            throw new NotImplementedException();
        }

        public byte[] EncodeMessageBase(int type)
        {
            throw new NotImplementedException();
        }

        public IEncoder Init(IFrame frame)
        {
            throw new NotImplementedException();
        }
    }
}
