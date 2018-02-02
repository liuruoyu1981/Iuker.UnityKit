/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/01 13:23
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

// ReSharper disable FieldCanBeMadeReadOnly.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.HotUpdate
{
    /// <summary>
    /// AssetBundle资源版本数据
    /// 用于资源热更新做版本比对
    /// </summary>
    [Serializable]
    public class AssetBundleVersionInfo
    {
        public string CreateDate;

        // ReSharper disable once MemberCanBePrivate.Global
        public List<AssetBundleInfo> AssetBundleInfos = new List<AssetBundleInfo>();

        private void UpdateVersionFile(Dictionary<string, string> assetDictionary, string writePath)
        {
            CreateDate = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();

            foreach (var keyValuePair in assetDictionary)
            {
                var name = keyValuePair.Key;
                var path = keyValuePair.Value;
                var md5 = Md5Uitlity.GetFileMd5(path);
                var reactivePath = path.Replace(string.Format("{0}", U3dConstants.LocalHttpDir), "");

                var exist = AssetBundleInfos.Find(info => info.BundleName == name);
                if (exist != null)
                    throw new Exception(string.Format("发现重复的资源{0}！", name));

                var bundleInfo = new AssetBundleInfo(name, md5, reactivePath);
                AssetBundleInfos.Add(bundleInfo);
            }

            var content = JsonUtility.ToJson(this);
            File.WriteAllText(writePath, content);
        }

        public static void CreateAssetBundleVersionInfo(SonProject son)
        {
            var bundlePaths = FileUtility
                .GetFilePathDictionary(son.AssetBundleLocalHttpDir,
                    s => !s.Contains(".meta") && s.EndsWith(".assetbundle")).FilePathDictionary;
            var versionInfo = new AssetBundleVersionInfo();
            versionInfo.UpdateVersionFile(bundlePaths, son.AssetBundleVersionLocalHttpPath);
        }

        /// <summary>
        /// 检测一个资源是否需要更新
        /// 1. 不存在
        /// 2. md5变化
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool IsRequireUpdate(AssetBundleInfo info)
        {
            var isExist = AssetBundleInfos.Find(i => i.BundleName == info.BundleName);
            if (isExist == null) return true;
            return isExist.Md5 != info.Md5;
        }

        /// <summary>
        /// 获得需要更新的资源列表
        /// </summary>
        /// <param name="left">最新资源数据</param>
        /// <param name="right">本地资源数据</param>
        /// <returns></returns>
        public static List<AssetBundleInfo> GetRequireUpdateInfos(AssetBundleVersionInfo left, AssetBundleVersionInfo right)
        {
            return left.AssetBundleInfos.Where(right.IsRequireUpdate).ToList();
        }

        /// <summary>
        /// 获得需要更新的资源列表
        /// </summary>
        /// <param name="left">最新资源数据路径</param>
        /// <param name="right">本地资源数据路径</param>
        /// <returns></returns>
        public static List<AssetBundleInfo> GetRequireUpdateInfos(string left, string right)
        {
            return GetRequireUpdateInfos(GetInstance(left), GetInstance(right));
        }

        private static AssetBundleVersionInfo GetInstance(string path)
        {
            return XmlUitlity.LoadFromXml<AssetBundleVersionInfo>(path);
        }
    }
}