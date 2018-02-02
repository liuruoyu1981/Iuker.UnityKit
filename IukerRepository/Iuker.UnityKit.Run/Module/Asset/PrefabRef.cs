using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    public class PrefabRef : AbsAssetRef<GameObject>
    {
        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public PrefabRef(GameObject asset, List<AssetBundleRef> bundleRefs = null) : base(asset, bundleRefs)
        {
        }
    }
}