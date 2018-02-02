using System;
using System.Linq;
using Iuker.Common;
using Iuker.UnityKit.Run.Module.LocalData;

namespace Iuker.Jint
{
    public class U3dJintModule_LocalData : DefaultU3dModule_LocalData
    {
        public string[] GetEntityStrLines(string assetName)
        {
            var textRef = mAssetModule.LoadTextAsset(assetName);
            var content = textRef.Asset.text;
            var lines = content.Split(Environment.NewLine.ToCharArray()).ToList().RemoveAllLineFeed().ToArray();
            return lines;
        }





    }
}
