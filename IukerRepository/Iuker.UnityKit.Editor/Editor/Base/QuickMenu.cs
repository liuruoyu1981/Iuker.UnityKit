/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/18 15:33:55
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.Assets;
using Iuker.UnityKit.Editor.AutoRunInEditor;
using Iuker.UnityKit.Editor.Configs;
using Iuker.UnityKit.Editor.Base.Assets;
using Iuker.UnityKit.Editor.Ts;
using Iuker.UnityKit.Editor.Hierarchy;
using Iuker.UnityKit.Editor.MVDA;
using Iuker.UnityKit.Editor.MVDA.Quick;
using Iuker.UnityKit.Editor.MVDA.ScriptCreate.Jint;
using Iuker.UnityKit.Editor.Protobuf;
using Iuker.UnityKit.Run;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.HotUpdate;
using Iuker.UnityKit.Run.Module.View.Assist;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEditor;
using UnityEditorInternal;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;

namespace Iuker.UnityKit.Editor
{
    /// <summary>
    /// 快捷菜单项
    /// </summary>
    public static class QuickMenu
    {
        #region 便捷操作

        [MenuItem("Iuker/快捷菜单/运行Window64", false, 1)]
        public static void RunCurrentProjectPcExe()
        {
            var runFilePath = RootConfig.BuildWindows64Dir + string.Format("{0}.exe", RootConfig.GetCurrentProject().ProjectName);
            FileUtility.OpenFolderAndSelectFile(runFilePath);
        }

        /// <summary>
        /// 使用内部编辑器打开选定文件
        /// </summary>
        [MenuItem("Assets/Iuker/使用当前项目编辑器打开目标文件")]
        private static void QuickMenuInternalOpen()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var curPath = path.Substring(6, path.Length - 6);
            var finalPath = Application.dataPath + curPath;
            InternalEditorUtility.OpenFileAtLineExternal(finalPath, 1);
        }

        [MenuItem("Assets/Iuker/创建C#脚本")]
        private static void OpenCreateScript()
        {
            TempCreateScriptWindow.ShowWindow();
        }

        [MenuItem("Assets/Iuker/便捷操作/", false, 1)]

        [MenuItem("Assets/Iuker/便捷操作/配置/", false, 1)]

        [MenuItem("Assets/Iuker/便捷操作/沙盒/", false, 2)]

        [MenuItem("Assets/Iuker/便捷操作/沙盒/打开沙盒", false, 1)]
        public static void OpenSandboxDIr()
        {
            Process.Start(Application.persistentDataPath);
        }

        [MenuItem("Assets/Iuker/便捷操作/沙盒/清空沙盒", false, 2)]
        public static void CleanSandboxDir()
        {
            var result = FileUtility.GetFilePathDictionary(Application.persistentDataPath);
            foreach (var path in result.FilePaths)
            {
                File.Delete(path);
            }
            foreach (var dir in result.Dirs)
            {
                FileUtility.DeleteDirectory(dir);
            }

            Debug.Log("沙盒目录已清空！");
        }

        [MenuItem("Assets/Iuker/便捷操作/配置/打开根配置", false, 1)]
        public static void OpenRootConfig()
        {
            InternalEditorUtility.OpenFileAtLineExternal(RootConfig.SandboxPathJson, 1);
        }

        [MenuItem("Assets/Iuker/便捷操作/配置/打开当前项目基础公共配置", false, 2)]
        public static void OpenCurrentBaseConfig()
        {
            IukerInitalzeWindow.OpenCurrentBaseConfig();
        }


        #endregion

        #region 项目操作

        [MenuItem("Assets/Iuker/项目操作", false, 3)]

        [MenuItem("Assets/Iuker/项目操作/切换当前项目", false, 1)]
        private static void SwitchProject()
        {
            ProjectOprateUitl.SwtichProject();
        }

        [MenuItem("Assets/Iuker/项目操作/删除选中的项目", false, 2)]
        private static void DeleteProject()
        {
            ProjectOprateUitl.DeleteProject();
        }

