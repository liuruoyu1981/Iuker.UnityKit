using Iuker.Common.Base;
using Iuker.Common.Constant;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.WidgetCreaters
{
#if DEBUG
    /// <summary>
    /// 动态贴图控件创建器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170907 10:08:57")]
    [ClassPurposeDesc("动态贴图控件创建器", "动态贴图控件创建器")]
#endif
    public class RawImageCreater
    {
        public static IRawImage Create()
        {
            GameObject source = new GameObject("rawimage_" + Constant.GetTimeToken);
            var rawImage = source.AddComponent<IukRawImage>();
            return rawImage;
        }
    }
}
