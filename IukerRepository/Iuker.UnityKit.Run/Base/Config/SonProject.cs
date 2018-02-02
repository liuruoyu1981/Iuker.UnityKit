using System;
using System.Collections.Generic;
using System.IO;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base.Config
{
    /// <summary>
    /// 子项目配置节
    /// </summary>
    [Serializable]
    public class SonProject
    {
        #region 基础字段

        /// <summary>
        /// 子项目项目名
        /// </summary>
        public string ProjectName;

        /// <summary>
        /// 父项目名
        /// </summary>
        public string ParentName;

        /// <summary>
        /// 子项目的父项目
        /// </summary>
        public Project ParentProject
        {
            get
            {
                var parentP = RootConfig.Instance.AllProjects.Find(p => p.ProjectName == ParentName);
                return parentP;
            }
        }

        /// <summary>
        /// 该子项目的客户端开发者列表
        /// 一个子项目允许有多个开发者
        /// </summary>
        public List<ClientCoder> ClientCoders;

        /// <summary>
        /// 该子项目的当前开发者
        /// </summary>
        public ClientCoder CurrentClientCoder;

        public SonProject Init(string projectName, string parentName)
        {
            ProjectName = projectName;
            ParentName = parentName;

            return this;
        }

        /// <summary>
        /// 更新子项目开发者配置信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ClientCoderError UpdateClientCoder(string name, string email)
        {
            var coderRepeat = ClientCoders.Find(coder => coder.Name == name);
            if (coderRepeat != null) return ClientCoderError.NameRepeat;

            var emailRepeat = ClientCoders.Find(coder => coder.Email == email);
            if (emailRepeat != null) return ClientCoderError.EmailRepeat;

            CurrentClientCoder = new ClientCoder { Name = name, Email = email };
            ClientCoders.Add(CurrentClientCoder);

            return ClientCoderError.None;
        }

        public enum ClientCoderError
        {
            /// <summary>
            /// 无错误
            /// </summary>
            None,

            /// <summary>
            /// 邮箱重复
            /// </summary>
            EmailRepeat,

            /// <summary>
            /// 开发者姓名重复
            /// </summary>
            NameRepeat,
        }

        /// <summary>
        /// 子项目命名空间
        /// </summary>
        public string NameSapce { get { return CompexName; } }

        #endregion

        #region 基础路径

        public string RootDir
        {
            get
            {
                return Application.dataPath + string.Format("/_{0}/{1}/", ParentName, CompexName);
            }
        }


        public string ResourcesDir
        {
            get
            {
                return RootDir + "Resources/";
            }
        }

        /// <summary>
        /// 子项目原生插件存放目录
        /// </summary>
        public string PluginsDir
        {
            get
            {
                return RootDir + ".Plugins/";
            }
        }

        public string CompexName
        {
            get
            {
                return ParentName + "_" + ProjectName;
            }
        }

        /// <summary>
        /// 用于区分事件类型（事件模块内部划分的业务模块）的事件类型名
        /// </summary>
        public string EventTypeName
        {
            get { return ParentName + ProjectName; }
        }

        public string UpdateViewPath
        {
            get
            {
                return ResourcesDir + string.Format("view_{0}_update.prefab", CompexName.ToLower());
            }
        }

        /// <summary>
        /// 子项目配置目录
        /// </summary>
        public string ConfigDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Configs/", CompexName);
            }
        }

        /// <summary>
        /// 子项目视图配置的Resources相对路径
        /// </summary>
        public string ViewsConfigResPath
        {
            get
            {
                return string.Format("Ab_Single_{0}_Configs/{1}_views", CompexName, CompexName.ToLower());
            }
        }

        /// <summary>
        /// 子项目视图配置Common子项目下Resources目录全路径
        /// </summary>
        public string ViewsConfigResourcesPath
        {
            get
            {
                return Application.dataPath +
                       string.Format("/_{0}/{1}_Common/Resources/{2}.json", ParentName,
                           ParentName, ViewsConfigName);
            }
        }

        public string ViewsConfigName
        {
            get
            {
                return string.Format("{0}_views", CompexName.ToLower());
            }
        }

        public string ViewsConfigHotUpdatePath
        {
            get
            {
                return ConfigDir + ViewsConfigName + ".json";
            }
        }

        public string ViewsConfigSandboxPath
        {
            get
            {
                return AssetBundleSanboxDir +
                       string.Format("Configs/{0}.assetbundle", ViewsConfigName);
            }
        }

        #endregion

        #region Cs目录

        /// <summary>
        /// 子项目Dll目录
        /// </summary>
        public string DllDir
        {
            get
            {
                return CSahrpDir + "Dll/";
            }
        }

        /// <summary>
        /// 子项目C#脚本目录
        /// </summary>
        public string CSahrpDir
        {
            get
            {
                return RootDir + "CSharp/";
            }
        }

        public string CsBackProtobufDir
        {
            get { return CSahrpDir + ".BackProtobuf/"; }
        }

        /// <summary>
        /// 子项目csharp视图mvda流程脚本目录
        /// </summary>
        public string CsMvdaDir
        {
            get
            {
                return CSahrpDir + "MVDA/";
            }
        }

        public string CsManagerDir
        {
            get
            {
                return CSahrpDir + "Managers/";
            }
        }

        public string CsEditorDir
        {
            get
            {
                return CSahrpDir + "Editor/";
            }
        }

        public string CsMonoEntiiesDir
        {
            get
            {
                return CSahrpDir + "MonoEnties/";
            }
        }

        public string CsFrameOverrideDir
        {
            get
            {
                return CSahrpDir + "FrameOverride/";
            }
        }

        public string CsReactiveDataModelDir
        {
            get
            {
                return CSahrpDir + "ReactiveDataModel/";
            }
        }

        public string CsCommunicationDir
        {
            get
            {
                return CSahrpDir + "Communication/";
            }
        }

        public string CsCommunicationResponsersDir { get { return CsCommunicationDir + "Responsers/"; } }
        public string CsCommunicationRequestDir { get { return CsCommunicationDir + "Request/"; } }

        public string CsLocalDataDir { get { return CSahrpDir + "LocalData/"; } }

        #endregion

        #region Ts JavaScript Jint

        private string SingleJintDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Jint/", CompexName);
            }
        }

        private string BundledJintDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Bundled_{0}_Jint/", CompexName);
            }
        }

        public string JintDir
        {
            get
            {
                return Directory.Exists(SingleJintDir) ? SingleJintDir : BundledJintDir;
            }
        }

        public string TsIukerDir
        {
            get
            {
                return TsDir + "Iuker/";
            }
        }

        public string TsProjectDir
        {
            get
            {
                return RootDir + "Ts/Project/";
            }
        }

        public string TsProjectLocalDataDir
        {
            get
            {
                return RootDir + "Ts/Project/LocalData/";
            }
        }

        public string TsProjectMvdaDir
        {
            get
            {
                return RootDir + "Ts/Project/Mvda/";
            }
        }

        public string TsCommunicationRequesterDir
        {
            get { return TsProjectDir + "Communication/Requester/"; }
        }

        public string TsCommunicationResponserDir
        {
            get { return TsProjectDir + "Communication/Responser/"; }
        }

        public string TsProjectBuildOutPutDir
        {
            get
            {
                var baseDir = TsDir + ".tsbuild/Project/";
                if (Directory.Exists(baseDir)) return baseDir;

                //  如果子项目的Ts项目有引用其他子项目，则最终的ts脚本输出目录需要重新定位
                var hasRefDir = TsDir + ".tsbuild/" + CompexName + "/Ts/Project/";
                return hasRefDir;
            }
        }

        public string TsBaseRelizeDir
        {
            get { return TsDir + ".tsbuild/Iuker/Realize/"; }
        }

        public string TsDir
        {
            get
            {
                return RootDir + "Ts/";
            }
        }

        public string TsBinDir
        {
            get
            {
                return RootDir + "Ts/bin/";
            }
        }

        public string TsObjDir
        {
            get
            {
                return RootDir + "Ts/obj/";
            }
        }

        /// <summary>
        /// 子项目Typescript项目的项目信息文件
        /// </summary>
        public string TsProjPath
        {
            get
            {
                return TsDir + CompexName + ".Ts.njsproj";
            }
        }

        public string TsAssetIdPath
        {
            get
            {
                return TsProjectDir + CompexName.ToLower() + "_assetid.ts";
            }
        }

        public string TsProtobufInterfaceName
        {
            get
            {
                return string.Format("{0}_Protobuf_Interface", CompexName);
            }
        }

        public string TsProtobufInterfacePath
        {
            get
            {
                return TsProjectDir + string.Format("/Interface/{0}.ts", TsProtobufInterfaceName);
            }
        }

        #endregion

        #region 资源存放路径


        public string TexturesDir
        {
            get
            {
                return OriginalAssets + "Textures/";
            }
        }

        public string FxEffectDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_FxEffect/", CompexName);
            }
        }

        public string AnimationsDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Animations/", CompexName);
            }
        }

        public string PrefabsDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Prefabs/", CompexName);
            }
        }

        public string AssetDataBaseDir
        {
            get
            {
                return RootDir + "AssetDatabase/";
            }
        }

        public string BinaryDataDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_BinaryData/", CompexName);
            }
        }

        public string LocalDataTxtDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_LocalDataTxt/", CompexName);
            }
        }

        /// <summary>
        /// 子项目音效数据表在Resources目录下的相对路径
        /// </summary>
        public string SoundTableResPath
        {
            get
            {
                return string.Format("{0}_ldsoundeffect", CompexName.ToLower());
            }
        }

        public string AtlasDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Atlas/", CompexName);
            }
        }

        public string AssetDatabaseAtlasDir
        {
            get
            {
                return RootDir + "AssetDatabase/" +
                       string.Format("Ab_Single_{0}_Atlas/", CompexName);
            }
        }

        public string TextureDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Texture/", CompexName);
            }
        }

        public string ViewPrefabDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Views/", CompexName);
            }
        }

        public string FontDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Font/", CompexName);
            }
        }

        public string MusicDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Music/", CompexName);
            }
        }

        public string SoundEffectDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_SoundEffect/", CompexName);
            }
        }

        public string ExcelDir
        {
            get
            {
                return OriginalAssets + "Excel/";
            }
        }

        public string OriginalAssets
        {
            get
            {
                return RootDir + "OriginalAsset/";
            }
        }


        /// <summary>
        /// 当前子项目资源id脚本路径
        /// </summary>
        public string CsAssetIdPath
        {
            get
            {
                return CSahrpDir + string.Format("{0}_AssetId.cs", CompexName);
            }
        }

        public string AssetInfoFileName
        {
            get
            {
                return string.Format("{0}_assetinfos", CompexName.ToLower());
            }
        }

        /// <summary>
        /// 项目二进制资源数据路径
        /// </summary>
        public string AssetInfoPath
        {
            get
            {
                return BinaryDataDir + string.Format("{0}.bytes", AssetInfoFileName);
            }
        }

        public string SpriteInfoFileName
        {
            get
            {
                return string.Format("{0}_spriteinfos", CompexName.ToLower());
            }
        }

        /// <summary>
        /// 子项目图集数据路径
        /// </summary>
        public string SpriteInfoPath
        {
            get
            {
                return BinaryDataDir + string.Format("{0}.bytes", SpriteInfoFileName);
            }
        }

        public string AtlasToSpriteDir
        {
            get
            {
                return OriginalAssets + "AtlasToSprite/";
            }
        }

        public string AtlasOriginDir
        {
            get
            {
                return OriginalAssets + "AtlasOrigin/";
            }
        }

        /// <summary>
        /// 资源导入器相对根目录
        /// </summary>
        public string ImporterDir
        {
            get
            {
                return string.Format("Assets/_{0}/{1}/", ParentName, CompexName) + "AssetDatabase";
            }
        }

        public string Il8nTxtPath
        {
            get
            {
                return string.Format("{0}{1}_ldil8n.txt", LocalDataTxtDir, CompexName.ToLower());
            }
        }

        public string SoundEffectTxtPath
        {
            get
            {
                return string.Format("{0}{1}_ldsoundeffect.txt", LocalDataTxtDir, CompexName.ToLower());
            }
        }


        #endregion

        #region protobuf

        public string ProtobufRootDir
        {
            get
            {
                return OriginalAssets + "Protobuf/";
            }
        }

        public string CsProtobufDir
        {
            get
            {
                return CSahrpDir + "ProtobufScripts/";
            }
        }

        public string ProtobufTxtPath
        {
            get
            {
                return ProtobufRootDir + string.Format("{0}_Protobufs.txt", CompexName);
            }
        }


        public string ProtobufExcelPath
        {
            get
            {
                return ProtobufRootDir + string.Format("{0}_Protobufs.xlsx", CompexName);
            }
        }

        /// <summary>
        /// 子项目proto协议定义文件的文件名包含.proto后缀
        /// </summary>
        public string ProtoFullName
        {
            get
            {
                return string.Format("{0}_ProtoDefine.proto", CompexName);
            }
        }

        /// <summary>
        /// 子项目的proto协议文件的导出路径
        /// </summary>
        public string ProtoExportPath
        {
            get
            {
                return ProtobufRootDir + ProtoFullName;
            }
        }

        /// <summary>
        /// 子项目Protobuf协议数据结构dll文件
        /// </summary>
        public string ProtobufMessageDllPath
        {
            get
            {
                return CsProtobufDir + string.Format("{0}_ProtobufMessage.dll", CompexName);
            }
        }

        /// <summary>
        /// 子项目Protobuf协议序列化器dll文件
        /// </summary>
        public string ProtobufSerializerDllPath
        {
            get
            {
                return CsProtobufDir +
                       string.Format("{0}_ProtobufSerializer.dll", CompexName);
            }
        }

        /// <summary>
        /// 子项目用于协议编译的临时protobuf的dll文件
        /// </summary>
        public string Protobuf_Temp_Net_DllPath
        {
            get
            {
                return CsProtobufDir + "protobuf-net.dll";
            }
        }

        /// <summary>
        /// 子项目Protobuf协议的命名空间
        /// </summary>
        public string ProtobufNameSpace
        {
            get
            {
                return string.Format("{0}_ProtoDefine", CompexName);
            }
        }

        public string ProtoClassPath
        {
            get
            {
                return U3dConstants.ProtobufModelRootDir + CompexName + "_ProtoDefineModel.cs";
            }
        }

        /// <summary>
        /// 子项目proto协议全参数替代构造函数工厂脚本路径
        /// </summary>
        public string ProtoConstructorFactoryPath
        {
            get
            {
                return CsProtobufDir + ProtoConstructorFactoryName + ".cs";
            }
        }

        public string ProtoBridgePath
        {
            get
            {
                return CsProtobufDir + ProtoBridgeName + ".cs";
            }
        }

        public string ProtoConstructorFactoryName
        {
            get
            {
                return CompexName + "_ProtoConstructorFactory";
            }
        }

        public string ProtoBridgeName
        {
            get
            {
                return CompexName + "_ProtoBridge";
            }
        }

        /// <summary>
        /// 子项目proto协议编号解析器脚本名
        /// </summary>
        public string ProtoIdResolverName
        {
            get
            {
                return CompexName + "_ProtoIdResolver";
            }
        }

        /// <summary>
        /// 子项目proto协议编号解析器脚本路径
        /// </summary>
        public string ProtoIdResolverPath
        {
            get
            {
                return CsProtobufDir + ProtoIdResolverName + ".cs";
            }
        }

        /// <summary>
        /// 子项目Proto协议编号解析器类型名
        /// </summary>
        public string ProtoIdResolverTypeName
        {
            get
            {
                return CompexName + "_ProtoIdResolver";
            }
        }

        #endregion\

        #region 沙盒

        /// <summary>
        /// 子项目的沙盒根目录
        /// </summary>
        public string SandboxDir
        {
            get
            {
                return Application.persistentDataPath +
                       string.Format("/_{0}/{1}/", ParentName, CompexName);
            }
        }

        #endregion

        #region AssetBundle

        #region 文件File操作

        private string LocalHttpRootDir
        {
            get
            {
                return string.Format("{0}/2_LocalHostHttp/_{1}/{2}/", Application.dataPath, ParentName, CompexName);
            }
        }


        public string AssetBundleMainDir
        {
            get
            {
                return string.Format("{0}AssetBundle/{1}_Main/", LocalHttpRootDir, CompexName);
            }
        }

        private AssetBundleManifest m_Manifest;

        public AssetBundleManifest Manifest
        {
            get
            {
                if (m_Manifest != null) return m_Manifest;

                var path = string.Empty;

                if (Application.platform == RuntimePlatform.LinuxEditor ||
                    Application.platform == RuntimePlatform.OSXEditor
                    || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    path = AssetBundleMainDir + string.Format("{0}_Main", CompexName);
                }

                if (Application.platform == RuntimePlatform.Android
                   || Bootstrap.Instance.IsAssetBundleLoad
                   || Application.platform == RuntimePlatform.IPhonePlayer
                   || Application.platform == RuntimePlatform.OSXPlayer
                   || Application.platform == RuntimePlatform.LinuxPlayer
                   || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    path = SandboxDir + string.Format("/AssetBundle/{0}_Main/{1}_Main", CompexName, CompexName);
                }

                if (!File.Exists(path)) throw new Exception(string.Format("子项目{0}的主AssetBundle描述文件不存在，请检查", CompexName));

                m_Manifest = AssetBundle.LoadFromFile(path).LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                return m_Manifest;
            }
        }

        public string AssetBundleManifestHttpPath
        {
            get
            {
                return ProjectBaseConfig.Instance.HotUpdateHttpServerUrl + AssetBundleManiFestReactivePath;
            }
        }

        private string AssetBundleManiFestReactivePath
        {
            get
            {
                return string.Format("_{0}/{1}/AssetBundle/{2}_Main/{3}_Main",
                    ParentName, CompexName, CompexName, CompexName);
            }
        }

        public string AssetBundleManifestSandboxPath
        {
            get
            {
                return U3dConstants.SandboxDir + AssetBundleManiFestReactivePath;
            }
        }

        public string AssetBundleVersionLocalHttpPath
        {
            get
            {
                return LocalHttpRootDir + AssetBundleVersionLatestName;
            }
        }

        private string AssetBundleVersionLatestName
        {
            get
            {
                return string.Format("{0}_AssetBundleVersionInfo_Latest.json", CompexName);
            }
        }


        /// <summary>
        /// Http服务器上对应子项目的AssetBundle版本文件的最新版本的Url地址。
        /// </summary>
        public string AssetBundleVersionHttpLatestUrl
        {
            get
            {
                return ProjectBaseConfig.Instance.HotUpdateHttpServerUrl + AssetBundleVersionLatestName;
            }
        }


        public string AssetBundleVersionSandboxPath
        {
            get
            {
                return AssetBundleSanboxDir +
                       string.Format("{0}_AssetBundleVersionInfo.json", CompexName);
            }
        }

        public string AssetBundleVersionSandboxTempPath
        {
            get
            {
                return AssetBundleSanboxDir +
                       string.Format("{0}_TempAssetBundleVersionInfo.json",
                           CompexName);
            }
        }

        private string AssetBundleSanboxDir
        {
            get
            {
                return Application.persistentDataPath +
                       string.Format("/_{0}/{1}/AssetBundle/", ParentName, CompexName);
            }
        }

        public string AssetBundleLocalHttpDir
        {
            get
            {
                return string.Format("{0}_{1}/{2}/AssetBundle/", U3dConstants.LocalHttpDir,
                    ParentName, CompexName);
            }
        }

        #endregion

        #endregion

        #region View

        /// <summary>
        /// 视图替换资源配置表
        /// </summary>
        public string ViewReaplaceId
        {
            get
            {
                return ("ld_" + CompexName + "_ViewReplace").ToLower();
            }
        }

        public string ViewActionReplaceDataPath
        {
            get
            {
                return BinaryDataDir + ViewActionReplaceDataName + ".bytes";
            }
        }


        public string ViewActionReplaceDataName
        {
            get
            {
                return (CompexName + "_ViewActionReplace").ToLower();
            }
        }


        #endregion

        #region 材质

        public string MaterialDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Material/", CompexName);
            }
        }


        #endregion

        #region 数据Id和事件Id

        public string DataIdTxtPath
        {
            get { return RootDir + string.Format("{0}_DataId.txt", CompexName); }
        }

        public string EventIdTxtPath
        {
            get { return RootDir + string.Format("{0}_EventId.txt", CompexName); }
        }

        public string DataIdCsScriptPath
        {
            get { return CSahrpDir + string.Format("{0}_DataId.cs", CompexName); }
        }

        public string DataIdTsScriptPath
        {
            get { return TsProjectDir + string.Format("{0}_dataid.ts", CompexName.ToLower()); }
        }

        public string EventIdCsScriptPath
        {
            get { return CSahrpDir + string.Format("{0}_EventId.cs", CompexName); }
        }

        public string EventIdTsScriptPath
        {
            get { return TsProjectDir + string.Format("{0}_eventid.ts", EventTypeName.ToLower()); }
        }

        #endregion

        public string ShaderDir
        {
            get
            {
                return AssetDataBaseDir + string.Format("Ab_Single_{0}_Shader/", CompexName);
            }
        }


    }
}