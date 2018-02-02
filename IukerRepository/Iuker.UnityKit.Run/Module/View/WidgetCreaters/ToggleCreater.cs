using Iuker.Common.Constant;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using Iuker.UnityKit.Run.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.WidgetCreaters
{
    /// <summary>
    /// 开关创建器
    /// </summary>
    public class ToggleCreater
    {
        public static IToggle Create()
        {
            GameObject source = new GameObject("toggle_" + Constant.GetTimeToken);
            var toggle = source.AddComponent<IukToggle>();
            var background = source.AddChild("Background").AddComponent<IukImage>();
            var checkmark = background.AddChild("Checkmark").AddComponent<IukImage>();
            source.AddChild("Label").AddComponent<IukText>();

            toggle.targetGraphic = background;
            toggle.graphic = checkmark;

            return toggle;
        }
    }
}