        #endregion

        #region 资源、图集、打包、版本

        [MenuItem("Assets/Iuker/资源管理", false, 1)]
        [MenuItem("Assets/Iuker/资源管理/资源/", false, 1)]
        [MenuItem("Assets/Iuker/资源管理/图集/", false, 2)]
        [MenuItem("Assets/Iuker/资源管理/AssetBundle打包/", false, 3)]
        [MenuItem("Assets/Iuker/资源管理/AssetBundle版本文件/", false, 4)]

        [MenuItem("Assets/Iuker/资源管理/创建资源和图集数据_当前项目", false, 9)]
        public static void CreateAssetAndSpriteInfo()
        {
            CopyTs();
            new AssetInfoCreater().CreateAssetInfo(RootConfig.CurrentProjectSons.ToArray());
            new SpriteInfoCreater().CreateSpriteInfo(RootConfig.GetCurrentProject().AllSonProjects.ToArray());
            AssetDatabase.Refresh();
        }

        #region 资源

        [MenuItem("Assets/Iuker/资源管理/资源/创建资源数据_当前项目", false, 1)]
        private static void CreateAssetInfoByNewAll()
        {
            new AssetInfoCreater().CreateAssetInfo(RootConfig.CurrentProjectSons.ToArray());
        }

        [MenuItem("Assets/Iuker/资源管理/资源/创建资源数据_当前子项目", false, 2)]
        private static void CreateAssetInfoBySon()
        {
            new AssetInfoCreater().CreateAssetInfo(RootConfig.GetCurrentSonProject());
        }

        #endregion

        #region 图集

        [MenuItem("Assets/Iuker/资源管理/图集/创建图集数据_当前项目", false, 1)]
        private static void CreateSpriteInfoByAll()
        {
            new SpriteInfoCreater().CreateSpriteInfo(RootConfig.GetCurrentProject
().AllSonProjects.ToArray());
        }

        [MenuItem("Assets/Iuker/资源管理/图集/创建图集数据_当前子项目", false, 2)]
        private static void CreateSpriteInfoCurrentSon()
        {
            new SpriteInfoCreater().CreateSpriteInfo(RootConfig.GetCurrentSonProject());
        }

        [MenuItem("Assets/Iuker/资源管理/图集/打包图集到Resources目录下", false, 3)]
        private static void BuildAtlas_Resources()
        {
            AtlasBuildUtil.BuildAtlas_Resources();
        }

        [MenuItem("Assets/Iuker/资源管理/图集/打包到图集AssetDatabase目录下", false, 4)]
        private static void BuildAtlas_AssetDataBases()
        {
            AtlasBuildUtil.BuildAtlas_AssetDataBases();
        }

        [MenuItem("Assets/Iuker/资源管理/图集/拆分图集为散图", false, 5)]
        private static void QuickMenuSplitAtlas()
        {
            AtlasSplittoSpriteEditor.SplitAtlas();
        }

        #endregion

        #region AssetBundle打包

        [MenuItem("Assets/Iuker/资源管理/AssetBundle打包/当前项目", false, 3)]
        private static void AssetBundleBuild_Current()
        {
            RootConfig.CurrentProjectSons.ForEach(son =>
            {
                var builder = new AssetBundleBuilder();
                builder.BuildSon(son);
            });
            AssetBundleVersionInfoCreate_Current();
        }

        private static void CopyTs()
        {
            var sons = RootConfig.GetCurrentProject().AllSonProjects;
            sons.ForEach(s => FileUtility.DeleteDirectory(s.JintDir));
            sons.ForEach(s => FileUtility.TryCreateDirectory(s.JintDir));

            var common = sons.Find(s => s.ProjectName == "Common");
            FileUtility.SyncDirectory(common.TsBaseRelizeDir, common.JintDir,
                s => !s.Contains("meta") && !s.EndsWith(".map") && !s.EndsWith(".gitKeep.txt"),
                p => p.Replace(".js", ".txt"));

            foreach (var son in RootConfig.GetCurrentProject().AllSonProjects)
            {
                FileUtility.SyncDirectory(son.TsProjectBuildOutPutDir, son.JintDir,
                    s => !s.Contains("meta") && !s.EndsWith(".map") && !s.EndsWith(".gitKeep.txt"),
                    p => p.Replace(".js", ".txt"));
            }
        }

