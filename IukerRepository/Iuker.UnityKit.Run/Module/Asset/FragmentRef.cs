using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    public class FragmentRef : AbsAssetRef<GameObject>
    {
        public FragmentRef(GameObject asset, List<AssetBundleRef> bundleRefs = null) : base(asset, bundleRefs)
        {
        }

        public override void Destroy()
        {

        }
    }
}
