using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;


namespace Iuker.UnityKit.Run.ViewWidget
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class IukTab : IukButton
    {
        private ITabGroup mTabGroup;
        private int mTabIndex;

        public void SetTabIndex(ITabGroup tabGroup, int index)
        {
            mTabGroup = tabGroup;
            mTabIndex = index;
        }

        public override IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            base.Init(u3DFrame, view);
            mTabControlTarget = transform.Find("tab_control_target").gameObject;

            return this;
        }

        protected override void OnClickDefaultDo()
        {
            base.OnClickDefaultDo();

            // 如果启动器框架实例为空则说明没有进入框架流程，可能是以下几种情况
            // 1. 测试视图预制件
            // 2. 测试其他功能
            if (Bootstrap.U3DFrame == null) return;

            // 如果处于新手引导状态下，则转让交互控制处理权
            if (Bootstrap.U3DFrame.AppContext.AppStatus == U3dAppStatus.Guide) return;

            // 播放点击音效，如果有
            PlayClickSoundEffect();
            //  将当前激活Tab切换为自身
            mTabGroup.SetTab(mTabIndex);
        }

        /// <summary>
        /// Tab组件操作的目标对象
        /// </summary>
        private GameObject mTabControlTarget;

        public void SetShow(bool status)
        {
            mTabControlTarget.SetActive(status);
        }

    }

}
