using BaseProto;
using Iuker.Common.Module.Socket;
using Iuker.UnityKit.Run.Module.Communication.Socket;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Communication
{
    public class MyU3dCommunicationDispatcher : AbsU3dCommunicationDispatcher
    {
        protected override void MessageDispatch()
        {
            while (U3dSocketInterchanger.ReceiveCount > 0 && mIsDispatch)
            {
                bytes = U3dSocketInterchanger.GetReceiveMessage();
                var messageBase = mSerializer.DeSerialize<IukMessageBase>(bytes);

                //  心跳处理
                if (messageBase.type == -1)
                {
                    Debug.Log("收到心跳，重置心跳计数为0");
                    mSocketModule.HeartLossCount = 0;
                    return;
                }

                var commandId = messageBase.cmdId;
                var responserName = mProtoIdResolver.GetProtoName(commandId);
#if UNITY_EDITOR || DEBUG
                Debuger.Log(string.Format("接收到网络消息,协议名为{0},协议编号为{1}！", responserName, commandId));
#endif  
                DoResponser(responserName, messageBase.dataContent);
            }

        }
    }
}
