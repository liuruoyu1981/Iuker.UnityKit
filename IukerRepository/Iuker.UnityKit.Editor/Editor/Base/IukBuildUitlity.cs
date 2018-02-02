using System;
using System.Collections.Generic;
using System.IO;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.Assets;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.HotUpdate;
using Iuker.UnityKit.Run.Module.View.Assist;
using UnityEditor;

#if UNITY_IOS

using Iuker.UnityKit.Editor.Setting;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

#endif

using UnityEngine;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Iuker.UnityKit.Editor.Build
{
    /// <summary>
    /// 打包工具
    /// </summary>
    public static class IukBuildUitlity
    {
        private static bool sRootResourcesExist;


        [MenuItem("Iuker/平台打包及项目导出/Windows32", false, 6)]
        public static void BuildWindows32()
        {
            BuildWindows(BuildTarget.StandaloneWindows);
        }

        [MenuItem("Iuker/平台打包及项目导出/Windows64", false, 7)]
        public static void BuildWindows64()
        {
            BuildWindows(BuildTarget.StandaloneWindows64);
        }

        private static void BuildWindows(BuildTarget buildTarget)
        {
            if (EditorUserBuildSettings.activeBuildTarget != buildTarget)
            {
                EditorUtility.DisplayDialog("", "目标平台和当前应用平台不一致，请先转换应用平台！", "确定");
                return;
            }

            //  打包前处理
            BeforBuild();
            string buildDir = RootConfig.BuildWindows64Dir;
            FileUtility.TryCreateDirectory(buildDir);
            var launcherScenePath = "Assets" + RootConfig.DefaultLauncherSceneNewPath.Replace(Application.dataPath, "");
            string[] levels = { launcherScenePath };
            BuildPipeline.BuildPlayer(levels, buildDir + string.Format("/{0}.exe",
                                                  RootConfig.GetCurrentProject().ProjectName), buildTarget, BuildOptions.None);
            //  打包后处理
            AfterBuild();
            var runFilePath = buildDir + string.Format("{0}.exe", RootConfig.GetCurrentProject().ProjectName);
            FileUtility.OpenFolderAndSelectFile(runFilePath);
        }

        [MenuItem("Iuker/平台打包及项目导出/Android/输出APK", false, 4)]
        private static void BuildApk()
        {
            BuildAndroid();
        }

        [MenuItem("Iuker/平台打包及项目导出/Android/输出APK并自动安装", false, 5)]
        private static void BuildApkAutoInstall()
        {
            BuildAndroid(AndroidBuildType.BuildApkAndInstall);
        }

        [MenuItem("Iuker/平台打包及项目导出/Android/输出调试版本APK并自动安装", false, 6)]
        private static void BuildDebugApkAndInstall()
        {
            BuildAndroid(AndroidBuildType.BuildApkAndInstallAndDebug);
        }

        private enum AndroidBuildType
        {
            BuildApk,
            BuildApkAndInstall,
            BuildApkAndInstallAndDebug,
        }

        private static void BuildAndroid(AndroidBuildType type = AndroidBuildType.BuildApk)
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                EditorUtility.DisplayDialog("", "目标平台和当前应用平台不一致，请先转换应用平台！", "确定");
                return;
            }

            //  输出新的APK版本信息文件
            UpdateFullApkFile();
            //  打包前处理
            BeforBuild();
            FileUtility.TryCreateDirectory(RootConfig.BuildAndriodDir);
            var launcherScenePath = "Assets" + RootConfig.DefaultLauncherSceneNewPath.Replace(Application.dataPath, "");
            string[] levels = { launcherScenePath };

            switch (type)
            {
                case AndroidBuildType.BuildApk:
                    BuildPipeline.BuildPlayer(levels, RootConfig.ApkPath, BuildTarget.Android, BuildOptions.None);
                    break;
                case AndroidBuildType.BuildApkAndInstall:
                    BuildPipeline.BuildPlayer(levels, RootConfig.ApkPath, BuildTarget.Android, BuildOptions.None | BuildOptions.AutoRunPlayer);
                    break;
                case AndroidBuildType.BuildApkAndInstallAndDebug:
                    BuildPipeline.BuildPlayer(levels, RootConfig.ApkPath, BuildTarget.Android, BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler | BuildOptions.AutoRunPlayer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }

            //  打包后处理
            AfterBuild();
            //  拷贝到本地Http服务器目标项目下
            FileUtility.CopyFile(RootConfig.ApkPath, RootConfig.ApkLocalHttpDir);
            Debug.Log("Apk打包完成");
            AssetDatabase.Refresh();
        }

        private static void UpdateFullApkFile()
        {
            FullApkInfo newInfo;

            if (File.Exists(RootConfig.ApkInfoPath))
            {
                var oldContentg = File.ReadAllText(RootConfig.ApkInfoPath);
                var fullApkInfos = JsonUtility.FromJson<FullApkInfos>(oldContentg);
                newInfo = fullApkInfos.CreateNew();
                File.Delete(RootConfig.ApkInfoPath);
                File.WriteAllText(RootConfig.ApkInfoPath, JsonUtility.ToJson(fullApkInfos));
            }
            else
            {
                var full = new FullApkInfos();
                newInfo = full.CreateNew();

                FileUtility.WriteAllText(RootConfig.ApkInfoPath, JsonUtility.ToJson(full));
            }

            PlayerSettings.bundleVersion = newInfo.VersionId.ToString();
        }

        private static string _buildDir;

        [MenuItem("Iuker/平台打包及项目导出/导出Xcode", false, 6)]
        private static void BuildIos()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                EditorUtility.DisplayDialog("", "目标平台和当前应用平台不一致，请先转换应用平台！", "确定");
                return;
            }

            FileUtility.EnsureDirExist(RootConfig.BuildIOSDir);
            _buildDir = EditorUtility.OpenFolderPanel("选择输出文件夹", RootConfig.BuildIOSDir, "");
            _buildDir += string.Format("/{0}Xcode/", RootConfig.GetCurrentProject().ProjectName);

            //  打包前处理
            BeforBuild();
            FileUtility.TryCreateDirectory(RootConfig.BuildIOSDir);
            var launcherScenePath = "Assets" + RootConfig.DefaultLauncherSceneNewPath.Replace(Application.dataPath, "");
            string[] levels = { launcherScenePath };
            BuildPipeline.BuildPlayer(levels, _buildDir, BuildTarget.iOS, BuildOptions.None);
            //  打包后处理
            AfterBuild();
            CopyXcode();
            FileUtility.OpenFolderAndSelectFile(_buildDir);
        }

        /// <summary>
        /// 清理打包工具状态
        /// </summary>
        private static void CleanStatus()
        {
            sRootResourcesExist = false;
        }

        /// <summary>
        /// 打包之前
        /// </summary>
        [MenuItem("Assets/Iuker/打包前处理")]
        private static void BeforBuild()
        {
            sRootResourcesExist = false;
            CopyRootConfigFile();
            UpdateAllViewConfig();
            CopyAllPlugins();
            TryDeleteAllSonProtobuf_netDll();
            DeleteTs();
            AssetDatabase.Refresh();
        }

        private static void DeleteTs()
        {
            FileUtility.DeleteDirectory(string.Format("{0}/1_Iuker.UnityKit/IukerRepository/Iuker.UnityKit.Ts/bin/",
                Application.dataPath));
            FileUtility.DeleteDirectory(string.Format("{0}/1_Iuker.UnityKit/IukerRepository/Iuker.UnityKit.Ts/obj/",
                Application.dataPath));

            RootConfig.Instance.AllProjects.ForEach(DeleteSonTs);
        }

        private static void DeleteSonTs(Project p)
        {
            var sons = p.AllSonProjects;
            foreach (var son in sons)
            {
                FileUtility.DeleteDirectory(son.TsBinDir);
                FileUtility.DeleteDirectory(son.TsObjDir);
            }
        }

