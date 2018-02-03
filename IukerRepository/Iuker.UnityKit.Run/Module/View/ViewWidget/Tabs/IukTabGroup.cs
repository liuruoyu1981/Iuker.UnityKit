using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Base;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.ViewWidget
{

#if DEBUG
    /// <summary>
    /// 标签组控件
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170904 14:55:30")]
    [ClassPurposeDesc("标签组控件", "标签组控件")]
#endif
    public class IukTabGroup : MonoBehaviour, ITabGroup
    {
        public GameObject DependentGo { get { return gameObject; } }

        public GameObject ViewRoot
        {
            get
            {
                return AttachView.RectRoot.gameObject;
            }
        }

        #region field

        private IU3dFrame mU3DFrame;
        private IViewActionDispatcher mActionDispatcher;
        private List<IukTab> mTabs;

        #endregion


        public IukViewWidget Init(IU3dFrame u3DFrame, IView view, IFragment fragment = null)
        {
            mU3DFrame = u3DFrame;
            AttachView = view;
            mActionDispatcher = u3DFrame.ViewModule.ViewActionispatcher;
            InitTabs();

            return this;
        }

        private void InitTabs()
        {
            var sons = gameObject.GetAllChild();
            mTabs = sons.Select(s => s.GetComponent<IukTab>()).ToList();
            mTabs.ForEach(tab => tab.Init(mU3DFrame, AttachView));
            for (var i = 0; i < mTabs.Count; i++)
            {
                mTabs[i].SetTabIndex(this, i);
            }
            SetTab(0);
        }

        private string mWidgetToken;

        public string WidgetToken
        {
            get
            {
                return mWidgetToken ?? (mWidgetToken = ViewRoot.name + "_" + gameObject.name);
            }
        }

        public IView AttachView { get; private set; }

        /// <summary>
        /// 设置当前处于激活状态的Tab组件
        /// </summary>
        /// <param name="tabIndex"></param>
        public void SetTab(int tabIndex)
        {
            for (var i = 0; i < mTabs.Count; i++)
            {
                var tab = mTabs[i];
                tab.SetShow(i == tabIndex);
            }
        }
    }




}
