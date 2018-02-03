/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/27 21:46:57
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module;
using Iuker.Common.Module.Communication;
using Iuker.Common.Module.Socket;
using Iuker.Common.Serialize;
using Iuker.UnityKit.Run.Base.Context;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.Communication.Socket;
using Iuker.UnityKit.Run.Base.Config;
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
using Iuker.UnityKit.Run.Module.View.MVDA;


namespace Iuker.UnityKit.Run.Base
{
    public interface IU3dFrame : IFrame
    {
        /// <summary>
        /// 应用配置实例
        /// </summary>
        RuntimeConfig UnityAppConfig { get; }

        /// <summary>
        /// 应用上下文环境
        /// </summary>
        UnityAppContext AppContext { get; }

        /// <summary>
        /// 当前项目的程序集
        /// </summary>
        List<Assembly> ProjectAssemblys { get; }

        IU3dFrame BindingAssemblys(params Assembly[] assemblies);

        /// <summary>
        /// 绑定通讯调度器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IU3dFrame BindingCommunicationDispatcher(Type type);

        IU3dFrame EnableJint();

        /// <summary>
        /// 启动一个协程
        /// </summary>
        /// <param name="ietor"></param>
        void StartCoroutine(IEnumerator ietor);

        /// <summary>
        /// 序列化器
        /// </summary>
        ISerializer Serializer { get; }

        IU3dFrame BindingSerializer(ISerializer serializer);

        /// <summary>
        /// 协议编号解析器
        /// </summary>
        IProtoIdResolver ProtoIdResolver { get; }

        IU3dFrame BindingProtoIdResolver(IProtoIdResolver protoIdResolver);

        /// <summary>
        /// 通讯调度器
        /// </summary>
        ICommunicationDispatcher CommunicationDispatcher { get; }

        /// <summary>
        /// 编码解码器
        /// </summary>
        IEncoder Encoder { get; }

        IU3dFrame BindingEncoder(IEncoder encoder);

        /// <summary>
        /// 设置当前运行的子项目的名字。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IU3dFrame SetCurrentSonProject(string name);

        /// <summary>
        /// 添加一个框架启动完成回调委托
        /// </summary>
        /// <param name="action"></param>
        void AddFrameInitedAction(Action action);

        /// <summary>
        /// 尝试进入登录视图
        /// 如果当前有模块未就绪则会跳出执行
        /// </summary>
        void TryEnterLoginView();

        IU3dFrame AddBefore(Action action);

        IU3dFrame AddAfter(Action action);

        #region 模块缓存属性

        /// <summary>
        /// 事件模块
        /// </summary>
        IU3dAppEventModule EventModule { get; }

        /// <summary>
        /// 视图模块
        /// </summary>
        IU3dViewModule ViewModule { get; }

        /// <summary>
        /// 多语言模块
        /// </summary>
        IU3dIl8nModule Il8NModule { get; }

        /// <summary>
        /// 音乐模块
        /// </summary>
        IU3dMusicModule MusicModule { get; }

        /// <summary>
        /// 本地数据模块
        /// </summary>
        IU3dLocalDataModule LocalDataModule { get; }

        /// <summary>
        /// 音效模块
        /// </summary>
        IU3dSoundEffectModule SoundEffectModule { get; }

        /// <summary>
        /// 资源模块
        /// </summary>
        IU3dAssetModule AssetModule { get; }

        /// <summary>
        /// 测试模块
        /// </summary>
        IU3dTestModule TestModule { get; }

        /// <summary>
        /// 计时器模块
        /// </summary>
        IU3dTimerModule TimerModule { get; }

        /// <summary>
        /// socket通信模块
        /// </summary>
        IU3dSocketModule SocketModule { get; }

        /// <summary>
        /// 调试器模块
        /// </summary>
        IU3dDebuggerModule DebuggerModule { get; }

        /// <summary>
        /// 数据解析模块
        /// </summary>
        IU3dDataModule DataModule { get; }

        /// <summary>
        /// 依赖注入模块
        /// </summary>
        IU3dInjectModule InjectModule { get; }

        /// <summary>
        /// 输入输出控制模块
        /// </summary>
        IU3dInputResponseModule InputResponseModule { get; }

        /// <summary>
        /// 响应式数据模型模块
        /// </summary>
        IU3dReactiveDataModelModule ReactiveDataModelModule { get; }

        /// <summary>
        /// 管理器模块
        /// </summary>
        IU3dManagerModule ManagerModule { get; }

        /// <summary>
        /// 性能分析模块
        /// </summary>
        IU3dProfilerModule ProfilerModule { get; }

        #endregion
    }
}
