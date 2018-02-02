using System.IO;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using UnityEditor;

namespace Iuker.UnityKit.Editor.MVDA.ScriptCreate.Jint
{
    public class MVDACreate_Model_Jint : MVDAJintCreateBase
    {
        public void CreateTypeScriptViewModelScript()
        {
            var className = mViewRoot.name + "_" + "model_jint";
            var sb = new StringBuilder().AppendTypeScriptFileNode(EditorConstant.HostClientName,
                EditorConstant.HostClientEmail, "视图数据模型Javascript脚本（使用Jint执行引擎）。");
            WriteNameSpaceHeader(sb);
            sb.AppendLine();
            sb.AppendLine(string.Format("    export class {0} ", className) + "{");
            sb.AppendLine();

            sb.AppendLine("        //   该函数做一次性的初始化。");
            sb.AppendLine("        Init() " + "{");
            sb.AppendLine("            ");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        //   该函数用于决定在执行脚本之前是否需要执行脚本自身替换的目标Csharp脚本，执行为True,不执行为False。");
            sb.AppendLine("        IsDoCsharp() " + "{");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("    }");
            sb.AppendLine("}");

            var targetDir = mSelectSon.TsProjectDir;
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
            var targetScriptPath = targetDir + string.Format("Mvda/{0}/", mViewRoot.name) + className + ".ts";
            FileUtility.WriteAllText(targetScriptPath, sb.ToString());
            TsProj.AddLine("Mvda" + "\\" + mViewRoot.name + "\\" + className, mSelectSon).UpdateToFile(mSelectSon.TsProjPath);
            AssetDatabase.Refresh();
        }




    }
}