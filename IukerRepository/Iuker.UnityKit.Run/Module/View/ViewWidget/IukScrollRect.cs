using UnityEngine.UI;

namespace Iuker.UnityKit.Run.ViewWidget
{
    public class IukScrollRect : ScrollRect
    {
        /// <summary>
        /// 是否执行滑动处理。
        /// </summary>
        private bool m_ExecuteScroll = true;

        protected override void LateUpdate()
        {
            if (!m_ExecuteScroll) return;

            base.LateUpdate();
        }

        public void StopScroll()
        {
            m_ExecuteScroll = false;
        }

        public void StartScroll()
        {
            m_ExecuteScroll = true;
        }
    }
}
