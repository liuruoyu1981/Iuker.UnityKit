using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common;
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
    /// 项目资源数据创建器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170913 22:13:01")]
    [ClassPurposeDesc("项目资源数据创建器", "项目资源数据创建器")]
#endif
    public class AssetInfoCreater
    {
        private bool mIsError;
        private readonly Dictionary<string, List<AssetInfo>> mInfoDictionary = new Dictionary<string, List<AssetInfo>>();

        public void CreateAssetInfo(params SonProject[] sons)
        {
            var parentName = sons[0].ParentName;
            var sonProjects = sons.ToList();
            sonProjects.ForEach(s => MyAssetIdCreater.CreateScript(s));
            sonProjects.ForEach(son => CreateAssetInfo(son, mInfoDictionary));
            mInfoDictionary.ForEach((path, infos) => { FileUtility.CreateBinaryFile(path, SerializeUitlity.Serialize(CreateAssetInfoDictionary(infos))); });
            var commonSonResourcesDir = Application.dataPath +
                                        string.Format("/_{0}/{1}_Common/Resources/", parentName, parentName);
            mInfoDictionary.ForEach((path, infos) => { FileUtility.CopyFile(path, commonSonResourcesDir); });
            if (mIsError)
            {
                Debug.LogError(string.Format("项目{0}的资源数据创建失败！", RootConfig.CrtProjectName));
            }
            else
            {
                Debug.Log(string.Format("项目{0}下所有子项目的资源数据已创建完毕！", RootConfig.CrtProjectName));
            }
            AssetDatabase.Refresh();
        }

        private void CreateAssetInfo(SonProject son, IDictionary<string, List<AssetInfo>> tempInfoDictionary)
        {
            var assetDir = son.AssetDataBaseDir;
            //  目标资源根目录不存在则跳出执行
            if (!Directory.Exists(assetDir))
            {
                Debug.Log(string.Format("资源目录{0}不存在！", assetDir));
                return;
            }

            //            var dirs = FileUtility.GetAllDir(assetDir, s => s.Contains("Ab")).Dirs;
            var dirs = Directory.GetDirectories(assetDir).ToList();
            foreach (var dir in dirs)
            {
                var targetFileDictionary = FileUtility.GetFilePathDictionary(dir, s => !s.Contains("meta") && !s.EndsWith(".gitKeep.txt")
                    && !s.Contains("tpsheet")).FilePathDictionary;
                if (targetFileDictionary.Count == 0) continue;

                //  检测是否有不符合资源命名规范的资源
                if (IsExistIllegalAsset(targetFileDictionary))
                {
                    mIsError = true;
                    return;
                }

                var infos = targetFileDictionary.Select(kv => new AssetInfo().Init(son, kv.Key, kv.Value,
                    son.ImporterDir, son.CompexName)).ToList();
                if (tempInfoDictionary.ContainsKey(son.AssetInfoPath))
                {
                    tempInfoDictionary[son.AssetInfoPath].AddRange(infos);
                }
                else
                {
                    tempInfoDictionary.Add(son.AssetInfoPath, infos);
                }
            }
        }

        /// <summary>
        /// 给定一个文件路径集判断该集合中的所有资源是否都符合资源命名规范
        /// </summary>
        /// <returns></returns>
        private static bool IsExistIllegalAsset(Dictionary<string, string> targetDictionary)
        {
            var fileNames = targetDictionary.Keys.ToList();
            var bigFiles = fileNames.FindAll(s => s.Containcapital());
            bigFiles.ForEach(fn => Debug.Log("发现不合法的资源：" + fn));
            return bigFiles.Count > 0;
        }

        private Dictionary<string, AssetInfo> mTempInfoDictionary;

        /// <summary>
        /// 创建资源信息字典
        /// </summary>
        /// <param name="assetInfos"></param>
        /// <returns></returns>
        private Dictionary<string, AssetInfo> CreateAssetInfoDictionary(IEnumerable<AssetInfo> assetInfos)
        {
            mTempInfoDictionary = new Dictionary<string, AssetInfo>();
            foreach (var assetInfo in assetInfos)
            {
                if (mTempInfoDictionary.ContainsKey(assetInfo.AssetName))
                {
                    if (mTempInfoDictionary[assetInfo.AssetName].Suffix == assetInfo.Suffix)
                    {
                        throw new Exception(string.Format("目标资源{0}已存在！", assetInfo.AssetName));
                    }
                }

                mTempInfoDictionary.Add(assetInfo.AssetName, assetInfo);
            }
            return mTempInfoDictionary;
        }





    }
}
