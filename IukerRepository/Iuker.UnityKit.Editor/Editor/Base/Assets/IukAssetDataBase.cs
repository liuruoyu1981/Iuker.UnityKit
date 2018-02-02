/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/7/24 10:37:27
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

using System.Collections.Generic;
using System.Linq;
using Iuker.UnityKit.Run;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Assets
{
    /// <summary>
    /// unity资源数据库工具
    /// 1. 解析各种加载路径
    /// </summary>
    public static class IukAssetDataBase
    {
        /// <summary>
        /// 在编辑器下加载一个资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static T LoadAssetAtPath<T>(string assetPath) where T : UnityEngine.Object
        {
            T asset;

            //  包含冒号则说明是操作系统全路径需要转为unity项目相对路径
            if (assetPath.Contains(":"))
            {
                var appDataPath = Application.dataPath;
                var tempPath = assetPath.Replace(appDataPath, "");
                var finalPath = "Assets" + tempPath;
                asset = AssetDatabase.LoadAssetAtPath<T>(finalPath);
                return asset;
            }

            asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
        }

        public static Object[] LoadAllAssetsAtPath(string assetPath)
        {
            Object[] assets = null;

            if (assetPath.Contains(":"))
            {
                var path = assetPath.AssetsPath();
                assets = AssetDatabase.LoadAllAssetsAtPath(path);
            }

            return assets;
        }

        public static List<Sprite> LoadAllSprites(string assetPath)
        {
            var assets = LoadAllAssetsAtPath(assetPath);
            return assets.OfType<Sprite>().ToList();
        }

        public static Texture GetTexture(string fileName, string folderPath)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture>(folderPath + fileName + ".png");
            return texture;
        }




    }
}