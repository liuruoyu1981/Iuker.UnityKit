using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;
using UnityEngine.UI;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
#if DEBUG
    /// <summary>
    /// 动态UI贴图，用于绘制动态UI元素。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170907 07:50:10")]
    [ClassPurposeDesc("动态UI贴图，用于绘制动态UI元素。", "动态UI贴图，用于绘制动态UI元素。")]
    [ExecuteInEditMode]
    //    [RequireComponent(typeof(IukRawImageInfo))]
#endif
    public class IukRawImage : RawImage, IRawImage
    {
        private IU3dFrame U3DFrame { get; set; }

        public GameObject DependentGo
        {
            get
            {
                return gameObject;
            }
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
            AttachView = view;

            return this;
        }

        public string WidgetToken
        {
            get { return ViewRoot.name + "_" + gameObject.name; }
        }

        public IView AttachView { get; private set; }

        public void SetTexture(Texture2D texture2D)
        {
            texture = texture2D;
        }

        public void SetName(string textureName)
        {
            var tex2D = U3DFrame.AssetModule.LoadTexture2D(textureName);
            texture = tex2D.Asset;
        }

        public Material Material
        {
            get
            {
                return material;
            }
        }

        public Texture Texture
        {
            get
            {
                return texture;
            }
        }

        public void recycle()
        {
            Resources.UnloadAsset(Texture);
        }
    }

}
