using System;
using Iuker.Common;

namespace Iuker.UnityKit.Run.Module.HotUpdate
{
    /// <summary>
    /// 热更新模块
    /// </summary>
    public interface IU3dHotUpdateModule : IModule
    {
        /// <summary>
        /// AssetBundle更新
        /// </summary>
        void Update(Action<string, float> progressAction);

        /// <summary>
        /// 安卓APK整包更新
        /// </summary>
        void ApkUpdate();




    }
}