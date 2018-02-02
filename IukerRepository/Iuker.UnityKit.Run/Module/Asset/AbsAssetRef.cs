/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 11/8/2017 6:13:56 PM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/


/*
*/

using System.Collections.Generic;
using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Module.Asset
{
#if DEBUG
    /// <summary>
    /// 资源引用，用以维护一个资源的依赖关系和引用次数。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171108 18:13:01")]
    [ClassPurposeDesc("资源引用，用以维护一个资源的依赖关系和引用次数。", "资源引用，用以维护一个资源的依赖关系和引用次数。")]
#endif
    public abstract class AbsAssetRef<T> where T : UnityEngine.Object
    {
        public T Asset { get; private set; }

        public List<AssetBundleRef> AssetBundleRefs { get; private set; }

        public abstract void Destroy();

        public AbsAssetRef(T asset, List<AssetBundleRef> bundleRefs = null)
        {
            Asset = asset;
            AssetBundleRefs = bundleRefs;
        }


    }
}
