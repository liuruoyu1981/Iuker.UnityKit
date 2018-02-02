/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/14 23:24:31
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
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.AtlasBuild;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Assets
{
    /// <summary>
    /// 图集打包工具
    /// </summary>
    public static class AtlasBuildUtil
    {
        public static void BuildAtlas_Resources()
        {
            BuildAtlas("Resources/");
        }

        public static void BuildAtlas_AssetDataBases()
        {
            BuildAtlas("AssetDatabase/");
        }

        private static string sFirstPath;
        private static string sAtlasLocation;

        /// <summary>
        /// 尝试获得图集输出路径列表
        /// </summary>
        private static void BuildAtlas(string atlasLocation)
        {
            sAtlasLocation = atlasLocation;
            var paths = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).Where(AssetDatabase.IsValidFolder).ToList();
            if (paths.Count == 1)
            {
                sFirstPath = paths.First();
                if (sFirstPath.Contains(" "))
                {
                    EditorUtility.DisplayDialog("", "所选择的目录含有空格！", "确定");
                    return;
                }

                var lastDir = sFirstPath.Split('/').Last();
                if (!lastDir.StartsWith("Atlas"))
                {
                    EditorUtility.DisplayDialog("", "所选择的目录不是一个符合图集存放规则的合法目录！", "确定");
                    return;
                }

                Extensions.IfElse(lastDir == "AtlasOrigin", OnAtlasOrigin, SelectOne);
            }
        }

        private static void OnAtlasOrigin()
        {
            Debug.Log("选择了图集散图根目录！");
            var dirArray = sFirstPath.Split('/');
            var projectName = dirArray[1].Substring(1, dirArray[1].Length - 1);
            var sonProjectName = dirArray[2];
            var atlasOutPutDir = string.Format("{0}/_{1}/{2}/{3}Ab_Single_{4}_Atlas/", Application.dataPath,
                projectName, sonProjectName, sAtlasLocation, sonProjectName);

            var fullPath = Application.dataPath + sFirstPath.Replace("Assets", "");
            var atlasDirs = FileUtility.GetAllDir(fullPath).Dirs;
            List<string> whereDirs = new List<string>();
            foreach (var atlasDir in atlasDirs)
            {
                if (atlasDir.Contains(" "))
                {
                    EditorUtility.DisplayDialog("", "所选择的目录含有空格！", "确定");
                    return;
                }
                if (atlasDir.StartsWith("Atlas_"))
                {
                    EditorUtility.DisplayDialog("", "所选择的目录不符合图集散图存放目录命名规则，必须以Atlas_开头！", "确定");
                    return;
                }
                if (atlasDir.EndsWith("AtlasOrigin")) continue;
                whereDirs.Add(atlasDir);
            }
            foreach (var dir in whereDirs)
            {
                TexturePacker.Create(dir, atlasOutPutDir).BuildAtlas();
            }

            AssetDatabase.Refresh();
        }


        private static void SelectOne()
        {
            Debug.Log("选择了图集散图分类子目录！");
            var dirArray = sFirstPath.Split('/');
            var projectName = dirArray[1].Substring(1, dirArray[1].Length - 1);
            var sonProjectName = dirArray[2];
            var atlasOutPutDir = string.Format("{0}/_{1}/{2}/{3}Ab_Single_{4}_Atlas/", Application.dataPath,
                projectName, sonProjectName, sAtlasLocation, sonProjectName);

            var atlasDir = sFirstPath;
            if (atlasDir.Contains(" "))
            {
                EditorUtility.DisplayDialog("", "所选择的目录含有空格！", "确定");
                return;
            }
            if (atlasDir.StartsWith("Atlas_"))
            {
                EditorUtility.DisplayDialog("", "所选择的目录不符合图集散图存放目录命名规则，必须以Atlas_开头！", "确定");
                return;
            }
            var fullPath = Application.dataPath + sFirstPath.Replace("Assets", "") + "/";
            var files = FileUtility.GetFilePathDictionary(fullPath, s => !s.Contains("meta")).FilePathDictionary.Keys
                .ToList();
            if (files.Find(s => CheckIsOk(s) == false) != null)
            {
                return;
            }

            TexturePacker.Create(fullPath, atlasOutPutDir).BuildAtlas();    // 输出图集
            AssetDatabase.Refresh();
        }


        private static bool CheckIsOk(string name)
        {
            if (name.Contains(" ") || name.Contains("-"))
            {
                Debug.Log(string.Format("发现不合法的资源{0}！", name));
                return false;
            }

            return true;
        }

    }
}