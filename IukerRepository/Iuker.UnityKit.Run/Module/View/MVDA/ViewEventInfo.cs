/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/22 10:16
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

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图生命周期事件数据
    /// </summary>
    public class ViewEventInfo
    {
        public string ViewId { get; private set; }

        public Action<IView> LifeEventAction { get; private set; }

        public ViewLifeEventType LifeEventType { get; private set; }

        public int ExecuteCount { get; private set; }

        public ViewEventInfo(string viewId, Action<IView> viewAction, ViewLifeEventType lifeEventType,
            int executeCount)
        {
            ExecuteCount = -1;
            ViewId = viewId;
            LifeEventAction = viewAction;
            LifeEventType = lifeEventType;
            ExecuteCount = executeCount;
        }
    }
}