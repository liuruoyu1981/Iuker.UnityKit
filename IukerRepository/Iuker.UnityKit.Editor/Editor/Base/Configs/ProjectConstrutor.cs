using System.IO;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Iuker.UnityKit.Editor.Configs
{
    /// <summary>
    /// 项目构建器
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectConstrutor
    {
        private static RootConfig sRootConfig
        {
            get
            {
                return RootConfig.Instance;
            }
        }

        private static string IukerUnityKitProjectTemplateDir
        {
            get
            {
                return Application.dataPath +
                       "/1_Iuker.UnityKit/UnityProjectTemplate/";
            }
        }

        private static string sIl8nTxtName = "ldil8n.txt";
        private static string sSoundEffectTxtName = "ldsoundeffect.txt";
        private static string sProtobufExcelName = "Protobufs.xlsx";

        private static string Il8nTxtPath
        {
            get
            {
                return IukerUnityKitProjectTemplateDir + sIl8nTxtName;
            }
        }

        private static string SoundEffectTxtPath
        {
            get
            {
                return IukerUnityKitProjectTemplateDir + sSoundEffectTxtName;
            }
        }

        /// <summary>
        /// Protobuf协议定义模板excel文件
        /// </summary>
        private static string ProtobufExcelPath
        {
            get
            {
                return IukerUnityKitProjectTemplateDir + sProtobufExcelName;
            }
        }

        private static string ProtobufExcelNewPath
        {
            get
            {
                return RootConfig.GetCurrentSonProject().ProtobufRootDir +
                       RootConfig.GetCurrentProject().ProjectName + "_" + RootConfig.GetCurrentSonProject().ProjectName + "_" + sProtobufExcelName;
            }
        }

        private static string DefaultLauncherScenePath
        {
            get
            {
                return IukerUnityKitProjectTemplateDir +
                       "Iuker.UnityKit.Bootstrap.unity";
            }
        }

        private static string DefaultUpdateViewPrefabPath
        {
            get
            {
                return IukerUnityKitProjectTemplateDir + "view_default_update.prefab";
            }
        }

        /// <summary>
        /// 初始化子项目项目结构
        /// </summary>
        public static void InitSonProjectStructure(SonProject son)
        {
            if (sRootConfig.CurrentProjectName == "Default")
            {
                EditorUtility.DisplayDialog("错误", "请先设置当前开发项目相关配置", "确定");
            }
            else
            {
                FileUtility.TryCreateDirectory(son.RootDir);          //  创建当前项目根目录

                // 在项目的根目录下创建一个隐藏的用于标识该目录是一个合法的基于Iukerr.UnityKit框架的项目目录的文本文档
                RootConfig.TryCreateThisIsProjectTxt();
                // --------------------创建当前项目的当前子项目的所有目录及初始化资源---------------------

                //  拷贝原始的Unity项目设置目录
                FileUtility.TryCopyDirectory(U3dConstants.OriginProjectSettingsDir,
                    RootConfig.GetCurrentProject().ProjectSettingsBakDir);

                // 创建AssetDataBase目录
                FileUtility.TryCreateDirectory(son.AssetDataBaseDir);
                // 创建项目的基础公共配置
                ProjectBaseConfig.CreateProjectBaseConfig();

                // 创建项目其他常用文件夹目录
                FileUtility.TryCreateDirectory(son.CsFrameOverrideDir);
                FileUtility.TryCreateDirectory(son.ConfigDir);
                //  写入空视图配置文档
                FileUtility.WriteAllText(son.ViewsConfigResourcesPath, JsonUtility.ToJson(new Views()));
                FileUtility.TryCreateDirectory(son.ExcelDir);
                FileUtility.TryCreateDirectory(son.AtlasToSpriteDir);
                FileUtility.TryCreateDirectory(son.AtlasOriginDir);
                FileUtility.TryCreateDirectory(son.ProtobufRootDir);
                FileUtility.TryCreateDirectory(son.TexturesDir);

                //  材质和shader
                FileUtility.TryCreateDirectory(son.MaterialDir);
                FileUtility.TryCreateDirectory(son.ShaderDir);

                // 创建Asset目录
                FileUtility.TryCreateDirectory(son.LocalDataTxtDir);
                FileUtility.TryCreateDirectory(son.JintDir);
                FileUtility.TryCreateDirectory(son.AtlasDir);
                FileUtility.TryCreateDirectory(son.FontDir);
                FileUtility.TryCreateDirectory(son.TextureDir);
                FileUtility.TryCreateDirectory(son.BinaryDataDir);
                FileUtility.TryCreateDirectory(son.ViewPrefabDir);
                if (son.ProjectName == "Common" && !File.Exists(son.UpdateViewPath))
                {
                    FileUtility.CopyFile(DefaultUpdateViewPrefabPath, son.ViewPrefabDir);
                    FileUtility.Move(son.ViewPrefabDir + "view_default_update.prefab", son.UpdateViewPath);
                }

                FileUtility.TryCreateDirectory(son.MusicDir);
                FileUtility.TryCreateDirectory(son.SoundEffectDir);
                FileUtility.TryCreateDirectory(son.FxEffectDir);
                FileUtility.TryCreateDirectory(son.AnimationsDir);
                FileUtility.TryCreateDirectory(son.PrefabsDir);

                //  拷贝多语言和音效的本地数据txt数据源文件
                TryCopyIl8nAndSoundTxt(son);

                // 如果默认的启动器场景当前不存在则拷贝默认的启动器场景
                if (!File.Exists(RootConfig.DefaultLauncherSceneNewPath))
                {
                    FileUtility.TryCopy(DefaultLauncherScenePath, RootConfig.DefaultLauncherSceneNewPath);
                    Debug.Log("项目启动器场景已存在，创建取消！");
                }

                TypeScriptConstruct(son);

                FileUtility.TryCreateDirectory(son.CSahrpDir);
                FileUtility.TryCreateDirectory(son.DllDir);
                FileUtility.TryCreateDirectory(son.CsReactiveDataModelDir);
                FileUtility.TryCreateDirectory(son.CsMvdaDir);
                FileUtility.TryCreateDirectory(son.CsManagerDir);
                FileUtility.TryCreateDirectory(son.CsEditorDir);
                FileUtility.TryCreateDirectory(son.CsMonoEntiiesDir);
                FileUtility.TryCreateDirectory(son.CsCommunicationDir);
                FileUtility.TryCreateDirectory(son.CsCommunicationRequestDir);
                FileUtility.TryCreateDirectory(son.CsCommunicationResponsersDir);
                FileUtility.TryCreateDirectory(son.CsLocalDataDir);

                //  拷贝Protobuf协议定义模板excel文件
                FileUtility.TryCopy(ProtobufExcelPath, ProtobufExcelNewPath);

                // 刷新unity编辑器
                AssetDatabase.Refresh();

                // 打开当前项目的启动器场景
                EditorSceneManager.OpenScene(RootConfig.DefaultLauncherSceneNewPath);

                Debug.Log(string.Format("{0}项目结构初始化完成！", RootConfig.GetCurrentProject().ProjectName));
            }
        }

        private static void TypeScriptConstruct(SonProject son)
        {
            FileUtility.CopyDirectory(U3dConstants.TsProjectDir, son.TsDir);
            AssetDatabase.Refresh();
            var tempPath = son.TsDir + "Iuker.UnityKit.Ts.njsproj";
            if (!File.Exists(son.TsProjPath))
            {
                File.Move(tempPath, son.TsProjPath);
            }
        }

        /// <summary>
        /// 拷贝多语言和音效的本地数据txt数据源文件
        /// </summary>
        private static void TryCopyIl8nAndSoundTxt(SonProject son)
        {
            FileUtility.TryCopy(Il8nTxtPath, son.Il8nTxtPath);
            FileUtility.TryCopy(SoundEffectTxtPath, son.SoundEffectTxtPath);
        }

    }
}
