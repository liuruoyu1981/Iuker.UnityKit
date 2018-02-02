/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/03 11:47:35
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

using System.Collections.Generic;
using Iuker.UnityKit.Run.Module.Asset;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图
    /// </summary>
    public interface IView : IWidgetContainer, IViewActionRequester<IView>
    {
        #region 属性字段

        /// <summary>
        /// 视图类型
        /// </summary>
        ViewType ViewType { get; }

        /// <summary>
        /// 寄生视图Id列表
        /// </summary>
        List<string> ParasiticList { get; }

        /// <summary>
        /// 遮罩视图Id
        /// </summary>
        string MaskViewId { get; }

        /// <summary>
        /// 是否是主视图
        /// </summary>
        bool IsMain { get; }

        /// <summary>
        /// 是否点击空白（根按钮）即关闭
        /// </summary>
        bool IsBlankClose { get; }

        bool IsHideOther { get; }
        bool IsCloseTop { get; }


        #endregion

        #region 生命周期

        /// <summary>
        /// 初始化视图实例
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="viewPrefabRef"></param>
        /// <returns></returns>
        IView Init(string viewId, ViewRef viewPrefabRef);

        void Active();

        void Hide();

        void Close();

        #endregion

        #region 性能优化

        void Remember(ViewLifeEventType type, bool isExist);

        void Remember(ViewDrawType type, bool isExist);




        #endregion

    }
}
