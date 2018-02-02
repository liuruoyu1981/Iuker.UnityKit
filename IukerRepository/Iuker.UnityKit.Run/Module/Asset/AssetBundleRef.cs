/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 11/8/2017 4:30:23 PM
Email: liuruoyu1981@gmail.com
***********************************************************************************************/


/*
*/

using Iuker.Common.Base;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
#if DEBUG
    /// <summary>
    /// AssetBundle引用
    /// </summary>
    [CreateDesc("liuruoyu1981","liuruoyu1981@gmail.com","20171108 16:30:04")]
    [ClassPurposeDesc("AssetBundle引用","AssetBundle引用")]
#endif
    public class AssetBundleRef
    {
        public AssetBundle Bundle;
        public int RefCount;
    }
}
