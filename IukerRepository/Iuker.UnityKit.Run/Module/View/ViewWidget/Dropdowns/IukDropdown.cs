using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;
using UnityEngine.UI;


namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 选项卡控件
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IukDropdown : Dropdown, IDropdown, IViewActionRequester<IDropdown>
    {
        protected IU3dFrame U3DFrame { get; private set; }
        public IViewActionDispatcher Dispatcher { get; protected set; }
        public GameObject DependentGo { get { return gameObject; } }
        public GameObject ViewRoot { get { return AttachView.RectRoot.gameObject; } }
        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            U3DFrame = u3DFrame;
            Dispatcher = u3DFrame.ViewModule.ViewActionispatcher;
            AttachView = view;
            return this;
        }

        public string WidgetToken { get; private set; }

        public IView AttachView { get; private set; }
        public IDropdown Origin { get { return this; } }
        public string ViewId { get { return ViewRoot.name; } }
        public void Issue(IViewActionRequest<IDropdown> request)
        {
            Dispatcher.DispatchRequest(request);
        }
    }
}