using Iuker.Common.Constant;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.WidgetCreaters
{
    /// <summary>
    /// 精灵图片创建器
    /// </summary>
    public class ImageCreater
    {
        public static IImage Create()
        {
            GameObject source = new GameObject("image_" + Constant.GetTimeToken);
            var image = source.AddComponent<IukImage>();
            return image;
        }









    }
}