using Iuker.Common.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
#if DEBUG
    /// <summary>
    ///标签组接口，用于管理一组标签。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170904 15:02:55")]
    [InterfacePurposeDesc("标签组接口，用于管理一组标签。", "标签组接口，用于管理一组标签。")]
#endif
    public interface ITabGroup : IukViewWidget, IViewElement
    {
        /// <summary>
        /// 设置当前处于激活状态的Tab组件
        /// </summary>
        /// <param name="tabIndex"></param>
        void SetTab(int tabIndex);



    }
}
