using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common.Utility;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base.Config.Develop
{
    /// <summary>
    /// 当前根配置
    /// </summary>
    [Serializable]
    public class RootConfig
    {
        public string CurrentProjectName = "Default";

        /// <summary>
        /// 尝试更新当前项目
        /// </summary>
        public void TryUpdateCurrentProject(string target)
        {
            var projects = Instance.AllProjects;
            var targetProject = projects.Find(p => p.ProjectName == target);
            if (targetProject == null)
            {
                throw new Exception(string.Format("当前项目列表中不存在所指定的目标项目{0}！", target));
            }

            //  备份项目的ProjectSettings目录
            ProjectSettingsBacker.Back();
            ProjectSettingsBacker.Back(targetProject);

            foreach (var project in projects)
            {
                if (project.ProjectName == target)
                {
                    project.CreateCurrentTagTxt();
                }
                else
                {
                    project.DeleteCurrentTagTxt();
                }
            }

            CurrentProjectName = target;
#if UNITY_EDITOR || DEBUG
            Debug.Log(string.Format("当前项目已修改为{0}!", CurrentProjectName));
#endif
        }

        /// <summary>
        /// 当前所有项目
        /// </summary>
        public List<Project> AllProjects = new List<Project>();

        /// <summary>
        /// 单例
        /// </summary>
        private static RootConfig mInstance;

        public static string SandboxPathJson { get { return Application.persistentDataPath + "/RootConfig.json"; } }

        /// <summary>
        /// unity应用的根配置保存目录
        /// </summary>
        private static string ApplicationConfigDir
        {
            get
            {
                return Application.dataPath.Replace("Assets", "") +
                       "/.ApplicationConfig/";
            }
        }

        private static string ApplicationConfigPathJson
        {
            get
            {
                return ApplicationConfigDir + "RootConfig.json";
            }
        }

        /// <summary>
        /// 根配置在根Resources目录下的全路径，用于打包前拷贝和打包后删除
        /// </summary>
        public static string RootResourcesPath
        {
            get
            {
                return Application.dataPath + "/Resources/RootConfig.xml";
            }
        }

        /// <summary>
        /// 更新项目根配置
        /// 重写当前项目的根配置文件
        /// </summary>
        public static void Update()
        {
            FileUtility.WriteAllText(SandboxPathJson, JsonUtility.ToJson(mInstance));
            FileUtility.WriteAllText(ApplicationConfigPathJson, JsonUtility.ToJson(mInstance));
        }

        public static void SetIntanceNull()
        {
            mInstance = null;
        }

        public static RootConfig Instance
        {
            get
            {
                if (mInstance == null)
                {
#if !UNITY_EDITOR
                    if (!File.Exists(SandboxPathJson))
                    {
                        var resFile = Resources.Load<TextAsset>("RootConfig");
                        if (resFile == null)
                            throw new Exception("Resources下没有发现根配置文件！");

                        File.WriteAllText(SandboxPathJson, resFile.text);
                    }
#else
                    InEditor();
#endif
                    mInstance = JsonUtility.FromJson<RootConfig>(File.ReadAllText(SandboxPathJson));
                }

                return mInstance;
            }
        }

        private static void InEditor()
        {
            if (File.Exists(SandboxPathJson)) return;

            // 开发者单独checkout了子git或者子svn仓库，此时需解析当前unity中有多少个符合命名规则的项目目录并创建根配置实例
            if (!File.Exists(ApplicationConfigPathJson))    //  unity根目录下不存在
            {
                DynamicParse();
                Debug.Log("动态解析并创建了新的根配置，请先检查根配置！");
            }
            else
            {
                RestoeToSandboxJson();
                Debug.Log("检测到开发机器更换，根配置已还原。请检查！");
            }
        }

        public static void RestoeToSandboxJson()
        {
            File.WriteAllText(SandboxPathJson, File.ReadAllText(ApplicationConfigPathJson));
        }

        public static void DynamicParse()
        {
            var sourceDirs = Directory.GetDirectories(Application.dataPath + "/").ToList();
            var vailDirs = sourceDirs.Where(d => d.Replace(Application.dataPath + "/", "").StartsWith("_")).ToList();
            var parents = sourceDirs.Select(d => d.Replace(Application.dataPath + "/", "")).Where(d => d.StartsWith("_"))
                .Select(d => d.Substring(1, d.Length - 1)).ToList();

            var parentList = new List<string>();
            var sonDirDictionary = new Dictionary<string, List<string>>();

            var forCount = vailDirs.Count;
            for (var i = 0; i < forCount; i++)
            {
                var okDir = vailDirs[i];
                var parent = parents[i];
                var projectTagTxt = Application.dataPath + string.Format("/_{0}/{1}.txt", parent, ProjectTagStr);
                if (!File.Exists(projectTagTxt)) continue;

                parentList.Add(parent);
                var sonList = Directory.GetDirectories(okDir).ToList();
                var targetDirs = sonList.Select(d => d.Split('\\').Last()).ToList();
                targetDirs = targetDirs.Where(d => d.StartsWith(parent)).ToList();
                sonDirDictionary.Add(parent, targetDirs);
            }

            mInstance = new RootConfig();
            foreach (var parent in parentList)
            {
                var project = new Project { ProjectName = parent };
                var sonList = sonDirDictionary[parent];
                foreach (var son in sonList)
                {
                    var sonProject = new SonProject
                    {
                        ParentName = parent,
                        ProjectName = son.Split('_').Last()
                    };
                    project.AllSonProjects.Add(sonProject);
                }

                mInstance.AllProjects.Add(project);
            }

            // 修正当前项目
            DynamicFixedCurrentProject(mInstance);
            Update();
        }

        private static void DynamicFixedCurrentProject(RootConfig rootConfig)
        {
            if (rootConfig.AllProjects.Count <= 0) return;

            var firstProject = rootConfig.AllProjects.First();
            rootConfig.CurrentProjectName = firstProject.ProjectName;
            rootConfig.AllProjects.ForEach(p => p.FindCurrentSonProject());
        }

        /// <summary>
        /// 检查目标项目在根配置中是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsProjectExist(string name)
        {
            var resutl = Instance.AllProjects.Find(p => p.ProjectName == name);
            return resutl != null;
        }

        /// <summary>
        /// 获得当前项目
        /// </summary>
        /// <returns></returns>
        public static Project GetCurrentProject()
        {
            if (string.IsNullOrEmpty(Instance.CurrentProjectName))
            {
                throw new Exception("当前项目为空，请检查根配置！");
            }
            var result = Instance.AllProjects.Find(p => p.ProjectName == Instance.CurrentProjectName);
            return result;
        }

        private List<SonProject> mSons;

        public List<SonProject> AllProjectsSons
        {
            get
            {
                if (mSons != null) return mSons;

                mSons = new List<SonProject>();
                foreach (var p in AllProjects)
                {
                    mSons.AddRange(p.AllSonProjects);
                }

                return mSons;
            }
        }

        /// <summary>
        /// 获得当前的子项目
        /// </summary>
        /// <returns></returns>
        public static SonProject GetCurrentSonProject()
        {
            var currentSonProjectName = GetCurrentProject().GetCurrentSonProject().ProjectName;
            var sonProject = GetCurrentProject().AllSonProjects.Find(son => son.ProjectName == currentSonProjectName);
            return sonProject;
        }

        #region 原RootConfig

        /// <summary>
        /// 当前项目根目录以_前缀的复合字符串
        /// </summary>
        /// <returns></returns>
        public static string ProjectNameSpace
        {
            get
            {
                return GetCurrentProject() == null ? null : GetCurrentProject().ProjectName;
            }
        }

        public static void TryCreateThisIsProjectTxt()
        {
            if (File.Exists(ThisIsProjectTextPath)) return;

            FileUtility.TryDeleteFile(GetCurrentProject().RootDir);
            // 在项目的根目录下创建一个隐藏的用于标识该目录是一个合法的基于Iukerr.UnityKit框架的项目目录的文本文档
            File.WriteAllText(ThisIsProjectTextPath, "");
            File.SetAttributes(ThisIsProjectTextPath, FileAttributes.Hidden);
        }

        /// <summary>
        /// 用于表示当前目录为合法项目目录的txt文档
        /// </summary>
        private static string ThisIsProjectTextPath
        {
            get
            {
                return GetCurrentProject().RootDir + ProjectTagStr + ".txt";
            }
        }

        private const string ProjectTagStr = ".This is Iuker.UnityKit Project";

        #endregion

        public static ClientCoder GetSonClientCoder()
        {
            var currentProject = GetCurrentProject();
            if (currentProject == null) return null;
            if (currentProject.AllSonProjects == null || currentProject.AllSonProjects.Count == 0)
            {
                return null;
            }

            return currentProject.GetCurrentSonProject().CurrentClientCoder;
        }

        /// <summary>
        /// 当前项目的所有子项目
        /// </summary>
        public static List<SonProject> CurrentProjectSons
        {
            get
            {
                return GetCurrentProject() == null ? null : GetCurrentProject().AllSonProjects;
            }
        }

        public static string DefaultLauncherSceneNewPath
        {
            get
            {
                return GetCurrentProject().RootDir +
                       string.Format("{0}_Common/{1}_Bootstrap.unity",
                           GetCurrentProject().ProjectName,
                           GetCurrentProject().ProjectName);
            }
        }

        /// <summary>
        /// 当前项目名
        /// </summary>
        public static string CrtProjectName
        {
            get { return GetCurrentProject().ProjectName; }
        }


        #region BuildPlayerDir

        public static string BuildWindows64Dir
        {
            get
            {
                return UnityEngine.Application.dataPath.Replace("Assets", "") +
                       string.Format("/Build/{0}/Windows/", CrtProjectName);
            }
        }

        public static string BuildOSX64Dir
        {
            get
            {
                return UnityEngine.Application.dataPath.Replace("Assets", "") +
                       string.Format("/Build/{0}/OSX64/", CrtProjectName);
            }
        }

        public static string BuildAndriodDir
        {
            get
            {
                return UnityEngine.Application.dataPath.Replace("Assets", "") +
                       string.Format("/Build/{0}/Android/", CrtProjectName);
            }
        }

        public static string BuildIOSDir
        {
            get
            {
                return UnityEngine.Application.dataPath.Replace("Assets", "") +
                       string.Format("Build/{0}/", CrtProjectName);
            }
        }

        public static string ApkInfoPath
        {
            get
            {
                string path = null;
#if UNITY_EDITOR
                path = string.Format("{0}{1}/{2}FullInfos.json", U3dConstants.LocalHttpDir, CrtProjectName,
                    CrtProjectName);
#else
                path = string.Format("{0}{1}/{2}FullInfos.json", ProjectBaseConfig.Instance.HotUpdateHttpServerUrl,
                    CrtProjectName, CrtProjectName);
#endif

                return path;
            }
        }

        /// <summary>
        /// 安卓Apk文件输出路径
        /// </summary>
        public static string ApkPath
        {
            get
            {
                return BuildAndriodDir + string.Format("/{0}.apk", GetCurrentProject().ProjectName);
            }
        }

        /// <summary>
        /// 安卓Apk文件位于本地Http服务器下所在目录
        /// </summary>
        public static string ApkLocalHttpDir
        {
            get
            {
                return U3dConstants.LocalHttpDir + string.Format("/{0}", CrtProjectName);
            }
        }




        #endregion



    }
}
