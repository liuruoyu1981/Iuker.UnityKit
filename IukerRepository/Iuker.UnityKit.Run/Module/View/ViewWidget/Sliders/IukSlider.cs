/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/12 14:01
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

using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 滑动杆
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IukSlider : Slider, ISlider, IViewActionRequester<ISlider>
    {
        private Slider mSlider;

        protected override void Start()
        {
            base.Start();

            onValueChanged.AddListener(OnDefaultValueChange);
        }

        private void OnDefaultValueChange(float tValue)
        {
            if (Application.isPlaying)
            {
                // 如果启动器框架实例为空则说明没有进入框架流程，可能是以下几种情况
                // 1. 测试视图预制件
                // 2. 测试其他功能
                if (Bootstrap.U3DFrame == null) return;

                // 如果处于新手引导状态下，则转让交互控制处理权
                if (Bootstrap.U3DFrame.AppContext.AppStatus == U3dAppStatus.Guide) return;
                // 播放点击音效，如果有
                IssueValueChange(); // 发起点击交互处理请求
            }
        }

        /// <summary>
        /// 发布值改变行为处理请求
        /// </summary>
        private void IssueValueChange()
        {
            IViewActionRequest<ISlider> request = Bootstrap.U3DFrame.InjectModule
                .GetInstance<IViewActionRequest<ISlider>>()
                .Init(this, OnValueChangeToken);
            Issue(request);
        }

        public ISlider Origin { get { return this; } }

        public string ViewId
        {
            get
            {
                return ViewRoot.name;
            }
        }

        public void Issue(IViewActionRequest<ISlider> request)
        {
            Dispatcher.DispatchRequest(request);
        }

        public GameObject DependentGo
        {
            get
            {
                return gameObject;
            }
        }

        public GameObject ViewRoot
        {
            get
            {
                return AttachView.RectRoot.gameObject;
            }
        }

        private IU3dFrame U3dFrame { get; set; }

        private IViewActionDispatcher Dispatcher { get; set; }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            U3dFrame = u3DFrame;
            Dispatcher = u3DFrame.ViewModule.ViewActionispatcher;
            AttachView = view;
            mSlider = DependentGo.GetComponent<Slider>();

            return this;
        }

        /// <summary>
        /// 视图控件唯一识别码
        /// </summary>
        private string mWidgetToken;

        /// <summary>
        /// 视图控件唯一识别码
        /// </summary>
        public string WidgetToken
        {
            get
            {
                return mWidgetToken ?? (mWidgetToken = ViewRoot.name + "_" + gameObject.name);
            }
        }

        private string OnValueChangeToken
        {
            get
            {
                return ViewRoot.name + "_" + gameObject.name + "_OnValueChange";
            }
        }


        public IView AttachView { get; private set; }

        public float CurrentValue
        {
            get
            {
                return mSlider.value;
            }
        }

        /// <summary>
        /// 设置滑动条的进度值
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(float newValue)
        {
            mSlider.value = newValue;
        }
    }
}