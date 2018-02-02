/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2/12/2017 19:33:07
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 事件处理器
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;

namespace Iuker.Common.Event
{
    /// <summary>
    /// 可计数事件处理器
    /// </summary>
    public sealed class TallyEventHandler : ITallyEventHandler
    {
        public TallyEventHandler(int residueCount, Action eventAction)
        {
            ResidueCount = residueCount;
            EventAction = eventAction;
        }

        public void HandleEvent()
        {
            if (EventAction == null) return;
            if (ResidueCount < 0)
            {
                EventAction();
                return;
            }
            if (ResidueCount > 0)
            {
                ResidueCount--;
                EventAction();
            }
        }

        public Action EventAction { get; private set; }

        public int ResidueCount { get; private set; }
    }


    /// <summary>
    /// 可计数事件处理器
    /// </summary>
    /// <typeparam name="TData">泛型数据类型</typeparam>
    public sealed class TallyEventHandler<TData> : ITallyEventHandler<TData>
    {
        public TallyEventHandler(int residueCount, Action<TData> eventAction)
        {
            ResidueCount = residueCount;
            EventAction = eventAction;
        }

        public void HandleEvent(TData eventData)
        {
            if (EventAction == null) return;
            if (ResidueCount < 0)
            {
                EventAction(eventData);
                return;
            }
            if (ResidueCount > 0)
            {
                EventAction(eventData);
                ResidueCount--;
            }
        }

        public Action<TData> EventAction { get; private set; }

        public int ResidueCount { get; private set; }
    }
}