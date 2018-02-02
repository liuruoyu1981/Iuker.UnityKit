using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    public class Texture2dRef : AbsAssetRef<Texture2D>
    {
        public Texture2dRef(Texture2D asset, List<AssetBundleRef> bundleRefs = null) : base(asset, bundleRefs)
        {
        }

        public override void Destroy()
        {

        }
    }
}