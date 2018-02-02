using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iuker.Common;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.LinqExtensions;
using Iuker.UnityKit.Run.Module.View.Assist;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA
{
    public class MVDACsCreater
    {
        #region base

        public GameObject mSelectedWidget;
        public GameObject mViewRoot;
        protected string mClassName;

        private bool IsCell
        {
            get
            {
                return mSelectedWidget.name.Contains("cell");
            }
        }

        private readonly Dictionary<string, Type> UnityDefualtNameDictionary = new Dictionary<string, Type>
        {
            {"Text",typeof( UnityEngine.UI.Text) },
            {"Button",typeof( UnityEngine.UI.Button) },
            {"InputField",typeof(UnityEngine.UI.InputField) },
            {"Toggles",typeof(UnityEngine.UI.Toggle) },
            {"Image",typeof(UnityEngine.UI.Image) },
            {"RawImage",typeof(UnityEngine.UI.RawImage) },
            {"Slider",typeof(UnityEngine.UI.Slider) },
            {"Dropdown",typeof(UnityEngine.UI.Dropdown) }
        };
        private static readonly HashSet<string> ViewRootHashSet = new HashSet<string>
        {
            "view_background_root",
            "view_normal_root",
            "view_parasitic_root",
            "view_popup_root",
            "view_top_root",
            "view_mask_root",
        };

        protected List<string> mContainers,
            mButtons,
            mTexts,
            mInputFields,
            mImageList,
            mRawImages,
            mToggles,
            mSliders,
            mTabgroups,
            mListViews,
            mToggleGrouops;

        protected readonly Dictionary<string, GameObject> mWidgetsDictionary
            = new Dictionary<string, GameObject>();
        protected readonly SonProject mSelectSon;
        private StringBuilder Sb;

        public MVDACsCreater()
        {
            mWidgetsDictionary.Clear();
            mSelectedWidget = Selection.activeGameObject;
            mSelectedWidget.FindViewRoot(out mViewRoot);
            var mTempSonName = mViewRoot.name.Split('_').ToList().SelectUnion("_", 1, 2);
            mTempSonName = mTempSonName.Substring(1, mTempSonName.Length - 1);
            mSelectSon = RootConfig.Instance.AllProjectsSons.Find(s => s.CompexName.ToLower() == mTempSonName) ?? RootConfig.GetCurrentSonProject();

            switch (mSelectedWidget.name)
            {
                case "view_background_root":
                case "view_normal_root":
                case "view_parasitic_root":
                case "view_popup_root":
                case "view_top_root":
                case "view_mask_root":
                    EditorUtility.DisplayDialog("错误", "不能选择视图分层挂载根对象", "确定");
                    return;
            }

            SaveAllViewWidgets(mSelectedWidget, mWidgetsDictionary);
        }

        private void SaveAllViewWidgets(GameObject root, Dictionary<string, GameObject> elementDictionary)
        {
            var trm = root.transform;
            var i = 0;
            var childCount = trm.childCount;
            while (i < childCount)
            {
                var currentGo = trm.GetChild(i).gameObject;
                if (!elementDictionary.ContainsKey(currentGo.name)
                    && !UnityDefualtNameDictionary.ContainsKey(currentGo.name) // 不是unity3d UI组件的默认名
                )   // 不是容器
                {
                    elementDictionary.Add(currentGo.name, currentGo.gameObject);
                }
                SaveAllViewWidgets(currentGo.gameObject, elementDictionary);
                i++;
            }
            var keys = mWidgetsDictionary.Keys.ToList();
            // 分别保存所有类型控件名至对应列表
            mContainers = keys.FindAll(r => r.StartsWith("container"));
            mButtons = keys.FindAll(r => r.StartsWith("button") && r != "button_root");
            mTexts = keys.FindAll(r => r.StartsWith("text") && !r.StartsWith("textmesh"));
            mInputFields = keys.FindAll(r => r.StartsWith("inputfield"));
            mImageList = keys.FindAll(r => r.StartsWith("image"));
            mRawImages = keys.FindAll(r => r.StartsWith("rawimage"));
            mToggles = keys.FindAll(r => r.StartsWith("toggle"));
            mSliders = keys.FindAll(r => r.StartsWith("slider"));
            mTabgroups = keys.FindAll(r => r.StartsWith("tabgroup"));
            mListViews = keys.FindAll(r => r.StartsWith("listview"));
            mToggleGrouops = keys.FindAll(r => r.StartsWith("togglegroup"));
        }

        protected static string GetWidgetPath(GameObject source, GameObject parent, string path)
        {
            while (true)
            {
                var parentName = parent.name;
                var grandfatherName = parent.Parent().name;
                if (!parentName.StartsWith("view"))
                {
                    path = parentName + "/" + path;
                    parent = parent.Parent();
                    continue;
                }
                if (!ViewRootHashSet.Contains(grandfatherName))
                {
                    path = parentName + "/" + path;
                    parent = parent.Parent();
                    continue;
                }
                var result = "\"" + path + source.name + "\"";
                return result;
            }
        }


        private readonly List<WidgetPathInfo> mWidgetPaths = new List<WidgetPathInfo>();

        private void WriteViewWidgetCode(string node, string interfaceType, List<string> elementList, string dictionaryName)
        {
            Sb.AppendLine(string.Format("        // {0}", node));
            elementList.ForEach(r =>
            {
                var targetObj = mWidgetsDictionary[r];
                var getPath = GetWidgetPath(targetObj, targetObj.Parent(), "");
                mWidgetPaths.Add(new WidgetPathInfo(targetObj.name, getPath));
                Sb.AppendLine(string.Format("        InitViewWidget<{1}>({2}).AddTo({2},{3});", r, interfaceType, getPath, dictionaryName));
            });
            Sb.AppendLine();
        }

        public void WriteViewModelNameSpace()
        {
            Sb.AppendLine("using Iuker.UnityKit.Run.Base;");
            Sb.AppendLine("using Iuker.UnityKit.Run.Module.View.MVDA;");
            Sb.AppendLine();
        }

        private string genericType;

        #endregion

        #region Processer

        public void TryCreateAllProcesserScript()
        {
            mButtons.ForEach(name => CreateProcesserScript<IButton>("OnClick", ViewScriptType.WidgetAction, mWidgetsDictionary[name]));
            mToggles.ForEach(name => CreateProcesserScript<IToggle>("OnValueChange", ViewScriptType.WidgetAction, mWidgetsDictionary[name]));
            mSliders.ForEach(name => CreateProcesserScript<ISlider>("OnValueChange", ViewScriptType.WidgetAction, mWidgetsDictionary[name]));
            mInputFields.ForEach(name => CreateProcesserScript<IInputField>("OnValueChange", ViewScriptType.WidgetAction, mWidgetsDictionary[name]));
        }

        public void CreateProcesserScript<T>(string actionName, ViewScriptType type =
         ViewScriptType.WidgetAction, GameObject widget = null) where T : IViewElement
        {
            if (widget == null) widget = mSelectedWidget;
            if (!CheckWidgetTypeExist<T>(widget)) return;

            Sb = new StringBuilder();
            genericType = typeof(T).Name;
            mClassName = IsCell ? GetCellStringName(mViewRoot, widget, actionName) :
                GetProcesserClassName(type, widget, actionName);

            Sb.WriteFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail,
                "视图行为处理器脚本，在这里实现视图控件交互、视图生命周期的行为处理逻辑。");
            AppendProcesserRefNameSpace();
            AppendProcesserDefineNameSpace();
            AppendProcesserHeaderCode();
            AppendFiledDefineCode();
            AppendProcesserInitMethodCode();
            AppendProcesserConcernedViewIdCode();
            AppendProcessRequestMethodCode(actionName);
            Sb.WriteStandardMethod("        public bool CheckProcessResult()", "        return true;", "        ");
            Sb.WriteStandardMethod("        public void ProcessException(Exception ex)", null, "        ");
            Sb.Append(string.Format("        public {0} Origin ", genericType) + "{ get { return _viewActionRequest.ActionRequester.Origin; } }");
            Sb.AppendLine();

            Sb.AppendLine("    }");
            Sb.AppendLine("}");
            // 生成脚本文件
            var targetScriptPath = mSelectSon.CsMvdaDir + mViewRoot.name + "/" + type + "/" + mClassName + ".cs";
            if (File.Exists(targetScriptPath))
            {
                EditorUtility.DisplayDialog("提示", "目标路径上脚本已存在！", "确定");
                return;
            }

            FileUtility.WriteAllText(targetScriptPath, Sb.ToString());
        }

        private string GetCellStringName(GameObject viewRoot, GameObject widget, string actionName)
        {
            var name = widget.name;
            string result;

            if (char.IsDigit(name.Last())
                && name[name.Length - 2] == '_')
            {
                result = viewRoot.name + "_" + name.Substring(0, name.Length - 2)
                         + "_" + actionName;
            }
            else
            {
                result = viewRoot.name + "_" + name
                         + "_" + actionName;
            }

            return result;
        }

        private void AppendProcesserRefNameSpace()
        {
            Sb.AppendLine("using System;");
            Sb.AppendLine("using Iuker.Common;");
            Sb.AppendLine("using Iuker.UnityKit.Run.Module.View.MVDA;");
            Sb.AppendLine("using Iuker.UnityKit.Run.Module.View.ViewWidget;");
            Sb.AppendLine("using Iuker.UnityKit.Run.Base;");
            Sb.AppendLine();
        }

        private void AppendProcesserHeaderCode()
        {
            Sb.AppendLine(string.Format("    public class {0} : IViewActionResponser<{1}>", mClassName, genericType));
            Sb.AppendLine("    {");
        }

        private void AppendProcesserDefineNameSpace()
        {
            Sb.AppendLine(string.Format("namespace {0}", mSelectSon.CompexName));
            Sb.AppendLine("{");
        }

        private bool CheckWidgetTypeExist<T>(GameObject widget) where T : IViewElement
        {
            if (typeof(T).Name == "IView")
            {
                var assister = widget.GetComponent<ViewAssister>();
                if (assister != null) return true;

                EditorUtility.DisplayDialog("错误", string.Format("目标对象{0}不是视图！", widget.name), "确定");
                return false;
            }

            var component = widget.GetComponent<T>();
            if (component != null) return true;

            EditorUtility.DisplayDialog("错误", string.Format("目标对象{0}上并不存在{1}的组件！", widget.name, typeof(T).Name), "确定");
            return false;
        }

        private void AppendFiledDefineCode()
        {
            Sb.AppendLine("        private IU3dFrame mU3DFrame;");
            Sb.AppendLine(string.Format("        private IViewActionRequest<{0}> _viewActionRequest;", genericType));
            Sb.AppendLine("        private IView mView;");
            Sb.AppendLine();
        }

        private void AppendProcesserConcernedViewIdCode()
        {
            Sb.WriteNote("行为处理器关注的视图Id", null, null, "        ");
            Sb.AppendLine("        public string ConcernedViewId { get { return " + "\"" + mViewRoot.name + "\"" + ";} }");
            Sb.AppendLine();
            Sb.WriteNote("行为处理器关注的视图的开启状态", null, null, "        ");
            Sb.AppendLine("        public bool IsConcernedViewClosed { get; set; }");
            Sb.AppendLine();
        }

        private void AppendProcesserInitMethodCode()
        {
            Sb.AppendLine(string.Format("        public IViewActionResponser<{0}> Init(IU3dFrame frame, IViewActionRequest<{0}> request, IViewModel model)"
                , genericType));
            Sb.AppendLine("        {");
            Sb.AppendLine("            mU3DFrame = frame;");
            Sb.AppendLine("            mView = request.ActionRequester.Origin.AttachView;");
            Sb.AppendLine("            return this;");
            Sb.AppendLine("        }");
            Sb.AppendLine();
        }

        private void AppendProcessRequestMethodCode(string actionName)
        {
            Sb.AppendLine(string.Format("        public void ProcessRequest(IViewActionRequest<{0}> request)",
                genericType));
            Sb.AppendLine("        {");
            Sb.AppendLine("            _viewActionRequest = request;");
            Sb.AppendLine("#if UNITY_EDITOR || DEBUG");
            Sb.AppendLine(string.Format("            Debuger.Log({0}{1}_{2} is  {3}{0});", "\"", mSelectedWidget.name, mSelectedWidget.name, actionName));
            Sb.AppendLine("#endif");
            Sb.AppendLine("        }");
            Sb.AppendLine();
        }

        private string GetProcesserClassName(ViewScriptType type, GameObject widget, string actionName)
        {
            string classname;
            switch (type)
            {
                case ViewScriptType.ViewPipeline:
                    classname = mViewRoot.name + "_" + actionName;
                    break;
                case ViewScriptType.WidgetAction:
                    classname = mViewRoot.name + "_" + widget.name + "_" + actionName;
                    break;
                case ViewScriptType.ViewDraw:
                    classname = mViewRoot.name + "_" + widget.name + "_Draw" + "_" + actionName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
            return classname;
        }

        #endregion

        #region Container

        public void CreateViewConstantScript()
        {
            Sb = new StringBuilder();
            Sb.WriteFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail);
            Sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
            Sb.AppendLine("{");
            Sb.AppendLine(string.Format("    public class {0}_Constant", mSelectedWidget.name));
            Sb.AppendLine("    {");
            mWidgetPaths.ForEach(r => Sb.AppendLine(string.Format("        public const string {0} = {1};", r.Name,
                r.Path)));
            Sb.AppendLine("    }");
            Sb.AppendLine("}");
            // 生成脚本文件
            if (!Directory.Exists(ContainerAndConstantDir)) Directory.CreateDirectory(ContainerAndConstantDir);
            var targetScriptPath = ContainerAndConstantDir + mViewRoot.name + "_Constant.cs";
            File.WriteAllText(targetScriptPath, Sb.ToString());
            mWidgetPaths.Clear();
        }

        private void WriteViewScriptNameSpace()
        {
            Sb.AppendLine("using Iuker.UnityKit.Run.Module.View.MVDA;");
            Sb.AppendLine("using Iuker.UnityKit.Run.ViewWidget;");
            Sb.AppendLine("using UnityEngine;");
            Sb.AppendLine("using Iuker.UnityKit.Run.Module.View.ViewWidget;");
            Sb.AppendLine();
        }

        public void CreateContainerScript()
        {
            Sb = new StringBuilder();
            Debug.Log(string.Format("视图{0}下符合命名规则的视图控件数量为：", mSelectedWidget.name) + mWidgetsDictionary.Count);
            Sb.WriteFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail);
            WriteViewScriptNameSpace();
            Sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
            Sb.AppendLine("{");
            Sb.AppendLine(string.Format("public class {0} : AbsViewBase", mSelectedWidget.name));
            Sb.AppendLine("{");
            WriteViewActionConstant();

            Sb.AppendLine("    #region 视图控件字段");
            Sb.AppendLine("    #endregion");
            Sb.AppendLine();

            // 获取控件脚本代码生成
            CreateGetViewWidgets();

            Sb.AppendLine("    #region 视图生命周期");
            WriteViewOverride();
            Sb.AppendLine("    #endregion");

            Sb.AppendLine("}");
            Sb.AppendLine("}");

            // 生成脚本文件
            if (!Directory.Exists(ContainerAndConstantDir)) Directory.CreateDirectory(ContainerAndConstantDir);
            var targetScriptPath = ContainerAndConstantDir + mViewRoot.name + ".cs";
            File.WriteAllText(targetScriptPath, Sb.ToString());
        }

        private string ContainerAndConstantDir
        {
            get
            {
                return mSelectSon.CsMvdaDir + mViewRoot.name + "/ContainerAndConstant/";
            }
        }

        private void WriteViewActionConstant()
        {
            var viewHoldStr = "\"" + mSelectedWidget.name;
            Sb.AppendLine("    // 视图行为字符串常量，避免临时字符串反复构建消耗。");
            Sb.AppendLine(string.Format("    private const string _onBeforeCreatActionToken = {0}_BeforeCreat",
                              viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onCreatedActionToken = {0}_OnCreated", viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onBeforeActiveActionToken = {0}_BeforeActive",
                              viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onActivedActionToken = {0}_OnActived", viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onBeforeHideActionToken = {0}_BeforeHide",
                              viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onHidedActionToken = {0}_OnHided", viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onBeforeCloseActionToken = {0}_BeforeClose",
                              viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onClosedActionToken = {0}_OnClosed", viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onCreatedDrawActionToken = {0}_Draw_OnCreated",
                              viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onActivedDrawActionToken = {0}_Draw_OnActived",
                              viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onHideDrawActionToken = {0}_Draw_OnHide",
                              viewHoldStr) + "\";");
            Sb.AppendLine(string.Format("    private const string _onCloseDrawActionToken = {0}_Draw_OnClose",
                              viewHoldStr) + "\";");
            Sb.AppendLine();
        }


        private void CreateGetViewWidgets()
        {
            Sb.AppendLine("    protected override void InitViewWidgets()");
            Sb.AppendLine("    {");
            Sb.AppendLine("        base.InitViewWidgets();");
            Sb.AppendLine();

            // 容器对象
            Sb.AppendLine("        // 容器对象");
            mContainers.ForEach(r =>
            {
                var target = mWidgetsDictionary[r];
                var getPath = GetWidgetPath(target, target.Parent(), "");
                mWidgetPaths.Add(new WidgetPathInfo(target.name, getPath));
                Sb.AppendLine(string.Format("        RectRoot.Find({0}).gameObject.AddTo({0},ContainerDictionary);", getPath));
            });
            Sb.AppendLine();

            WriteViewWidgetCode("按钮对象", typeof(IButton).Name, mButtons, "ButtonDictionary");     // 按钮对象
            WriteViewWidgetCode("文本对象", typeof(IText).Name, mTexts, "TextDictionary");     // 文本对象
            WriteViewWidgetCode("输入框对象", typeof(IInputField).Name, mInputFields, "InputFieldDictionary");     // 输入框对象
            WriteViewWidgetCode("Image对象", typeof(IImage).Name, mImageList, "ImageDictionary"); //  图片对象
            WriteViewWidgetCode("RawImage对象", typeof(IRawImage).Name, mRawImages, "RawImageDictionary"); //  原始图片对象
            WriteViewWidgetCode("Toggle对象", typeof(IToggle).Name, mToggles, "ToggleDictionary"); //  开关对象
            WriteViewWidgetCode("Slider对象", typeof(ISlider).Name, mSliders, "SliderDictionary"); //  滑动杆对象
            WriteViewWidgetCode("TabGroup对象", typeof(ITabGroup).Name, mTabgroups, "TabGroupDictionary"); //  滑动杆对象
            WriteViewWidgetCode("ListView对象", typeof(IListView).Name, mListViews, "ListViewDictionary"); //  滚动列表对象
            WriteViewWidgetCode("ToggleGroup对象", typeof(IToggleGroup).Name, mToggleGrouops, "ToggleGrouopDictionary"); //  滚动列表对象

            Sb.AppendLine("    }");
            Sb.AppendLine();
            Sb.AppendLine();
        }


        private void WriteViewOverride()
        {
            Sb.WriteStandardMethod("    protected override void BeforeCreat()", @"
        base.BeforeCreat();
        var onCreateRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeCreatActionToken,ViewScriptType.ViewPipeline);
        Issue(onCreateRequest);
");

            Sb.WriteStandardMethod("    protected override void OnCreated()", @"
        base.OnCreated();
        var onCreatedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onCreatedActionToken,ViewScriptType.ViewPipeline);
        Issue(onCreatedRequest);
        var onCreatedDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onCreatedDrawActionToken,ViewScriptType.ViewDraw);
        Issue(onCreatedDrawRequest);   
