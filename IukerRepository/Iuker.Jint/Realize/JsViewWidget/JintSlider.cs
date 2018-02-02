using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// 用于Jint（Js）脚本环境的Slider代理。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170930 16:59:56")]
    [ClassPurposeDesc("用于Jint（Js）脚本环境的Slider代理。", "用于Jint（Js）脚本环境的Slider代理。")]
#endif
    public class JintSlider : ISlider
    {
        private readonly ISlider mSlider;



        public JintSlider(ISlider slider) { mSlider = slider; }

        public float CurrentValue
        {
            get
            {
                return mSlider.CurrentValue;
            }
        }

        public GameObject DependentGo
        {
            get
            {
                return mSlider.DependentGo;
            }
        }

        public GameObject ViewRoot
        {
            get
            {
                return mSlider.ViewRoot;
            }
        }

        public string WidgetToken
        {
            get
            {
                return mSlider.WidgetToken;
            }
        }

        public IView AttachView
        {
            get
            {
                return mSlider.AttachView;
            }
        }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            return mSlider.Init(u3DFrame, view, fragment);
        }

        public void SetValue(float newValue)
        {
            mSlider.SetValue(newValue);
        }
    }
}
