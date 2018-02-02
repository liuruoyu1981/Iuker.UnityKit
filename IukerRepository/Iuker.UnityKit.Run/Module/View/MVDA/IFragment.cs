/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/03 15:09:42
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using Iuker.UnityKit.Run.Base;
using UnityEngine;
using Iuker.UnityKit.Run.Module.Asset;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图碎片
    /// 多个界面共用且动态加载的UI元素
    /// </summary>
    public interface IFragment : IViewElement, IViewActionRequester<IFragment>
    {
        /// <summary>
        /// 视图碎片的根节点游戏对象
        /// </summary>
        RectTransform RectRoot { get; }

        /// <summary>
        /// 视图碎片的预制件名
        /// </summary>
        string AssetName { get; }

        /// <summary>
        /// 视图碎片ID
        /// </summary>
        string FragmentId { get; }

        /// <summary>
        /// 初始化一个视图碎片
        /// </summary>
        /// <param name="fragmentId">视图碎片Id</param>
        /// <param name="view"></param>
        /// <param name="u3DFrame"></param>
        /// <param name="fragmentRef"></param>
        /// <returns></returns>
        IFragment Init(string fragmentId, IView view, IU3dFrame u3DFrame, FragmentRef fragmentRef);

        void Active();

        void Hide();

        void Close();

    }
}
