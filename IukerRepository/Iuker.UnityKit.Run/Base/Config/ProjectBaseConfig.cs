using System;
using System.Collections.Generic;
using System.IO;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base.Config
{
    /// <summary>
    /// 项目基础配置
    /// 所有子项目共用的配置项
    /// </summary>
    [Serializable]
    public class ProjectBaseConfig
    {
        /// <summary>
        /// Wx应用包名
        /// </summary>
        public string WxBundleId;

        /// <summary>
        /// 微信应用Id
        /// </summary>
        public string WxAppId;

        /// <summary>
        /// 资源及脚本更新http服务器地址
        /// 默认为本机
        /// </summary>
        public string HotUpdateHttpServerUrl = "http://127.0.0.1/";

        /// <summary>
        /// 心跳包发送频率
        /// </summary>
        public int HeartFrequency;

        /// <summary>
        /// 音效音量
        /// </summary>
        public float SoundEffectVolume;

        /// <summary>
        /// 默认点击音效
        /// 为None则说明没有
        /// </summary>
        public string DefaultClickSound = "None";

        /// <summary>
        /// 音乐音量
        /// </summary>
        public float MusicVolume;

        /// <summary>
        /// 日志输出器配置
        /// 允许多少个客户端开发人员拥有独自的日志输出器在这里配置
        /// </summary>
        public List<string> Logers;

        /// <summary>
        /// 是否播放开场视频
        /// </summary>
        public bool IsPlayStartVideo;

        /// <summary>
        /// 是否使用lua模块
        /// </summary>
        public bool IsUseLuaModule;

        /// <summary>
        /// 是否使用内网服务器
        /// </summary>
        public bool IsUseLocalServer;

        /// <summary>
        /// 外网服务器地址列表配置
        /// </summary>
        public List<Server> WlanServers;

        /// <summary>
        /// 内网服务器地址配置
        /// </summary>
        public Server LocalServer;

        /// <summary>
        /// 模块配置
        /// </summary>
        public List<Module> Modules;

        public List<Server> Servers
        {
            get
            {
                var servers = new List<Server>();
                if (IsUseLocalServer)
                {
                    servers.Add(LocalServer);
                }
                else
                {
                    servers.AddRange(WlanServers);
                }
                return servers;
            }
        }

        /// <summary>
        /// 当前项目公共基础配置沙盒下路径
        /// </summary>
        public static string SandboxPath
        {
            get
            {
                return Application.persistentDataPath +
                       string.Format("/_{0}/Resources/{1}_projectbaseconfig.json",
                           RootConfig.CrtProjectName, RootConfig.CrtProjectName.ToLower());
            }
        }

        private static ProjectBaseConfig mInsance;

        public static ProjectBaseConfig Instance
        {
            get
            {
                if (mInsance != null) return mInsance;

                if (Application.platform == RuntimePlatform.LinuxEditor ||
                    Application.platform == RuntimePlatform.OSXEditor
                    || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (!File.Exists(RootConfig.GetCurrentProject().BaseConfigFullPath))
                    {
                        CreateProjectBaseConfig();
                    }

                    mInsance = JsonUtility.FromJson<ProjectBaseConfig>(Resources
                        .Load<TextAsset>(string.Format("{0}_projectbaseconfig", RootConfig.CrtProjectName.ToLower())).text);
                }
                else
                {
                    if (File.Exists(SandboxPath))
                    {
                        mInsance = JsonUtility.FromJson<ProjectBaseConfig>(File.ReadAllText(SandboxPath));
                    }
                    {
                        var resPath = string.Format("{0}_projectbaseconfig", RootConfig.CrtProjectName.ToLower());
                        var content = Resources.Load<TextAsset>(resPath).text;
                        FileUtility.WriteAllText(SandboxPath, content);
                        mInsance = JsonUtility.FromJson<ProjectBaseConfig>(content);
                    }
                }

                return mInsance;
            }
        }

        public static void CreateProjectBaseConfig()
        {
            var project = RootConfig.GetCurrentProject();

            if (File.Exists(project.BaseConfigFullPath))
            {
                Debug.Log(string.Format("当前项目{0}的共用配置文件已存在，将跳过创建！", RootConfig.GetCurrentProject().ProjectName));
                return;
            }

            ProjectBaseConfig projectBaseConfig = new ProjectBaseConfig();
            // 配置根

            projectBaseConfig.SoundEffectVolume = 0.5f;
            projectBaseConfig.MusicVolume = 0.5f;

            projectBaseConfig.Logers = new List<string>
            {
                "Default",
            };

            projectBaseConfig.WlanServers = new List<Server>
            {
                new Server{Name = "Default",Url = "www.baidu.com",Port = 8888},
            };

            projectBaseConfig.LocalServer = new Server { Url = "192.168.1.168", Port = 8888 };

            // 内网服务器开关
            projectBaseConfig.IsUseLocalServer = true;

            // 模块配置示例项
            projectBaseConfig.Modules = new List<Module>
            {
                new Module {ModuleType = "IU3dAppEventModule",CsharpType = "DefaultU3dModule_Event",JintType = "U3dJintModule_Event",LaucherIndex = 1},   // 事件模块
                new Module {ModuleType = "IU3dHotUpdateModule",CsharpType = "DefaultU3dModule_HotUpdate",LaucherIndex = 2},   // 热更新模块
                new Module {ModuleType = "IU3dAssetModule",CsharpType = "DefaultU3dModule_Asset",LaucherIndex = 3},   // 资源模块
                new Module {ModuleType = "IU3dJavaScriptModule",CsharpType = "DefaultU3dModule_JavaScript",LaucherIndex = 4},   // 脚本模块
                new Module {ModuleType = "IU3dViewModule",CsharpType = "DefaultU3dModule_View",JintType = "U3dJintModule_View"},   // 视图模块
                new Module {ModuleType = "IU3dRouterModule",CsharpType = "DefaultIu3DModule_Router"},   //路由模块
                new Module {ModuleType = "IU3dLocalDataModule",CsharpType = "DefaultU3dModule_LocalData",JintType = "U3dJintModule_LocalData"},   // 本地数据模块
                new Module {ModuleType = "IU3dDataModule",CsharpType = "DefaultU3dModule_Data"},   // 数据模块
                new Module {ModuleType = "IU3dIl8nModule",CsharpType = "DefaultU3dModule_Il8n"},   // 多语言数据模块
                new Module {ModuleType = "IU3dInjectModule",CsharpType = "DefaultU3dModule_Inject"},   // 依赖注入解析模块
                new Module {ModuleType = "IU3dInputResponseModule",CsharpType = "DefaultU3dModule_InputResponse",JintType = "U3dJintModule_InputResponser"},   // 输入输出控制模块
                new Module {ModuleType = "IU3dMusicModule",CsharpType = "DefaultU3dModule_Music"},   // 音乐模块
                new Module {ModuleType = "IU3dSoundEffectModule",CsharpType = "DefaultU3dModule_SoundEffect"},   // 音效模块
                new Module {ModuleType = "IU3dManagerModule",CsharpType = "DefaultU3dModule_Manager"},   // 管理器模块
                new Module {ModuleType = "IU3dVideoModule",CsharpType = "DefaultU3dModule_Video"},   // 视频模块
                new Module {ModuleType = "IU3dTimerModule",CsharpType = "DefaultU3dModule_Timer"},   // 计时器模块
                new Module {ModuleType = "IU3dHttpModule",CsharpType ="DefaultU3dModule_Http"},   // Http通信模块模块
                new Module {ModuleType = "IU3dSocketModule",CsharpType = "DefaultU3dModule_Socket"},   // Socket通信模块模块
                new Module {ModuleType = "IU3dReactiveDataModelModule",CsharpType = "DefaultU3dModule_ReactiveDataModel"},   // 响应式数据模型模块
                new Module {ModuleType = "IU3dTestModule",CsharpType = "DefaultU3dModule_Test"},   // 测试模块
                new Module {ModuleType = "IU3dDebuggerModule",CsharpType = "DefaultU3dModule_Debugger"},   // 调试器模块
                new Module {ModuleType = "IU3dProfilerModule",CsharpType = "DefaultU3dModule_Profiler"},   // 性能模块
            };

            if (File.Exists(project.BaseConfigFullPath))
            {
                Debug.LogWarning(string.Format("指定项目{0}的基础公共运行时配置文件已存在，将不会覆盖创建！",
                    RootConfig.GetCurrentProject().ProjectName));
                return;
            }

            FileUtility.WriteAllText(project.BaseConfigFullPath, JsonUtility.ToJson(projectBaseConfig));
        }

        public static void SetInstanceNull() { mInsance = null; }

        public static void Update()
        {
            var project = RootConfig.GetCurrentProject();
            var content = JsonUtility.ToJson(Instance);
            File.Delete(project.BaseConfigFullPath);
            File.WriteAllText(project.BaseConfigFullPath, content);
            File.WriteAllText(SandboxPath, content);
            Debug.Log(string.Format("项目{0}的基础公共配置已更新！", RootConfig.GetCurrentProject().ProjectName));
        }
    }

}