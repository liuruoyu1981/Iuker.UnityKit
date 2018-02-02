/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 08:05:29
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
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Module.SoundEffect;
using Iuker.UnityKit.Run.Module.View;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.ViewWidget
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class IukButton : Button, IButton, IViewActionRequester<IButton>, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public PointerEventData PointerEventData { get; private set; }

        #region 字段

        private string mDefaultClick;

        private IU3dFrame mU3DFrame;

        private IU3dSoundEffectModule mSoundModule;

        private IViewActionDispatcher mActionDispatcher;

        private bool mIsCell;

        /// <summary>
        /// 按钮所负载的数据
        /// </summary>
        public virtual object Data { get; set; }

        /// <summary>
        /// 源对象（按钮自身）
        /// </summary>
        public IButton Origin { get { return this; } }

        /// <summary>
        /// 发起交互行为调度
        /// </summary>
        /// <param name="request"></param>
        public void Issue(IViewActionRequest<IButton> request)
        {
            mActionDispatcher.DispatchRequest(request);
        }

        /// <summary>
        /// 按钮所依附的游戏对象
        /// </summary>
        public GameObject DependentGo { get { return gameObject; } }

        /// <summary>
        /// 按钮所依附的视图
        /// </summary>
        public IView AttachView { get; private set; }

        public GameObject ViewRoot { get { return AttachView.RectRoot.gameObject; } }

        /// <summary>
        /// 按钮所在界面的视图Id
        /// </summary>
        public string ViewId { get { return AttachView.ViewId; } }

        /// <summary>
        /// 自身的音效资源名
        /// </summary>
        private string mSelfSoundName;

        #endregion

        #region 接口实现

        public virtual IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            mU3DFrame = u3DFrame;
            mSoundModule = mU3DFrame.SoundEffectModule;
            mActionDispatcher = mU3DFrame.ViewModule.ViewActionispatcher;
            AttachView = view;
            mIsCell = name.Contains("cell");
            mDefaultClick = ProjectBaseConfig.Instance.DefaultClickSound;
            onClick.AddListener(OnClickDefaultDo);
            //  获取自身的音效名
            mSelfSoundName = mSoundModule.GetSoundAssetName(OnClickToken);
            var am = DependentGo.GetComponent<Animator>();
            if (am == null || am.runtimeAnimatorController != null) return this;

            var baseAmController = Resources.Load<RuntimeAnimatorController>("amc_button_base");
            am.runtimeAnimatorController = baseAmController;

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
                return mWidgetToken ?? (mWidgetToken = AttachView == null ? null : AttachView.ViewId + "_" + gameObject.name);
            }
        }

        private IImage mImage;
        private IImage Image
        {
            get
            {
                if (mImage != null) return mImage;

                mImage = gameObject.GetComponent<IImage>();
                if (image == null)
                {
#if UNITY_EDITOR || DEBUG
                    Debuger.LogException(string.Format("目标按钮{0}上不存在IImage接口实例！", gameObject.name));
#endif
                    return null;
                }
                mImage.Init(mU3DFrame, AttachView);

                return mImage;
            }
        }

        public string ImageName
        {
            get { return Image.ImageName; }
            set { Image.ImageName = value; }
        }

        public string RawImageName
        {
            get
            {
                var rawImage = GetComponent<IukRawImage>();
                return rawImage.texture.name;
            }
            set
            {
                var tex2D = mU3DFrame.AssetModule.LoadTexture2D(value);
                var texture = tex2D.Asset;
                var rawImage = GetComponent<IukRawImage>();
                rawImage.texture = texture;
            }
        }

        public void BindingHotAction(ButtonActionType type, Action action, bool isDoCsahrp = true)
        {
            if (!mActions.ContainsKey(type))
            {
                mActions.Add(type, new WidgetActionAgent<IButton>(action));
            }
            else
            {
                mActions[type].HotAction = action;
                mActions[type].IsDoCsharp = isDoCsahrp;
            }
        }

        #endregion

        /// <summary>
        /// 点击事件的默认操作
        /// </summary>
        protected virtual void OnClickDefaultDo()
        {
            // 如果启动器框架实例为空则说明没有进入框架流程，可能是以下几种情况
            // 1. 测试视图预制件
            // 2. 测试其他功能
            if (mU3DFrame == null) return;

            // 如果处于新手引导状态下，则转让交互控制处理权
            if (mU3DFrame.AppContext.AppStatus == U3dAppStatus.Guide) return;

            // 播放点击音效，如果有
            PlayClickSoundEffect();
            InvokeAgent(ButtonActionType.OnClick, "_OnClick");
        }

        /// <summary>
        /// 按钮点击事件唯一识别码
        /// </summary>
        private string mOnClickToken;

        /// <summary>
        /// 按钮点击事件唯一识别码
        /// </summary>
        private string OnClickToken { get { return mOnClickToken ?? (mOnClickToken = WidgetToken + "_OnClick"); } }

        #region Cell识别码

        private int mLastCellIndex { get { return gameObject.name.LastIndexOf("_", StringComparison.Ordinal); } }

        /// <summary>
        /// cell按钮_下划线截断索引
        /// </summary>
        private int mLastCellSubIndex { get { return gameObject.name.Length - mLastCellIndex; } }

        #endregion

        /// <summary>
        /// 播放点击音效
        /// </summary>
        protected void PlayClickSoundEffect()
        {
            if (gameObject.name == "button_root") return;           //  根按钮静音
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

        #region UGUI事件接口实现

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnPointerClick, "_OnPointerClick");
        }

        /// <summary>
        /// 当控件作为模板使用时符合规则的模板对象名
        /// </summary>
        private string CellName
        {
            get
            {
                return gameObject.name.Substring(0, gameObject.name.Length - mLastCellSubIndex);
            }
        }

        /// <summary>
        /// 抛出指针进入事件处理请求
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnPointerEnter, "_OnPointerEnter");
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnPointerDown, "_OnPointerDown");
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnPointerExit, "_OnPointerExit");
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnPointerUp, "_OnPointerUp");
        }

        private readonly Dictionary<ButtonActionType, WidgetActionAgent<IButton>> mActions = new Dictionary<ButtonActionType, WidgetActionAgent<IButton>>();

        private void InvokeAgent(ButtonActionType type, string apend)
        {
            if (mU3DFrame == null) return;

            WidgetActionAgent<IButton> agent;

            if (mActions.ContainsKey(type)) agent = mActions[type];
            else
            {
                var token = mIsCell ? ViewId + "_" + CellName + apend : WidgetToken + apend;
                agent = mIsCell
                    ? new WidgetActionAgent<IButton>(token, mU3DFrame, this, true)
                    : new WidgetActionAgent<IButton>(token, mU3DFrame, this);
                mActions.Add(type, agent);
            }

            agent.Invoke();
        }

        #endregion

        public void OnDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnDrag, "_OnDrag");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnBeginDrag, "_OnEndDrag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
            InvokeAgent(ButtonActionType.OnEndDrag, "_OnEndDrag");
        }
    }
}
