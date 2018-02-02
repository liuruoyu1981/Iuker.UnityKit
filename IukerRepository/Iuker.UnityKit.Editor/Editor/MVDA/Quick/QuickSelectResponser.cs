using System.IO;
using System.Linq;
using Iuker.UnityKit.Run.LinqExtensions;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA.Quick
{
    /// <summary>
    /// 通过扩展层次菜单快速选中视图行为处理器脚本
    /// </summary>
    public static class QuickSelectResponser
    {
        public static void QuickSelect()
        {
            if (Selection.gameObjects.Length > 1)
            {
                EditorUtility.DisplayDialog("错误", "不能同时选中多个游戏对象！", "确定");
                return;
            }

            var selectGo = Selection.gameObjects[0];

            if (selectGo.name == "button_root")
            {
                EditorUtility.DisplayDialog("警告", "按钮button_root为根按钮，无需创建脚本！", "确定");
                return;
            }

            var selectGoName = selectGo.name;
            GameObject parent;
            selectGo.Parent().FindViewRoot(out parent);

            if (selectGoName.EndsWith("cell") || selectGoName.Substring(0, selectGoName.Length - 3).EndsWith("cell"))
            {
                OnSelectButtonCell(parent, selectGo);
                return;
            }
            if (selectGoName.StartsWith("button"))
            {
                OnSelectButton(parent, selectGo);
            }
        }

        private static void OnSelectButton(GameObject parent, GameObject selectGo)
        {
            var targetDir = RootConfig.GetCurrentSonProject().CsMvdaDir + parent.name + "/Button/" + selectGo.name + "/OnClick";
            if (!Directory.Exists(targetDir))
            {
                EditorUtility.DisplayDialog("警告", "该按钮的处理器脚本目录当前不存在！", "确定");
                return;
            }

            var files = Directory.GetFiles(targetDir).Where(r => !r.Contains("meta")).OrderBy(r => r).ToList();
            if (files.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", "该按钮的处理器脚本当前不存在！", "确定");
                return;
            }

            var targetScriptPath = files.Last();
            targetScriptPath = targetScriptPath.Replace("\\", "/").Replace(Application.dataPath, "");
            targetScriptPath = "Assets" + targetScriptPath;
            var scriptObject = AssetDatabase.LoadAssetAtPath<Object>(targetScriptPath);
            EditorGUIUtility.PingObject(scriptObject.GetInstanceID());
        }

        private static void OnSelectButtonCell(GameObject parent, GameObject selectGo)
        {
            var targetDir = RootConfig.GetCurrentSonProject().CsMvdaDir + parent.name + "/Cell/";
            if (!Directory.Exists(targetDir))
            {
                EditorUtility.DisplayDialog("警告", "Cell模板脚本目录当前不存在！", "确定");
                return;
            }

            var targetScriptPath = targetDir + parent.name + "_" + selectGo.name + "_OnClick.cs";
            Debug.Log("目标模板脚本路径：" + targetScriptPath);
            if (!File.Exists(targetScriptPath))
            {
                EditorUtility.DisplayDialog("警告", "目标Cell模板脚本当前不存在！", "确定");
                return;
            }

            targetScriptPath = targetScriptPath.Replace("\\", "/").Replace(Application.dataPath, "");
            var scriptObject = AssetDatabase.LoadAssetAtPath<Object>(targetScriptPath);
            EditorGUIUtility.PingObject(scriptObject.GetInstanceID());
        }

    }
}