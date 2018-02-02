using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using TMPro;
using UnityEngine;

namespace Iuker.UnityKit.Run.ViewWidget
{
    /// <summary>
    /// 高级文本组件。
    /// </summary>
    public class IukTextMeshPro : TextMeshProUGUI, IText
    {
        private IU3dFrame mU3DFrame;
        public GameObject DependentGo { get { return gameObject; } }
        public GameObject ViewRoot { get { return AttachView.RectRoot.gameObject; } }
        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            mU3DFrame = u3DFrame;
            AttachView = view;

            return this;
        }

        public string WidgetToken { get { return ViewRoot.name + "_" + gameObject.name; } }
        public IView AttachView { get; private set; }
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
            }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}
