/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 11:18:45
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

using System;
using System.Collections.Generic;
using Iuker.Common;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    public interface IU3dViewModule : IModule
    {
        /// <summary>
        /// 观察目标视图的生命周期事件
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="lifeEventType"></param>
        /// <param name="action"></param>
        /// <param name="executeCount"></param>
        void WatchViewLiefEvent(string viewId, ViewLifeEventType lifeEventType, Action<IView> action,
            int executeCount = -1);

        void RemoveViewLifeEvent(string viewId, Action<IView> action);

        /// <summary>
        /// 跳至目标视图
        /// </summary>
        /// <param name="viewId"></param>
        void SkipTo(string viewId);

        /// <summary>
        /// 挂载一个视图
        /// </summary>
        /// <param name="view"></param>
        void MountView(IView view);

        /// <summary>
        /// 创建一个视图
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="assetId"></param>
        /// <param name="isCache"></param>
        void CreateView(string viewId, string assetId = null, bool isCache = true);

        /// <summary>
        /// 关闭一个视图
        /// </summary>
        /// <param name="viewId"></param>
        void CloseView(string viewId);

        /// <summary>
        /// 打开一个会话视图
        /// </summary>
        /// <param name="content">会话内容</param>
        /// <param name="dialogViewId">目标视图资源Id同时也是数据Id</param>
        /// <param name="ensure">确定按钮委托</param>
        /// <param name="isShowCancel">是否显示取消按钮</param>
        void OpenDialog(string content, string dialogViewId,
            Action ensure, bool isShowCancel = false);

        /// <summary>
        /// 关闭指定类型的所有视图
        /// </summary>
        /// <param name="type">目标视图类型</param>
        /// <param name="selecter">选择器</param>
        void CloseAllByType(ViewType type, Func<IView, bool> selecter);

        /// <summary>
        /// 获取指定你类型的所有视图
        /// </summary>
        /// <param name="type">目标视图类型</param>
        /// <param name="selecter">选择器</param>
        List<IView> GetAllByType(ViewType type, Func<IView, bool> selecter);

        /// <summary>
        /// 获得目标视图
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        IView GetView(string viewId);

        /// <summary>
        /// 视图行为处理器调度器
        /// </summary>
        IViewActionDispatcher ViewActionispatcher { get; }

        /// <summary>
        /// 视图数据模型调度器
        /// </summary>
        IViewModelDispatcher ModelDispatcher { get; }

        /// <summary>
        /// 获得指定视图的生命周期外部委托列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<ViewEventInfo> GetViewEventInfos(string key);

        List<Base.Config.View> ProjectViews { get; }

    }
}
