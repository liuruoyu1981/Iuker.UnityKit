/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 07:37:19
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
using System.Linq;
using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    /// <summary>
    /// 资源数据
    /// </summary>
    [Serializable]
#if DEBUG
    [ClassPurposeDesc(null, "资源数据，用于支持动态解析一个资源的各种依赖路径信息")]
#endif
    public class AssetInfo : IAssetTypeStr
    {
        /// <summary>
        /// 资源分类字符串
        /// </summary>
        public string AssetTypeStr { get; private set; }

        /// <summary>
        /// AssetBundle包名
        /// </summary>
        public string BundleName;

        /// <summary>
        /// 是否独立资源
        /// </summary>
        private bool mIsSingle;

        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetName { get; private set; }

        /// <summary>
        /// 资源加载相对路径
        /// 1. 相对于AssetDatabase
        /// 2. 相对于Rersources
        /// </summary>
        public string RelativePath { get; private set; }

        /// <summary>
        /// 资源属于哪个子项目
        /// </summary>
        public string MapSon { get; private set; }

        public AssetInfo Init(SonProject son, string fn, string sourcePath, string importerParent,
           string mapSon)
        {
            MapSon = mapSon;
            AssetName = fn;
            var subAssetDatabaseStr = sourcePath.Replace(son.AssetDataBaseDir, "");
            var firstTypeDirStr = subAssetDatabaseStr.Split('/').First();
            var sonTypeLoadStr = subAssetDatabaseStr.Replace(firstTypeDirStr, "");
            sonTypeLoadStr = sonTypeLoadStr.Substring(0, sonTypeLoadStr.LastIndexOf('.'));
            if (firstTypeDirStr.StartsWith("Ab_Single")) mIsSingle = true;
            if (firstTypeDirStr.StartsWith("Ab_Bundled")) mIsSingle = false;
            AssetTypeStr = firstTypeDirStr.Split('_').Last();
            BundleName = mIsSingle ? AssetName.ToLower() : AssetTypeStr.ToLower();
            RelativePath = sourcePath.Replace(son.AssetDataBaseDir, "").Split('.').First();
            Suffix = "." + sourcePath.Replace(son.AssetDataBaseDir, "").Split('.').Last();
            ImporterPath = importerParent + "/" + RelativePath + Suffix;
            ImporterPath = ImporterPath.Replace("Resources", "AssetDatabase");

            BundleRelativePath = mIsSingle ? string.Format("_{0}/{1}/AssetBundle/{2}{3}.assetbundle", son.ParentName,
                    son.CompexName, AssetTypeStr, sonTypeLoadStr)
                : string.Format("_{0}/{1}/AssetBundle/{2}.assetbundle", son.ParentName, son.CompexName, AssetTypeStr);

            return this;
        }

        /// <summary>
        /// AssetBundle加载全路径
        /// </summary>
        public string AssetBundlePath
        {
            get
            {
                string path;

#if UNITY_EDITOR
                if (Application.isPlaying && Bootstrap.Instance.IsAssetBundleLoad)
                {
                    path = Application.persistentDataPath + "/" + BundleRelativePath;
                }
                else
                {
                    path = U3dConstants.LocalHttpDir + BundleRelativePath;
                }
#else
                path = Application.persistentDataPath + "/" + BundleRelativePath;
#endif

                return path;
            }
        }

        /// <summary>
        /// AssetBundle相对路径，动态计算获取完整路径
        /// </summary>
        private string BundleRelativePath;

        /// <summary>
        /// 资源的导入路径
        /// </summary>
        public string ImporterPath { get; private set; }

        /// <summary>
        /// AssetBundle包临时路径
        /// </summary>
        /// <returns></returns>
        public string GetAbTempPath(SonProject son)
        {
            return son.AssetBundleMainDir + BundleName + ".assetbundle";
        }

        /// <summary>
        /// 资源后缀名
        /// </summary>
        public string Suffix { get; private set; }

    }
}