        [MenuItem("Assets/Iuker/资源管理/AssetBundle打包/所在子项目", false, 4)]
        private static void AssetBundleBuild_CurrentSon()
        {
            var builder = new AssetBundleBuilder();
            builder.BuildSon(InSonProject);
            AssetBundleVersionInfoCreate_CurrentSon();
        }

        #endregion

        #region AssetBundle版本

        [MenuItem("Assets/Iuker/资源管理/AssetBundle版本文件/当前项目", false, 5)]
        private static void AssetBundleVersionInfoCreate_Current()
        {
            var sons = RootConfig.GetCurrentProject().AllSonProjects;
            foreach (var son in sons)
            {
                AssetBundleVersionInfoCreate(son);
            }
        }

        [MenuItem("Assets/Iuker/资源管理/AssetBundle版本文件/当前子项目", false, 6)]
        private static void AssetBundleVersionInfoCreate_CurrentSon()
        {
            AssetBundleVersionInfoCreate(RootConfig.GetCurrentSonProject());
        }

        private static void AssetBundleVersionInfoCreate(SonProject son)
        {
            AssetBundleVersionInfo.CreateAssetBundleVersionInfo(son);
        }

        #endregion

        #endregion

        #region 配置

        [MenuItem("Iuker/快捷菜单/动态解析当前所有项目配置")]
        private static void DynamicParse()
        {
            RootConfig.DynamicParse();
        }

        #endregion

        #region 路由配置

        [MenuItem("Assets/Iuker/路由", false, 3)]

        [MenuItem("Assets/Iuker/路由/创建视图行为替换脚本", false, 1)]
        private static void MenuReplaceMvdaScript()
        {
            ViewReplaceWindow.ShowWindow();
        }

        [MenuItem("Assets/Iuker/路由/删除视图行为替换脚本", false, 2)]
        private static void MenuDeleteMvdaReplace()
        {
            ViewReplaceWindow.DeleteMvdaReplace();
        }


        #endregion

        #region Protobuf操作

        [MenuItem("Assets/Iuker/Protobuf", false, 5)]
        [MenuItem("Assets/Iuker/Protobuf/打开Protobuf协议窗口", false, 1)]
        public static void ShowProtobufWindow()
        {
            ProtobufEditorWindow.ShowWindow();
        }

        [MenuItem("Assets/Iuker/Protobuf/一键生成当前子项目Protobuf协议相关文件", false, 2)]
        private static void QuickMenuOnStep()
        {
            ProtobufEditorWindow.ShowWindow();
            ProtobufEditorWindow.Instance.DoOnStep();
        }

        #endregion

        #region Mvda菜单项

        #region CSharp

        #region MVDA

        [MenuItem("GameObject/Csharp视图脚本/")]
        [MenuItem("GameObject/Csharp视图脚本/容器和常量", false, 1)]
        [MenuItem("GameObject/Csharp视图脚本/数据模型", false, 2)]
        [MenuItem("GameObject/Csharp视图脚本/生命周期", false, 3)]
        [MenuItem("GameObject/Csharp视图脚本/按钮", false, 4)]
        [MenuItem("GameObject/Csharp视图脚本/开关", false, 5)]
        [MenuItem("GameObject/Csharp视图脚本/开关组", false, 6)]
        [MenuItem("GameObject/Csharp视图脚本/进度条", false, 7)]
        [MenuItem("GameObject/Csharp视图脚本/输入框", false, 8)]
        [MenuItem("GameObject/Csharp视图脚本/动画及效果", false, 9)]

