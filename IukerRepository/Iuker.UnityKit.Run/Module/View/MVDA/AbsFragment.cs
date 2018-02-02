using System;
using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
#if DEBUG
    /// <summary>
    /// 视图碎片基类
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170913 11:11:16")]
    [ClassPurposeDesc("视图碎片基类", "视图碎片基类")]
#endif
    public abstract class AbsFragment : AbsWidgetContainer, IFragment
    {
        #region 字段

        private IViewActionDispatcher mViewActionDispatcher;
        private IU3dAssetModule mAssetModule;
        private IU3dViewModule mViewModule;
        private FragmentRef mRef;
        public IFragment Origin
        {
            get { return this; }
        }

        public string ViewId { get; private set; }
        public string FragmentId { get; private set; }

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
                var widgetAttcher = RectRoot.Find(widgetPath).gameObject;
                var component = widgetAttcher.GetComponent<T>();
                if (component == null)
                {
                    var typename = typeof(T).Name;
                    Debuger.LogException(string.Format("指定路径{0}的游戏对象上没有需求的组件类型{1}", widgetPath, typename));
                    return default(T);
                }

                component.Init(U3DFrame, AttachView, this);
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

        protected GameObject InitViewWidget(string widgetPath)
        {
            var containner = RectRoot.Find(widgetPath).gameObject;
            ContainerDictionary.Add(widgetPath, containner);
            return containner;
        }

        #endregion

        #endregion

        #region 初始化

        public IFragment Init(string fragmentId, IView view, IU3dFrame frame, FragmentRef fragmentRef)
        {
            mRef = fragmentRef;
            FragmentId = fragmentId;
            AttachView = view;
            U3DFrame = frame;
            AssetName = fragmentRef.Asset.name;
            mViewActionDispatcher = U3DFrame.ViewModule.ViewActionispatcher;
            mAssetModule = U3DFrame.AssetModule;
            mViewModule = U3DFrame.ViewModule;

            RectRoot = Object.Instantiate(fragmentRef.Asset).GetComponent<RectTransform>();
            RectRoot.gameObject.name = AssetName;

            return this;
        }

        #endregion

        #region 控件获取

        protected virtual void InitViewWidgets() { }

        #endregion

        #region 生命周期

        public override void Active()
        {
        }

        public override void Hide()
        {
        }

        public override void Close()
        {
        }


        #endregion

        #region 动态调度

        public void Issue(IViewActionRequest<IFragment> request)
        {
            mViewActionDispatcher.DispatchRequest(request);
        }

        #endregion

    }
}
