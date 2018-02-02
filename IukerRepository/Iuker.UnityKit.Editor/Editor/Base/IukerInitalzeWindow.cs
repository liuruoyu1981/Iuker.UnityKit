using System.Linq;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.Configs;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Iuker.UnityKit.Editor.AutoRunInEditor
{
    /// <summary>
    /// 框架初始化工具
    /// 负责框架本身的一些初始化工作
    /// </summary>
    //[InitializeOnLoad]
    public class IukerInitalzeWindow : EditorWindow
    {
        /// <summary>
        /// 尝试打开项目构建和配置窗口
        /// 如果当前根配置有错误或不存在则自动打开该窗口
        /// </summary>
        private static void TryOpenWindow()
        {
            RootConfig.SetIntanceNull();
            ProjectBaseConfig.SetInstanceNull();
            sHelpBoxContent = "没有发现配置错误";
            CheckRootConfigError(); //  检查错误，更新用于提示的提示文本

            sHelpStyle = new GUIStyle
            {
                fontSize = 20,
                alignment = TextAnchor.UpperCenter
            };

            sWindow = GetWindow<IukerInitalzeWindow>();
            sWindow.InitConfigContext();
            sWindow.titleContent = mTitleContent;
            sWindow.minSize = mWindowSize;
            sWindow.maxSize = mWindowSize;
        }

        [MenuItem("Iuker/项目构建及根配置修改")]
        private static void ShowWindow()
        {
            TryOpenWindow();
        }

        private static IukerInitalzeWindow sWindow;

        /// <summary>
        /// 检查当前unity应用的根配置是否存在错误
        /// </summary>
        /// <returns></returns>
        private static bool CheckRootConfigError()
        {
            var currentProject = RootConfig.GetCurrentProject();

            if (currentProject == null)
            {
                sHelpBoxContent = "当前项目为空，请检查！";
                return true;
            }

            if (string.IsNullOrEmpty(currentProject.ProjectName) || currentProject.ProjectName == "None")
            {
                sHelpBoxContent = "当前项目名为空或为None，请检查！";
                return true;
            }

            if (currentProject.GetCurrentSonProject() == null || currentProject.CurrentSonProject == "None")
            {
                sHelpBoxContent = "当前子项目名为空或为None，请检查！";
                return true;
            }

            foreach (var sonProject in currentProject.AllSonProjects)
            {
                if (sonProject.CurrentClientCoder == null)
                {
                    sHelpBoxContent = string.Format("子项目{0}的当前开发者为空，请检查！", sonProject.ProjectName);
                    return true;

                }
                if (sonProject.ClientCoders == null || sonProject.ClientCoders.Count == 0)
                {
                    sHelpBoxContent = string.Format("子项目{0}的开发者列表为空，请检查！", sonProject.ProjectName);
                    return true;

                }
                foreach (var coder in sonProject.ClientCoders)
                {
                    if (string.IsNullOrEmpty(coder.Name))
                    {
                        sHelpBoxContent = string.Format("子项目{0}的开发者姓名为空，请检查！", sonProject.ProjectName);
                        return true;

                    }
                    if (string.IsNullOrEmpty(coder.Email))
                    {
                        sHelpBoxContent = string.Format("子项目{0}的开发者邮箱为空，请检查！", sonProject.ProjectName);
                        return true;

                    }
                    if (!VerifyUitlity.IsEmail(coder.Email))
                    {
                        sHelpBoxContent = string.Format("子项目{0}的开发者邮箱格式不合法，请检查！", sonProject.ProjectName);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 提示窗口内容
        /// </summary>
        private static string sHelpBoxContent;

        private static readonly GUIContent mTitleContent = new GUIContent("项目构建及根配置");
        private static readonly Vector2 mWindowSize = new Vector2(500f, 420f);

        #region MyRegion

        /// <summary>
        /// 当前项目索引
        /// </summary>
        private int sProjectIndex = 0;

        /// <summary>
        /// 当前子项目索引
        /// </summary>
        private int sSonProjectIndex = 0;

        /// <summary>
        /// 当前项目列表
        /// </summary>
        private string[] sProjectArray = { "None" };

        /// <summary>
        /// 当前项目的所有子项目
        /// </summary>
        private string[] sSonProjectArray = { "None" };

        /// <summary>
        /// 标题样式
        /// </summary>

        /// <summary>
        /// 当前项目名
        /// </summary>
        private string sCurrentProjectName;

        /// <summary>
        /// 新增的项目名 
        /// </summary>
        private string sAddProject;

        /// <summary>
        /// 新增项目的新增（默认）子项目名
        /// </summary>
        private string sAddSonProject;

        /// <summary>
        /// 当前项目将要新增的子项目名
        /// </summary>
        private string sCurrentAddSonProject;

        /// <summary>
        /// 当前子项目名
        /// </summary>
        private string sCurrentSonProject;

        private Project sCurrentProject;

        /// <summary>
        /// 初始化项目配置初始化插件显示环境
        /// </summary>
        private void InitConfigContext()
        {
            if (IsError()) return;

            sProjectIndex = sProjectArray.ToList().FindIndex(r => r == RootConfig.GetCurrentProject().ProjectName);
            if (sProjectIndex < 0 || sProjectIndex >= sProjectArray.Length)
            {
                sProjectIndex = 0;
            }
            sCurrentProjectName = sProjectArray[sProjectIndex];

            sSonProjectIndex = sSonProjectArray.ToList().FindIndex(r => r == RootConfig.GetCurrentSonProject().ProjectName);
            if (sSonProjectIndex < 0 || sSonProjectIndex >= sSonProjectArray.Length)
            {
                sSonProjectIndex = 0;
            }
            sCurrentSonProject = sSonProjectArray[sSonProjectIndex];

            var sonProject = RootConfig.GetCurrentSonProject();
            if (sonProject.ClientCoders == null)
            {
                sSonProjectClientCoderArray = new[] { "None" };
            }
            else
            {
                if (sonProject.ClientCoders != null)
                {
                    sSonProjectClientCoderArray = sonProject.ClientCoders.Select(coder => coder.Name)
                        .ToArray();
                }
            }

            var currentClientCoder = sonProject.CurrentClientCoder;
            sLocalClientCoder = currentClientCoder == null ? "None" : currentClientCoder.Name;
            sLocalCoderEmail = currentClientCoder == null ? "None" : currentClientCoder.Email;
        }

        /// <summary>
        /// 在初始化项目配置窗口环境时是否发生错误
        /// </summary>
        /// <returns></returns>
        private bool IsError()
        {
            bool isError = false;

            sCurrentProjectName = RootConfig.Instance.CurrentProjectName;   //  当前项目
            sProjectArray = RootConfig.Instance.AllProjects.Select(p => p.ProjectName).ToArray(); //  当前所有项目
            sCurrentProject = RootConfig.GetCurrentProject();
            if (sCurrentProject != null)
            {
                sSonProjectArray = sCurrentProject.AllSonProjects.Select(p => p.ProjectName).ToArray();  //  当前项目的所有子项目
            }

            if (sCurrentProject == null)
            {
                sCurrentProjectName = "None";
                isError = true;
            }

            if (sProjectArray == null || sProjectArray.Length == 0)
            {
                sProjectArray = new[] { "None" };
                isError = true;
            }

            if (sSonProjectArray == null || sSonProjectArray.Length == 0)
            {
                sSonProjectArray = new[] { "None" };
                isError = true;
            }

            return isError;
        }

        private void OnGUI()
        {
            // 上部显示当前配置整体情况
            using (new IukVerticalLayout())
            {
                using (new IukHorizontalLayout())
                {
                    LeftWindow();
                    RightWindow();
                }
            }
            // 下部用于修改当前选中的项目配置及子项目配置
            GUILayout.Space(40);
            BottomRight();
        }

        #region 修改设置

        /// <summary>
        /// 当前子项目的本台机器上的开发者的姓名
        /// </summary>
        private string sLocalClientCoder = "None";

        /// <summary>
        /// 新增开发者的姓名
        /// </summary>
        private string sAddClientCoder;

        /// <summary>
        /// 当前子项目的本台机器上的开发者的邮箱
        /// </summary>
        private string sLocalCoderEmail = "None";

        /// <summary>
        /// 新增开发者的邮箱地址
        /// </summary>
        private string sAddCoderEmail;

        /// <summary>
        /// 当前子项目开发者名字符串数组
        /// </summary>
        private string[] sSonProjectClientCoderArray = new string[] { "None" };

        /// <summary>
        /// 开发者数组索引
        /// </summary>
        private int sonProjectClientCoderIndex;

        private void BottomRight()
        {
            using (new IukVerticalLayout())
            {
                EditorGUILayout.LabelField(sHelpBoxContent, sHelpStyle);
            }
        }

        private static GUIStyle sHelpStyle;


        #endregion

        /// <summary>
        /// 左侧项目配置初始化窗口
        /// 1.  新增项目
        /// 2. 新增子项目
        /// 3. 更新配置
        /// </summary>
        private void LeftWindow()
        {
            using (new IukVerticalLayout())
            {
                EditorGUILayout.LabelField("当前项目", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(sCurrentProjectName, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("项目列表", EditorStyles.boldLabel);
                var tempProjectIndex = EditorGUILayout.Popup(sProjectIndex, sProjectArray);
                if (sProjectIndex != tempProjectIndex)
                {
                    sProjectIndex = tempProjectIndex;
                    sCurrentProjectName = sProjectArray[sProjectIndex];
                    var tempConfig = RootConfig.Instance;
                    tempConfig.TryUpdateCurrentProject(sCurrentProjectName);
                    RootConfig.Update();
                    InitConfigContext();
                }

                EditorGUILayout.LabelField("当前子项目", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(sCurrentSonProject, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("当前项目子项目列表", EditorStyles.boldLabel);
                var tempSonProjectIndex = EditorGUILayout.Popup(sSonProjectIndex, sSonProjectArray);
                if (sSonProjectIndex != tempSonProjectIndex)
                {
                    sSonProjectIndex = tempSonProjectIndex;
                    sCurrentSonProject = sSonProjectArray[sSonProjectIndex];
                    var currentProject = RootConfig.GetCurrentProject();
                    currentProject.CurrentSonProject = sCurrentSonProject;
                    RootConfig.Update();
                    InitConfigContext();
                }

                EditorGUILayout.LabelField("当前子项目开发者列表", EditorStyles.boldLabel);
                var tempClientCoderIndex = EditorGUILayout.Popup(sonProjectClientCoderIndex, sSonProjectClientCoderArray);
                if (tempClientCoderIndex != sonProjectClientCoderIndex)
                {
                    sonProjectClientCoderIndex = tempClientCoderIndex;
                    sLocalClientCoder = sSonProjectClientCoderArray[sonProjectClientCoderIndex];
                    RootConfig.GetCurrentSonProject().CurrentClientCoder.Name = sLocalClientCoder;
                }

                EditorGUILayout.LabelField("当前子项目本机开发者", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(sLocalClientCoder, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("当前子项目本机开发者邮箱", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(sLocalCoderEmail, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("新增开发者姓名", EditorStyles.boldLabel);
                sAddClientCoder = EditorGUILayout.TextField(sAddClientCoder);
                EditorGUILayout.LabelField("新增开发者邮箱", EditorStyles.boldLabel);
                sAddCoderEmail = EditorGUILayout.TextField(sAddCoderEmail);
            }
        }

        /// <summary>
        /// 右侧项目配置初始化窗口
        /// </summary>
        private void RightWindow()
        {
            using (new IukVerticalLayout())
            {
                EditorGUILayout.LabelField("新增项目名", EditorStyles.boldLabel);
                sAddProject = EditorGUILayout.TextField(sAddProject);
                EditorGUILayout.LabelField("新增项目默认子项目名", EditorStyles.boldLabel);
                sAddSonProject = EditorGUILayout.TextField(sAddSonProject);
                EditorGUILayout.LabelField("当前项目新增子项目名", EditorStyles.boldLabel);
                sCurrentAddSonProject = EditorGUILayout.TextField(sCurrentAddSonProject);

                if (GUILayout.Button("新增项目")) AddProject();
                if (GUILayout.Button("新增子项目")) AddSonProject();
                if (GUILayout.Button("打开根配置")) OpenRootConfig();
                if (GUILayout.Button("删除当前子项目")) DeleteSonProject();
                if (GUILayout.Button("打开当前项目基础公共配置")) OpenCurrentBaseConfig();
                if (GUILayout.Button("当前项目结构初始化或更新")) InitCurrentProject();
                if (GUILayout.Button("保存对配置的修改")) UpdateConfig();
                if (GUILayout.Button("新增当前子项目开发者")) AddClientCoder();
                if (GUILayout.Button("删除当前项目")) DeleteCurrentProject();
            }
        }

        private void AddClientCoder()
        {
            if (string.IsNullOrEmpty(sAddClientCoder))
            {
                EditorUtility.DisplayDialog("错误", "新增开发者姓名为空，请检查后重试！", "确定");
                return;
            }

            if (string.IsNullOrEmpty(sAddCoderEmail))
            {
                EditorUtility.DisplayDialog("错误", "新增开发者邮箱为空，请检查后重试！", "确定");
                return;
            }

            if (!VerifyUitlity.IsEmail(sAddCoderEmail))
            {
                EditorUtility.DisplayDialog("错误", "新增开发者邮箱格式不合法，请检查后重试！", "确定");
                return;
            }

            var sonProject = RootConfig.GetCurrentSonProject();
            var coderError = sonProject.UpdateClientCoder(sAddClientCoder, sAddCoderEmail);
            if (coderError == SonProject.ClientCoderError.EmailRepeat)
            {
                EditorUtility.DisplayDialog("错误", "子项目开发者邮箱名已存在，请修改后重试！", "确定");
                return;
            }
            if (coderError == SonProject.ClientCoderError.NameRepeat)
            {
                EditorUtility.DisplayDialog("错误", "子项目开发者已存在，请修改后重试！", "确定");
                return;
            }

            sonProject.ClientCoders.Add(new ClientCoder { Email = sAddCoderEmail, Name = sAddClientCoder });
            RootConfig.Update();
            InitConfigContext();    // 刷新根配置插件显示环境
            CheckRootConfigError();
        }

        /// <summary>
        /// 当前项目结构初始化或更新
        /// </summary>
        private void InitCurrentProject()
        {
            var currentProject = RootConfig.GetCurrentProject();
            if (currentProject == null)
            {
                EditorUtility.DisplayDialog("错误", "当前项目为空，请修改根配置后重试！", "确定");
                return;
            }

            if (currentProject.AllSonProjects == null || currentProject.AllSonProjects.Count == 0)
            {
                EditorUtility.DisplayDialog("错误", "当前子项目为空，请修改根配置后重试！", "确定");
                return;
            }

            // 在项目的根目录下创建一个隐藏的用于标识该项目为当前项目的文本文档
            RootConfig.Instance.CurrentProjectName = sCurrentProjectName;
            RootConfig.Update();
            //  创建新项目的基础公告配置
            ProjectBaseConfig.CreateProjectBaseConfig();
            AssetDatabase.Refresh();
            // 关闭窗口
            GetWindow<IukerInitalzeWindow>().Close();
            RootConfig.GetCurrentProject().AllSonProjects.ForEach(ProjectConstrutor.InitSonProjectStructure);
            QuickMenu.CreateAssetAndSpriteInfo();
        }

        /// <summary>
        /// 删除选中的子项目
        /// </summary>
        private void DeleteSonProject()
        {
            var allProjects = RootConfig.Instance.AllProjects;
            foreach (var project in allProjects)
            {
                foreach (var sonProject in project.AllSonProjects)
                {
                    if (sonProject.ProjectName != sCurrentSonProject) continue;

                    project.AllSonProjects.Remove(sonProject);
                    break;
                }
            }

            RootConfig.Update();
            InitConfigContext();
        }

        public static void OpenCurrentBaseConfig()
        {
            var currentProject = RootConfig.GetCurrentProject();
            if (currentProject == null)
            {
                EditorUtility.DisplayDialog("错误", "当前项目为空，请修改根配置后重试！", "确定");
                return;
            }
            InternalEditorUtility.OpenFileAtLineExternal(RootConfig.GetCurrentProject().BaseConfigFullPath, 1);
        }

        private void OpenRootConfig()
        {
            InternalEditorUtility.OpenFileAtLineExternal(RootConfig.SandboxPathJson, 1);
        }

        /// <summary>
        /// 新增加一个项目
        /// </summary>
        private void AddProject()
        {
            if (string.IsNullOrEmpty(sAddProject))
            {
                EditorUtility.DisplayDialog("错误", "新增的项目名不能为空，请检查！", "确定");
                return;
            }

            if (string.IsNullOrEmpty(sAddSonProject))
            {
                EditorUtility.DisplayDialog("错误", "新增的子项目名不能为空，请检查！", "确定");
                return;
            }

            if (sAddProject == "None")
            {
                EditorUtility.DisplayDialog("错误", "新增项目名不能为None，请修改后重试！", "确定");
                return;
            }

            if (sAddSonProject == "None")
            {
                EditorUtility.DisplayDialog("错误", "新增子项目名不能为None，请修改后重试！", "确定");
                return;
            }

            if (sAddProject.Contains(".") || sAddSonProject.Contains(".") ||
                sAddProject.Contains("_") || sAddSonProject.Contains("_"))
            {
                EditorUtility.DisplayDialog("错误", "新增项目及子项目名不能包含.符号或者_符号，请修改后重试！", "确定");
                return;
            }

            if (sAddSonProject == "Common")
            {
                EditorUtility.DisplayDialog("错误", "新增子项目名不能为Common，每个项目都会自动创建一个默认的Common子项目，用于作为项目的共享子项目，请修改后重试！", "确定");
                return;
            }

            var targetProject = RootConfig.Instance.AllProjects.Find(p => p.ProjectName == sAddProject);
            if (targetProject == null)
            {
                var newSon = new SonProject().Init(sAddSonProject, sAddProject);
                var newProject = new Project().Init(sAddProject, newSon);
                RootConfig.Instance.AllProjects.Add(newProject);
                RootConfig.Instance.CurrentProjectName = sAddProject;
                RootConfig.Update();
                InitConfigContext();    // 刷新根配置插件显示环境
            }
        }

        /// <summary>
        /// 从根配置中删除当前选中的项目
        /// </summary>
        private void DeleteCurrentProject()
        {
            var targetProject = RootConfig.Instance.AllProjects.Find(p => p.ProjectName == sCurrentProjectName);
            if (targetProject == null) return;

            RootConfig.Instance.AllProjects.Remove(targetProject);
            if (RootConfig.Instance.AllProjects.Count == 0)
            {
                RootConfig.Update();
                InitConfigContext();
                return;
            }

            var target = RootConfig.Instance.AllProjects.First().ProjectName;
            RootConfig.Instance.CurrentProjectName = target;
            RootConfig.Update();
            InitConfigContext();    // 刷新根配置插件显示环境
        }

        private void AddSonProject()
        {
            if (string.IsNullOrEmpty(sAddSonProject))
            {
                EditorUtility.DisplayDialog("错误", "新增的子项目名不能为空，请检查！", "确定");
                return;
            }


            if (sAddSonProject == "None")
            {
                EditorUtility.DisplayDialog("错误", "新增子项目名不能为None，请修改后重试！", "确定");
                return;
            }

            if (sAddSonProject == "Common")
            {
                EditorUtility.DisplayDialog("错误", "新增子项目名不能为Common，每个项目都会自动创建一个默认的Common子项目，用于作为项目的共享子项目，请修改后重试！", "确定");
                return;
            }

            var targetProject = RootConfig.Instance.AllProjects.Find(p => p.ProjectName == sAddProject);
            if (targetProject == null)
            {
                var newSon = new SonProject().Init(sAddSonProject, sCurrentProjectName);
                var newProject = new Project().Init(sAddProject, newSon);
                RootConfig.Instance.AllProjects.Add(newProject);
                RootConfig.Instance.CurrentProjectName = sAddProject;
                RootConfig.Update();
                InitConfigContext();    // 刷新根配置插件显示环境
            }
        }

        private void UpdateConfig()
        {
            RootConfig.Instance.CurrentProjectName = sCurrentProjectName;
            RootConfig.Instance.AllProjects[sProjectIndex].ProjectName = sCurrentProjectName;
            RootConfig.GetCurrentProject().CurrentSonProject = sCurrentSonProject;
            RootConfig.GetCurrentProject().AllSonProjects[sSonProjectIndex].ProjectName = sCurrentSonProject;
            var sonProject = RootConfig.GetCurrentSonProject();
            sonProject.UpdateClientCoder(sLocalClientCoder, sLocalCoderEmail);
            RootConfig.Update();
            InitConfigContext();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "根配置修改已保存！", "确定");
        }

        private void OnDestroy()
        {
            Debug.Log("项目构建及配置窗口已关闭！");
            sWindow = null;
            RootConfig.SetIntanceNull();
        }


        #endregion


    }
}