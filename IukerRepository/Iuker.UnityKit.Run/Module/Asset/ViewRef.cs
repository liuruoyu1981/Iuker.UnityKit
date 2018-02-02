using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    /// <summary>
    /// 视图预制件引用
    /// </summary>
    public class ViewRef : AbsAssetRef<GameObject>
    {
        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public ViewRef(GameObject asset, List<AssetBundleRef> bundleRefs = null) : base(asset, bundleRefs)
        {
        }
    }
}