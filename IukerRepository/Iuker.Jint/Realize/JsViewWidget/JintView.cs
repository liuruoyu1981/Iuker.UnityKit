using Iuker.Common;
using Iuker.Common.Base;
using Iuker.UnityKit.Run.Module.JavaScript;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using Jint.Native;
using UnityEngine;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// 用于Jint（Js）脚本环境的视图代理
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170930 17:12:09")]
    [ClassPurposeDesc("用于Jint（Js）脚本环境的视图代理", "用于Jint（Js）脚本环境的视图代理")]
#endif
    public sealed class JintView : AbsViewBase
    {
        private IU3dJavaScriptModule mJsModule;

        private IU3dJavaScriptModule JsModule
        {
            get
            {
                return mJsModule ?? (mJsModule = U3DFrame.GetModule<IU3dJavaScriptModule>());
            }
        }

        #region 生命周期

        private void CallJintViewPipeline(string jintId)
        {
            if (!JsModule.Exist(jintId)) return;

            var code = string.Format("Iuker.ViewModule.PipelineProcessers.{0} = new {1}.{0}();", jintId
                , U3DFrame.AppContext.CurrentSonProject);
            JsModule.DoString(code);
            var jsIuker = mJsModule.GetGlobalValue("Iuker").As<JsValue>();
            var jsViewModule = jsIuker.GetJsValue("ViewModule");
            var processers = jsViewModule.GetJsValue("PipelineProcessers");
            var processer = processers.GetJsValue(jintId);
            var isDoCsharpFunc = processer.GetJsValue("IsDoCsharp");
            var isDoCsharp = isDoCsharpFunc.Invoke(processer).AsBoolean();
            if (isDoCsharp)
            {
                var request = U3DFrame.InjectModule.GetInstance<IViewActionRequest<IView>>().Init(this, jintId.Substring(0, jintId.Length - 5), ViewScriptType.ViewPipeline);
                U3DFrame.ViewModule.ViewActionispatcher.DispatchRequest(request);
            }

            var pqFunc = processer.GetJsValue("ProcessRequest");

#if UNITY_EDITOR || DEBUG
            Debug.Log("[JS] " + jintId + " is trigger!");
#endif

            pqFunc.InvokeByThis(processer);
        }

        protected override void InitViewWidgets()
        {
            var jintId = ViewId + "_jint";
            var code = string.Format("Iuker.ViewModule.Views.{0} = new {1}.{0}();",
                jintId, U3DFrame.AppContext.CurrentSonProject);
            JsModule.DoString(code);
            var jsIuker = mJsModule.GetGlobalValue("Iuker").As<JsValue>();
            var jsViewModule = jsIuker.GetJsValue("ViewModule");
            var views = jsViewModule.GetJsValue("Views");
            var view = views.GetJsValue(jintId);
            var initViewWidgets = view.GetJsValue("InitViewWidgets");
            initViewWidgets.Invoke();
        }

        protected override void BeforeCreat()
        {
            CallJintViewPipeline(ViewId + "_beforecreat_jint");
        }

        protected override void OnCreated()
        {
            CallJintViewPipeline(ViewId + "_oncreated_jint");
        }

        protected override void BeforeActive()
        {
            CallJintViewPipeline(ViewId + "_beforeactive_jint");
        }

        protected override void OnActived()
        {
            CallJintViewPipeline(ViewId + "_onactived_jint");
        }

        protected override void BeforeHide()
        {
            CallJintViewPipeline(ViewId + "_beforehide_jint");
        }

        protected override void OnHided()
        {
            CallJintViewPipeline(ViewId + "_onhided_jint");
        }

        protected override void BeforeClose()
        {
            CallJintViewPipeline(ViewId + "_beforeclose_jint");
        }

        protected override void OnClosed()
        {
            CallJintViewPipeline(ViewId + "_onclosed_jint");
        }

        #endregion

        #region Jint控件获取

        public GameObject GetJintContainer(string path)
        {
            if (ContainerDictionary.ContainsKey(path))
            {
                return ContainerDictionary[path];
            }

            var contaienr = RectRoot.Find(path).gameObject;
            contaienr.AddTo(path, ContainerDictionary);
            return contaienr;
        }

        public JintButton GetJintButton(string path)
        {
            if (ButtonDictionary.ContainsKey(path))
            {
                return (JintButton)ButtonDictionary[path];
            }

            var button = InitViewWidget<IButton>(path);
            var jintButton = new JintButton(button);
            jintButton.Init(U3DFrame, this);
            jintButton.AddTo(path, ButtonDictionary);
            return jintButton;
        }

        public JintText GetJintText(string path)
        {
            if (TextDictionary.ContainsKey(path))
            {
                return TextDictionary[path].As<JintText>();
            }

            var text = InitViewWidget<IText>(path);
            var jintText = new JintText(text);
            jintText.AddTo(path, TextDictionary);
            return jintText;
        }

        public JintImage GetJintImage(string path)
        {
            if (ImageDictionary.ContainsKey(path))
            {
                return ImageDictionary[path].As<JintImage>();
            }

            var image = InitViewWidget<IImage>(path);
            var jintImage = new JintImage(image);
            jintImage.AddTo(path, ImageDictionary);
            return jintImage;
        }

        public JintRawImage GetJintRawImage(string path)
        {
            if (RawImageDictionary.ContainsKey(path))
            {
                return RawImageDictionary[path].As<JintRawImage>();
            }

            var rawImage = InitViewWidget<IRawImage>(path);
            var jintRawImage = new JintRawImage(rawImage);
            jintRawImage.AddTo(path, RawImageDictionary);
            return jintRawImage;
        }

        public JintSlider GetJintSlider(string path)
        {
            if (SliderDictionary.ContainsKey(path))
            {
                return SliderDictionary[path].As<JintSlider>();
            }

            var slider = InitViewWidget<ISlider>(path);
            var jintSlider = new JintSlider(slider);
            jintSlider.AddTo(path, SliderDictionary);
            return jintSlider;
        }

        public JintToggle GetJintToggle(string path)
        {
            if (ToggleDictionary.ContainsKey(path))
            {
                return ToggleDictionary[path].As<JintToggle>();
            }

            var toggle = InitViewWidget<IToggle>(path);
            var jintToggle = new JintToggle(toggle);
            jintToggle.AddTo(path, ToggleDictionary);
            return jintToggle;
        }

        public JintInputField GetJintInputField(string path)
        {
            if (InputFieldDictionary.ContainsKey(path))
            {
                return InputFieldDictionary[path].As<JintInputField>();
            }

            var inputField = InitViewWidget<IInputField>(path);
            var jintInputField = new JintInputField(inputField);
            jintInputField.AddTo(path, InputFieldDictionary);
            return jintInputField;
        }



        #endregion
    }
}
