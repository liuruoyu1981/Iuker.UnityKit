using System.Collections.Generic;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.LinqExtensions;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA.ScriptCreate.Jint
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MVDACreate_ContainerAndConstant_Jint : MVDAJintCreateBase
    {
        public void CreateTypeScriptViewContainerAndConstant()
        {
            var targetDir = mSelectSon.TsProjectMvdaDir;
            var containerPath = targetDir + string.Format("{0}/", mViewRoot.name) + mViewRoot.name + "_jint.ts";
            var constantPath = targetDir + string.Format("{0}/", mViewRoot.name) + mViewRoot.name + "_constant_jint.ts";

            CreateJintContainer(mSelectSon, containerPath);
            CreateJintConstant(mSelectSon, constantPath);
            Debug.Log(string.Format("视图{0}的Ts容器及常量脚本已成功创建！", mViewRoot.name));
            AssetDatabase.Refresh();
        }

        private void CreateJintContainer(SonProject son, string targetScriptPath)
        {
            var sb = new StringBuilder().AppendTypeScriptFileNode(EditorConstant.HostClientName,
                EditorConstant.HostClientEmail, "视图容器Typescript脚本（使用Jint执行引擎）。");
            WriteNameSpaceHeader(sb);
            sb.AppendLine();
            sb.AppendLine(string.Format("    export class {0} ", ClassName + "_jint") + "{");
            sb.AppendLine();
            sb.AppendLine("        InitViewWidgets() {");
            sb.AppendLine();
            sb.AppendLine(string.Format("            let v = Iuker.ViewModule.GetJintView('{0}');", ClassName));
            sb.AppendLine();
            AppendWidgetCodeStr(sb, mContainers, "GetJintContainer", "获取容器控件");
            AppendWidgetCodeStr(sb, mButtons, "GetJintButton", "获取按钮控件");
            AppendWidgetCodeStr(sb, mTexts, "GetJintText", "获取文本控件");
            AppendWidgetCodeStr(sb, mInputFields, "GetJintInputField", "获取输入框控件");
            AppendWidgetCodeStr(sb, mImageList, "GetJintImage", "获取图片控件");
            AppendWidgetCodeStr(sb, mRawImages, "GetJintRawImage", "获取原始图片控件");
            AppendWidgetCodeStr(sb, mToggles, "GetJintToggle", "获取开关控件");
            AppendWidgetCodeStr(sb, mSliders, "GetJintSlider", "获取滑动器控件");

            sb.AppendLine();
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            FileUtility.WriteAllText(targetScriptPath, sb.ToString());
            TsProj.AddLine("Mvda" + "\\" + mViewRoot.name + "\\" + ClassName + "_jint", son).UpdateToFile(son.TsProjPath);
        }

        private void CreateJintConstant(SonProject son, string targetScriptPath)
        {
            var sb = new StringBuilder().AppendTypeScriptFileNode(EditorConstant.HostClientName,
                EditorConstant.HostClientEmail, "视图常量Typescript脚本（使用Jint执行引擎）。");

            WriteNameSpaceHeader(sb);
            sb.AppendLine();
            sb.AppendLine(string.Format("    export class {0}_constant_jint ", ClassName) + "{");
            sb.AppendLine();
            mContainers.ForEach(e => WriteFieldConstant(sb, e));
            mButtons.ForEach(e => WriteFieldConstant(sb, e));
            mTexts.ForEach(e => WriteFieldConstant(sb, e));
            mInputFields.ForEach(e => WriteFieldConstant(sb, e));
            mImageList.ForEach(e => WriteFieldConstant(sb, e));
            mRawImages.ForEach(e => WriteFieldConstant(sb, e));
            mToggles.ForEach(e => WriteFieldConstant(sb, e));
            mSliders.ForEach(e => WriteFieldConstant(sb, e));
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");

            FileUtility.WriteAllText(targetScriptPath, sb.ToString());
            TsProj.AddLine("Mvda" + "\\" + mViewRoot.name + "\\" + ClassName + "_constant_jint", son).UpdateToFile(son.TsProjPath);
        }

        private void WriteFieldConstant(StringBuilder sb, string name)
        {
            var targetGo = mWidgetsDictionary[name];
            var path = GetWidgetPath(targetGo, targetGo.Parent(), "");
            sb.AppendLine(string.Format("        public static {0}: string = {1};", name, path));
        }

        private void AppendWidgetCodeStr(StringBuilder sb, List<string> elementList, string methodName,
            string node)
        {
            sb.AppendLine(string.Format("            //   {0}", node));
            elementList.ForEach(e =>
            {
                var targetGo = mWidgetsDictionary[e];
                var getPath = GetWidgetPath(targetGo, targetGo.Parent(), "");
                sb.AppendLine(string.Format("            v.{0}({1});", methodName, getPath));
            });
            sb.AppendLine();
        }

    }
}