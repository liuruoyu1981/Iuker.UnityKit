using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// Jint文本控件
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171020 10:45:57")]
    [ClassPurposeDesc("Jint文本控件", "Jint文本控件")]
#endif
    public class JintText : IText
    {
        private readonly IText mText;
        public JintText(IText text)
        {
            mText = text;
        }

        public string Text
        {
            get { return mText.Text; }
            set { mText.Text = value; }
        }

        public Color Color
        {
            get { return mText.Color; }
            set { mText.Color = value; }
        }

        public GameObject DependentGo
        {
            get
            {
                return mText.DependentGo;
            }
        }

        public GameObject ViewRoot
        {
            get
            {
                return mText.ViewRoot;
            }
        }

        public string WidgetToken
        {
            get
            {
                return mText.WidgetToken;
            }
        }

        public IView AttachView
        {
            get
            {
                return mText.AttachView;
            }
        }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            return mText.Init(u3DFrame, view, fragment);
        }
    }
}
