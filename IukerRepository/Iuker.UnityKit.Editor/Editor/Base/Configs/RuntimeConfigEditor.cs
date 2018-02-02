using System.Collections.Generic;
using System.IO;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Configs
{
    /// <summary>
    /// 运行时配置编辑器
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RuntimeConfigEditor
    {
        /// <summary>
        /// 创建子项目的视图配置
        /// </summary>
        private static void CreateSonProjectViewsConfig()
        {
            var sonProject = RootConfig.GetCurrentSonProject();

            if (File.Exists(sonProject.ViewsConfigResourcesPath))
            {
                Debug.Log(string.Format("当前子项目{0}的视图配置文件已存在，将跳过创建！", sonProject.CompexName));
                return;
            }
            var currentSonProjectName = sonProject.CompexName;
            Views views = new Views();
            views.ProjectViews = new List<View>
            {
                new View
                {
                    ViewId = string.Format("view_{0}_default", currentSonProjectName),
                    ViewType = "Normal",
                    AssetName = string.Format("view_{0}_default", currentSonProjectName),
                    IsBlankClose = false,
                    IsCloseTop = false,
                    IsHideOther = false,
                    IsMain = false,
                },
            };
            File.WriteAllText(sonProject.ViewsConfigResourcesPath, JsonUtility.ToJson(views));
        }


    }
}
