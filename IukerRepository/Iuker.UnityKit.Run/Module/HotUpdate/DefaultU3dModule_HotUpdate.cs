/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/01 13:18
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
using System.Globalization;
using System.IO;
using Iuker.Common.Base.Enums;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.Communication.Http;
using Iuker.UnityKit.Run.Parallel.Core;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Iuker.UnityKit.Run.Module.HotUpdate
{
    /// <summary>
    /// 默认热更新模块
    /// </summary>
    public class DefaultU3dModule_HotUpdate : AbsU3dModule, IU3dHotUpdateModule
    {
        #region 基础字段

        public override string ModuleName
        {
            get
            {
                return ModuleType.HotUpdate.ToString();
            }
        }

        #endregion

        #region 模块重写

        protected override void onFrameInited()
        {
            base.onFrameInited();

            mHttpRootUrl = ProjectBaseConfig.Instance.HotUpdateHttpServerUrl;
        }

        #endregion

        #region 资源更新

        private List<List<AssetBundleInfo>> mDownInfos = new List<List<AssetBundleInfo>>();
        private List<AssetBundleInfo> mRequireDownInfos = new List<AssetBundleInfo>();
        private Dictionary<SonProject, string> mLatestInfos = new Dictionary<SonProject, string>();
        private Action<string, float> mProgressAction;
        private int updateIndex;
        private string mHttpRootUrl;
        private int mDownedIndex;
        private SonProject mSon;

        public void Update(Action<string, float> progressAction)
        {
            if (!Bootstrap.Instance.IsAssetBundleLoad)
            {
                HotUpdateCompleteOrNotDoUpdate();
                return;
            }

            mProgressAction = progressAction;
            DownSonAssetBundleInfo();
        }

        private void DownSonAssetBundleInfo()
        {
            if (updateIndex == Bootstrap.Instance.TotalSons.Count)
            {
                DownRequireUpdateAssetBundle();
                return;
            }

            mSon = Bootstrap.Instance.TotalSons[updateIndex];
            if (!File.Exists(mSon.AssetBundleVersionSandboxTempPath))
            {
                FileUtility.WriteAllText(mSon.AssetBundleVersionSandboxPath, JsonUtility.ToJson(new AssetBundleVersionInfo()));
            }

            var httpUrl = ProjectBaseConfig.Instance.HotUpdateHttpServerUrl + string.Format(
                              "_{0}/{1}/{2}_AssetBundleVersionInfo_Latest.json", mSon.ParentName, mSon.CompexName,
                              mSon.CompexName);
            U3dHttpUitlity.GetFile(httpUrl, mSon.AssetBundleVersionSandboxTempPath, CompareMd5);
        }

        private void CompareMd5()
        {
            updateIndex++;
            var httpMd5 = Md5Uitlity.GetFileMd5(mSon.AssetBundleVersionSandboxTempPath);
            var localMd5 = Md5Uitlity.GetFileMd5(mSon.AssetBundleVersionSandboxPath);
            if (httpMd5 == localMd5)
            {
                DownSonAssetBundleInfo();
            }
            else
            {
                //  下载子项目的AssetBundle描述文件
                U3dHttpUitlity.GetFile(mSon.AssetBundleManifestHttpPath, mSon.AssetBundleManifestSandboxPath, DownDiffrentInfos);
            }
        }

        private void DownDiffrentInfos()
        {
            try
            {
                var httpContent = File.ReadAllText(mSon.AssetBundleVersionSandboxTempPath);
                var httpInfo = JsonUtility.FromJson<AssetBundleVersionInfo>(httpContent);
                var localContent = File.ReadAllText(mSon.AssetBundleVersionSandboxPath);
                var localInfo = JsonUtility.FromJson<AssetBundleVersionInfo>(localContent);
                var compares = AssetBundleVersionInfo.GetRequireUpdateInfos(httpInfo, localInfo);
                mLatestInfos.Add(mSon, httpContent);
                mDownInfos.Add(compares);
            }
            catch (Exception exception)
            {
                Debug.Log(string.Format("解析子项目{0}的AssetBundle版本文件时发生错误，异常信息为{1}！", mSon.CompexName, exception.Message));
            }

            DownSonAssetBundleInfo();
        }

        private void DownRequireUpdateAssetBundle()
        {
            if (mDownInfos.Count == 0)
            {
                HotUpdateCompleteOrNotDoUpdate();
            }
            else
            {
                Debug.Log("UpdateAssetBundle");
                foreach (var infos in mDownInfos)
                {
                    mRequireDownInfos.AddRange(infos);
                }
                Debug.Log(string.Format("总共有{0}个资源需要更新！", mRequireDownInfos.Count));
                U3DFrame.StartCoroutine(DownAssetBundle());
            }
        }

        private void HotUpdateCompleteOrNotDoUpdate()
        {
            U3DFrame.EventModule.IssueEvent(U3dEventCode.App_HotUpdateComplete.Literals);
        }

        private IEnumerator DownAssetBundle()
        {
            if (mDownedIndex == mRequireDownInfos.Count)
            {
                foreach (var mLatestInfo in mLatestInfos)
                {
                    FileUtility.WriteAllText(mLatestInfo.Key.AssetBundleVersionSandboxPath, mLatestInfo.Value);
                    Debug.Log("沙盒下所有子项目的AssetBundle版本文件当前已更新到最新版本！");
                }

                mRequireDownInfos.Clear();
                mRequireDownInfos = null;
                mDownInfos.Clear();
                mDownInfos = null;
                mLatestInfos.Clear();
                mLatestInfos = null;
                HotUpdateCompleteOrNotDoUpdate();
                yield break;
            }

            var info = mRequireDownInfos[mDownedIndex];
            var httpUrl = mHttpRootUrl + info.ReactivePath;
            var localUrl = Application.persistentDataPath + "/" + info.ReactivePath;
            U3dHttpUitlity.GetFile(httpUrl, localUrl, () =>
            {
                mDownedIndex++;
                var progress = (float)mDownedIndex / mRequireDownInfos.Count;
                mProgressAction(null, progress);
                U3DFrame.StartCoroutine(DownAssetBundle());
            });
        }

        #endregion

        #region Apk更新

        public void ApkUpdate()
        {
            var cp = RootConfig.GetCurrentProject();

            if (!File.Exists(cp.ApkInfoSbPath))
            {
                CoroutineHelper
                    .WhenAll(U3dHttpUitlity.GetFileByCoroutine, RootConfig.ApkInfoPath, cp.ApkInfoSbPath, FullDownComplete)
                    .WhenAll(U3dHttpUitlity.GetFileByCoroutine, RootConfig.ApkInfoPath, cp.ApkInfosSbTempPath, FullDownComplete)
                    .Do(CompareFullVersion);
            }
            else
            {
                U3dHttpUitlity.GetFile(RootConfig.ApkInfoPath, cp.ApkInfosSbTempPath, CompareFullVersion);
            }
            Debug.Log("进入HotUpdate");
            if (!File.Exists(ProjectBaseConfig.SandboxPath))
            {
                Debug.Log("进入HotUpdate，没有检测到沙盒下有基础配置文件,下载最新基础配置到沙盒下");
                CoroutineHelper.WhenAll(U3dHttpUitlity.GetFileByCoroutine,
                    "http://client-oss.oss-cn-hangzhou.aliyuncs.com/Jiax/JiaxBaseRuntime.json",
                    ProjectBaseConfig.SandboxPath, null);
            }
            else
            {
                Debug.Log("进入HotUpdate，检测到沙盒下有基础配置文件,下载最新基础配置到沙盒下并替换");
                CoroutineHelper.WhenAll(U3dHttpUitlity.GetFileByCoroutine,
                    "http://client-oss.oss-cn-hangzhou.aliyuncs.com/Jiax/JiaxBaseRuntime.json",
                    ProjectBaseConfig.SandboxPath, null);
            }
        }

        void FullDownComplete()
        {
            Debug.Log("Apk文件下载进度更新");
        }

        /// <summary>
        /// 对比本地和服务器的Apk版本信息文件
        /// </summary>
        private void CompareFullVersion()
        {
            var cp = RootConfig.GetCurrentProject();

            var serverContent = File.ReadAllText(cp.ApkInfosSbTempPath);
            Debug.Log("服务器APK版本文件内容: " + serverContent);

            var tempInfos = JsonUtility.FromJson<FullApkInfos>(serverContent);
            Debug.Log("本地APK版本为" + Application.version);
            Debug.Log("服务器APK版本为" + tempInfos.Last.VersionId);

            //  已经是最新版本则退出
            if (Application.version == tempInfos.Last.VersionId.ToString())
            {
                U3DFrame.ViewModule.CloseView(cp.UpdateViewId);
                U3DFrame.ViewModule.CreateView(RootConfig.GetCurrentProject().LoginViewId);
                return;
            }

            //  更新为最新版本数据
            //  尝试删除之前下载的Apk文件
            FileUtility.TryDeleteFile(cp.FullApkSandboxTempPath);
            //  开始下载新的APK包
            U3dHttpUitlity.GetFile(cp.FullApkDownPath, cp.FullApkSandboxTempPath, InstallFullApk, UpdateApkDownProgress);
        }

        void UpdateApkDownProgress(float p)
        {
            Debug.Log("Apk下载进度更新：" + p);
            U3DFrame.EventModule.IssueEvent(U3dEventCode.Frame_ApkDownUpdate.Literals, null, p.ToString(CultureInfo.InvariantCulture));
        }

        private void InstallFullApk()
        {
            var cp = RootConfig.GetCurrentProject();

            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidCaller.InstallAPK(RootConfig.GetCurrentProject().FullApkSandboxTempPath, true);
            }
            else
            {
                Debug.Log("Apk下载完成，当前非安卓平台，将进入项目Login视图");
                U3DFrame.ViewModule.CloseView(cp.UpdateViewId);
                U3DFrame.ViewModule.CreateView(RootConfig.GetCurrentProject().LoginViewId);
            }
        }


        #endregion


    }
}