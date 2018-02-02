/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/27 12:40:55
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


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.Asset;
using UnityEngine;
#pragma warning disable 649

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// unity3d项目启动器
    /// </summary>
    public abstract class Bootstrap : MonoBehaviour
    {
        /// <summary>
        /// 框架对象
        /// </summary>
        public static IU3dFrame U3DFrame { get; protected set; }

        /// <summary>
        /// 启动器单例
        /// </summary>
        public static Bootstrap Instance { get; protected set; }

        protected void Start()
        {
            InitConfig();
            StartCoroutine(StartFrame());
        }

        /// <summary>
        /// 初始化启动器配置项
        /// </summary>
        protected virtual void InitConfig()
        {
            if (mCombinProjects.Count == 0)
            {
                mCombinProjects = new List<string> { RootConfig.GetCurrentProject().ProjectName };
            }
        }

        /// <summary>
        /// 启动框架
        /// </summary>
        protected abstract IEnumerator StartFrame();

        #region 启动器配置项

        private Project mEntryProject;
        public Project EntryProject
        {
            get
            {
                if (CombinProjects.Count == 0 || CombinProjects == null)
                {
                    mEntryProject = RootConfig.GetCurrentProject();
                }
                else
                {
                    var entryName = CombinProjects.First();
                    mEntryProject = RootConfig.Instance.AllProjects.Find(p => p.ProjectName == entryName);
                }

                return mEntryProject;
            }
        }

        [SerializeField]
        [Tooltip("在这里配置需要同时运行的多个项目的项目名，如果没有配置则默认为根配置的当前项目。")]
        private List<string> mCombinProjects;

        /// <summary>
        /// 多项目运行列表
        /// </summary>
        public List<string> CombinProjects { get { return mCombinProjects; } }

        [SerializeField]
        [Tooltip("更新视图的资源Id,在这里指定要用于构建视图实例的视图预制件资源名，如果没有指定则默认为view_update。")]
        private string mUpdateViewAssetId;

        /// <summary>
        /// 更新视图的资源Id
        /// 具体项目可以在这里指定要用于构建视图实例的目标视图预制件的资源名。
        /// </summary>
        public string UpdateViewAssetId { get { return mUpdateViewAssetId; } }

        [SerializeField]
        [Tooltip("是否为发布版本")]
        private bool mIsRelease;

        /// <summary>
        /// 是否为发布版本
        /// </summary>
        public bool IsRelease { get { return mIsRelease; } }

        /// <summary>
        /// 是否使用AssetBundle加载
        /// </summary>
        [SerializeField]
        private bool mIsAssetBundleLoad;

        public bool IsAssetBundleLoad { get { return mIsAssetBundleLoad; } }

        /// <summary>
        /// 视图挂载是是否对齐锚点
        /// </summary>
        [SerializeField]
        private bool mAnchorAlignment;

        public bool IsAnchorAlignment { get { return mAnchorAlignment; } }

        [SerializeField]
        private AssetUpdateType mUpdateType;
        public AssetUpdateType UpdateType { get { return mUpdateType; } }

        private List<Project> mCombinPs;

        [SerializeField]
        [Tooltip("需要过滤，不包含在合并启动子项目列表中的目标子项目。")]
        private List<string> mFilterSons;

        /// <summary>
        /// 获得当前实际启动的合并项目列表
        /// </summary>
        /// <returns></returns>
        public List<Project> GetCombinProject()
        {
            return mCombinPs ?? (mCombinPs = CombinProjects.Select(pname => RootConfig.Instance.AllProjects.Find(p => p.ProjectName == pname)).ToList());
        }

        [SerializeField] [Tooltip("更新视图之后需要启动的视图Id，如果没有指定则默认为项目_login")] private string mAfterUpdateViewId;

        public string AfterUpdateViewId
        {
            get
            {
                return mAfterUpdateViewId;
            }
        }

        private List<SonProject> mTotalSons;

        public List<SonProject> TotalSons
        {
            get
            {
                if (mTotalSons != null) return mTotalSons;

                mTotalSons = new List<SonProject>();

                foreach (var project in GetCombinProject())
                {
                    foreach (var son in project.AllSonProjects)
                    {
                        if (mFilterSons == null || !mFilterSons.Exists(s => s == son.CompexName))
                        {
                            mTotalSons.Add(son);
                        }
                    }
                }

                return mTotalSons;
            }
        }


        #endregion




    }
}
