//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using Application = UnityEngine.Application;

//namespace Iuker.UnityKit.Editor.Configs
//{
//    // ReSharper disable once ClassNeverInstantiated.Global
//    public class RootConfigEditor
//    {
//        /// <summary>
//        /// 创建应用根配置文件
//        /// </summary>
//        public static void CreateRootConfig()
//        {
//            var targetPath = Application.streamingAssetsPath + "/RootConfig.json";
//            if (File.Exists(targetPath))
//            {
//                EditorUtility.DisplayDialog("错误", "根配置文件已存在。", "确定");
//            }
//            else
//            {
//                RootConfig rootConfig = new RootConfig();
//                rootConfig.CurrentProjectName = "Default";
//                rootConfig.AllProjects = new List<Project>
//                {
//                    new Project
//                    {
//                        ProjectName =  "Default",
//                        CurrentSonProject = "SonProject1",
//                        AllSonProjects = new List<SonProject>
//                        {
//                            new SonProject {ProjectName = "SonProject1"},
//                            new SonProject {ProjectName = "SonProject2"},
//                            new SonProject {ProjectName = "SonProject3"},
//                        }
//                    }
//                };

//                var jsContent = JsonUtility.ToJson(rootConfig);
//                File.WriteAllText(targetPath, jsContent);

//                AssetDatabase.Refresh();
//            }
//        }



//    }
//}
