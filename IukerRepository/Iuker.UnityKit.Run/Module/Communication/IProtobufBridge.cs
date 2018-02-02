using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.Communication
{
    public interface IProtobufBridge
    {
        object DeSerialize(int protoId, byte[] messageBytes);

        void Init(IU3dFrame frame);

        void InjectMessageCreateFunction();
    }
}