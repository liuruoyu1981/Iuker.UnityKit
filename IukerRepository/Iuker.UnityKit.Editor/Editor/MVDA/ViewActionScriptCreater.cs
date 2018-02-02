using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Constant;
using Iuker.UnityKit.Run.LinqExtensions;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA
{
#if DEBUG
    /// <summary>
    /// 视图行为处理脚本创建器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170914 15:48:04")]
    [ClassPurposeDesc("视图行为处理脚本创建器", "视图行为处理脚本创建器")]
#endif
    public class ViewActionScriptCreater
    {
        private string mMarkStr;
        private Type mUiType;
        private string mInterfaceType;
        private string mActionType;
        private GameObject mViewRoot;

        private string mRootName
        {
            get { return mViewRoot.name; }
        }

        private List<GameObject> mWidgets;
        private CsharpScriptAppender mAppender;
        private GameObject mCurrentWidget;

        private readonly string[] mUsingNameSpaces =
        {
            "using System;",
            "using Iuker.Common;",
            "using Iuker.UnityKit.Run.Base;",
            "using Iuker.UnityKit.Run.Module.View.MVDA;",
            "using Iuker.UnityKit.Run.Module.View.ViewWidget;"
        };

        public static void CreateScript(string markStr, Type uiType, string actionType, string interfaceType)
        {
            var creater = new ViewActionScriptCreater();
            creater.CreateActionScript(markStr, uiType, actionType, interfaceType);
        }

        private string mCurrentWidgetName { get { return mCurrentWidget.name; } }

        private void CreateActionScript(string markStr, Type uiType, string actionType, string interfaceType)
        {
            mMarkStr = markStr;
            mUiType = uiType;
            mActionType = actionType;
            mInterfaceType = interfaceType;

            mWidgets = Selection.gameObjects.ToList();
            if (mWidgets.Count > 1)
            {
                EditorUtility.DisplayDialog("错误", "不能一次创建多个！", "确定");
                return;
            }

            if (mWidgets[0].name.StartsWith("view"))
            {
                EditorUtility.DisplayDialog("错误", "控件命名不能以view开头！", "确定");
                return;
            }

            if (mWidgets[0].GetComponent(uiType) == null)
            {
                EditorUtility.DisplayDialog("错误", string.Format("控件上不存在目标类型{0}组件！", markStr), "确定");
                return;
            }

            GetViweRoot();
            if (mWidgets.ToList().MatchAll(CheckMatch))
            {
                mWidgets.ForEach(go =>
                {
                    mCurrentWidget = go;
                    CreateActionScript(mCurrentWidgetName);
                });
            }
        }

        private void GetViweRoot()
        {
            ViewWidgetsExtensions.FindViewRoot(mWidgets[0].Parent(), out mViewRoot);
        }

        private bool CheckMatch(GameObject go)
        {
            return go.name.StartsWith(mMarkStr) && go.GetComponent(mUiType) != null;
        }

        private static string GetCellName(string goName)
        {
            var cellClassName = goName.EndsWith("cell")
                ? goName
                : goName.TrimTargetCharAfter("_");
            return cellClassName;
        }

        private void CreateActionScript(string widgetName)
        {
            if (IsCell(widgetName))
            {
                var cellClassName = GetCellName(widgetName);
                CreateActionResponserScript(cellClassName, mActionType, mInterfaceType);
            }
            else
            {
                CreateActionResponserScript(widgetName, mActionType, mInterfaceType);
            }
        }

        private bool IsCell(string name)
        {
            var endStr = name.Substring(0, name.LastIndexOf('_'));
            return name.EndsWith("cell") || endStr.EndsWith("cell");
        }

        private string ClassName
        {
            get
            {
                return IsCell(mCurrentWidgetName) ?
                     mRootName + "_" + mCurrentWidgetName + "_" + mActionType :
                     GetClassName(ViewScriptType.WidgetAction, mCurrentWidgetName, mActionType, Constant.GetTimeToken);
            }
        }

        /// <summary>
        /// 创建视图行为处理器脚本
        /// </summary>
        /// <param name="elementName">UI控件名</param>
        /// <param name="actionName">视图行为名</param>
        /// <param name="genericType"></param>
        private void CreateActionResponserScript(string elementName, string actionName, string genericType)
        {
            using (mAppender = new CsharpScriptAppender(EditorConstant.HostClientName, EditorConstant.HostClientEmail, ClassName, mUsingNameSpaces,
                string.Format("IViewActionResponser<{0}>", genericType), RootConfig.CrtProjectName, "class", "public", "视图行为处理器脚本，在这里实现视图控件交互、视图生命周期的行为处理逻辑。"))
            {
                mAppender.SetWritePath(ScriptPath); //  设置脚本的创建路径
                mAppender.AppendLine("        private IU3dFrame mU3DFrame;");
                mAppender.AppendLine(string.Format("        private  IViewActionRequest<{0}> _viewActionRequest;",
                    genericType));
                mAppender.AppendLine("        private  IView mView;");
                mAppender.AppendLine();

                mAppender.AppendLine(string.Format("        public IViewActionResponser<{0}> Init(IU3dFrame frame,IViewActionRequest<{0}> request,IViewModel model)"
                    , genericType));
                mAppender.AppendLine("        {");
                mAppender.AppendLine("            mU3DFrame = frame;");
                mAppender.AppendLine("            mView = request.ActionRequester.Origin.AttachView;");
                mAppender.AppendLine("            return this;");
                mAppender.AppendLine("        }");
                mAppender.AppendLine();

                // 关注视图Id与关注视图开启状态
                mAppender.WriteNote("行为处理器关注的视图Id", null, null, "        ");
                mAppender.AppendLine("        public string ConcernedViewId =>" + "\"" + mRootName + "\"" + ";");
                mAppender.AppendLine();
                mAppender.WriteNote("行为处理器关注的视图的开启状态", null, null, "        ");
                mAppender.AppendLine("        public bool IsConcernedViewClosed { get; set; }");
                mAppender.AppendLine();

                mAppender.AppendLine(
                    string.Format("        public void ProcessRequest(IViewActionRequest<{0}> request)", genericType));
                mAppender.AppendLine("        {");
                mAppender.AppendLine("            _viewActionRequest = request;");
                mAppender.AppendLine("#if UNITY_EDITOR || DEBUG");
                mAppender.AppendLine(string.Format("            Debuger.Log({0}{1}_{2} is  {3}{0});", "\"", mRootName, elementName, actionName));
                mAppender.AppendLine("#endif");
                mAppender.AppendLine("        }");
                mAppender.AppendLine();

                mAppender.WriteMethod("public bool CheckProcessResult()", "return true;");
                mAppender.WriteMethod("public void ProcessException(Exception ex)");
                mAppender.AppendLine(string.Format(
                    "        public {0} Origin   { get { return _viewActionRequest.ActionRequester.Origin; } }", genericType));
                mAppender.AppendLine();
            }

            AssetDatabase.Refresh();
        }

        private string ScriptPath
        {
            get
            {
                return TargetDir + ClassName + ".cs";
            }
        }

        private string TargetDir
        {
            get
            {
                return IsCell(mCurrentWidgetName)
                    ? RootConfig.GetCurrentSonProject().CsMvdaDir + mCurrentWidgetName + "/Cell/"
                    : RootConfig.GetCurrentSonProject().CsMvdaDir + mRootName + "/" + mMarkStr + "/" + mCurrentWidgetName + "/" + mActionType + "/";
            }
        }

        private string GetClassName(ViewScriptType type, string elementName, string actionName, string time)
        {
            string classname = null;
            switch (type)
            {
                case ViewScriptType.ViewPipeline:
                    classname = elementName + "_" + actionName + "_" + time;
                    break;
                case ViewScriptType.WidgetAction:
                    classname = mRootName + "_" + elementName + "_" + actionName + time;
                    break;
                case ViewScriptType.ViewDraw:
                    classname = mRootName + "_Draw" + "_" + actionName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
            return classname;
        }


    }
}
