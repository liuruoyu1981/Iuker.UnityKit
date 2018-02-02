using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common.Base;
using Iuker.Common.Base.Enums;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Context;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Iuker.UnityKit.Run.Module.Asset
{
#if DEBUG
    /// <summary>
    /// 资源模块，各种类型的资源加载及缓存、清理。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170906 11:20:42")]
    [ClassPurposeDesc("资源模块，各种类型的资源加载及缓存、清理。", "资源模块，各种类型的资源加载及缓存、清理。")]
#endif
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DefaultU3dModule_Asset : AbsU3dModule, IU3dAssetModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Asset.ToString();
            }
        }

        #region 字段

        /// <summary>
        /// 资源数据字典
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, AssetInfo>> mAssetInfoDictionary = new Dictionary<string, Dictionary<string, AssetInfo>>();

        private readonly Dictionary<string, string> mAssetTypeDictionary = new Dictionary<string, string>();

        /// <summary>
        /// 精灵数据字典
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, SpriteInfo>> mSpriteInfoDictionary =
            new Dictionary<string, Dictionary<string, SpriteInfo>>();

        private readonly Dictionary<string, string> mSpriteTypeDictionary = new Dictionary<string, string>();

        /// <summary>
        /// AssetBundle依赖缓存字典
        /// </summary>
        private readonly Dictionary<string, AssetBundle> mDependeAssetBundleDictionary = new Dictionary<string, AssetBundle>();

        private bool mIsAssetBundleLoad;

        #endregion

        #region 基类方法

        protected override void OnHotUpdateComplete()
        {
            base.OnHotUpdateComplete();

            //  非编辑器下只有Ab加载一种方式
            mIsAssetBundleLoad = Bootstrap.Instance.IsAssetBundleLoad || U3DFrame.AppContext.DevelopContextType == DevelopContextType.Device;

            InitAssetInfoBinaryDictionary();
            InitSpriteInfoBinaryDictionary();
            Bootstrap.Instance.TotalSons.ForEach(son => mSonDictionary.Add(son.CompexName, son));
            U3DFrame.TryEnterLoginView();
        }

        #endregion

        #region 资源描述数据初始化及获取接口

        private void InitAssetInfoBinaryDictionary()
        {
            foreach (var project in Bootstrap.Instance.GetCombinProject())
            {
                var sons = project.AllSonProjects;
                sons.ForEach(son =>
                {
                    var infos = LoadAssetInfo<AssetInfo>(son.AssetInfoFileName, son);
                    if (infos != null) AddAssetInfoByType(mAssetInfoDictionary, mAssetTypeDictionary, infos);
                });
            }
        }

        private void InitSpriteInfoBinaryDictionary()
        {
            foreach (var project in Bootstrap.Instance.GetCombinProject())
            {
                var sons = project.AllSonProjects;
                sons.ForEach(son =>
                {
                    var infos = LoadAssetInfo<SpriteInfo>(son.SpriteInfoFileName, son);
                    if (infos != null) AddAssetInfoByType(mSpriteInfoDictionary, mSpriteTypeDictionary, infos);
                });
            }
        }

        private static void AddAssetInfoByType<T>(IDictionary<string, Dictionary<string, T>> typeDics,
              IDictionary<string, string> typeMap, IDictionary<string, T> dic) where T : IAssetTypeStr
        {
            foreach (var info in dic.Values)
            {
                if (!typeDics.ContainsKey(info.AssetTypeStr))
                {
                    typeDics.Add(info.AssetTypeStr, new Dictionary<string, T>());
                }

                if (typeMap.ContainsKey(info.AssetName))
                {
                    Debug.LogWarning(string.Format("发现重复的资源名{0}！", info.AssetName));
                    continue;
                }

                typeMap.Add(info.AssetName, info.AssetTypeStr);
                var typeDic = typeDics[info.AssetTypeStr];
                typeDic.Add(info.AssetName, info);
            }
        }

        public SpriteInfo GetSpriteInfo(string spriteName)
        {
            if (!mSpriteTypeDictionary.ContainsKey(spriteName))
            {
                Debug.Log(string.Format("目标精灵{0}没有图集数据，请检查！", spriteName));
                return null;
            }

            var atlasType = mSpriteTypeDictionary[spriteName];
            var spInfoDic = mSpriteInfoDictionary[atlasType];
            var info = spInfoDic[spriteName];
            return info;
        }

        public AssetInfo GetAssetInfo(string assetName)
        {
            if (!mAssetTypeDictionary.ContainsKey(assetName))
            {
                Debug.Log(string.Format("目标资源{0}没有资源数据，请检查！", assetName));
                return null;
            }

            var typeStr = mAssetTypeDictionary[assetName];
            var typeInfoDic = mAssetInfoDictionary[typeStr];
            var info = typeInfoDic[assetName];
            return info;
        }

        #endregion

        #region 顶级入口

        private T InternalSyncLoad<T>(string assetName) where T : Object
        {
            var assetInfo = GetAssetInfo(assetName);
#if UNITY_EDITOR
            if (assetInfo == null)
            {
                Debug.LogWarning(string.Format("目标资源{0}没有找到资源数据！", assetName));
            }
#endif

            if (assetInfo == null) return default(T);

            var asset = mIsAssetBundleLoad ? AssetBundleSyncLoad<T>(assetInfo) : AssetDataBaseSyncLoad<T>(assetInfo);
            mCurBundleRefs.Clear();
            return asset;
        }

        private T InternalSyncLoad<T>(AssetInfo assetInfo) where T : Object
        {
            var asset = mIsAssetBundleLoad ? AssetBundleSyncLoad<T>(assetInfo) : AssetDataBaseSyncLoad<T>(assetInfo);
            mCurBundleRefs.Clear();
            return asset;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        private void AsyncLoad<T>(string assetName, Action<T> callback) where T : Object
        {
            var assetInfo = GetAssetInfo(assetName);

            if (!mAssetInfoDictionary.ContainsKey(assetName))
            {
                Debuger.Log(string.Format("目标资源{0}不存在资源数据！", assetName));
                return;
            }
        }

        public void AsyncLoad<T>(string assetName, Action<AbsAssetRef<T>> callback) where T : Object
        {
            //            var existLoader = AssetLoaderDictionary<T>.TryGetAssetLoader(assetName);
            //            if (existLoader != null)
            //            {
            //                callback?.Invoke(existLoader);
            //                return;
            //            }

            //            var loader = AssetLoaderPool<T>.TakeAssetLoader();
            //#if UNITY_EDITOR
            //            TimeUtility.CalculateMethodCostTime(() =>
            //            {
            //                AsyncLoad<T>(assetName, asset =>
            //                {
            //                    loader.Init(asset, assetName);
            //                    AssetLoaderDictionary<T>.CacheAssetLoader(loader);
            //                    callback?.Invoke(loader);
            //                });
            //            }, time =>
            //            {
            //                loader.SetCostTime(time);
            //            });
            //#else
            //            AsyncLoad<T>(assetName, asset =>
            //            {
            //                loader.Init(asset, assetName);
            //                AssetLoaderDictionary<T>.CacheAssetLoader(loader);
            //                callback?.Invoke(loader);
            //            });
            //#endif
        }

        public List<T> SyncLoadAllAsset<T>(string assetName) where T : Object
        {
            var assetInfo = GetAssetInfo(assetName);

#if UNITY_EDITOR
            return AssetDataBaseSyncLoadAll<T>(assetInfo);
#else
            return AssetBundleSyncLoadAllAsset<T>(assetInfo);
#endif
        }

        #endregion

        #region Resources加载

        #region 同步

        private T ResourcesSyncLoad<T>(AssetInfo assetInfo)
            where T : Object
        {
            return Resources.Load<T>(assetInfo.RelativePath);
        }

        private List<T> ResourceSyncLoadAllAsset<T>(AssetInfo assetInfo) where T : Object
        {
            return Resources.LoadAll<T>(assetInfo.RelativePath).ToList();
        }

        #endregion

        #region 异步

        private IEnumerator ResourcesAsyncLoad<T>(AssetInfo assetInfo, Action<T> callback) where T : Object
        {
            var resourceRequest = Resources.LoadAsync<T>(assetInfo.RelativePath);

            yield return resourceRequest;

            if (resourceRequest == null)
            {
                throw new Exception(string.Format("目标资源{0}加载失败，Resources相对路径为{1}", assetInfo.AssetName,
                    assetInfo.RelativePath));
            }

            var asset = resourceRequest.asset as T;
            callback(asset);
        }

        #endregion


        #endregion

        #region AssetDataBase加载

        #region 同步

        private T AssetDataBaseSyncLoad<T>(AssetInfo assetInfo) where T : Object
        {
            return AssetDatabaseLoader.LoadAssetAtPath<T>(assetInfo.ImporterPath);
        }

        private List<T> AssetDataBaseSyncLoadAll<T>(AssetInfo assetInfo) where T : Object
        {
            return AssetDatabaseLoader.LoadAllAssetsAtPath<T>(assetInfo.ImporterPath);
        }

        #endregion

        #region 异步

        #endregion

        #endregion

        #region AssetBundle加载

        private readonly Dictionary<string, SonProject> mSonDictionary = new Dictionary<string, SonProject>();

        //private void GetAllSonProject()
        //{
        //    foreach (var s in Bootstrap.GetInstance.CombinProjects)
        //    {
        //        var Pn = RootConfig.GetInstance.AllProjects.Find(p => p.ProjectName == s);
        //        var sons = Pn.AllSonProjects;
        //        foreach (var son in sons)
        //        {
        //            mSonProjects.Add(son.CompexName, son);
        //        }
        //    }
        //}

        /// <summary>
        /// AssetBundle同步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetInfo"></param>
        /// <returns></returns>
        private T AssetBundleSyncLoad<T>(AssetInfo assetInfo) where T : Object
        {
            var assetName = assetInfo.AssetName;
            var son = mSonDictionary[assetInfo.MapSon];

            if (son.Manifest == null)
                throw new Exception("AssetBundle主描述文件为空，请确认是否进行了项目的AsstBundle打包及更新操作。");

            var dependes = son.Manifest.GetAllDependencies(assetName + ".assetbundle");
            //var tempDependeAssetBundles = new List<AssetBundle>();

            if (dependes.Length > 0)
            {
                foreach (var depende in dependes)
                {
                    var dependeAssetName = depende.Replace(".assetbundle", "");

                    if (!mDependeAssetBundleDictionary.ContainsKey(dependeAssetName))
                    {    // 如果依赖资源当前未缓存则加载
                        var tempInfo = GetAssetInfo(dependeAssetName);
                        var tempAssetBundle = AssetBundle.LoadFromFile(tempInfo.AssetBundlePath);
                        mDependeAssetBundleDictionary.Add(dependeAssetName, tempAssetBundle);
                        //tempDependeAssetBundles.Add(tempAssetBundle);
                    }
                }
            }

            var assetBundle = AssetBundle.LoadFromFile(assetInfo.AssetBundlePath);
            if (assetBundle == null)
            {
                Debuger.LogError(string.Format("目标资源{0}的AssetBundle加载失败!", assetName));
                return null;
            }

            var asset = assetBundle.LoadAsset<T>(assetName);

            assetBundle.Unload(false);
            //tempDependeAssetBundles.ForEach(bundle => bundle.Unload(false));
            return asset;
        }

        /// <summary>
        /// 使用AssetBundle同步加载所有资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        private List<T> AssetBundleSyncLoadAllAsset<T>(AssetInfo assetInfo) where T : Object
        {
            var assetBundle = mDependeAssetBundleDictionary.ContainsKey(assetInfo.AssetName) ?
                mDependeAssetBundleDictionary[assetInfo.AssetName] :
                AssetBundle.LoadFromFile(assetInfo.AssetBundlePath);

            if (assetBundle == null)
            {
                Debuger.LogException(string.Format("目标资源{0}的AssetBundle加载失败!", assetInfo.AssetName));
                return null;
            }

            var assets = assetBundle.LoadAllAssets<T>().ToList();
            assetBundle.Unload(false);
            return assets;
        }

        private T AssetBundleAsyncLoad<T>(AssetInfo assetInfo, Action<T> callback) where T : Object
        {
            return default(T);
        }

        #endregion

        #region 动态加载

        public Texture2D LoadTexture2D(string p, int width, int heigh)
        {
            var fs = new FileStream(p, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            fs.Close();
            fs.Dispose();

            var texture2 = new Texture2D(width, heigh);
            texture2.LoadImage(buffer);
            return texture2;
        }



        #endregion

        private readonly List<AssetBundleRef> mCurBundleRefs = new List<AssetBundleRef>();

        public Dictionary<AssetInfo, T> LoadType<T>(string typeStr) where T : Object
        {
            if (!mAssetInfoDictionary.ContainsKey(typeStr)) return null;

            var assets = new Dictionary<AssetInfo, T>();

            var infos = mAssetInfoDictionary[typeStr];
            foreach (var pair in infos)
            {
                var asset = InternalSyncLoad<T>(pair.Value);
                assets.Add(pair.Value, asset);
            }

            return assets;
        }

        #region 显式加载

        public ViewRef LoadView(string name)
        {
            var asset = InternalSyncLoad<GameObject>(name);
            if (asset == null) return null;

            var viewref = new ViewRef(asset, mCurBundleRefs);
            return viewref;
        }

        public MusicRef LoadMusic(string name)
        {
            var asset = InternalSyncLoad<AudioClip>(name);
            if (asset == null) return null;

            var musicRef = new MusicRef(asset, mCurBundleRefs);
            return musicRef;
        }

        public TextAssetRef LoadTextAsset(string name)
        {
            var asset = InternalSyncLoad<TextAsset>(name);
            if (asset == null) return null;

            var textRef = new TextAssetRef(asset, mCurBundleRefs);
            return textRef;
        }

        public SoundRef LoadSound(string name)
        {
            var asset = InternalSyncLoad<AudioClip>(name);
            if (asset == null) return null;

            var soundRef = new SoundRef(asset, mCurBundleRefs);
            return soundRef;
        }

        public PrefabRef LoadPrefab(string name)
        {
            var asset = InternalSyncLoad<GameObject>(name);
            if (asset == null) return null;

            var prefabRef = new PrefabRef(asset, mCurBundleRefs);
            return prefabRef;
        }

        public Texture2dRef LoadTexture2D(string name)
        {
            var asset = InternalSyncLoad<Texture2D>(name);
            if (asset == null) return null;

            var texRef = new Texture2dRef(asset, mCurBundleRefs);
            return texRef;
        }

        #endregion

        private Dictionary<string, T> LoadAssetInfo<T>(string assetName, SonProject son)
        {
            TextAsset textAsset = null;

            if (mIsAssetBundleLoad)
            {
                var sandBoxPath = Application.persistentDataPath + string.Format(
                                      "/_{0}/{1}/AssetBundle/BinaryData/{2}.assetbundle", son.ParentName,
                                      son.CompexName, assetName);
                if (File.Exists(sandBoxPath)) return LoadSandbox<T>(assetName, sandBoxPath);

                textAsset = Resources.Load<TextAsset>(assetName);
            }
            else
            {
                textAsset = Resources.Load<TextAsset>(assetName);
            }

            if (textAsset == null)
            {
#if UNITY_EDITOR
                Debug.Log(string.Format("资源或图集数据{0}加载失败！", assetName));
#endif
                return null;
            }

            var infos = SerializeUitlity.DeSerialize<Dictionary<string, T>>(textAsset.bytes);
            return infos;
        }

        private Dictionary<string, T> LoadSandbox<T>(string assetName, string path)
        {
            var ab = AssetBundle.LoadFromFile(path);
            var textAsset = ab.LoadAsset<TextAsset>(assetName);
            var infos = SerializeUitlity.DeSerialize<Dictionary<string, T>>(textAsset.bytes);
            return infos;
        }




    }
}