        [MenuItem("GameObject/Csharp视图脚本/所有必要脚本")]
        private static void TryCreateAllMVDAScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateContainerScript();
            createer.CreateViewConstantScript();
            createer.TryCreateAllProcesserScript();
            AssetDatabase.Refresh();
        }

        #region ContainerAndConstant

        [MenuItem("GameObject/Csharp视图脚本/容器和常量", false, 1)]
        private static void CreateCsharpViewContainerAndConstantScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateContainerScript();
            createer.CreateViewConstantScript();
            AssetDatabase.Refresh();
        }

        #endregion

        #region Model

        [MenuItem("GameObject/Csharp视图脚本/数据模型", false, 2)]
        private static void CreateCsharpViewModelScript()
        {
        }

        #endregion

        #region Button

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnClick", false, 1)]
        private static void CreateCsharpButtonClickScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnClick");
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnPointerEnter", false, 2)]
        private static void CreateCsharpButtonOnPointerEnterScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnPointerEnter");
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnPointerDown", false, 3)]
        private static void CreateCsharpButtonOnPointerDownScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnPointerDown");
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnPointerExit", false, 4)]
        private static void CreateCsharpButtonOnPointerExitScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnPointerExit");
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnPointerUp", false, 5)]
        private static void CreateCsharpButtonOnPointerUpScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnPointerUp");
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnDrag", false, 6)]
        private static void CreateCsharpButtonOnDragScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnDrag");
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnBeginDrag", false, 7)]
        private static void CreateCsharpButtonOnBeginDragScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnBeginDrag");
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/按钮/OnEndDrag", false, 8)]
        private static void CreateCsharpButtonOnEndDragScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IButton>("OnEndDrag");
            AssetDatabase.Refresh();
        }



        #endregion

        #region Pipeline

        [MenuItem("GameObject/Csharp视图脚本/生命周期/BeforeCreat", false, 1)]
        private static void CreateCsharpPipelineBeforeCreatScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("BeforeCreat", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/生命周期/OnCreated", false, 2)]
        private static void CreateCsharpPipelineOnCreatedScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("OnCreated", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/生命周期/BeforeActive", false, 3)]
        private static void CreateCsharpPipelineBeforeActiveScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("BeforeActive", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/生命周期/OnActived", false, 4)]
        private static void CreateCsharpPipelineOnActiveScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("OnActived", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/生命周期/BeforeHide", false, 5)]
        private static void CreateCsharpPipelineBeforeHideScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("BeforeHide", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/生命周期/OnHided", false, 6)]
        private static void CreateCsharpPipelineOnHidedScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("OnHided", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }


        [MenuItem("GameObject/Csharp视图脚本/生命周期/BeforeClose", false, 7)]
        private static void CreateCsharpPipelineBeforeCloseScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("BeforeHide", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/生命周期/OnClosed", false, 8)]
        private static void CreateCsharpPipelineOnClosedScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IView>("OnClosed", ViewScriptType.ViewPipeline);
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Csharp视图脚本/生命周期/All", false, 9)]
        private static void CreateCsharpPipelineAllScript()
        {
            CreateCsharpPipelineBeforeCreatScript();
            CreateCsharpPipelineOnCreatedScript();
            CreateCsharpPipelineBeforeActiveScript();
            CreateCsharpPipelineOnActiveScript();
            CreateCsharpPipelineBeforeHideScript();
            CreateCsharpPipelineOnHidedScript();
            CreateCsharpPipelineBeforeCloseScript();
            CreateCsharpPipelineOnClosedScript();
        }

        #endregion

        #region Toggle

        [MenuItem("GameObject/Csharp视图脚本/开关/OnValueChange", false, 1)]
        private static void CreateCsharpToggleOnValueChangeScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IToggle>("OnValueChange");
            AssetDatabase.Refresh();
        }

        #endregion

        #region ToggleGroup

        [MenuItem("GameObject/Csharp视图脚本/开关组/OnValueChange", false, 1)]
        private static void CreateCsharpToggleGroupScript()
        {
        }

        #endregion

        #region 进度条

        [MenuItem("GameObject/Csharp视图脚本/进度条/OnValueChange", false, 1)]
        private static void CreateCsharpSliderOnValueChangeScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<ISlider>("OnValueChange");
            AssetDatabase.Refresh();
        }

        #endregion

        #region 输入框

        [MenuItem("GameObject/Csharp视图脚本/输入框/OnValueChange", false, 1)]
        private static void CreateCsharpInputFieldOnValueChangeScript()
        {
            var createer = new MVDACsCreater();
            createer.CreateProcesserScript<IInputField>("OnValueChange");
            AssetDatabase.Refresh();
        }

        #endregion

        [MenuItem("GameObject/视图控件/", false, 3)]

        #endregion

        [MenuItem("GameObject/Iuker/创建新视图")]
        private static void MenuViewCreateWindow()
        {
            CreateViewWindow.ShowWindow();
        }

        [MenuItem("GameObject/Iuker/定位视图脚本位置")]
        private static void QuickSelect()
        {
            QuickSelectResponser.QuickSelect();
        }

        [MenuItem("GameObject/Iuker/视图脚本/Csharp/", false, 1)]

        #endregion

        #region Fragment

        [MenuItem("GameObject/Iuker/视图碎片脚本/Csharp/容器和常量", false, 1)]
        private static void FragmenntTest()
        {
            FragmentScriptCreater.FragmenntTest();
        }


        #endregion

        #region TypeScript

        [MenuItem("GameObject/TypeScript视图脚本/", false, 2)]
        [MenuItem("GameObject/TypeScript视图脚本/容器和常量", false, 1)]
        [MenuItem("GameObject/TypeScript视图脚本/数据模型", false, 2)]
        [MenuItem("GameObject/TypeScript视图脚本/生命周期", false, 3)]
        [MenuItem("GameObject/TypeScript视图脚本/按钮", false, 4)]
        [MenuItem("GameObject/TypeScript视图脚本/开关", false, 5)]
        [MenuItem("GameObject/TypeScript视图脚本/进度条", false, 6)]
        [MenuItem("GameObject/TypeScript视图脚本/输入框", false, 7)]
        [MenuItem("GameObject/TypeScript视图脚本/动画及效果", false, 8)]

        [MenuItem("GameObject/TypeScript视图脚本/容器和常量", false, 1)]
        private static void MenuViewBaseJintContainerAndConstant()
        {
            var creater = new MVDACreate_ContainerAndConstant_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewContainerAndConstant();
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/数据模型", false, 2)]
        private static void MenuViewBaseJintModel()
        {
            var creater = new MVDACreate_Model_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewModelScript();
            }
        }

        private static bool CheckViewExistByJint(MVDACsCreater creater)
        {
            if (creater.mSelectedWidget.GetComponent<ViewAssister>() != null) return true;
            EditorUtility.DisplayDialog("错误", string.Format("目标对象{0}上并不存在视图辅助器组件！", creater.mSelectedWidget.name), "确定");
            return false;
        }

        #region Button

        [MenuItem("GameObject/TypeScript视图脚本/Button/OnClick", false, 1)]
        private static void MenuViewBaseJintAction_OnClick()
        {
            var creater = new MVDACreate_WidgetAction_Jint();
            if (CheckButtonExistByJint(creater))
            {
                creater.CreateTypeScriptWidgetActionScript("_OnClick");
            }
        }

        private static bool CheckButtonExistByJint(MVDACreate_WidgetAction_Jint creater)
        {
            if (creater.mSelectedWidget.GetComponent<IButton>() != null) return true;
            EditorUtility.DisplayDialog("错误", string.Format("目标对象{0}上并不存在Button组件！", creater.mSelectedWidget.name), "确定");
            return false;
        }

        [MenuItem("GameObject/TypeScript视图脚本/Button/OnPointerEnter", false, 2)]
        private static void MenuViewBaseJintAction_OnPointerEnter()
        {
            var creater = new MVDACreate_WidgetAction_Jint();
            if (CheckButtonExistByJint(creater))
            {
                creater.CreateTypeScriptWidgetActionScript("_OnPointerEnter");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/Button/OnPointerDown", false, 3)]
        private static void MenuViewBaseJintAction_OnPointerDown()
        {
            var creater = new MVDACreate_WidgetAction_Jint();
            if (CheckButtonExistByJint(creater))
            {
                creater.CreateTypeScriptWidgetActionScript("_OnPointerDown");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/Button/PointerExit", false, 4)]
        private static void MenuViewBaseJintAction_OnPointerExit()
        {
            var creater = new MVDACreate_WidgetAction_Jint();
            if (CheckButtonExistByJint(creater))
            {
                creater.CreateTypeScriptWidgetActionScript("_OnPointerExit");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/Button/OnPointerUp", false, 5)]
        private static void MenuViewBaseJintAction_OnPointerUp()
        {
            var creater = new MVDACreate_WidgetAction_Jint();
            if (CheckButtonExistByJint(creater))
            {
                creater.CreateTypeScriptWidgetActionScript("_ONPointerUp");
            }
        }

        #endregion

        #region Toggle

        [MenuItem("GameObject/TypeScript视图脚本/开关/OnValueChanged", false, 1)]
        private static void MenuViewBaseJintAction_ToggleClick()
        {
            MVDACreate_WidgetAction_Jint.CreateScript("_OnValueChanged");
        }

        #endregion

        #region Pipeline

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/BeforeCreat", false, 1)]
        private static void MenuViewBaseJintPipeline_BeforeCreat()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_BeforeCreat");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/OnCreated", false, 2)]
        private static void MenuViewBaseJintPipelineOnCreated()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_OnCreated");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/BeforeHide", false, 3)]
        private static void MenuViewBaseJintPipeline_BeforeHide()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_BeforeHide");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/OnHided", false, 4)]
        private static void MenuViewBaseJintPipeline_OnHided()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_OnHided");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/BeforeActive", false, 5)]
        private static void MenuViewBaseJintPipeline_BeforeActive()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_BeforeActive");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/OnActived", false, 6)]
        private static void MenuViewBaseJintPipeline_OnActived()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_OnActived");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/BeforeClose", false, 7)]
        private static void MenuViewBaseJintPipeline_BeforeClose()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_BeforeClose");
            }
        }

        [MenuItem("GameObject/TypeScript视图脚本/生命周期/OnClosed", false, 8)]
        private static void MenuViewBaseJintPipeline_OnClosed()
        {
            var creater = new MVDACreate_Pipeline_Jint();
            if (CheckViewExistByJint(creater))
            {
                creater.CreateTypeScriptViewPipelineScript("_OnClosed");
            }
        }


        #endregion

        #endregion

        #endregion

        #region Ts项目

        [MenuItem("Assets/Iuker/TypeScript", false, 6)]
        [MenuItem("Assets/Iuker/TypeScript/更新Ts项目到框架最新版本", false, 1)]
        private static void UpdateTsProject()
        {
            var sons = RootConfig.GetCurrentProject().AllSonProjects;
            sons.ForEach(UpdateTsProjectBySon);
            AssetDatabase.Refresh();
        }

        private static void UpdateTsProjectBySon(SonProject son)
        {
            //  删除子项目下的Iuker目录
            FileUtility.DeleteDirectory(son.TsIukerDir);
            //  拷贝框架Ts项目下的Iuker目录到子项目下
            FileUtility.CopyDirectory(U3dConstants.TsIukerDir, son.TsIukerDir);
            //  删除子项目的Ts项目文件
            File.Delete(son.TsProjPath);
            //  拷贝框架Ts项目的Ts项目文件并重命名为子项目的Ts项目文件名
            FileUtility.CopyFile(U3dConstants.TsProjectFilePath, son.TsDir);
            var leftPath = son.TsDir + "Iuker.UnityKit.Ts.njsproj";
            File.Move(leftPath, son.TsProjPath);
            //  修改项目配置文件中的项目名
            var replacedContent = File.ReadAllText(son.TsProjPath).Replace("Iuker.UnityKit.Ts", son.CompexName + ".Ts");
            FileUtility.WriteAllText(son.TsProjPath, replacedContent);
            //  将子项目的Ts项目下Project目录下的所有Ts脚本都添加到新拷贝的Ts项目文件中
            FileUtility.EnsureDirExist(son.TsProjectDir);
            var sonTsFiles = FileUtility.GetFilePaths(son.TsProjectDir, s => !s.Contains(".meta")).FilePaths;
            sonTsFiles = sonTsFiles.Select(s => s.Replace(son.TsProjectDir, "")).ToList();
            sonTsFiles = sonTsFiles.Select(s =>
            {
                var substring = s.Substring(0, s.LastIndexOf('.'));
                substring = substring.Replace("/", "\\");
                return substring;
            }).ToList();
            TsProj.AddLines(sonTsFiles, son).UpdateToFile(son.TsProjPath);
        }

        #endregion

        #region Excel相关、本地数据脚本、数据源

        private static string _SelectExcelFileName;

        private static ExcelParser GetExcelParser()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var curPath = path.Substring(6, path.Length - 6);
            var finalPath = Application.dataPath + curPath;
            if (!finalPath.EndsWith("xlsx"))
                throw new Exception("错误,所选择的不是一个Excel文件！");

            _SelectExcelFileName = Path.GetFileNameWithoutExtension(finalPath);
            var excelParser = new ExcelParser(finalPath, finalPath.PathInSonProject());
            return excelParser;
        }

        [MenuItem("Assets/Iuker/Excel", false, 7)]
        [MenuItem("Assets/Iuker/Excel/导出选择的Excel本地数据表Cs、Ts脚本及Txt数据源文件", false, 1)]
        private static void ExportSeletedLocalDataExcel()
        {
            var excelParser = GetExcelParser();
            excelParser.ExportCsharpScript();
            excelParser.ExportTypeScriptScript();
            excelParser.ExportTxts();
            Debug.Log(string.Format("Excel本地数据表{0}的Cs、Ts脚本及Txt数据有文件导出成功！", _SelectExcelFileName));
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Iuker/Excel/导出选择的Excel本地数据表的Txt数据源文档", false, 2)]
        private static void ExportSelectedLocalExcelTxtFile()
        {
            var excelParser = GetExcelParser();
            excelParser.ExportTxts();
            Debug.Log(string.Format("Excel本地数据表{0}的Txt数据源文件导出成功！", _SelectExcelFileName));
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Iuker/Excel/导出选择的Excel本地数据表的Typescript脚本", false, 3)]
        private static void ExportSelectedLocalExcelTypescriptScript()
        {
            var excelParser = GetExcelParser();
            excelParser.ExportTypeScriptScript();
            Debug.Log(string.Format("Excel本地数据表{0}的Typescript脚本导出成功！", _SelectExcelFileName));
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Iuker/Excel/导出选择的Excel本地数据表的Csharp脚本", false, 3)]
        private static void ExportSelectedLocalExcelCsharpScript()
        {
            var excelParser = GetExcelParser();
            excelParser.ExportCsharpScript();
            Debug.Log(string.Format("Excel本地数据表{0}的Csharp脚本导出成功！", _SelectExcelFileName));
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Iuker/Excel/导出当前目录所在子项目的Protobuf消息的Typescript接口", false, 4)]
        private static void ExportSelectedProtobufExcelTypescriptScript()
        {
            if (InSonProject != null)
            {
                ProtobufTypeScriptCreater.ExportTypeScriptProtubufProxy(InSonProject);
            }
        }

        private static SonProject InSonProject
        {
            get
            {
                string finalPath;

                var path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (path != "")
                {
                    var curPath = path.Substring(6, path.Length - 6);
                    finalPath = Application.dataPath + curPath;
                }
                else
                {
                    var paths = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).Where(AssetDatabase.IsValidFolder).ToList();
                    if (paths.Count > 1)
                    {
                        EditorUtility.DisplayDialog("", "不能同时选择多个目录进行该操作！", "确定");
                        return null;
                    }

                    finalPath = paths[0];
                }

                return finalPath.PathInSonProject();
            }
        }


        #endregion


        #region 辅助

        [MenuItem("GameObject/辅助功能/批量增加前缀", false, 1)]
        private static void OpnBatchAddPrefixWindow()
        {
            BatchAddPrefixWindow.ShowWindow();
        }

        [MenuItem("GameObject/辅助功能/批量增加后缀", false, 2)]
        private static void OpnBatchAddSuffixWindow()
        {
            BatchAddSuffixWindow.ShowWindow();
        }

        [MenuItem("GameObject/辅助功能/批量替换前缀", false, 3)]
        private static void OpnBatchReplacePrefixWindow()
        {
            BatchReplacePrefixWindow.ShowWindow();
        }

        [MenuItem("GameObject/辅助功能/批量替换后缀", false, 4)]
        private static void OpenBatchReplaceSuffixWindow()
        {
            BatchReplaceSuffixWindow.ShowWindow();
        }



        #endregion

        #region LocalHttpServer

        [MenuItem("Assets/Iuker/打开本地Http服务器")]
        private static void OpenLocalHttpServer()
        {
            FileUtility.TryCreateDirectory(U3dConstants.LocalHttpDir);
            AssetDatabase.Refresh();
            var pNames = Process.GetProcesses().Select(p => p.MainModule.ModuleName).ToList();
            var targetP = pNames.Find(p => p.StartsWith("mywebserver"));
            if (targetP == null)
            {
                TryChangeMyWebServerConfig();
                LauncherMyWebServer();
            }
        }

        /// <summary>
        /// 尝试修改MyWebServer的服务器配置
        /// 修改为UNITY项目的StreamingAssets目录
        /// </summary>
        private static void TryChangeMyWebServerConfig()
        {
            var myWebServerConfigPath = Application.dataPath + "/1_Iuker.UnityKit/.Tools/MyWebServer/server.ini";
            var webServerPath = "serverpath=" + U3dConstants.LocalHttpDir.Replace('/', '\\');
            var allLines = File.ReadAllLines(myWebServerConfigPath);
            if (allLines[63] != webServerPath)
            {
                allLines[63] = webServerPath;
                File.WriteAllLines(myWebServerConfigPath, allLines);
            }
        }

        private static void LauncherMyWebServer()
        {
            var path = Application.dataPath + "/1_Iuker.UnityKit/.Tools/MyWebServer/mywebserver.exe";
            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.UseShellExecute = true;
            pinfo.FileName = path;
            pinfo.Arguments = "/H";
            //启动进程
            Process p = Process.Start(pinfo);
            Debug.Log("本地http资源服务器已启动！");
        }


        #endregion

        [MenuItem("Assets/Iuker/脚本创建", false, 8)]

        [MenuItem("Assets/Iuker/脚本创建/数据及事件脚本创建", false, 1)]
        private static void SyncAndCreateDataIdScript()
        {
            var dataIdCreater = new IdScriptCreater(InSonProject, InSonProject.CompexName,
                InSonProject.CompexName, InSonProject.DataIdTxtPath, "DataId",
                InSonProject.DataIdCsScriptPath, InSonProject.DataIdTsScriptPath);
            dataIdCreater.CreateScript();

            var eventIdCreater = new IdScriptCreater(InSonProject, InSonProject.CompexName,
                InSonProject.EventTypeName, InSonProject.EventIdTxtPath, "EventId",
                InSonProject.EventIdCsScriptPath, InSonProject.EventIdTsScriptPath);
            eventIdCreater.CreateScript();
        }

    }
}
