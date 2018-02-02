using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Utility;

namespace Iuker.UnityKit.Run.Base.Config
{
    /// <summary>
    /// 项目配置节
    /// </summary>
    [Serializable]
    public class Project
    {
        /// <summary>
        /// 当前项目的所有子项目
        /// </summary>
        public List<SonProject> AllSonProjects = new List<SonProject>();

        /// <summary>
        /// 当前项目的当前子项目
        /// </summary>
        public string CurrentSonProject;

        /// <summary>
        /// 项目名
        /// </summary>
        public string ProjectName;

        /// <summary>
        /// 获得项目的当前子项目
        /// </summary>
        /// <returns></returns>
        public SonProject GetCurrentSonProject()
        {
            return AllSonProjects.Find(son => son.ProjectName == CurrentSonProject);
        }

        /// <summary>
        /// 初始化一个项目配置
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="newSon"></param>
        /// <returns></returns>
        public Project Init(string projectName, SonProject newSon)
        {
            ProjectName = projectName;
            AllSonProjects.Add(new SonProject().Init("Common", ProjectName));
            AllSonProjects.Add(newSon);
            CurrentSonProject = newSon.ProjectName;

            return this;
        }

        /// <summary>
        /// 使用标识文档查找当前子项目
        /// 如果没有找到标识文档则默认第一个子项目为当前子项目
        /// </summary>
        public void FindCurrentSonProject()
        {
            CurrentSonProject = AllSonProjects.First().ProjectName;
        }

        private static readonly string sCurrentTagStr = ".This is current Project";

        #region Propertys

        /// <summary>
        /// 项目用于标识自身为当前项目的txt文件路径
        /// </summary>
        private string CurrentTagTxtPath
        {
            get
            {
                return UnityEngine.Application.dataPath +
                           string.Format("/_{0}/{1}.txt", ProjectName, sCurrentTagStr);
            }
        }

        public void CreateCurrentTagTxt()
        {
            FileUtility.TryWriteAllText(CurrentTagTxtPath, "");
        }

        public void DeleteCurrentTagTxt()
        {
            FileUtility.TryDeleteFile(CurrentTagTxtPath);
        }

        /// <summary>
        /// 项目首视图id
        /// 默认为view_XX_update
        /// </summary>
        public string UpdateViewId
        {
            get
            {
                return string.Format("view_{0}_update", ProjectName.ToLower());
            }
        }

        public string LoginViewId
        {
            get
            {
                return string.Format("view_{0}_common_login", ProjectName.ToLower());
            }
        }

        /// <summary>
        /// 项目根目录
        /// </summary>
        public string RootDir
        {
            get
            {
                return string.Format("{0}/_{1}/", UnityEngine.Application.dataPath, ProjectName);
            }
        }

        /// <summary>
        /// 项目自身Unity项目设置文件的备份目录
        /// </summary>
        public string ProjectSettingsBakDir
        {
            get
            {
                return RootDir + ".ProjectSettings/";
            }
        }

        public string BaseConfigFullPath
        {
            get
            {
                return RootDir + string.Format("{0}_Common/Resources/{1}_projectbaseconfig.json",
                           ProjectName, ProjectName.ToLower());
            }
        }

        public string RootCinfigResourcesPath
        {
            get
            {
                return RootDir + string.Format("{0}_Common/Resources/RootConfig.json",
                           ProjectName);
            }
        }

        /// <summary>
        /// 安卓整包更新数据文件的沙盒路径
        /// </summary>
        public string ApkInfoSbPath
        {
            get
            {
                return UnityEngine.Application.persistentDataPath +
                       string.Format("/{0}/{1}FullInfos.json", ProjectName, ProjectName);
            }
        }

        /// <summary>
        /// 安卓整包更新数据文件的沙盒临时路径
        /// </summary>
        public string ApkInfosSbTempPath
        {
            get
            {
                return UnityEngine.Application.persistentDataPath +
                       string.Format("/{0}/{1}TempFullInfos.json", ProjectName, ProjectName);
            }
        }

        /// <summary>
        /// 项目完整Apk包的下载路径
        /// </summary>
        public string FullApkDownPath
        {
            get
            {
                return string.Format("{0}{1}/{2}.apk",
                    ProjectBaseConfig.Instance.HotUpdateHttpServerUrl, ProjectName, ProjectName);
            }
        }

        /// <summary>
        /// 项目完整Apk包的沙盒临时路径
        /// </summary>
        public string FullApkSandboxTempPath
        {
            get
            {
                return string.Format("{0}/{1}/{2}.apk",
                    UnityEngine.Application.persistentDataPath, ProjectName, ProjectName);
            }
        }


        #endregion













    }
}