/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 07:29:49
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;
using System.Collections.Generic;
using Iuker.Common;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.Asset
{
    /// <summary>
    /// 资源模块
    /// </summary>
    public interface IU3dAssetModule : IModule
    {
        /// <summary>
        /// 获得资源描述数据
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        AssetInfo GetAssetInfo(string assetName);

        /// <summary>
        /// 同步加载某种类型的所有资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        List<T> SyncLoadAllAsset<T>(string assetName) where T : UnityEngine.Object;

        /// <summary>
        /// 获得精灵数据
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        SpriteInfo GetSpriteInfo(string spriteName);

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        void AsyncLoad<T>(string assetName, Action<AbsAssetRef<T>> callback) where T : UnityEngine.Object;

        Dictionary<AssetInfo, T> LoadType<T>(string typeStr) where T : UnityEngine.Object;

        #region 动态加载

        /// <summary>
        /// 加载一张贴图
        /// </summary>
        /// <param name="p"></param>
        /// <param name="width"></param>
        /// <param name="heigh"></param>
        /// <returns></returns>
        Texture2D LoadTexture2D(string p, int width, int heigh);



        #endregion

        #region 显式类型加载

        ViewRef LoadView(string name);

        MusicRef LoadMusic(string name);

        TextAssetRef LoadTextAsset(string name);

        SoundRef LoadSound(string name);

        PrefabRef LoadPrefab(string name);

        Texture2dRef LoadTexture2D(string name);

        #endregion


    }
}
