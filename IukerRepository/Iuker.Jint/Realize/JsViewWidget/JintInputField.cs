using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// Jint输入框控件
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171020 10:59:11")]
    [ClassPurposeDesc("Jint输入框控件", "Jint输入框控件")]
#endif
    public class JintInputField : IInputField
    {
        private readonly IInputField mInputField;

        public JintInputField(IInputField inputField)
        {
            mInputField = inputField;
        }

        public string PlaceHolder
        {
            get
            {
                return mInputField.PlaceHolder;
            }
            set
            {
                mInputField.PlaceHolder = value;
            }
        }

        public string Text
        {
            get
            {
                return mInputField.Text;
            }
            set { mInputField.Text = value; }
        }

        public GameObject DependentGo
        {
            get
            {
                return mInputField.DependentGo;
            }
        }

        public GameObject ViewRoot
        {
            get
            {
                return mInputField.ViewRoot;
            }
        }

        public string WidgetToken
        {
            get
            {
                return mInputField.WidgetToken;
            }
        }

        public IView AttachView
        {
            get
            {
                return mInputField.AttachView;
            }
        }

        public string GetPlaceHolder()
        {
            return mInputField.GetPlaceHolder();
        }

        public string GetText()
        {
            return mInputField.GetText();
        }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            return mInputField.Init(u3DFrame, view, fragment);
        }
    }
}
