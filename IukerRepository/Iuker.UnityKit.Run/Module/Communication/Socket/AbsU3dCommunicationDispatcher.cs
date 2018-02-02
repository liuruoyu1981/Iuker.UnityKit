/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/1 18:29:49
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
using System.Linq;
using System.Reflection;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Communication;
using Iuker.Common.Module.Socket;
using Iuker.Common.Serialize;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Communication.Socket
{
    /// <inheritdoc />
    /// <summary>
    /// Unity通讯调度器基类
    /// </summary>
    public abstract class AbsU3dCommunicationDispatcher : ICommunicationDispatcher
    {
        /// <summary>
        /// 是否进行调度
        /// </summary>
        protected bool mIsDispatch = true;

        /// <summary>
        /// 初始化状态标识
        /// </summary>
        private bool mIsInited;

        /// <summary>
        /// 框架引用
        /// </summary>
        protected IU3dFrame U3DFrame;

        /// <summary>
        /// 项目序列化器
        /// </summary>
        protected ISerializer mSerializer;

        /// <summary>
        /// 协议id解析器
        /// </summary>
        protected IProtoIdResolver mProtoIdResolver;

        /// <summary>
        /// 通信答复处理器字典
        /// </summary>
        private readonly Dictionary<string, ICommunicationResponser> mResponserDictionary = new Dictionary<string, ICommunicationResponser>();

        /// <summary>
        /// 通信请求处理器字典
        /// </summary>
        private readonly Dictionary<string, ICommunicationRequester> mRequesterDictionary = new Dictionary<string, ICommunicationRequester>();

        /// <summary>
        /// 通信答复处理器类型列表
        /// </summary>
        private List<Type> mResponsersTypes;

        /// <summary>
        /// 通信请求处理器类型列表
        /// </summary>
        private List<Type> mRequesterTypes;

        public virtual ICommunicationDispatcher Init(IFrame u3DFrame)
        {
            if (mIsInited) return this;

            U3DFrame = (IU3dFrame)u3DFrame;
            mSerializer = U3DFrame.Serializer;
            mProtoIdResolver = U3DFrame.ProtoIdResolver;
            U3DFrame.AddFrameInitedAction(OnFrameInited);
            mIsInited = true;

            return this;
        }

        private void CloseDispatch()
        {
            mIsDispatch = false;
        }

        private void OpenDispatch()
        {
            mIsDispatch = true;
        }

        protected IU3dSocketModule mSocketModule;

        protected virtual void OnFrameInited()
        {
            mSocketModule = U3DFrame.SocketModule;
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), MessageDispatch);
            U3DFrame.EventModule.WatchEvent(U3dEventCode.Net_DispatchClose.Literals, CloseDispatch);
            U3DFrame.EventModule.WatchEvent(U3dEventCode.Net_DispatchOpen.Literals, OpenDispatch);
            InitAllType(U3DFrame.ProjectAssemblys);
            InitProtobufBridges();
        }

        private void InitAllType(List<Assembly> assemblies)
        {
            mResponsersTypes =
                ReflectionUitlity.GetTypeList<ICommunicationResponser>(assemblies);
            mRequesterTypes =
                ReflectionUitlity.GetTypeList<ICommunicationRequester>(assemblies);
        }

        protected readonly List<IProtobufBridge> ProtobufBridges = new List<IProtobufBridge>();

        private void InitProtobufBridges()
        {
            var bridgeTypes = ReflectionUitlity.GetTypeList<IProtobufBridge>(U3DFrame.ProjectAssemblys);
            foreach (var bridgeType in bridgeTypes)
            {
                var bridge = Activator.CreateInstance(bridgeType) as IProtobufBridge;
                if (bridge == null)
                {
                    Debug.LogError(string.Format("目标protobuf解析器脚本{0}创建失败！", bridgeType.FullName));
                }
                else
                {
                    bridge.Init(U3DFrame);
                    bridge.InjectMessageCreateFunction();
                    ProtobufBridges.Add(bridge);
                }
            }
        }

        /// <summary>
        /// 请求通信
        /// </summary>
        /// <param name="protoname"></param>
        public virtual void RequestCommunication(string protoname)
        {
            if (!mRequesterDictionary.ContainsKey(protoname))
            {
                var projects = Bootstrap.Instance.CombinProjects;
                var types = new List<Type>();
                foreach (var name in projects)
                {
                    var tempTypes = mRequesterTypes.FindAll(t => t.Namespace != null && t.Name.StartsWith(protoname) && t.Namespace.StartsWith(name)).OrderByDescending(t => t.Name).ToList();
                    types.AddRange(tempTypes);
                }

                if (types.Count == 0)
                {
#if UNITY_EDITOR || DEBUG
                    Debuger.LogError(string.Format("没有找到所请求的通信请求处理器脚本{0}！", protoname));
#endif
                    return;
                }

                var requestType = types[0];

                var requester = Activator.CreateInstance(requestType) as ICommunicationRequester;
                if (requester == null)
                    throw new Exception(string.Format("指定的通信请求处理器{0}创建失败！", protoname));

                mRequesterDictionary.Add(protoname, requester);
                requester.Init(U3DFrame);
                requester.ProcessRequest();
            }
            else
            {
                var requester = mRequesterDictionary[protoname];
                requester.ProcessRequest();
            }
        }

        /// <summary>
        /// 调度通信答复处理器
        /// </summary>
        /// <param name="protoname"></param>
        /// <param name="messageBytes"></param>
        protected virtual void DoResponser(string protoname, byte[] messageBytes)
        {
            //  todo 限定命名空间
            if (!mResponserDictionary.ContainsKey(protoname))
            {
                var projectName = RootConfig.GetCurrentProject().ProjectName;
                var types = mResponsersTypes.FindAll(t => t.Namespace != null && (t.Name.StartsWith(protoname) && t.Namespace.StartsWith(projectName))).OrderByDescending(t => t.Name)
                    .ToList();

                if (types.Count == 0)
                    throw new Exception(string.Format("没有找到所请求的通信答复处理器脚本{0}！", protoname));

                var responserType = types[0];

                var responser = Activator.CreateInstance(responserType) as ICommunicationResponser;
                if (responser == null)
                    throw new Exception(string.Format("指定的通信答复处理器{0}创建失败！", protoname));

                mResponserDictionary.Add(protoname, responser);
                responser.Init(U3DFrame);
                responser.ProcessMessage(messageBytes);
            }
            else
            {
                var responser = mResponserDictionary[protoname];
                responser.ProcessMessage(messageBytes);
            }
        }

        /// <summary>
        /// 全局socket消息字段
        /// </summary>
        protected byte[] bytes;

        /// <summary>
        /// socket消息调度
        /// </summary>
        protected abstract void MessageDispatch();
    }
}