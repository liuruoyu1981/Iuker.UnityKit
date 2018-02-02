using Iuker.Common.Base;
using Iuker.UnityKit.Run.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
#if DEBUG
    /// <summary>
    /// 滚动列表子项
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171009 14:09:34")]
    [ClassPurposeDesc("滚动列表子项", "滚动列表子项")]
#endif
    [DisallowMultipleComponent]
    public sealed class IukListViewItem : IukButton, IListViewItem
    {
        public int DataIndex { get; set; }
        private IListView mListView;

        public Transform Transform
        {
            get
            {
                return transform;
            }
        }

        /// <summary>
        /// 滚动列表子项指向数据
        /// </summary>
        public override object Data
        {
            get
            {
                return mListView.Datas[DataIndex];
            }
        }

        public void SetListView(IListView listView)
        {
            mListView = listView;
        }

    }
}