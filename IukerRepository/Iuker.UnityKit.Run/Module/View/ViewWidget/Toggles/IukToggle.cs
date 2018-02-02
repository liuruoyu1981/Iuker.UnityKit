/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/6/10 06:22:26
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
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Module.SoundEffect;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 开关控件
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IukToggle : Toggle, IToggle, IViewActionRequester<IToggle>
    {
        protected override void Start()
        {
            base.Start();

            onValueChanged.AddListener(OnClickDefefaultDo);
        }

        public GameObject DependentGo { get { return gameObject; } }

        private IU3dFrame U3dFrame { get; set; }

        private IViewActionDispatcher Dispatcher { get; set; }

        public GameObject ViewRoot
        {
            get
            {
                return AttachView.RectRoot.gameObject;
            }
        }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            U3dFrame = u3DFrame;
            Dispatcher = u3DFrame.ViewModule.ViewActionispatcher;
            mSoundModule = u3DFrame.SoundEffectModule;
            AttachView = view;
            mDefaultClick = ProjectBaseConfig.Instance.DefaultClickSound;
            mSelfSoundName = mSoundModule.GetSoundAssetName(OnValueChangeToken);

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
                return mWidgetToken ?? (mWidgetToken = AttachView.ViewId + "_" + gameObject.name);
            }
        }

        /// <summary>
        /// 获取当前开关控件的选择状态
        /// </summary>
        public bool IsOn
        {
            get { return isOn; }
            set { isOn = value; }
        }

        private IToggleGroup mToggleGroup;

        public void BindingToggleGrouop(IToggleGroup toggleGroup)
        {
            mToggleGroup = toggleGroup;
        }

        /// <summary>
        /// 依附的视图对象
        /// </summary>
        public IView AttachView { get; private set; }


        private void OnClickDefefaultDo(bool isCheck)
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
                PlayClickSoundEffect();
                TryUnActiveOtherToggles();
                IssueValueChange(); // 发起点击交互处理请求
            }
        }

        void TryUnActiveOtherToggles()
        {
            if (mToggleGroup != null)
            {
                mToggleGroup.SwitchActiveToggle(this);
            }
        }

        private IImage _image;
        private IImage Image
        {
            get
            {
                if (_image != null) return _image;

                _image = gameObject.GetComponent<IImage>();
                if (image == null)
                {
                    Debuger.LogException(string.Format("目标按钮{0}上不存在IImage接口实例！", gameObject.name));
                    return null;
                }
                _image.Init(U3dFrame, AttachView);

                return _image;
            }
        }

        private string OnValueChangeToken
        {
            get
            {
                return ViewId + "_" + gameObject.name + "_OnValueChange";
            }
        }

        public string ToggleImage
        {
            get { return Image.ImageName; }
            set { Image.ImageName = value; }
        }

        public string ViewId
        {
            get
            {
                return AttachView.ViewId;
            }
        }

        public IToggle Origin { get { return this; } }

        /// <summary>
        /// 发布值改变行为处理请求
        /// </summary>
        private void IssueValueChange()
        {
            var request = Bootstrap.U3DFrame.InjectModule
                .GetInstance<IViewActionRequest<IToggle>>()
                .Init(this, OnValueChangeToken);
            Issue(request);
        }

        /// <summary>
        /// 自身的音效资源名
        /// </summary>
        private string mSelfSoundName;

        private IU3dSoundEffectModule mSoundModule;
        private string mDefaultClick;

        /// <summary>
        /// 播放点击音效
        /// </summary>
        private void PlayClickSoundEffect()
        {
            if (gameObject.name.Contains("mute")) return;           //  静音

            var soundName = !string.IsNullOrEmpty(mSelfSoundName) ? mSelfSoundName : mDefaultClick;
#if UNITY_EDITOR
            if (soundName == "None")
            {
                Debug.Log("默认音效的值None，请检查项目基础公共配置中默认音效的配置节！");
                return;
            }
#endif
            mSoundModule.Play(soundName);
        }

        public void Issue(IViewActionRequest<IToggle> request)
        {
            Dispatcher.DispatchRequest(request);
        }
    }
}