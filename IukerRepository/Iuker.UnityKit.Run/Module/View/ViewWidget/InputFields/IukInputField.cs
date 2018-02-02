using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 输入框控件
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IukInputField : InputField, IInputField, IViewActionRequester<IInputField>
    {
        private IU3dFrame U3DFrame { get; set; }

        private IViewActionDispatcher Dispatcher;

        public IView AttachView { get; private set; }

        public string ViewId
        {
            get
            {
                return ViewRoot.name;
            }
        }

        private Text placeholderText;

        private Text PlaceholderText
        {
            get
            {
                return placeholderText ?? (placeholderText = placeholder.GetComponent<Text>());
            }
        }

        public string PlaceHolder
        {
            get { return PlaceholderText.text; }
            set { PlaceholderText.text = value; }
        }

        public string GetPlaceHolder()
        {
            return PlaceHolder;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string GetText()
        {
            return Text;
        }

        public GameObject ViewRoot
        {
            get
            {
                return AttachView.RectRoot.gameObject;
            }
        }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            U3DFrame = u3DFrame;
            Dispatcher = u3DFrame.ViewModule.ViewActionispatcher;
            AttachView = view;

            OnSelectActionToken = WidgetToken + "_OnSelect";
            OnSubmitActionToken = WidgetToken + "_OnSubmit";
            OnValueChangedActionToken = WidgetToken + "_OnValueChanged";

            // 注册输入框值改变处理函数，发送交互行为处理请求
            onValueChanged.AddListener(s =>
            {
                var request = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IInputField>>()
                    .Init(this, OnValueChangedActionToken);
                Issue(request);
            });

            return this;
        }

        public string WidgetToken
        {
            get
            {
                return ViewRoot.name + "_" + gameObject.name;
            }
        }

        private string OnSelectActionToken;
        private string OnSubmitActionToken;
        private string OnValueChangedActionToken;

        private IViewActionRequest<IInputField> mOnSelectRequest;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            PlaceHolder = null;

            if (Bootstrap.U3DFrame == null) return;
            if (mOnSelectRequest == null)
            {
                mOnSelectRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IInputField>>()
                    .Init(this, OnSelectActionToken);
            }

            Issue(mOnSelectRequest);
        }

        private IViewActionRequest<IInputField> mOnSubmitRequest;

        public override void OnSubmit(BaseEventData eventData)
        {
            if (Bootstrap.U3DFrame == null) return;

            if (mOnSubmitRequest == null)
            {
                mOnSubmitRequest = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IInputField>>()
                    .Init(this, OnSubmitActionToken);
            }

            Issue(mOnSubmitRequest);
        }

        public IInputField Origin
        {
            get
            {
                return this;
            }
        }

        public void Issue(IViewActionRequest<IInputField> request)
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

    }
}
