using System;
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Iuker.UnityKit.Run.Module.HotUpdate
{
    [Serializable]
    public class AssetBundleInfo
    {
        public string BundleName;
        public string Md5;
        public string ReactivePath;

        public AssetBundleInfo() { }
        public AssetBundleInfo(string name, string md5, string reactivePath)
        {
            BundleName = name;
            Md5 = md5;
            ReactivePath = reactivePath;
        }
    }
}