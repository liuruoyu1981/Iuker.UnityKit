/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 11:16:55
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iuker.Common;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Context;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.Replace;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 默认视图模块
    /// </summary>
    public class DefaultU3dModule_View : AbsU3dModule, IU3dViewModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.View.ToString();
            }
        }

        public IViewActionDispatcher ViewActionispatcher { get; private set; }
        public IViewModelDispatcher ModelDispatcher { get; private set; }

        #region 基类覆写

        public override void Init(IFrame frame)
        {
            base.Init(frame);

            ProjectViews = new List<Base.Config.View>();

            mBackgroundRoot = GameObject.Find("view_background_root").GetComponent<RectTransform>();
            mNormalRoot = GameObject.Find("view_normal_root").GetComponent<RectTransform>();
            mParasiticRoot = GameObject.Find("view_parasitic_root").GetComponent<RectTransform>();
            mPopupRoot = GameObject.Find("view_popup_root").GetComponent<RectTransform>();
            mTopRoot = GameObject.Find("view_top_root").GetComponent<RectTransform>();
            mMaskRoot = GameObject.Find("view_mask_root").GetComponent<RectTransform>();

            viewTypes = ReflectionUitlity.GetTypeList<IView>(U3DFrame.ProjectAssemblys);
            ViewActionispatcher = new DefualtViewActionDispatcher().Init(U3DFrame);
            ModelDispatcher = new DefaultViewModelDispatcher().Init(U3DFrame);
        }

        protected override void OnHotUpdateComplete()
        {
            base.OnHotUpdateComplete();

            mAssetModule = U3DFrame.AssetModule;
            InitViewConfigs();
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();

            U3DFrame.EventModule.WatchEvent(U3dEventCode.View_InitFirst.Literals, CreateUpdateView);
        }

        #endregion

        #region 视图配置

        public List<Base.Config.View> ProjectViews { get; private set; }

        private void InitViewConfigs()
        {
            if (U3DFrame.AppContext.DevelopContextType != DevelopContextType.Device) return;

            var sons = Bootstrap.Instance.TotalSons;
            foreach (var son in sons)
            {
                TextAsset textAsset = null;

                if (File.Exists(son.ViewsConfigSandboxPath))
                {
                    var bundle = AssetBundle.LoadFromFile(son.ViewsConfigSandboxPath);
                    textAsset = bundle.LoadAsset<TextAsset>(son.ViewsConfigName);
                }
                else
                {
                    textAsset = Resources.Load<TextAsset>(son.ViewsConfigName);
                }

                if (textAsset == null)
                {
                    Debug.Log(string.Format("子项目{0}的视图配置加载结果为空！", son.CompexName));
                    continue;
                }

                var viewNodes = JsonUtility.FromJson<Views>(textAsset.text);
                ProjectViews.AddRange(viewNodes.ProjectViews);
            }
        }

        #endregion

        #region 视图挂载根

        private RectTransform mNormalRoot, mParasiticRoot, mPopupRoot, mBackgroundRoot, mMaskRoot, mTopRoot;

        #endregion

        #region 字段

        private List<Type> viewTypes;
        private readonly Dictionary<string, Type> viewTypeDictionary = new Dictionary<string, Type>();
        public List<ViewEventInfo> GetViewEventInfos(string viewId)
        {
            return mViewEventInfoDictionary.ContainsKey(viewId) ? mViewEventInfoDictionary[viewId] : null;
        }

        private IU3dRouterModule mRouterModule;

        private IU3dRouterModule RouterModule
        {
            get
            {
                return mRouterModule ?? (mRouterModule = U3DFrame.GetModule<IU3dRouterModule>());
            }
        }


        #endregion

        #region ViewStack

        private IView mTopView; // 置顶视图同一时刻只允许存在一个，所以无需使用视图栈管理
        private IView mBackgroundView;  //   背景视图同一时刻只允许存在一个，所以无需使用视图栈管理
        private IView mMaskView;
        private readonly Stack<IView> normalStack = new Stack<IView>();
        private readonly Stack<IView> popupStack = new Stack<IView>();
        private readonly Stack<IView> parasiticStack = new Stack<IView>();

        #endregion

        #region Create

        /// <summary>
        /// 处于激活状态的视图实例字典
        /// </summary>
        private readonly Dictionary<string, IView> activeViewDictionary = new Dictionary<string, IView>();

        private void CreateUpdateView()
        {
            var assetId = string.IsNullOrEmpty(Bootstrap.Instance.UpdateViewAssetId)
                ? "view_" + Bootstrap.Instance.EntryProject.ProjectName.ToLower() + "_common_update"
                : Bootstrap.Instance.UpdateViewAssetId;
            var prefab = Resources.Load<GameObject>(assetId);
            var viewRef = new ViewRef(prefab);
            var view = new view_default_update();
            view.Init("view_default_update", viewRef);
        }

        public virtual void CreateView(string viewId, string assetid = null, bool isCache = true)
        {
            if (CreateViewErrorCheck(viewId)) return;

            ModelDispatcher.GetModel(viewId);
            ViewRef viewRef;
            string containerParserScriptId = null;
            var routeAssetId = RouterModule.GetAssetRoute(viewId);
            var finallyAssetId = routeAssetId ?? assetid;
            var typeRouterId = RouterModule.GetTypeRoute(viewId);
            containerParserScriptId = typeRouterId ?? viewId;

            if (!viewTypeDictionary.ContainsKey(viewId))
            {
                var types = viewTypes.FindAll(r => r.Name.Contains(containerParserScriptId)).OrderBy(r => r.Name).ToList();
                if (types.Count == 0)
                {
                    Debug.LogError(string.Format("请求的视图Id：{0}, 没有找到相关的视图容器脚本，请先创建对应的视图脚本（Base菜单项）！", viewId));
                    return;
                }

                var type = types[0];
                viewTypeDictionary.Add(viewId, type);
                viewRef = LoadView(finallyAssetId, isCache);
                var instance = Activator.CreateInstance(type) as IView;
                if (instance != null)
                {
                    instance.Init(viewId, viewRef);
                }
            }
            else
            {
                var type = viewTypeDictionary[viewId];
                viewRef = LoadView(finallyAssetId, isCache);
                var instance = Activator.CreateInstance(type) as IView;
                if (instance != null)
                {
                    instance.Init(viewId, viewRef);
                }
            }
        }

        private bool CreateViewErrorCheck(string viewId)
        {
            if (viewId == null)
            {
                Debuger.LogError("视图Id为空！");
                return true;
            }

            if (activeViewDictionary.ContainsKey(viewId))
            {
                Debuger.Log(string.Format("待创建的目标视图{0}当前已存在，视图行为唯一码为{1}", viewId,
                    U3DFrame.AppContext.ViewContext.ActionToken));
                var targetView = activeViewDictionary[viewId];
                if (!targetView.RectRoot.gameObject.activeInHierarchy)
                {
                    targetView.RectRoot.gameObject.SetActive(true);
                }
                return true;
            }

            return false;
        }

        private IU3dAssetModule mAssetModule;

        protected ViewRef LoadView(string assetId, bool isCache = true)
        {
            if (mViewRefs.ContainsKey(assetId))
            {
                return mViewRefs[assetId];
            }

            var viewRef = mAssetModule.LoadView(assetId);
            if (viewRef == null)
                throw new NullReferenceException(string.Format("目标视图资源{0}加载失败！", assetId));

            if (isCache)
            {
                mViewRefs.Add(assetId, viewRef);
            }

            return viewRef;
        }

        private readonly Dictionary<string, ViewRef> mViewRefs = new Dictionary<string, ViewRef>();


        #endregion

        #region GetView

        public virtual IView GetView(string viewId)
        {
            IView view = null;

            if (activeViewDictionary.ContainsKey(viewId))
            {
                view = activeViewDictionary[viewId];
            }
            return view;
        }


        public List<IView> GetAllByType(ViewType type, Func<IView, bool> selecter)
        {
            var views = new List<IView>();

            switch (type)
            {
                case ViewType.Background:
                    views.Add(mBackgroundView);
                    break;
                case ViewType.Normal:
                    views.AddRange(normalStack);
                    break;
                case ViewType.Parasitic:
                    views.AddRange(parasiticStack);
                    break;
                case ViewType.Popup:
                    views.AddRange(popupStack);
                    break;
                case ViewType.Top:
                    views.Add(mTopView);
                    break;
                case ViewType.Mask:
                    views.Add(mMaskView);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }

            if (selecter != null)
            {
                views = views.Where(selecter).ToList();
            }

            return views;
        }


        #endregion

        #region Mount

        public void MountView(IView view)
        {
            switch (view.ViewType)
            {
                case ViewType.Background:
                    MountBackground(view);
                    break;
                case ViewType.Normal:
                    MountNormal(view);
                    break;
                case ViewType.Parasitic:
                    MountParasitic(view);
                    break;
                case ViewType.Popup:
                    MountPopup(view);
                    break;
                case ViewType.Top:
                    MountTop(view);
                    break;
                case ViewType.Mask:
                    MoutMask(view);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // 抛出视图被挂载事件
            U3DFrame.EventModule.IssueEvent(U3dEventCode.View_Mounted.Literals, null, view);
            // 挂载完成，执行视图的Active方法以发出OnActive事件
            view.Active();
            // 将视图添加到活动视图字典中
            activeViewDictionary.Add(view.ViewId, view);
        }


        private void MountNormal(IView view)
        {
            if (!string.IsNullOrEmpty(view.MaskViewId)) CreateView(view.MaskViewId);
            //  如果普通视图有寄生视图存在则先创建寄生视图
            if (view.ViewType != ViewType.Parasitic && view.ParasiticList != null)
            {
                view.ParasiticList.ForEach(s => CreateView(s));
            }
            //  挂载自身
            SetViewAnchorsPostion(view.RectRoot, mNormalRoot);
            //  隐藏其他视图层
            (normalStack.Count > 0 && view.IsHideOther).TrueDo(() => normalStack.ForEach(v => v.Hide()));
            (popupStack.Count > 0 && view.IsHideOther).TrueDo(() => popupStack.ForEach(v => v.Hide()));
            (parasiticStack.Count > 0 && view.IsHideOther).TrueDo(() => parasiticStack.ForEach(v => v.Hide()));

            if (normalStack.Count > 0)
            {
                var topNormal = normalStack.Peek();
                if (view.IsCloseTop && !topNormal.IsMain)
                {
                    CloseView(topNormal.ViewId);
                }
            }
            normalStack.Push(view);
        }

        private void MountParasitic(IView view)
        {
            if (parasiticStack.Count > 0 && view.IsCloseTop) CloseView(view.ViewId);
            if (parasiticStack.Count > 0 && view.IsHideOther) parasiticStack.ForEach(v => v.Hide());

            SetViewAnchorsPostion(view.RectRoot, mParasiticRoot);
            parasiticStack.Push(view);
        }

        private void MountPopup(IView view)
        {
            // 如果当前有模态置顶视图存在则关闭
            if (mTopView != null) CloseView(mTopView.ViewId);

            //  挂载自身
            SetViewAnchorsPostion(view.RectRoot, mPopupRoot);

            // 如果当前弹出视图栈大于零并且待创建视图需要关闭栈顶视图
            // 则取出弹出视图栈顶视图对象关闭
            if (popupStack.Count > 0 && view.IsCloseTop) CloseView(popupStack.Peek().ViewId);

            // 如果待创建视图对象需要隐藏其他视图则遍历隐藏
            if (popupStack.Count > 0 && view.IsHideOther) popupStack.ForEach(v => v.Hide());

            popupStack.Push(view);
        }

        private void MountBackground(IView view)
        {
            if (mBackgroundView == null)
            {
                SetViewAnchorsPostion(view.RectRoot, mBackgroundRoot);
                mBackgroundView = view;
            }
            else if (mBackgroundView.ViewId != view.ViewId)
            {
                CloseView(mBackgroundView.ViewId);
                SetViewAnchorsPostion(view.RectRoot, mBackgroundRoot);
                mBackgroundView = view;
            }
        }

        private void MountTop(IView view)
        {
            if (mTopView == null)
            {
                SetViewAnchorsPostion(view.RectRoot, mTopRoot);
                mTopView = view;
            }
            else if (mTopView.ViewId != view.ViewId) // 置顶视图不为空且待创建视图和当前的置顶视图Id不相同
            {
                CloseView(mTopView.ViewId); // 关闭当前置顶视图
                SetViewAnchorsPostion(view.RectRoot, mTopRoot);
                mTopView = view;
            }
        }

        /// <summary>
        /// 挂载遮罩层
        /// </summary>
        /// <param name="view"></param>
        private void MoutMask(IView view)
        {
            if (mMaskView == null)
            {
                SetViewAnchorsPostion(view.RectRoot, mMaskRoot);
                mMaskView = view;
            }
            else if (mMaskView.ViewId != view.ViewId)
            {
                CloseView(mMaskView.ViewId);
                SetViewAnchorsPostion(view.RectRoot, mMaskRoot);
                mMaskView = view;
            }
        }

        /// <summary>
        /// 设置视图对象的锚点及位置缩放
        /// </summary>
        private static void SetViewAnchorsPostion(RectTransform son, Transform parent)
        {
            son.SetParent(parent);
            son.localScale = new Vector3(1, 1, 0);

            if (!Bootstrap.Instance.IsAnchorAlignment) return;

            son.offsetMax = new Vector2(0, 0);
            son.offsetMin = new Vector2(0, 0);
        }

        #endregion

        #region Close

        public virtual void CloseView(string viewId)
        {
            if (!activeViewDictionary.ContainsKey(viewId))
            {
                Debuger.LogException(string.Format("指定id为{0}的视图不存在，无法执行关闭视图！", viewId));
                return;
            }

            var view = activeViewDictionary[viewId];

            switch (view.ViewType)
            {
                case ViewType.Background:
                    CloseBackground();
                    break;
                case ViewType.Normal:
                    CloseNormal();
                    break;
                case ViewType.Parasitic:
                    CloseParasitic();
                    break;
                case ViewType.Popup:
                    ClosePopup();
                    break;
                case ViewType.Top:
                    CloseTop();
                    break;
                case ViewType.Mask:
                    CloseMask();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            activeViewDictionary.Remove(view.ViewId);
        }


        private void CloseNormal()
        {
            if (normalStack.Count == 0) return;
            var view = normalStack.Pop();
            view.Close();
            if (normalStack.Count > 0)
            {
                normalStack.Peek().Active();
            }
        }

        private void CloseBackground()
        {
            if (mBackgroundView == null) return;
            mBackgroundView.Close();
            mBackgroundView = null;
        }

        private void CloseParasitic()
        {
            if (parasiticStack.Count == 0) return;
            var view = parasiticStack.Pop();
            view.Close();
        }

        private void ClosePopup()
        {
            if (popupStack.Count == 0) return;
            var view = popupStack.Pop();
            view.Close();
        }

        private void CloseTop()
        {
            if (mTopView == null) return;
            mTopView.Close();
            mTopView = null;
        }

        private void CloseMask()
        {
            if (mMaskView == null) return;
            mMaskView.Close();
            mMaskView = null;
        }

        public void CloseAllByType(ViewType type, Func<IView, bool> selecter)
        {
            var views = new List<IView>();

            switch (type)
            {
                case ViewType.Background:
                    views.Add(mBackgroundView);
                    break;
                case ViewType.Normal:
                    views.AddRange(normalStack);
                    break;
                case ViewType.Parasitic:
                    views.AddRange(parasiticStack);
                    break;
                case ViewType.Popup:
                    views.AddRange(popupStack);
                    break;
                case ViewType.Top:
                    views.Add(mTopView);
                    break;
                case ViewType.Mask:
                    views.Add(mMaskView);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }

            if (selecter != null)
            {
                views = views.Where(selecter).ToList();
            }

            views.ForEach(v => CloseView(v.ViewId));
        }

        #endregion

        #region Hide

        private readonly Dictionary<string, IView> mNotActiveViews = new Dictionary<string, IView>();
        private RectTransform mHideRoot;

        public void HideView(string viewId)
        {

        }

        #endregion

        #region 生命周期

        /// <summary>
        /// 观察目标视图的视图生命周期事件
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="lifeEventType"></param>
        /// <param name="action"></param>
        /// <param name="executeCount"></param>
        public void WatchViewLiefEvent(string viewId, ViewLifeEventType lifeEventType, Action<IView> action, int executeCount = -1)
        {
            if (!mViewEventInfoDictionary.ContainsKey(viewId))
            {
                mViewEventInfoDictionary.Add(viewId, new List<ViewEventInfo>());
            }

            var viewEventInfo = new ViewEventInfo(viewId, action, lifeEventType, executeCount);
            mViewEventInfoDictionary[viewId].Add(viewEventInfo);
        }

        public void RemoveViewLifeEvent(string viewId, Action<IView> action)
        {
            if (!mViewEventInfoDictionary.ContainsKey(viewId)) return;

            var infos = mViewEventInfoDictionary[viewId];
            var targetInfo = infos.Find(info => info.LifeEventAction == action);
            if (targetInfo != null)
            {
                infos.Remove(targetInfo);
            }
        }

        private readonly Dictionary<string, List<ViewEventInfo>> mViewEventInfoDictionary = new Dictionary<string, List<ViewEventInfo>>();

        #endregion

        /// <summary>
        /// 关闭当前所有视图然后打开指定的目标视图
        /// </summary>
        /// <param name="ryViewId"></param>
        public void SkipTo(string ryViewId)
        {
            var views = activeViewDictionary.Values.ToList();

            mBackgroundView = null;
            mMaskView = null;
            mTopView = null;
            normalStack.Clear();
            popupStack.Clear();
            parasiticStack.Clear();
            views.ForEach(v => v.Close());
            activeViewDictionary.Clear();
            CreateView(ryViewId);
        }

        #region 常用功能

        /// <summary>
        /// 打开一个会话视图
        /// </summary>
        /// <param name="content"></param>
        /// <param name="dialogViewId"></param>
        /// <param name="ensure"></param>
        /// <param name="isShowCancel"></param>
        public void OpenDialog(string content, string dialogViewId,
            Action ensure = null, bool isShowCancel = false)
        {
            var diaLog = new DefaultViewDiaLog();
            diaLog.Init(content, ensure, isShowCancel);
            U3DFrame.DataModule.SetData(dialogViewId, diaLog);
            CreateView(dialogViewId);
        }

        #endregion
    }
}