#if UNITY_IOS

        private const string _iosBuildVersionId = "mIosBuildVersionId";

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.Log("Ios PostProcessBuild!");
            Debug.Log("Ios Xcode 导出后回调处理!");

            if (target != BuildTarget.iOS) return;

            //每一次编译成功将xcode编译版本号+1
            var versionId = IukerEditorPrefs.GetInt(_iosBuildVersionId, 0);
            versionId++;
            IukerEditorPrefs.SetInt(_iosBuildVersionId, versionId);
            PlayerSettings.iOS.buildNumber = versionId.ToString();

            //XCode项目路径
            var projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            // 获取当前项目名targetName
            var projectTarget = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

        #region 更改BuildSetting配置

            // 对所有的编译配置设置选项  
            //关闭BitCode
            proj.SetBuildProperty(projectTarget, "ENABLE_BITCODE", "NO");

            // 设置签名  
            proj.SetBuildProperty(projectTarget, "DEVELOPMENT_TEAM", "jin long ning");

            //对所有的编译配置添加选项
            proj.AddBuildProperty(projectTarget, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)/Libraries/Plugins/iOS");
            proj.AddBuildProperty(projectTarget, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)/Libraries/Plugins/iOS/thirdlibs");
            proj.AddBuildProperty(projectTarget, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)/Libraries/Plugins/iOS/openSDK1.7.3");

        #endregion

        #region 添加依赖库

            // 添加依赖库(false表示是必须的Required,true表示是可选的Optional)  
            proj.AddFrameworkToProject(projectTarget, "JavaScriptCore.framework", true);
            proj.AddFrameworkToProject(projectTarget, "StoreKit.framework", false);
            proj.AddFrameworkToProject(projectTarget, "CoreTelephony.framework", false);
            proj.AddFrameworkToProject(projectTarget, "MobileCoreServices.framework", false);
            proj.AddFrameworkToProject(projectTarget, "Security.framework", false);
            proj.AddFrameworkToProject(projectTarget, "SystemConfiguration.framework", false);
            proj.AddFrameworkToProject(projectTarget, "libz.1.tbd", false);
            proj.AddFrameworkToProject(projectTarget, "libc++.1.tbd", false);
            proj.AddFrameworkToProject(projectTarget, "libstdc++.6.0.9.tbd", false);
            proj.AddFrameworkToProject(projectTarget, "libsqlite3.0.tbd", false);
            proj.AddFrameworkToProject(projectTarget, "libiconv.2.tbd", false);

        #endregion


        #region 修改Plist文件

            // 修改plist  
            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            var rootDict = plist.root;

            //需要的权限声明 
            rootDict.SetString("NSLocationWhenInUseUsageDescription", "是否允许此App使用你的地理位置？");
            rootDict.SetString("NSCameraUsageDescription", "是否允许此App使用你的相机？");
            rootDict.SetString("NSMicrophoneUsageDescription", "是否允许此游戏使用麦克风？");
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
            var arr = rootDict.CreateArray("CFBundleURLTypes");
            var dic = arr.AddDict();
            dic.SetString("CFBundleTypeRole", "Editor");
            dic.SetString("CFBundleURLName", ProjectBaseConfig.GetInstance.WxBundleId);
            dic.CreateArray("CFBundleURLSchemes").AddString(ProjectBaseConfig.GetInstance.WxAppId);

            var array = rootDict.CreateArray("LSApplicationQueriesSchemes");
            array.AddString("weixin");
            array.AddString("cydia");
            array.AddString("xxassistant");
            array.AddString("xxassistantsdk");
            array.AddString("weixin");
            array.AddString("wechat");

        #endregion

            //修改代码
            //EditorCode(projPath);

            // 保存plist  
            plist.WriteToFile(plistPath);

            // 保存工程  
            proj.WriteToFile(projPath);
        }

        /// <summary>
        /// 向xcode工程添加一个Lib
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="targetGuid"></param>
        /// <param name="lib"></param>
        private static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
        {
            var fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }

