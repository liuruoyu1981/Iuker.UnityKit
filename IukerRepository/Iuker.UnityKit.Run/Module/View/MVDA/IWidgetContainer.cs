using System.Collections.Generic;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    public interface IWidgetContainer : IViewElement
    {
        #region 属性字段

        /// <summary>
        /// 视图的根节点游戏对象
        /// </summary>
        RectTransform RectRoot { get; }

        /// <summary>
        /// 视图的资源名（即预制件的名字）
        /// </summary>
        string AssetName { get; }

        #endregion

        #region 控件获取

        GameObject GetContainer(string path);

        IButton GetButton(string path);

        IText GetText(string path);

        IImage GetImage(string path);

        IRawImage GetRawImage(string path);

        IToggle GetToggle(string path);
        ISlider GetSlider(string path);

        IInputField GetInputField(string path);

        IListView GetListView(string path);

        #endregion

        #region 控件组获取属性

        /// <summary>
        /// 视图下所持有的所有按钮
        /// </summary>
        List<IButton> AllButton { get; }
        List<GameObject> AllContainer { get; }

        List<IInputField> AllInputField { get; }

        List<IText> AllText { get; }

        List<IImage> AllImage { get; }

        List<IToggle> AllToggle { get; }

        List<ISlider> AllSlider { get; }

        List<IRawImage> AllRawImage { get; }

        List<IListView> AllListView { get; }

        List<IToggleGroup> AllToggleGroups { get; }

        #endregion
    }
}