using System.Linq;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Configs
{
    /// <summary>
    /// 项目相关快捷菜单
    /// 扩展Project视图菜单
    /// </summary>
    public class ProjectOprateUitl
    {
        /// <summary>
        /// 切换项目
        /// </summary>

        public static void SwtichProject()
        {
            var paths = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).Where(AssetDatabase.IsValidFolder).ToList();
            if (paths.Count > 1)
            {
                EditorUtility.DisplayDialog("", "不能同时选择多个目录进行该操作！", "确定");
                return;
            }

            var projectname = paths.First().Split('/').Last();
            if (!projectname.StartsWith("_"))
            {
                EditorUtility.DisplayDialog("", "选择的不是有效项目目录！", "确定");
                return;
            }

            var selectDirName = projectname.Substring(1, projectname.Length - 1);
            if (!RootConfig.IsProjectExist(selectDirName))
            {
                EditorUtility.DisplayDialog("", string.Format("选择的目录{0}在根配置中不存在，请检查根配置！", selectDirName), "确定");
                return;
            }

            var rootConfig = RootConfig.Instance;
            var target = projectname.Substring(1, projectname.Length - 1);
            rootConfig.TryUpdateCurrentProject(target);
            RootConfig.Update();
            EditorSceneManager.OpenScene(RootConfig.DefaultLauncherSceneNewPath);
            EditorUtility.DisplayDialog("", string.Format("当前项目已切换为{0}。", RootConfig.Instance.CurrentProjectName), "确定");
        }

        public static void DeleteProject()
        {
            var projectDir = AssetDatabase.GetAssetPath(Selection.activeObject);
            var projectname = projectDir.Split('/').Last();
            if (!projectname.StartsWith("_")) return;
            var rootConfig = RootConfig.Instance;
            var targetProject = rootConfig.AllProjects.Find(p => p.ProjectName == projectname.Substring(1, projectname.Length - 1));
            rootConfig.AllProjects.Remove(targetProject);
            var target = rootConfig.AllProjects.First().ProjectName;
            rootConfig.CurrentProjectName = target;
            RootConfig.Update();
            var projectFullDir = Application.dataPath + string.Format("/{0}/", projectname);
            FileUtility.DeleteDirectory(projectFullDir);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("", string.Format("目标项目{0}已删除！", targetProject.ProjectName), "确定");
        }





    }
}