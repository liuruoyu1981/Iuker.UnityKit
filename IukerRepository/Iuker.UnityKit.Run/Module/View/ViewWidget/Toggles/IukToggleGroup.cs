using System.Collections.Generic;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// 开关组
    /// </summary>
    public class IukToggleGroup : MonoBehaviour, IToggleGroup
    {
        #region 字段

        private IU3dFrame mU3DFrame;
        private IFragment mFragment;
        private IView mView;

        #endregion

        #region 属性

        private readonly List<IToggle> mToggles = new List<IToggle>();
        public List<IToggle> Toggles { get { return mToggles; } }
        public void SwitchActiveToggle(IToggle target)
        {
            foreach (var toggle in mToggles)
            {
                if (toggle != target)
                {
                    toggle.IsOn = false;
                }
            }
        }

        public IToggle ActiveToggle { get; private set; }

        #endregion


        public void Init(IU3dFrame frame, IView view)
        {
            mU3DFrame = frame;
            mView = view;
            GetToggles();
        }

        public void Init(IU3dFrame frame, IFragment fragment)
        {
            mU3DFrame = frame;
            mFragment = fragment;
            GetToggles();
        }

        private void GetToggles()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var toggle = child.GetComponent<IToggle>();
                toggle.BindingToggleGrouop(this);
                mToggles.Add(toggle);
            }
        }


    }

}
