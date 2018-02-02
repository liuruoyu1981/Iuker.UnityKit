using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    public class MusicRef : AbsAssetRef<AudioClip>
    {
        public override void Destroy()
        {


        }

        public MusicRef(AudioClip asset, List<AssetBundleRef> bundleRefs = null) : base(asset, bundleRefs)
        {
        }
    }
}