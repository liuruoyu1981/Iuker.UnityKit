using System;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;

namespace Iuker.UnityKit.Run.Module.View
{
    /// <summary>
    /// 控件行为代理
    /// </summary>
    public class WidgetActionAgent<T> where T : IViewElement
    {
        private readonly IViewActionDispatcher mDispatcher;
        private readonly IViewActionRequest<T> mRequest;
        public bool NoResponser;
        public Action HotAction;
        public bool IsDoCsharp;

        public WidgetActionAgent(string token, IU3dFrame frame, IViewActionRequester<T> requester, bool isCell = false)
        {
            var mActionToken = token;
            mDispatcher = frame.ViewModule.ViewActionispatcher;

            if (isCell)
            {
                mRequest = frame.InjectModule.GetInstance<IViewActionRequest<T>>().Init(requester, mActionToken,
                    ViewScriptType.WidgetCell);
            }
            else
            {
                mRequest = frame.InjectModule.GetInstance<IViewActionRequest<T>>().Init(requester, mActionToken);
            }
        }

        public WidgetActionAgent(Action hotAction)
        {
            HotAction = hotAction;
        }

        public void Invoke()
        {
            if (HotAction != null)
            {
                if (IsDoCsharp)
                {
                    DoCsharp();
                    HotAction();
                }
                else
                {
                    HotAction();
                }
            }
            else
            {
                DoCsharp();
            }
        }

        private void DoCsharp()
        {
            if (!NoResponser && mRequest != null)
            {
                mDispatcher.DispatchRequest(mRequest, this);
            }
        }

    }
}