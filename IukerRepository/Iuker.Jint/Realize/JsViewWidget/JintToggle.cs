using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// 用于Jint（Js）脚本环境的开关代理
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170930 16:51:27")]
    [ClassPurposeDesc("用于Jint（Js）脚本环境的开关代理", "用于Jint（Js）脚本环境的开关代理")]
#endif
    public class JintToggle : IToggle
    {
        private readonly IToggle mToggle;

        public GameObject DependentGo { get { return mToggle.DependentGo; } }

        public GameObject ViewRoot { get { return mToggle.ViewRoot; } }

        public string WidgetToken { get { return mToggle.WidgetToken; } }

        public IView AttachView { get { return mToggle.AttachView; } }

        public bool IsOn
        {
            get { return mToggle.IsOn; }
            set { mToggle.IsOn = value; }
        }

        public JintToggle(IToggle toggle) { mToggle = toggle; }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            return mToggle.Init(u3DFrame, view, fragment);
        }

        public void BindingToggleGrouop(IToggleGroup toggleGroup)
        {
            mToggle.BindingToggleGrouop(toggleGroup);
        }
    }
}
