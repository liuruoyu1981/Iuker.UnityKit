using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common.Base;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Base.Assets
{
#if DEBUG
    /// <summary>
    /// AssetBundle打包器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170914 09:15:46")]
    [ClassPurposeDesc("AssetBundle打包器", "AssetBundle打包器")]
#endif
    public class AssetBundleBuilder
    {
        public static bool IsBuilding { get; private set; }

        private SonProject mCurrentSon;

        public void BuildSon(SonProject son)
        {
            mCurrentSon = son;

            if (!File.Exists(son.AssetInfoPath))
            {
                Debug.Log(string.Format("子项目{0}当前没有资源数据存在，该子项目将不进行AssetBundle打包！", mCurrentSon.ProjectName));
                return;
            }

            FileUtility.TryCreateDirectory(mCurrentSon.AssetBundleMainDir);
            IsBuilding = true;
            ClearBundleNameNotCurrentSonBundleName();
            AssetDatabase.RemoveUnusedAssetBundleNames();
            var assetInfos = SerializeUitlity.DeSerialize<Dictionary<string, AssetInfo>>(mCurrentSon.AssetInfoPath)
                .Values.ToList();
            assetInfos.ForEach(SetBundleName);
            //  构建AssetBundle文件
            BuildPipeline.BuildAssetBundles(mCurrentSon.AssetBundleMainDir, BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);
            assetInfos.ForEach(info => MoveAssetBundle(info.GetAbTempPath(mCurrentSon), info.AssetBundlePath));
            IsBuilding = false;
            Debug.Log(string.Format("子项目{0}的AssetBundle文件打包完成！", mCurrentSon.ProjectName));
        }

        /// <summary>
        /// 清理非当前子项目的所有其他子项目的AssetBundle包名
        /// </summary>
        private void ClearBundleNameNotCurrentSonBundleName()
        {
            var allProjects = RootConfig.Instance.AllProjects;
            foreach (var project in allProjects)
            {
                if (project.ProjectName != mCurrentSon.ParentName)
                {
                    ClearNotSameProjectNotCurrentSon(project);
                }
                else
                {
                    ClearSameProjectNotCurrentSon(project);
                }
            }
            Debug.Log("非当前项目的AssetBundle名已全部清理！");
        }

        /// <summary>
        /// 清理当前子项目所在项目中其他子项目的AssetBundle包名
        /// </summary>
        /// <param name="project"></param>
        private void ClearSameProjectNotCurrentSon(Project project)
        {
            var sons = project.AllSonProjects;

            foreach (var son in sons)
            {
                if (son.ProjectName == mCurrentSon.ProjectName) continue;

                ClearnBundleName(son);
            }
        }

        /// <summary>
        /// 清理非当前子项目所在项目所有子项目的AssetBundle包名
        /// </summary>
        /// <param name="project"></param>
        private void ClearNotSameProjectNotCurrentSon(Project project)
        {
            var sonProjects = project.AllSonProjects;
            foreach (var son in sonProjects)
            {
                if (!Directory.Exists(son.AssetDataBaseDir)) continue;

                ClearnBundleName(son);
            }
        }

        private void ClearnBundleName(SonProject son)
        {
            var dirs = FileUtility.GetAllDir(son.AssetDataBaseDir, s => s.Contains("Ab")).Dirs;
            foreach (var dir in dirs)
            {
                var targetFiles = FileUtility.GetFilePathDictionary(dir, s => !s.Contains("meta") && !s.Contains("tpsheet")
                        && !s.EndsWith(".gitKeep.txt")).FilePaths;
                targetFiles = targetFiles.Select(s => "Assets" + s.Replace(Application.dataPath, ""))
                    .ToList();
                foreach (var path in targetFiles)
                {
                    var assetImporter = AssetImporter.GetAtPath(path);
                    if (assetImporter == null)
                        throw new Exception(string.Format("目标路径{0}上的资源获取导入句柄对象时出错，请检查！", path));

                    assetImporter.assetBundleName = null;
                }
            }
        }

        private void SetBundleName(AssetInfo assetInfo)
        {
            try
            {
                var importer = AssetImporter.GetAtPath(assetInfo.ImporterPath);
                importer.assetBundleName = assetInfo.BundleName;
                importer.assetBundleVariant = "assetbundle";
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                Debug.LogError(assetInfo.ImporterPath);
            }
        }

        /// <summary>
        /// 当前已移动的资源列表
        /// </summary>
        private readonly List<string> movedAssets = new List<string>();

        /// <summary>
        /// 移动AssetBundle包到约定子目录
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetFilePath"></param>
        private void MoveAssetBundle(string sourceFilePath, string targetFilePath)
        {
            //  源文件不存在则可能已移动
            if (!File.Exists(sourceFilePath))
            {
                return;
            }

            //  目标文件已存在则删除
            if (File.Exists(targetFilePath))
            {
                File.Delete(targetFilePath);
                Debug.Log(string.Format("删除已存在的目标文件_{0}", sourceFilePath));
            }

            //  已经移动过跳出
            if (movedAssets.Find(r => r == sourceFilePath) != null)
            {
                return;
            }

            movedAssets.Add(sourceFilePath);
            FileUtility.Move(sourceFilePath, targetFilePath);
        }
    }
}
