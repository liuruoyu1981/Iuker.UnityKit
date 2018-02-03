using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
#if DEBUG
    /// <summary>
    /// 滚动列表
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20171010 16:34:59")]
    [InterfacePurposeDesc("滚动列表", "滚动列表")]
#endif
    public interface IListView : IukViewWidget, IViewElement
    {
        List<object> Datas { get; }

        /// <summary>
        /// 设置滚动列表子项模板
        /// </summary>
        /// <param name="datas">列表数据集合</param>
        /// <param name="updateItem">列表项更新绘制委托</param>
        /// <param name="itemName">可选的指定模板名</param>
        /// <param name="pollDownAction">下拉更新委托。</param>
        void SetItemTemplate(List<object> datas, Action<GameObject, int> updateItem = null,
           string itemName = "liveviewitem", Action pollDownAction = null);

        /// <summary>
        /// 添加当前数据索引数据
        /// </summary>
        /// <param name="dataIndex"></param>
        void AddItem(int dataIndex);

        /// <summary>
        /// 删除当前数据索引下的数据
        /// </summary>
        /// <param name="dataIndex"></param>
        void DeleteItem(int dataIndex);

        /// <summary>
        /// 增加一个数据到数据列表末尾。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        void AddDataToEnd<T>(params T[] datas);

        void AddDataToHead<T>(params T[] datas);

    }
}
