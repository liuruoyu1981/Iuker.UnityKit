using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// AssetDatabase加载器
    /// 反射加载
    /// </summary>
    public class AssetDatabaseLoader
    {
        private readonly MethodInfo _LoadAssetAtPath;
        private readonly MethodInfo _LoadAllAssetsAtPath;
        private static AssetDatabaseLoader mInstance;

        private static AssetDatabaseLoader Instance
        {
            get
            {
                return mInstance ?? (mInstance = new AssetDatabaseLoader());
            }
        }

        private AssetDatabaseLoader()
        {
            var editorAssembly = Assembly.Load("UnityEditor");
            var assetDatabaseType = editorAssembly.GetType("UnityEditor.AssetDatabase");
            var methods = assetDatabaseType.GetMethods();
            _LoadAssetAtPath = methods.ToList().Find(m => m.Name == "LoadAssetAtPath");
            _LoadAllAssetsAtPath = methods.ToList().Find(m => m.Name == "LoadAllAssetsAtPath");
        }

        public static T LoadAssetAtPath<T>(string path) where T : Object
        {
            var args = new object[] { path, typeof(T) };
            var asset = (T)Instance._LoadAssetAtPath.Invoke(null, args);
            return asset;
        }

        public static List<T> LoadAllAssetsAtPath<T>(string path) where T : Object
        {
            var args = new object[] { path };
            var objs = (object[])Instance._LoadAllAssetsAtPath.Invoke(null, args);
            var assets = objs.OfType<T>().ToList();
            return assets;
        }
    }
}