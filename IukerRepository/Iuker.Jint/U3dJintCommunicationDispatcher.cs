using System.Collections.Generic;
using BaseProto;
using Iuker.Common;
using Iuker.Common.Module.Socket;
using Iuker.UnityKit.Run.Module.Communication.Socket;
using Iuker.UnityKit.Run.Module.JavaScript;
using Jint;
using Jint.Native;
using UnityEngine;

namespace Iuker.Jint
{
    public class U3dJintCommunicationDispatcher : AbsU3dCommunicationDispatcher
    {
        private Engine mJsEngine;
        private IU3dJavaScriptModule mJsModule;

        protected override void OnFrameInited()
        {
            base.OnFrameInited();

            mJsModule = U3DFrame.GetModule<IU3dJavaScriptModule>();
            mJsEngine = mJsModule.Engine.As<Engine>();
        }

        protected override void MessageDispatch()
        {
            while (U3dSocketInterchanger.ReceiveCount > 0)
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
                var jintId = responserName.ToLower() + "_jint";
                var jsClassName = responserName + "_jint";
                if (mJsModule.Exist(jintId))
                {
#if UNITY_EDITOR || DEBUG
                    Debuger.Log(string.Format("发现Jint通信协议处理器脚本{0},将执行Jint处理流程！", jintId));
#endif
                    GetOneMessage(messageBase.cmdId, messageBase.dataContent);
                    InvokeOneJintResponser(responserName, messageBase, jsClassName);
                }
                else
                {
                    DoResponser(responserName, messageBase.dataContent);
                }
            }
        }

        private readonly Dictionary<string, CommunicationAgent> mJsResponserDic = new Dictionary<string, CommunicationAgent>();
        private readonly Dictionary<string, CommunicationAgent> mJsRequesterDic = new Dictionary<string, CommunicationAgent>();
        private JsValue mJsRequesters;
        private JsValue mJsResponsers;


        private void InvokeOneJintResponser(string responserName, IukMessageBase messageBase, string jintId)
        {
            if (!mJsResponserDic.ContainsKey(jintId))
            {
                var code = string.Format("Iuker.NetProcessers.Responsers.{0} = new {1}.{0}();", jintId
                    , U3DFrame.AppContext.CurrentSonProject);
                mJsModule.DoString(code);
                if (mJsResponsers == null)
                {
                    var jsIuker = mJsModule.GetGlobalValue("Iuker").As<JsValue>();
                    var jsNetProcessers = jsIuker.GetJsValue("NetProcessers");
                    mJsResponsers = jsNetProcessers.GetJsValue("Responsers");
                }

                var JsReponser = mJsResponsers.GetJsValue(jintId);
                var newAgent = new CommunicationAgent(JsReponser);
                mJsResponserDic.Add(jintId, newAgent);
            }

            var jsAgent = mJsResponserDic[jintId];
            if (jsAgent.IsDoCs)
            {
                DoResponser(responserName, messageBase.dataContent);
            }

            jsAgent.Execute();
        }

        public override void RequestCommunication(string protoname)
        {
            var jintId = protoname.ToLower() + "_requester_jint";
            var JsClassName = protoname + "_Requester_jint";
            if (!mJsModule.Exist(jintId))
            {
                base.RequestCommunication(protoname);
            }
            else
            {
                if (!mJsRequesterDic.ContainsKey(jintId))
                {
                    var code = string.Format("Iuker.NetProcessers.Requesters.{0} = new {1}.{0}();", JsClassName
                        , U3DFrame.AppContext.CurrentSonProject);
                    mJsModule.DoString(code);
                    if (mJsRequesters == null)
                    {
                        var jsIuker = mJsModule.GetGlobalValue("Iuker").As<JsValue>();
                        var jsNetProcessers = jsIuker.GetJsValue("NetProcessers");
                        mJsRequesters = jsNetProcessers.GetJsValue("Requesters");
                    }

                    var jsRequester = mJsRequesters.GetJsValue(JsClassName);
                    var newAgent = new CommunicationAgent(jsRequester);
                    mJsRequesterDic.Add(JsClassName, newAgent);
                }

                var jsAgent = mJsRequesterDic[JsClassName];
                if (jsAgent.IsDoCs)
                {
                    base.RequestCommunication(protoname);
                }

                jsAgent.Execute();
            }
        }

        private void GetOneMessage(int protoId, byte[] messageBytes)
        {
            foreach (var bridge in ProtobufBridges)
            {
                var message = bridge.DeSerialize(protoId, messageBytes);
                if (message == null) continue;

                var protoType = message.GetType().Name;
                U3dSocketInterchanger.PushProtoToJs(U3DFrame.AppContext.CurrentSonProject,
                    protoType, message);
            }
        }

        /// <summary>
        /// 通信处理器代理
        /// </summary>
        private class CommunicationAgent
        {
            private readonly JsValue mJsPq;
            private readonly JsValue mJsObject;
            public readonly bool IsDoCs;

            public CommunicationAgent(JsValue jsObject)
            {
                mJsObject = jsObject;
                var iD = jsObject.GetJsValue("IsDoCsharp");
                IsDoCs = iD.Invoke(jsObject).AsBoolean();
                var initFunc = jsObject.GetJsValue("Init");
                initFunc.InvokeByThis(jsObject);
                mJsPq = jsObject.GetJsValue("ProcessRequest");
            }

            public void Execute()
            {
                mJsPq.InvokeByThis(mJsObject);
            }
        }
    }
}