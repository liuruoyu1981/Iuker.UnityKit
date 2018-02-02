using System;
using System.IO;
using System.Linq;
using System.Text;
using Iuker.Common;
using Iuker.UnityKit.Editor.Setting;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TempCreateScriptWindow : EditorWindow
    {
        private static TempCreateScriptWindow window;
        private static string mTargetDir;
        private static string targetRootNs;
        private static string mRepalceStrRun = "Assets/1_Iuker.UnityKit/IukerRepository/Run/";
        private static string mRepalceStrEditor = "Assets/1_Iuker.UnityKit/IukerRepository/Editor/";
        private static string mAuthor;
        private static string mEmail;
        private static string mTimeToken;
        private static string mScriptName = "DefaultScript";
        private static string mClassPurpose;

        public static void ShowWindow()
        {
            mAuthor = IukerEditorPrefs.GetString("HostClientName");
            mEmail = IukerEditorPrefs.GetString("HostClientEmail");
            mTimeToken = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

            var paths = Selection.assetGUIDs.Select(AssetDatabase.GUIDToAssetPath).Where(AssetDatabase.IsValidFolder).ToList();
            if (paths.Count > 1)
            {
                EditorUtility.DisplayDialog("", "不能同时选择多个目录进行该操作！", "确定");
                return;
            }

            mTargetDir = paths.First();
            if (mTargetDir.StartsWith(mRepalceStrEditor))
            {
                targetRootNs = mTargetDir.Replace(mRepalceStrEditor, "");
            }
            else if (mTargetDir.StartsWith(mRepalceStrRun))
            {
                targetRootNs = mTargetDir.Replace(mRepalceStrRun, "");
            }
            else
            {
                targetRootNs = mTargetDir.Replace("Assets/", "");
            }
            targetRootNs = targetRootNs.Replace('/', '.');

            window = GetWindow<TempCreateScriptWindow>();
            window.titleContent = new GUIContent("脚本创建");
            window.minSize = new Vector2(550, 300);
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
                mScriptName = EditorGUILayout.TextField(mScriptName);
            }
            using (new IukHorizontalLayout())
            {
                EditorGUILayout.LabelField("脚本用途");
                mClassPurpose = EditorGUILayout.TextField(mClassPurpose);
            }

            if (GUILayout.Button("创建"))
            {
                CreateScript();
            }

        }

        private static void CreateScript()
        {
            var sb = new StringBuilder();

            sb.AppendCsahrpFileInfo(mAuthor, mEmail);

            sb.AppendLine("using Iuker.Common.Base;");
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
            sb.AppendLine(string.Format("    public class {0}", mScriptName));
            sb.AppendLine("    {");
            sb.AppendLine("     ");
            sb.AppendLine("    }");

            sb.AppendLine("}");

            if (!mTargetDir.EndsWith("/")) mTargetDir += "/";
            mTargetDir = UnityEngine.Application.dataPath.Replace("Assets", "") + mTargetDir;
            var scriptPath = mTargetDir + mScriptName + ".cs";
            Debug.Log(scriptPath);

            var content = sb.ToString();
            File.WriteAllText(scriptPath, content);
            AssetDatabase.Refresh();
            window.Close();
        }












    }
}