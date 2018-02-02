using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    /// <summary>
    /// 音效资源引用
    /// </summary>
    public class SoundRef : AbsAssetRef<AudioClip>
    {
        public override void Destroy()
        {
        }

        public SoundRef(AudioClip asset, List<AssetBundleRef> bundleRefs = null) : base(asset, bundleRefs)
        {
        }
    }
}