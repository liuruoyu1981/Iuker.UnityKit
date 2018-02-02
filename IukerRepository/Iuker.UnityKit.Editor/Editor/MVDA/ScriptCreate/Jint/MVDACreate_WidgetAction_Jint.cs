using System.IO;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using UnityEditor;

namespace Iuker.UnityKit.Editor.MVDA.ScriptCreate.Jint
{
    public class MVDACreate_WidgetAction_Jint : MVDAJintCreateBase
    {
        public static void CreateScript(string actionType)
        {
            new MVDACreate_WidgetAction_Jint().CreateTypeScriptWidgetActionScript(actionType);
        }

        public void CreateTypeScriptWidgetActionScript(string actionType)
        {
            var className = mViewRoot.name + "_" + mSelectedWidget.name + actionType.ToLower() + "_jint";
            var sb = new StringBuilder().AppendTypeScriptFileNode(EditorConstant.HostClientName,
                EditorConstant.HostClientEmail, "视图交互行为处理Javascript脚本（使用Jint执行引擎）。");
            WriteNameSpaceHeader(sb);
            sb.AppendLine();
            sb.AppendLine(string.Format("    export class {0} ", className) + "{");
            sb.AppendLine();

            sb.AppendLine("        private view: IJintView;");
            sb.AppendLine(string.Format("        private model: {0}_model_jint;", mViewRoot.name));
            sb.AppendLine();

            sb.AppendLine("        //   该函数做一次性的初始化。");
            sb.AppendLine("        Init() " + "{");
            sb.AppendLine(string.Format("            this.view = Iuker.ViewModule.GetJintView('{0}');", mViewRoot.name));
            sb.AppendLine(string.Format(
                "            this.model = Iuker.ViewModule.GetViewModel('{0}_model_jint',{1}_model_jint);",
                mViewRoot.name, mViewRoot.name));
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        //   该函数用于决定在执行脚本之前是否需要执行脚本自身替换的目标Csharp脚本，执行为True,不执行为False。");
            sb.AppendLine("        IsDoCsharp() " + "{");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        //   在这里处理目标视图行为请求。");
            sb.AppendLine("        ProcessRequest() " + "{");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            var targetDir = mSelectSon.TsProjectDir;
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
            var targetScriptPath = targetDir + string.Format("Mvda/{0}/WidgetAction/", mViewRoot.name) + className + ".ts";
            FileUtility.WriteAllText(targetScriptPath, sb.ToString());
            TsProj.AddLine("Mvda" + "\\" + mViewRoot.name + "\\" + "WidgetAction" + "\\" + className, mSelectSon).UpdateToFile(mSelectSon.TsProjPath);
            AssetDatabase.Refresh();
        }




    }
}