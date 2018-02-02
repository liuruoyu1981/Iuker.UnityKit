using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
#if DEBUG
    /// <summary>
    /// 滚动列表子项
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171010 16:35:46")]
    [InterfacePurposeDesc("滚动列表子项", "滚动列表子项")]
#endif
    public interface IListViewItem
    {
        /// <summary>
        /// 滚动列表子项数据索引
        /// </summary>
        int DataIndex { get; }

        void SetListView(IListView listView);
    }
}
