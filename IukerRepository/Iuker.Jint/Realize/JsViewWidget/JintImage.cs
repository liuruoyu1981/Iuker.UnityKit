using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// 用于Jint（js）脚本环境的图片代理
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170930 16:46:10")]
    [ClassPurposeDesc("用于Jint（js）脚本环境的图片代理", "用于Jint（js）脚本环境的图片代理")]
#endif
    public class JintImage : IImage
    {
        private readonly IImage mImage;

        public string ImageName
        {
            get { return mImage.ImageName; }
            set { mImage.ImageName = value; }
        }

        public Color Color
        {
            get { return mImage.Color; }
            set { mImage.Color = value; }
        }

        public GameObject DependentGo
        {
            get
            {
                return mImage.DependentGo;
            }
        }

        public GameObject ViewRoot
        {
            get
            {
                return mImage.ViewRoot;
            }
        }

        public string WidgetToken
        {
            get
            {
                return mImage.WidgetToken;
            }
        }

        public IView AttachView
        {
            get
            {
                return mImage.AttachView;
            }
        }

        public JintImage(IImage image) { mImage = image; }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            return mImage.Init(u3DFrame, view, fragment);
        }

        public IImage SetImage(string imageName)
        {
            return mImage.SetImage(imageName);
        }

        public void ToNativeSize()
        {
            mImage.ToNativeSize();
        }
    }
}
