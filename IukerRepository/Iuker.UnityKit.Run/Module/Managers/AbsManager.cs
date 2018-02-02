using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.Event;

namespace Iuker.UnityKit.Run.Module.Managers
{
    /// <summary>
    /// 管理器基类
    /// </summary>
    public abstract class AbsManager : IManager
    {
        protected IU3dFrame U3DFrame { get; private set; }
        protected IU3dAppEventModule EventModule;

        public virtual void Init(IU3dFrame frame)
        {
            U3DFrame = frame;
            EventModule = U3DFrame.EventModule;
            RegisterEvent();
        }

        protected virtual void RegisterEvent()
        {
            EventModule.WatchEvent(U3dEventCode.App_HotUpdateComplete.Literals, OnHotUpdateComplete);
        }

        protected virtual void OnHotUpdateComplete()
        {

        }





    }
}
