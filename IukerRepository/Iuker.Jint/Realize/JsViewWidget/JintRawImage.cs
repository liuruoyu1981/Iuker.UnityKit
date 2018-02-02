using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.Jint.JsViewWidget
{
#if DEBUG
    /// <summary>
    /// 用于Jint（Js）脚本环境的源图片代理
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170930 16:50:23")]
    [ClassPurposeDesc("用于Jint（Js）脚本环境的源图片代理", "用于Jint（Js）脚本环境的源图片代理")]
#endif
    public class JintRawImage : IRawImage
    {
        private readonly IRawImage mRawImage;

        public Material Material
        {
            get
            {
                return mRawImage.Material;
            }
        }

        public Texture Texture
        {
            get
            {
                return mRawImage.Texture;
            }
        }

        public GameObject DependentGo
        {
            get
            {
                return mRawImage.DependentGo;
            }
        }

        public GameObject ViewRoot
        {
            get
            {
                return mRawImage.ViewRoot;
            }
        }

        public string WidgetToken
        {
            get
            {
                return mRawImage.WidgetToken;
            }
        }

        public IView AttachView
        {
            get
            {
                return mRawImage.AttachView;
            }
        }

        public JintRawImage(IRawImage image)
        {
            mRawImage = image;
        }

        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            return mRawImage.Init(u3DFrame, view, fragment);
        }

        public void recycle()
        {
            mRawImage.recycle();
        }

        public void SetName(string textureName)
        {
            mRawImage.SetName(textureName);
        }

        public void SetTexture(Texture2D texture2D)
        {
            mRawImage.SetTexture(texture2D);
        }
    }
}
