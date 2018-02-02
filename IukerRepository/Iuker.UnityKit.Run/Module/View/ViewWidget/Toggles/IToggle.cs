using Iuker.UnityKit.Run.Module.View.MVDA;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 视图控件
    /// 开关
    /// </summary>
    public interface IToggle : IukViewWidget, IViewElement
    {
        bool IsOn { get; set; }

        void BindingToggleGrouop(IToggleGroup toggleGroup);

    }
}