using System.IO;
using Iuker.Common.Serialize;
using ProtoBuf.Meta;

namespace Iuker.UnityKit.Run.Base
{
    public class IukBaseProtobufSerializer_Proxy : ISerializer
    {
        public IukBaseProtobufSerializer_Proxy()
        {
            mTypeModel = new IukBaseProtobufSerializer();
        }

        private readonly TypeModel mTypeModel;

        public byte[] Serialize(object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                mTypeModel.Serialize(ms, value);
                ms.Position = 0;
                int length = (int)ms.Length;
                var buffer = new byte[length];
                ms.Read(buffer, 0, length);
                return buffer;
            }
        }

        public T DeSerialize<T>(byte[] messageBytes) where T : class, new()
        {
            using (MemoryStream ms = new MemoryStream(messageBytes))
            {
                var message = mTypeModel.Deserialize(ms, null, typeof(T)) as T;
                return message;
            }
        }
    }
}