/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/7/24 11:32:24
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

using Iuker.Common.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace Iuker.UnityKit.Editor.Assets
{
    /// <summary>
    /// 预制件工具
    /// </summary>
    public class IukPrafabUtility
    {
        public static GameObject CreatePrefab(string path, GameObject go,
            [DefaultValue("ReplacePrefabOptions.Default")] ReplacePrefabOptions options)
        {
            GameObject prefab;

            //  包含冒号则说明是操作系统全路径需要转为unity项目相对路径
            if (path.Contains(":"))
            {
                var appDataPath = Application.dataPath;
                var tempPath = path.Replace(appDataPath, "");
                var finalPath = "Assets" + tempPath;
                FileUtility.EnsureDirExist(finalPath);
                prefab = PrefabUtility.CreatePrefab(finalPath, go);
                return prefab;
            }

            prefab = PrefabUtility.CreatePrefab(path, go);
            return prefab;
        }
    }
}