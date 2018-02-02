/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/03 14:51:10
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
using System.Linq;
using Iuker.Common;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.View.Assist;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using Iuker.UnityKit.Run.ViewWidget;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图基类
    /// </summary>
    public abstract class AbsViewBase : AbsWidgetContainer, IView
    {
        #region 属性字段

        /// <summary>
        /// 视图模块
        /// </summary>
        private IU3dViewModule mViewModule;

        /// <summary>
        /// 资源模块
        /// </summary>
        private IU3dAssetModule mAssetModule;

        /// <summary>
        /// 根按钮（处理空白点击）
        /// </summary>
        private IukButton buttonRoot;

        /// <summary>
        /// 视图配置
        /// </summary>
        private Base.Config.View mViewConfig;

        /// <summary>
        /// 源对象（自身）
        /// </summary>
        public IView Origin { get { return this; } }

        /// <summary>
        /// 视图辅助器
        /// 用于可视化视图配置项
        /// </summary>
        private ViewAssister mViewAssister;

        private bool mIsInEditor;

        private ViewRef mViewRef;

        #endregion

        #region 控件获取

        /// <summary>
        /// 初始化视图控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="widgetPath">控件获取路径</param>
        /// <returns></returns>
        protected T InitViewWidget<T>(string widgetPath) where T : IukViewWidget
        {
#if UNITY_EDITOR || DEBUG
            try
            {
#endif
                var name = widgetPath.Split('/').Last();
                var widgetAttcher = RectRoot.Find(widgetPath).gameObject;
                var component = widgetAttcher.GetComponent<T>();
                if (component == null)
                {
                    var typename = typeof(T).Name;
                    Debuger.LogException(string.Format("指定路径{0}的游戏对象上没有需求的组件类型{1}", widgetPath, typename));
                    return default(T);
                }

                mHideGoMaps.Add(name, component.DependentGo);
                component.Init(U3DFrame, this);
                return component;
#if UNITY_EDITOR || DEBUG
            }
            catch (Exception ex)
            {
                Debuger.LogException(string.Format("在获取路径为{0}的游戏对象上的组件{1}时发生异常，", widgetPath, typeof(T).Name) +
                                     string.Format("异常信息为{0}", ex.Message));
                return default(T);
            }
#endif
        }

        #endregion

        #region 基础配置项

        public ViewType ViewType { get; private set; }
        public string MaskViewId { get; private set; }
        public bool IsMain { get; private set; }
        public bool IsBlankClose { get; private set; }
        public bool IsHideOther { get; private set; }
        public bool IsCloseTop { get; private set; }
        public List<string> ParasiticList { get; private set; }
        public string ViewId { get; private set; }

        #endregion

        #region 视图生命周期

        /// <summary>
        /// 初始化一个视图实例
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="viewRef"></param>
        /// <returns></returns>
        public virtual IView Init(string viewId, ViewRef viewRef)
        {
#if UNITY_EDITOR
            mIsInEditor = true;
#endif

            AttachView = this;
            U3DFrame = Bootstrap.U3DFrame;
            mViewModule = U3DFrame.ViewModule;
            mAssetModule = U3DFrame.AssetModule;
            mViewRef = viewRef;
            AssetName = viewRef.Asset.name;
            RectRoot = Object.Instantiate(viewRef.Asset).GetComponent<RectTransform>();
            RectRoot.gameObject.name = AssetName;
            InitRememberContext();
            if (mIsInEditor) LoadConfigByAssiter(viewId);
            else LoadConfigByFile(viewId);
            StartPipeline();

            return this;
        }

        private void LoadConfigByAssiter(string viewId)
        {
            mViewAssister = RectRoot.GetComponent<ViewAssister>();
            if (mViewAssister != null)
            {
                ViewId = mViewAssister.ViewId;
                ViewType = mViewAssister.ViewType;
                AssetName = mViewAssister.AssetName;
                IsMain = mViewAssister.IsMain;
                IsBlankClose = mViewAssister.IsBlankClose;
                IsHideOther = mViewAssister.IsHideOther;
                IsCloseTop = mViewAssister.IsCloseTop;
                ParasiticList = mViewAssister.ParasiticList;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning(string.Format("目标视图{0}的视图根上没有视图辅助器组件，请检查！将进入视图配置文件解析。", viewId));
#endif
                LoadConfigByFile(viewId);
            }
        }

        private void LoadConfigByFile(string viewId)
        {
            if (viewId == "view_default_update")
            {
                Debug.Log("更新视图（首视图从视图配置辅助器组件上加载配置，视图配置节点加载行为取消！）");
                LoadConfigByAssiter(viewId);
                return;
            }

            mViewConfig = mViewModule.ProjectViews.Find(r => r.ViewId == viewId);
            if (mViewConfig == null)
            {
                throw new Exception(string.Format("指定的视图{0}，没有找到视图配置节点！", viewId));
            }

            ViewId = mViewConfig.ViewId;
            ViewType = (ViewType)Enum.Parse(typeof(ViewType), mViewConfig.ViewType);
            IsMain = mViewConfig.IsMain;
            IsBlankClose = mViewConfig.IsBlankClose;
            IsHideOther = mViewConfig.IsHideOther;
            IsCloseTop = mViewConfig.IsCloseTop;
            ParasiticList = mViewConfig.ParasiticList;
        }

        private void StartPipeline()
        {
            InitViewWidgets();  // 获取所有需要操作的UI控件
            TryInitRootButton();   //  获取根按钮用于支持点击空白关闭视图自身功能
            TryCallOvrrideLifeHandler(ViewLifeEventType.BeforeCreat);
            TryCallExternalLifeHandler(ViewLifeEventType.BeforeCreat);
            mViewModule.MountView(this); // 调用视图模块挂载自身
            TryCallOvrrideLifeHandler(ViewLifeEventType.OnCreated);
            TryCallExternalLifeHandler(ViewLifeEventType.OnCreated);
        }

        /// <summary>
        /// 尝试调用外部对于自身生命周期的处理委托
        /// </summary>
        private void TryCallExternalLifeHandler(ViewLifeEventType type)
        {
            var eventInfos = mViewModule.GetViewEventInfos(ViewId);
            if (eventInfos == null) { return; }
            var targetInfos = eventInfos.FindAll(r => r.LifeEventType == type);
            targetInfos.ForEach(info => info.LifeEventAction(this));
        }

        protected virtual void InitViewWidgets()
        {
        }

        /// <summary>
        /// 尝试初始化根按钮
        /// 寄生视图无需根按钮
        /// </summary>
        private void TryInitRootButton()
        {
            if (ViewType == ViewType.Parasitic) return;
            buttonRoot = InitViewWidget<IukButton>("button_root");
            buttonRoot.onClick.AddListener(OnRootButtonClick);
        }

        /// <summary>
        /// 根按钮被点击时
        /// 具体实现视图可以重写该方法
        /// </summary>
        protected virtual void OnRootButtonClick()
        {
            IsBlankClose.TrueDo(() => mViewModule.CloseView(ViewId));
        }

        #region 基础生命周期处理

        protected virtual void BeforeCreat() { }

        protected virtual void OnCreated() { }

        protected virtual void BeforeActive() { }

        protected virtual void OnActived() { }

        protected virtual void BeforeHide() { }

        protected virtual void OnHided() { }

        protected virtual void BeforeClose() { }

        protected virtual void OnClosed() { }

        private void TryCallOvrrideLifeHandler(ViewLifeEventType type)
        {
            if (!HasRemember() || !HasPipelineKey(type) || HasPipelineHandler(type))
            {
                mLifeHandlers[type]();
            }
        }

        private readonly Dictionary<ViewLifeEventType, Action> mLifeHandlers = new Dictionary<ViewLifeEventType, Action>();

        /// <summary>
        /// 初始化视图执行记忆上下文环境
        /// </summary>
        private void InitRememberContext()
        {
            mLifeHandlers.Add(ViewLifeEventType.BeforeCreat, BeforeCreat);
            mLifeHandlers.Add(ViewLifeEventType.OnCreated, OnCreated);
            mLifeHandlers.Add(ViewLifeEventType.BeforeActive, BeforeActive);
            mLifeHandlers.Add(ViewLifeEventType.OnActived, OnActived);
            mLifeHandlers.Add(ViewLifeEventType.BeforeHide, BeforeHide);
            mLifeHandlers.Add(ViewLifeEventType.OnHided, OnHided);
            mLifeHandlers.Add(ViewLifeEventType.BeforeClose, BeforeClose);
            mLifeHandlers.Add(ViewLifeEventType.OnClosed, OnClosed);
        }

        #endregion

        #region 生命周期外部调用接口

        public override void Hide()
        {
            TryCallOvrrideLifeHandler(ViewLifeEventType.BeforeHide);
            TryCallExternalLifeHandler(ViewLifeEventType.BeforeHide);
            RectRoot.gameObject.SetActive(false);
            TryCallOvrrideLifeHandler(ViewLifeEventType.OnHided);
            TryCallExternalLifeHandler(ViewLifeEventType.OnHided);
        }

        public override void Active()
        {
            TryCallOvrrideLifeHandler(ViewLifeEventType.BeforeActive);
            TryCallExternalLifeHandler(ViewLifeEventType.BeforeActive);
            RectRoot.gameObject.SetActive(true);
            TryCallOvrrideLifeHandler(ViewLifeEventType.OnActived);
            TryCallExternalLifeHandler(ViewLifeEventType.OnActived);
        }

        /// <summary>
        /// 关闭视图
        /// </summary>
        public override void Close()
        {
            TryCallOvrrideLifeHandler(ViewLifeEventType.BeforeClose);
            TryCallExternalLifeHandler(ViewLifeEventType.BeforeClose);
            RectRoot.gameObject.SafeDestroy();
            TryCallOvrrideLifeHandler(ViewLifeEventType.OnClosed);
            TryCallExternalLifeHandler(ViewLifeEventType.OnClosed);
            U3DFrame.EventModule.IssueEvent(U3dEventCode.View_Closed.ToString(), null, ViewId);
        }

        #endregion

        /// <summary>
        /// 发起一个视图行为处理请求
        /// </summary>
        /// <param name="request"></param>
        public void Issue(IViewActionRequest<IView> request)
        {
            U3DFrame.ViewModule.ViewActionispatcher.DispatchRequest(request);
        }

        #endregion

        #region 视图碎片






        #endregion

        #region 性能优化

        private static readonly Dictionary<string, ViewRemeber> _remeberDictionary = new Dictionary<string, ViewRemeber>();

        private ViewRemeber mSelfRemeber
        {
            get
            {
                return _remeberDictionary.ContainsKey(ViewId) ? _remeberDictionary[ViewId] : null;
            }
        }

        private bool HasRemember()
        {
            return _remeberDictionary.ContainsKey(ViewId);
        }

        private bool HasPipelineKey(ViewLifeEventType type)
        {
            return mSelfRemeber != null && mSelfRemeber.HasPipelineKey(type);
        }

        private bool HasPipelineHandler(ViewLifeEventType type)
        {
            return mSelfRemeber.HasPipelineHandler(type);
        }

        private bool HasDrawHandler(ViewDrawType type)
        {
            return mSelfRemeber.HasDrawHandler(type);
        }

        public void Remember(ViewLifeEventType type, bool isExist)
        {
            if (!_remeberDictionary.ContainsKey(ViewId))
            {
                _remeberDictionary.Add(ViewId, new ViewRemeber());
            }

            var remember = _remeberDictionary[ViewId];
            remember.Remember(type, isExist);
        }

        public void Remember(ViewDrawType type, bool isExist)
        {
            if (!_remeberDictionary.ContainsKey(ViewId))
            {
                _remeberDictionary.Add(ViewId, new ViewRemeber());
            }

            var remember = _remeberDictionary[ViewId];
            remember.Remember(type, isExist);
        }

        private class ViewRemeber
        {
            private readonly Dictionary<ViewLifeEventType, bool> mLifeRemember = new Dictionary<ViewLifeEventType, bool>();
            private readonly Dictionary<ViewDrawType, bool> mDrawRemember = new Dictionary<ViewDrawType, bool>();

            public void Remember(ViewLifeEventType type, bool isExist)
            {
                if (mLifeRemember.ContainsKey(type))
                {
                    mLifeRemember[type] = isExist;
                }
                else
                {
                    mLifeRemember.Add(type, isExist);
                }
            }

            public void Remember(ViewDrawType type, bool isExist)
            {
                if (mDrawRemember.ContainsKey(type))
                {
                    mDrawRemember[type] = isExist;
                }
                else
                {
                    mDrawRemember.Add(type, isExist);
                }
            }

            public bool HasPipelineKey(ViewLifeEventType type)
            {
                return mLifeRemember.ContainsKey(type);
            }

            public bool HasPipelineHandler(ViewLifeEventType type)
            {
                return mLifeRemember[type] = true;
            }

            public bool HasDrawHandler(ViewDrawType type)
            {
                return mDrawRemember.ContainsKey(type);
            }
        }


        #endregion

    }
}
