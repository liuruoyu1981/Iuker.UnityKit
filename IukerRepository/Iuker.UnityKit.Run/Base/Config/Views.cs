/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/022 13:37:55
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
using System.IO;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base.Config
{
    /// <summary>
    /// 视图配置
    /// 用于子项目分别管理自己的视图配置
    /// </summary>
    [Serializable]
    public class Views
    {
        /// <summary>
        /// 子项目的视图配置节点列表
        /// </summary>
        public List<View> ProjectViews;

        /// <summary>
        /// 当前子项目视图配置实例
        /// </summary>
        private static Views GetInstance(SonProject son)
        {
            var sonResViewsConfigPath = son.ViewsConfigResPath;
            var resViewsConfig = Resources.Load<TextAsset>(sonResViewsConfigPath);
            string viewsContent;
            if (resViewsConfig == null)
            {
                var path = son.ViewsConfigResourcesPath;
                viewsContent = File.ReadAllText(path);
            }
            else
            {
                viewsContent = resViewsConfig.text;
            }

            var views = JsonUtility.FromJson<Views>(viewsContent);
            return views;
        }

        /// <summary>
        /// 如果当前子项目的视图配置不存在，则创建一个新的
        /// </summary>
        public static void CreateNewVIewsConfig()
        {
            var sonProject = RootConfig.GetCurrentSonProject();
            FileUtility.EnsureDirExist(sonProject.ViewsConfigResourcesPath);
            var fullPath = sonProject.ViewsConfigResourcesPath;
            if (!File.Exists(fullPath))
            {
                File.WriteAllText(fullPath, JsonUtility.ToJson(new Views()));
            }
        }

        /// <summary>
        /// 更新当前子项目的视图配置文件
        /// </summary>
        public static void Update(SonProject son, View newView)
        {
            var targetView = GetInstance(son).ProjectViews.Find(v => v.ViewId == newView.ViewId);
            var views = GetInstance(son);

            if (targetView == null)
            {
                views.ProjectViews.Add(newView);
            }
            else
            {
                views.ProjectViews.Remove(targetView);
                views.ProjectViews.Add(newView);
            }

            var viewsContent = JsonUtility.ToJson(views);
            FileUtility.WriteAllText(son.ViewsConfigResourcesPath, viewsContent);
            FileUtility.WriteAllText(son.ViewsConfigHotUpdatePath, viewsContent);
        }

    }
}