#region NoteHead
/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2017/02/15 15:25:49
/*  Email:          liuruoyu1981@gmail.com
/*  QQCode:         35490136
***********************************************************************************************/
#endregion


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Module;
using Iuker.Common.Module.Communication;
using Iuker.Common.Module.Socket;
using Iuker.Common.Serialize;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Base.Context;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.Communication.Socket;
using Iuker.UnityKit.Run.Module.Data;
using Iuker.UnityKit.Run.Module.Debugger;
using Iuker.UnityKit.Run.Module.Event;
using Iuker.UnityKit.Run.Module.Il8n;
using Iuker.UnityKit.Run.Module.Inject;
using Iuker.UnityKit.Run.Module.InputResponse;
using Iuker.UnityKit.Run.Module.LocalData;
using Iuker.UnityKit.Run.Module.Managers;
using Iuker.UnityKit.Run.Module.Music;
using Iuker.UnityKit.Run.Module.Profiler;
using Iuker.UnityKit.Run.Module.ReactiveDataModel;
using Iuker.UnityKit.Run.Module.SoundEffect;
using Iuker.UnityKit.Run.Module.Test;
using Iuker.UnityKit.Run.Module.Timer;
using Iuker.UnityKit.Run.Module.Video;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Iuker.UnityKit.Run
{
    public sealed class DefaultU3DFrame : BaseFrame, IU3dFrame
    {
        #region 框架启动流程

        private readonly List<Action> mBeforeActions = new List<Action>();
        private readonly List<Action> mAfterActions = new List<Action>();

        public IU3dFrame AddBefore(Action action)
        {
            if (action != null)
            {
                mBeforeActions.Add(action);
            }

            return this;
        }

        public IU3dFrame AddAfter(Action action)
        {
            if (action != null)
            {
                mAfterActions.Add(action);
            }

            return this;
        }

        private void InvokeBeforeActions()
        {
            mBeforeActions.ForEach(del => del());
        }

        private void InvokeAfterActions()
        {
            mAfterActions.ForEach(del => del());
        }

        public void Init()
        {
            InvokeBeforeActions();

            UnityAppConfig = new RuntimeConfig();

            InitLog();
            Application.runInBackground = true;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.targetFrameRate = 30;
            }

            UnityAppConfig.ProjectBaseConfig = ProjectBaseConfig.Instance;

#if UNITY_EDITOR
            DestroyDevelopRoot();
#endif

            InitCommonInterfaces();
            InitModule();
            AppStart();
            InvokeAfterActions();
        }

        private List<IModule> mNotReadyModules;

        public void TryEnterLoginView()
        {
            if (mNotReadyModules == null) mNotReadyModules = ModuleDictionary.Values.ToList();

            mNotReadyModules = mNotReadyModules.FindAll(m => !m.IsReady).ToList();
            if (mNotReadyModules.Count > 0) return;

            var viewId = string.IsNullOrEmpty(Bootstrap.Instance.AfterUpdateViewId)
                ? Bootstrap.Instance.EntryProject.LoginViewId
                : Bootstrap.Instance.AfterUpdateViewId;
            ViewModule.CreateView(viewId);
        }

        private void InitCommonInterfaces()
        {
            InitSerializer();
            InitProtoIdResolver();
            InitCommunicationDispatcher();
            InitSocketEncoder();
        }

        private void DestroyDevelopRoot()
        {
            GameObject.Find("ui_root/view_background_root").GetAllChild().ForEach(g => g.SafeDestroy());
            GameObject.Find("ui_root/view_normal_root").GetAllChild().ForEach(g => g.SafeDestroy());
            GameObject.Find("ui_root/view_parasitic_root").GetAllChild().ForEach(g => g.SafeDestroy());
            GameObject.Find("ui_root/view_popup_root").GetAllChild().ForEach(g => g.SafeDestroy());
            GameObject.Find("ui_root/view_top_root").GetAllChild().ForEach(g => g.SafeDestroy());
            GameObject.Find("ui_root/view_mask_root").GetAllChild().ForEach(g => g.SafeDestroy());
            GameObject.Find("scene_root").GetAllChild().ForEach(g => g.SafeDestroy());
        }

        public List<Assembly> ProjectAssemblys { get; private set; }

        public IU3dFrame BindingAssemblys(params Assembly[] assemblies)
        {
            ProjectAssemblys = assemblies.ToList();
            return this;
        }

        private static void InitLog()
        {
            Debuger.Init(Debug.Log, Debug.LogWarning, Debug.LogError, Debug.LogException);
        }

        private bool mEnableJint;

        public IU3dFrame EnableJint()
        {
            mEnableJint = true;
            return this;
        }

        private void InitModule()
        {
            ModuleTypeDictionary = ReflectionUitlity.GetTypeDictionary<IModule>(ProjectAssemblys);
            var modules = UnityAppConfig.ProjectBaseConfig.Modules.OrderBy(m => m.LaucherIndex).ToList();
            foreach (var m in modules)
            {
                var typeName = GetModuleTypeName(m);
                if (!ModuleTypeDictionary.ContainsKey(typeName))
                    throw new Exception(string.Format("没有找到指定的目标模块实例类型{0}", typeName));

                var targetT = ModuleTypeDictionary[typeName];
                var moduleInstance = Activator.CreateInstance(targetT) as IModule;
                if (moduleInstance == null)
                {
                    throw new Exception(string.Format("目标模块类型{0}为空", targetT.Name));
                }

                ModuleDictionary.Add(m.ModuleType, moduleInstance); //  缓存模块
                moduleInstance.Init(this);  // 执行模块的初始化
            }

            // 发送框架初始化完毕事件
            EventModule.IssueEvent(U3dEventCode.Frame_Inited.Literals);
            // 执行外部的框架完成回调委托
            mFrameOnIntedActions.ForEach(del => del());
        }

        private string GetModuleTypeName(Base.Config.Module m)
        {
            string typeName;
            if (!mEnableJint)
            {
                typeName = m.CsharpType;
            }
            else
            {
                typeName = m.JintType == "" ? m.CsharpType : m.JintType;
            }

            if (typeName == null)
                throw new ArgumentNullException("typeName");

            return typeName;
        }

        private void AppStart()
        {
            if (UnityAppConfig.ProjectBaseConfig.IsPlayStartVideo)
            {
                var video = GetModule<IU3dVideoModule>();
                var path = RootConfig.GetCurrentProject().ProjectName + "_start.mp4";
                video.Play(path, OnStartVideoPlayOver);
                var inputModule = GetModule<IU3dInputResponseModule>();
                inputModule.WatchInputEvent(InputEventType.Click, OnClickExitStartVideo, 1);
            }
            else
            {
                EventModule.IssueEvent(U3dEventCode.View_InitFirst.Literals);
            }
        }
        private void OnStartVideoPlayOver()
        {
            var inputModule = GetModule<IU3dInputResponseModule>();
            inputModule.RemoveInputEvent(InputEventType.Click, OnClickExitStartVideo);
            EventModule.IssueEvent(U3dEventCode.View_InitFirst.Literals);
        }

        private void OnClickExitStartVideo() { GetModule<IU3dVideoModule>().Stop(); }

        public void StartCoroutine(IEnumerator ietor)
        {
            U3dCoroutiner.Instance.StartCoroutine(ietor);
        }

        #endregion

        #region 通用接口

        public IU3dFrame BindingSerializer(ISerializer serializer)
        {
            Serializer = serializer;
            return this;
        }

        public IProtoIdResolver ProtoIdResolver { get; private set; }
        public ISerializer Serializer { get; private set; }
        public IU3dFrame BindingProtoIdResolver(IProtoIdResolver protoIdResolver)
        {
            ProtoIdResolver = protoIdResolver;
            return this;
        }

        public ICommunicationDispatcher CommunicationDispatcher { get; private set; }
        private Type mCommunicationType;
        public IU3dFrame BindingCommunicationDispatcher(Type type)
        {
            mCommunicationType = type;
            return this;
        }

        private void InitCommunicationDispatcher()
        {
            CommunicationDispatcher = Activator.CreateInstance(mCommunicationType) as ICommunicationDispatcher;
            if (CommunicationDispatcher == null)
                throw new ArgumentNullException("CommunicationDispatcher");

            CommunicationDispatcher.Init(this);
        }

        public IEncoder Encoder { get; private set; }

        private void InitSerializer()
        {
            Serializer = new DefaultU3dProtobufSerializer().Init(this);
        }

        private void InitProtoIdResolver()
        {
            var tempProtoResolver = new RootProtoResolver();
            Bootstrap.Instance.GetCombinProject().ForEach(p => InitSonPrjectsProtoIdResolver(p, tempProtoResolver));
        }

        private void InitSonPrjectsProtoIdResolver(Project project, RootProtoResolver tempProtoResolver)
        {
            var allSons = project.AllSonProjects;
            foreach (var son in allSons)
            {
                var sonProtoIdResolverType = ReflectionUitlity.GetAllTypes(ProjectAssemblys).Find(t => t.Name == son.ProtoIdResolverTypeName);

                if (sonProtoIdResolverType != null)
                {
                    var sonProtoIdResolver = Activator.CreateInstance(sonProtoIdResolverType) as IProtoIdResolver;
                    if (sonProtoIdResolver == null)
                        throw new Exception(string.Format("当前项目的协议id解析器{0}构建失败！", sonProtoIdResolverType));

                    sonProtoIdResolver.Init();
                    tempProtoResolver.AddSonProjectProtoIdResolver(sonProtoIdResolver);
                }
                else
                {
#if DEBUG || UNITY_EDITOR
                    Debug.LogWarning(string.Format("没有找到按照约定需要实现的子项目{0}的协议Id解析器类型，类型名为{1}，协议Id解析器构建将跳过！",
                        son.ProjectName, son.ProtoIdResolverTypeName));
#endif
                }
                ProtoIdResolver = tempProtoResolver;
            }
        }

        private void InitSocketEncoder()
        {
            Encoder = new DefaultU3dEncoder().Init(this);
        }

        #endregion

        #region 框架启动结束外部回调注册

        private readonly List<Action> mFrameOnIntedActions = new List<Action>();
        public IU3dFrame BindingEncoder(IEncoder encoder)
        {
            Encoder = encoder;
            return this;
        }

        public IU3dFrame SetCurrentSonProject(string name)
        {
            if (AppContext == null)
            {
                AppContext = new UnityAppContext();
            }

            AppContext.CurrentSonProject = name;
            return this;
        }

        public void AddFrameInitedAction(Action action)
        {
            mFrameOnIntedActions.Add(action);
        }

        #endregion

        #region 配置解析

        public RuntimeConfig UnityAppConfig { get; private set; }
        public UnityAppContext AppContext { get; private set; }

        #endregion

        #region 模块缓存属性

        private IU3dAppEventModule _eventModule;

        public IU3dAppEventModule EventModule
        {
            get
            {
                return _eventModule ?? (_eventModule = GetModule<IU3dAppEventModule>());
            }
        }

        private IU3dViewModule _viewModule;

        public IU3dViewModule ViewModule
        {
            get
            {
                return _viewModule ?? (_viewModule = GetModule<IU3dViewModule>());
            }
        }

        private IU3dIl8nModule _il8NModule;

        public IU3dIl8nModule Il8NModule
        {
            get
            {
                return _il8NModule ?? (_il8NModule = GetModule<IU3dIl8nModule>());
            }
        }

        private IU3dMusicModule _musicModule;

        public IU3dMusicModule MusicModule
        {
            get
            {
                return _musicModule ?? (_musicModule = GetModule<IU3dMusicModule>());
            }
        }

        private IU3dLocalDataModule _localDataModule;

        public IU3dLocalDataModule LocalDataModule
        {
            get
            {
                return _localDataModule ?? (_localDataModule = GetModule<IU3dLocalDataModule>());
            }
        }

        private IU3dSoundEffectModule _soundEffectModule;

        public IU3dSoundEffectModule SoundEffectModule
        {
            get
            {
                return _soundEffectModule ?? (_soundEffectModule = GetModule<IU3dSoundEffectModule>());
            }
        }

        private IU3dAssetModule _assetModule;

        public IU3dAssetModule AssetModule
        {
            get
            {
                return _assetModule ?? (_assetModule = GetModule<IU3dAssetModule>());
            }
        }

        private IU3dTestModule _testModule;

        public IU3dTestModule TestModule
        {
            get
            {
                return _testModule ?? (_testModule = GetModule<IU3dTestModule>());
            }
        }

        private IU3dTimerModule _timerModule;

        public IU3dTimerModule TimerModule
        {
            get
            {
                return _timerModule ?? (_timerModule = GetModule<IU3dTimerModule>());
            }
        }

        private IU3dSocketModule _socketModule;

        public IU3dSocketModule SocketModule
        {
            get
            {
                return _socketModule ?? (_socketModule = GetModule<IU3dSocketModule>());
            }
        }

        private IU3dDebuggerModule _debuggerModule;

        public IU3dDebuggerModule DebuggerModule
        {
            get
            {
                return _debuggerModule ?? (_debuggerModule = GetModule<IU3dDebuggerModule>());
            }
        }

        private IU3dDataModule _dataModule;

        public IU3dDataModule DataModule
        {
            get
            {
                return _dataModule ?? (_dataModule = GetModule<IU3dDataModule>());
            }
        }

        private IU3dInjectModule mInjectModule;

        public IU3dInjectModule InjectModule
        {
            get
            {
                return mInjectModule ?? (mInjectModule = GetModule<IU3dInjectModule>());
            }
        }

        private IU3dReactiveDataModelModule mReactiveDataModelModule;

        public IU3dReactiveDataModelModule ReactiveDataModelModule
        {
            get
            {
                return mReactiveDataModelModule ?? (mReactiveDataModelModule = GetModule<IU3dReactiveDataModelModule>());
            }
        }

        private IU3dInputResponseModule _mIu3DInputResponseModule;

        public IU3dInputResponseModule InputResponseModule
        {
            get { return _mIu3DInputResponseModule ?? (_mIu3DInputResponseModule = GetModule<IU3dInputResponseModule>()); }
        }

        private IU3dManagerModule mManagerModule;

        public IU3dManagerModule ManagerModule
        {
            get
            {
                return mManagerModule ?? (mManagerModule = GetModule<IU3dManagerModule>());
            }
        }


        private IU3dProfilerModule mProfilerModule;

        public IU3dProfilerModule ProfilerModule
        {
            get
            {
                return mProfilerModule ?? (mProfilerModule = GetModule<IU3dProfilerModule>());
            }
        }

        #endregion

        /// <summary>
        /// Unity协程器
        /// </summary>
        // ReSharper disable once ClassNeverInstantiated.Local
        private class U3dCoroutiner : MonoSingleton<U3dCoroutiner>
        {
        }
    }
}

