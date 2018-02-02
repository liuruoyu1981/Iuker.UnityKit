using Iuker.Common.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
#if DEBUG
    /// <summary>
    /// UI贴图，用于动态绘制的UI元素。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170907 07:38:08")]
    [InterfacePurposeDesc("UI贴图，用于动态绘制的UI元素。", "UI贴图，用于动态绘制的UI元素。")]
#endif
    public interface IRawImage : IukViewWidget, IViewElement
    {
        /// <summary>
        /// 修改动态贴图的材质
        /// </summary>
        /// <param name="textureName"></param>
        void SetName(string textureName);

        void SetTexture(Texture2D texture2D);

        /// <summary>
        /// 材质贴图
        /// </summary>
        Material Material { get; }

        /// <summary>
        /// 贴图资源
        /// </summary>
        Texture Texture { get; }

        void recycle();
    }
}
