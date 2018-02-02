using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.Setting;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA
{
    /// <summary>
    /// 视图替换脚本创建窗口
    /// </summary>
    public class ViewReplaceWindow : EditorWindow
    {
        private static ViewReplaceWindow _window;
        private static string targetRootNs;
        private static string mAuthor;
        private static string mEmail;
        private static string mTimeToken;
        private static string mScriptName = "DefaultScript";
        private string mClassPurpose;
        private string widgetType;
        private string viewId;
        private static string sourceName;

        public static void ShowWindow()
        {
            mAuthor = IukerEditorPrefs.GetString("HostClientName");
            mEmail = IukerEditorPrefs.GetString("HostClientEmail");
            mTimeToken = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            var responserTypes = new List<Type>();

            var paths = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).ToList();
            if (paths.Count > 1)
            {
                EditorUtility.DisplayDialog("", "不能同时选择多个目标进行该操作！", "确定");
                return;
            }

            var path = paths[0];
            sourceName = path.FileName();
            if (!File.Exists(paths[0]) || !path.EndsWith(".cs"))
            {
                EditorUtility.DisplayDialog("", "目标不是Csharp文件！", "确定");
                return;
            }

            var assemblyCSharp = Assembly.LoadFile(U3dConstants.AssemblyCSharpPath);
            responserTypes.AddRange(ReflectionUitlity.GetTypeList<IViewActionResponser<IView>>(assemblyCSharp));
            responserTypes.AddRange(ReflectionUitlity.GetTypeList<IViewActionResponser<IButton>>(assemblyCSharp));
            responserTypes.AddRange(ReflectionUitlity.GetTypeList<IViewActionResponser<IToggle>>(assemblyCSharp));
            responserTypes.AddRange(ReflectionUitlity.GetTypeList<IViewActionResponser<ISlider>>(assemblyCSharp));
            responserTypes.AddRange(ReflectionUitlity.GetTypeList<IViewActionResponser<IInputField>>(assemblyCSharp));

            var targetType = responserTypes.Find(t => t.Name == sourceName);
            if (targetType == null)
            {
                EditorUtility.DisplayDialog("", "目标不是一个视图行为处理器脚本！", "确定");
                return;
            }

            sourceName = TrimDigit(sourceName);
            mScriptName = "replace_" + sourceName;
            targetRootNs = RootConfig.GetCurrentProject().ProjectName;

            if (_window == null)
            {
                _window = GetWindow<ViewReplaceWindow>();
                _window.titleContent = new GUIContent("视图行为替换");
                _window.minSize = new Vector2(900, 300);
                _window.InitCreateInstanceDictionary();
                var result = _window.CreateResponserInstance(targetType);
                _window.viewId = result.Split('@').Last();
                _window.widgetType = result.Split('@').First();
            }
        }

        public static void DeleteMvdaReplace()
        {
            var paths = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).ToList();
            if (paths.Count > 1)
            {
                EditorUtility.DisplayDialog("", "不能同时选择多个目标进行该操作！", "确定");
                return;
            }

            var path = paths[0];
            sourceName = path.FileName();
            if (!File.Exists(paths[0]) || !path.EndsWith(".cs"))
            {
                EditorUtility.DisplayDialog("", "目标不是Csharp文件！", "确定");
                return;
            }

            var son = RootConfig.GetCurrentSonProject();
            if (!File.Exists(son.ViewActionReplaceDataPath))
            {
                EditorUtility.DisplayDialog("", "当前子项目不存在视图行为替换数据！", "确定");
                return;
            }

            var bytes = File.ReadAllBytes(son.ViewActionReplaceDataPath);
            var replaceDic = SerializeUitlity.DeSerialize<Dictionary<string, string>>(bytes);

            var targetKey = string.Empty;
            foreach (KeyValuePair<string, string> keyValuePair in replaceDic)
            {
                if (keyValuePair.Value == sourceName)
                {
                    targetKey = keyValuePair.Key;
                }
            }
            if (targetKey != string.Empty)
            {
                replaceDic.Remove(targetKey);
                File.WriteAllBytes(son.ViewActionReplaceDataPath, SerializeUitlity.Serialize(replaceDic));
                EditorUtility.DisplayDialog("", "选中的视图行为替换脚本当前已移除！", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("", "视图行为替换数据中没有选中脚本的替换关系存在，请检查！", "确定");
            }
        }

        private static string TrimDigit(string name)
        {
            var l = name.Length - 1;

            for (var i = l; i > 0; i--)
            {
                var c = name[i];
                var result = (int)c;
                if (result > 58 || result < 48)
                {
                    var index = i;
                    var newName = name.Substring(0, index + 1);
                    if (newName.EndsWith("_"))
                    {
                        newName = newName.Substring(0, newName.Length - 1);
                    }
                    return newName;
                }
            }

            return null;
        }

        private List<Func<Type, string>> mFuncList;

        private void InitCreateInstanceDictionary()
        {
            mFuncList = new List<Func<Type, string>>
            {
                CreateResponser_IButton,
                CreateResponser_IToggle,
                CreateResponser_IView,
                CreateResponser_ISlider
            };
        }

        private void OnGUI()
        {
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("脚本创建者");
                mAuthor = EditorGUILayout.TextField(mAuthor);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("创建者邮箱");
                mEmail = EditorGUILayout.TextField(mEmail);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("创建日期");
                EditorGUILayout.LabelField(mTimeToken);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("命名空间");
                targetRootNs = EditorGUILayout.TextField(targetRootNs);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("脚本名");
                EditorGUILayout.LabelField(mScriptName);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("用途");
                mClassPurpose = EditorGUILayout.TextField(mClassPurpose);
            }
            if (GUILayout.Button("创建"))
            {
                CreateReplaceScript();
            }
        }

        /// <summary>
        /// 创建替换已有Csharp脚本的目标脚本
        /// </summary>
        private void CreateReplaceScript()
        {
            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using Iuker.Common.Base;");
            sb.AppendLine("using Iuker.UnityKit.Run.Base;");
            sb.AppendLine("using Iuker.UnityKit.Run.Module.View.MVDA;");
            sb.AppendLine("using Iuker.UnityKit.Run.Module.View.ViewWidget;");
            sb.AppendLine();
            sb.AppendLine(string.Format("namespace {0}", targetRootNs));
            sb.AppendLine("{");

            sb.AppendLine("#if DEBUG");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine(string.Format("    /// {0}", mClassPurpose));
            sb.AppendLine("    /// </summary>");
            sb.AppendLine(
                string.Format("    [CreateDesc({0},{1},{2})]", "\"" + mAuthor + "\"", "\"" + mEmail + "\"",
                    "\"" + mTimeToken + "\""));
            sb.AppendLine(
                string.Format("    [ClassPurposeDesc({0},{1})]", "\"" + mClassPurpose + "\"",
                    "\"" + mClassPurpose + "\""));

            sb.AppendLine("#endif");
            sb.AppendLine(string.Format("    public class {0} : IViewActionResponser<{1}>", mScriptName, widgetType));
            sb.AppendLine("    {");
            sb.AppendLine("        private IU3dFrame mU3DFrame;");
            sb.AppendLine(string.Format("        private  IViewActionRequest<{0}> _viewActionRequest;", widgetType));
            sb.AppendLine("        private  IView mView;");
            sb.AppendLine();

            sb.AppendLine(string.Format("        public IViewActionResponser<{0}> Init(IU3dFrame frame,IViewActionRequest<{0}> request,IViewModel model)"
                , widgetType));
            sb.AppendLine("        {");
            sb.AppendLine("            mU3DFrame = frame;");
            sb.AppendLine("            mView = request.ActionRequester.Origin.AttachView;");
            sb.AppendLine("            return this;");
            sb.AppendLine("        }");
            sb.AppendLine();

            // 关注视图Id与关注视图开启状态
            sb.AppendCsharpNote("行为处理器关注的视图Id", null, null, "        ");
            sb.AppendLine("        public string ConcernedViewId =>" + "\"" + viewId + "\"" + ";");
            sb.AppendLine();
            sb.AppendCsharpNote("行为处理器关注的视图的开启状态", null, null, "        ");
            sb.AppendLine("        public bool IsConcernedViewClosed { get; set; }");
            sb.AppendLine();

            sb.AppendLine(string.Format("        public void ProcessRequest(IViewActionRequest<{0}> request)",
                widgetType));
            sb.AppendLine("        {");
            sb.AppendLine("            _viewActionRequest = request;");
            sb.AppendLine("        }");
            sb.AppendLine();

            Extensions.WriteStandardMethod(sb, "        public bool CheckProcessResult()", "        return true;", "    ");
            Extensions.WriteStandardMethod(sb, "        public void ProcessException(Exception ex)", null, "    ");
            sb.AppendLine(string.Format("        public {0} Origin  { get { return _viewActionRequest.ActionRequester.Origin; } }",
                widgetType));
            sb.AppendLine();

            sb.AppendLine("    }");
            sb.AppendLine("}");

            sb.AppendLine("     ");

            var son = RootConfig.GetCurrentSonProject();
            FileUtility.EnsureDirExist(son.ViewActionReplaceDataPath);
            if (File.Exists(son.ViewActionReplaceDataPath))
            {
                var bytes = File.ReadAllBytes(son.ViewActionReplaceDataPath);
                var replaceDic = SerializeUitlity.DeSerialize<Dictionary<string, string>>(bytes);
                if (replaceDic.ContainsKey(sourceName))
                {
                    replaceDic[sourceName] = mScriptName;
                }
                else
                {
                    replaceDic.Add(sourceName, mScriptName);
                }
                var newBytes = SerializeUitlity.Serialize(replaceDic);
                File.WriteAllBytes(son.ViewActionReplaceDataPath, newBytes);
            }
            else
            {
                var newDic = new Dictionary<string, string>();
                newDic.Add(sourceName, mScriptName);
                var bytes = SerializeUitlity.Serialize(newDic);
                File.WriteAllBytes(son.ViewActionReplaceDataPath, bytes);
            }

            var targetScriptPath = RootConfig.GetCurrentSonProject().CsMvdaDir + mScriptName + ".cs";
            FileUtility.EnsureDirExist(targetScriptPath);
            File.WriteAllText(targetScriptPath, sb.ToString());
            AssetDatabase.Refresh();
            _window.Close();
            _window = null;
        }



        /// <summary>
        /// 创建替换类型实例
        /// </summary>
        private string CreateResponserInstance(Type type)
        {
            return mFuncList.Select(func => func(type)).FirstOrDefault(result => result != null);
        }

        private string CreateResponser_IButton(Type type)
        {
            var instance = Activator.CreateInstance(type) as IViewActionResponser<IButton>;
            if (instance == null)
            {
                return null;
            }

            var id = instance.ConcernedViewId;
            var result = "IButton@" + id;
            return result;
        }

        private string CreateResponser_IToggle(Type type)
        {
            var instance = Activator.CreateInstance(type) as IViewActionResponser<IToggle>;
            if (instance == null)
            {
                return null;
            }

            var id = instance.ConcernedViewId;
            var result = "IToggle@" + id;
            return result;
        }

        private string CreateResponser_IView(Type type)
        {
            var instance = Activator.CreateInstance(type) as IViewActionResponser<IView>;
            if (instance == null)
            {
                return null;
            }

            var id = instance.ConcernedViewId;
            var result = "IView@" + id;
            return result;
        }

        private string CreateResponser_ISlider(Type type)
        {
            var instance = Activator.CreateInstance(type) as IViewActionResponser<ISlider>;
            if (instance == null)
            {
                return null;
            }

            var id = instance.ConcernedViewId;
            var result = "ISlider@" + id;
            return result;
        }



    }
}