");

            Sb.WriteStandardMethod("    protected override void BeforeActive()", @"
        base.BeforeActive();
        var onActiveRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeActiveActionToken,ViewScriptType.ViewPipeline);
        Issue(onActiveRequest);
");

            Sb.WriteStandardMethod("    protected override void OnActived()", @"
        base.OnActived();
        var onActivedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onActivedActionToken,ViewScriptType.ViewPipeline);
        Issue(onActivedRequest);
        var onActivedDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onActivedDrawActionToken,ViewScriptType.ViewDraw);
        Issue(onActivedDrawRequest);   
");

            Sb.WriteStandardMethod("    protected override void BeforeHide()", @"
        base.BeforeHide();
        var onHideRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeHideActionToken,ViewScriptType.ViewPipeline);
        Issue(onHideRequest);
        var onHideDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onHideDrawActionToken,ViewScriptType.ViewDraw);
        Issue(onHideDrawRequest);   
");

            Sb.WriteStandardMethod("    protected override void OnHided()", @"
        base.OnHided();
        var onHidedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onHidedActionToken,ViewScriptType.ViewPipeline);
        Issue(onHidedRequest);
");

            Sb.WriteStandardMethod("    protected override void BeforeClose()", @"
        base.BeforeClose();
        var onCloseRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onBeforeCloseActionToken,ViewScriptType.ViewPipeline);
        Issue(onCloseRequest);
        var onCloseDrawRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>()
            .Init(this, _onCloseDrawActionToken,ViewScriptType.ViewDraw);
        Issue(onCloseDrawRequest);   
");

            Sb.WriteStandardMethod("    protected override void OnClosed()", @"
        base.OnClosed();
        var onClosedRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, _onClosedActionToken,ViewScriptType.ViewPipeline);
        Issue(onClosedRequest);
");
        }

        #endregion
    }
}