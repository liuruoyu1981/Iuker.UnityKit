using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    public class TextAssetRef : AbsAssetRef<TextAsset>
    {
        public override void Destroy()
        {
            
        }

        public TextAssetRef(TextAsset asset, List<AssetBundleRef> bundleRefs = null) : base(asset, bundleRefs)
        {
        }
    }
}