#endif


        /// <summary>
        /// 打包之后
        /// </summary>
        private static void AfterBuild()
        {
            DeleteResources();
            DeletePlugins();
            CleanStatus();
        }

        private static void CopyXcode()
        {
            Debug.LogError("拷贝xcode需求文件");
            var xcodeDir = RootConfig.GetCurrentProject().RootDir + ".Xcode/";
            if (!Directory.Exists(xcodeDir)) return;
            var filePaths = FileUtility.GetFilePaths(xcodeDir, s => !s.Contains(".meta")).FilePaths;
            var xcodeClassesDir = _buildDir + "Classes/";
            FileUtility.EnsureDirExist(xcodeClassesDir);
            foreach (var path in filePaths)
            {
                FileUtility.CopyFile(path, xcodeClassesDir);
            }
        }

        private static void DeleteResources()
        {
            if (sRootResourcesExist)
            {
                File.Delete(RootConfig.RootResourcesPath);
            }
            else
            {
                FileUtility.DeleteDirectory(U3dConstants.RootResourcesDir);
            }
        }

        private static void CopyAllPlugins()
        {
            var sourceDirs = new List<string>();

            var allSonProjects = RootConfig.GetCurrentProject().AllSonProjects;
            foreach (var sonProject in allSonProjects)
            {
                if (Directory.Exists(sonProject.PluginsDir))
                {
                    sourceDirs.Add(sonProject.PluginsDir);
                }
            }

            sourceDirs.Add(sIukerPluginsDir);
            foreach (var sourceDir in sourceDirs)
            {
                if (Directory.Exists(sourceDir))
                {
                    CopyPlugins(sourceDir);
                }
            }
        }

        /// <summary>
        /// 拷贝框架下的原生插件
        /// </summary>
        private static void CopyPlugins(string sourceDir)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            string sourcePlatformDir;
            string targetPlatformDir;

            FileUtility.TryCreateDirectory(sPluginsDir);
            switch (buildTarget)
            {
                case BuildTarget.iOS:
                    sourcePlatformDir = sourceDir + "iOS/";
                    targetPlatformDir = sPluginsDir + "iOS/";
                    break;
                case BuildTarget.Android:
                    sourcePlatformDir = sourceDir + "Android/";
                    targetPlatformDir = sPluginsDir + "Android/";
                    break;
                case BuildTarget.StandaloneWindows64:
                    sourcePlatformDir = sourceDir + "x86_64/";
                    targetPlatformDir = sPluginsDir + "x86_64/";
                    break;
                case BuildTarget.StandaloneWindows:
                    sourcePlatformDir = sourceDir + "x86/";
                    targetPlatformDir = sPluginsDir + "x86/";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            FileUtility.CopyDirectory(sourcePlatformDir, targetPlatformDir);
        }

        private static void DeletePlugins()
        {
            FileUtility.DeleteDirectory(sPluginsDir);
        }

        private static string sPluginsDir
        {
            get { return Application.dataPath + "/Plugins/"; }
        }

        private static string sIukerPluginsDir { get { return Application.dataPath + "/1_Iuker.UnityKit/.Plugins/"; } }

        private static void TryDeleteAllSonProtobuf_netDll()
        {
            var allProject = RootConfig.Instance.AllProjects;
            foreach (var project in allProject)
            {
                foreach (var sonProject in project.AllSonProjects)
                {
                    var path = string.Format("{0}/_{1}/{2}_{3}/CSharp/ProtobufScripts/protobuf-net.dll",
                        Application.dataPath, project.ProjectName, project.ProjectName, sonProject.ProjectName);
                    FileUtility.TryDeleteFile(path);
                }
            }
            AssetDatabase.Refresh();
        }

        private static void CopyRootConfigFile()
        {
            RootConfig.RestoeToSandboxJson();
            File.WriteAllText(RootConfig.GetCurrentProject().RootCinfigResourcesPath, File.ReadAllText(RootConfig.SandboxPathJson));
        }

        /// <summary>
        /// 读取所有视图预制件上的视图辅助器组件
        /// 更新所有视图辅助器组件的设置到子项目的视图配置文件
        /// </summary>
        [MenuItem("Iuker/快捷菜单/合并视图配置")]
        private static void UpdateAllViewConfig()
        {
            Views.CreateNewVIewsConfig();

            var currentProject = RootConfig.GetCurrentProject();
            var allSonProjects = currentProject.AllSonProjects;
            foreach (var son in allSonProjects)
            {
                var sonViewPrefabsDir = son.ViewPrefabDir;
                if (!Directory.Exists(sonViewPrefabsDir))   //  如果某个子项目的目录不存在，则说明该子项目已经被删除了，忽略。
                {
                    continue;
                }
                var sonViewPaths = FileUtility.GetFilePaths(sonViewPrefabsDir, s => !s.Contains("meta")).FilePaths;

                foreach (var viewPath in sonViewPaths)
                {
                    try
                    {
                        var prefab = IukAssetDataBase.LoadAssetAtPath<GameObject>(viewPath);
                        var view = Object.Instantiate(prefab);
                        var viewAssister = view.GetComponent<ViewAssister>();
                        viewAssister.UpdateToViewConfig(son);
                        Object.DestroyImmediate(view);
                    }
                    catch (Exception exception)
                    {
                        Debug.Log(exception.Message);
                        Debug.LogError(string.Format("在处理路径为{0}的视图预制件时发生了异常，请检查！", viewPath));
                    }
                }

            }
        }

    }
}