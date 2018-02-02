using System.Collections.Generic;
using System.Linq;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    public abstract class AbsWidgetContainer : IWidgetContainer
    {

        #region 基础属性

        public RectTransform RectRoot { get; protected set; }
        public string AssetName { get; protected set; }
        public IU3dFrame U3DFrame { get; protected set; }
        public IView AttachView { get; protected set; }

        #endregion

        #region 生命周期

        public abstract void Active();

        public abstract void Hide();

        public abstract void Close();

        #endregion

        #region 控件字典

        protected readonly Dictionary<string, GameObject> ContainerDictionary = new Dictionary<string, GameObject>();
        protected readonly Dictionary<string, IButton> ButtonDictionary = new Dictionary<string, IButton>();
        protected readonly Dictionary<string, IInputField> InputFieldDictionary = new Dictionary<string, IInputField>();
        protected readonly Dictionary<string, IText> TextDictionary = new Dictionary<string, IText>();
        protected readonly Dictionary<string, IImage> ImageDictionary = new Dictionary<string, IImage>();
        protected readonly Dictionary<string, IRawImage> RawImageDictionary = new Dictionary<string, IRawImage>();
        protected readonly Dictionary<string, IToggle> ToggleDictionary = new Dictionary<string, IToggle>();
        protected readonly Dictionary<string, ISlider> SliderDictionary = new Dictionary<string, ISlider>();
        protected readonly Dictionary<string, ITabGroup> TabGroupDictionary = new Dictionary<string, ITabGroup>();
        protected readonly Dictionary<string, IListView> ListViewDictionary = new Dictionary<string, IListView>();
        protected readonly Dictionary<string, IToggleGroup> ToggleGrouopDictionary = new Dictionary<string, IToggleGroup>();

        #endregion

        #region 控件组获取属性

        private List<IButton> mAllButton;

        public List<IButton> AllButton
        {
            get
            {
                return mAllButton ?? (mAllButton = ButtonDictionary.Values.ToList());
            }
        }

        private List<GameObject> mAllContainer;

        public List<GameObject> AllContainer
        {
            get
            {
                return mAllContainer ?? (mAllContainer = ContainerDictionary.Values.ToList());
            }
        }

        private List<IInputField> mAllInputField;

        public List<IInputField> AllInputField
        {
            get
            {
                return mAllInputField ?? (mAllInputField = InputFieldDictionary.Values.ToList());
            }
        }

        private List<IText> mAllText;

        public List<IText> AllText
        {
            get
            {
                return mAllText ?? (mAllText = TextDictionary.Values.ToList());
            }
        }

        private List<IImage> mAllImage;

        public List<IImage> AllImage
        {
            get
            {
                return mAllImage ?? (mAllImage = ImageDictionary.Values.ToList());
            }
        }

        private List<IToggle> mAllToggle;

        public List<IToggle> AllToggle
        {
            get
            {
                return mAllToggle ?? (mAllToggle = ToggleDictionary.Values.ToList());
            }
        }

        private List<ISlider> mAllSlider;

        public List<ISlider> AllSlider
        {
            get
            {
                return mAllSlider ?? (mAllSlider = SliderDictionary.Values.ToList());
            }
        }

        private List<ITabGroup> mAllTabGroup;

        public List<ITabGroup> AllTabGroup
        {
            get
            {
                return mAllTabGroup ?? (mAllTabGroup = TabGroupDictionary.Values.ToList());
            }
        }

        private List<IRawImage> mAllRawImage;

        public List<IRawImage> AllRawImage
        {
            get
            {
                return mAllRawImage ?? (mAllRawImage = RawImageDictionary.Values.ToList());
            }
        }

        private List<IListView> mAllListView;

        public List<IListView> AllListView
        {
            get
            {
                return mAllListView ?? (mAllListView = ListViewDictionary.Values.ToList());
            }
        }

        private List<IToggleGroup> mToggleGroups;

        public List<IToggleGroup> AllToggleGroups
        {
            get
            {
                return mToggleGroups ?? (mToggleGroups = ToggleGrouopDictionary.Values.ToList());
            }
        }


        #endregion

        #region 控件获取

        /// <summary>
        /// 控件名映射字典
        /// </summary>
        protected Dictionary<string, GameObject> mHideGoMaps = new Dictionary<string, GameObject>();

        public GameObject GetContainer(string path)
        {
            var result = ContainerDictionary.ContainsKey(path) ? ContainerDictionary[path] : null;
            return result;
        }

        public IButton GetButton(string path)
        {
            var result = ButtonDictionary.ContainsKey(path) ? ButtonDictionary[path] : null;
            return result;
        }

        public IText GetText(string path)
        {
            var result = TextDictionary.ContainsKey(path) ? TextDictionary[path] : null;
            return result;
        }

        public IImage GetImage(string path)
        {
            var result = ImageDictionary.ContainsKey(path) ? ImageDictionary[path] : null;
            return result;
        }

        public IRawImage GetRawImage(string path)
        {
            var result = RawImageDictionary.ContainsKey(path) ? RawImageDictionary[path] : null;
            return result;
        }

        public IToggle GetToggle(string path)
        {
            var result = ToggleDictionary.ContainsKey(path) ? ToggleDictionary[path] : null;
            return result;
        }

        public ISlider GetSlider(string path)
        {
            var result = SliderDictionary.ContainsKey(path) ? SliderDictionary[path] : null;
            return result;
        }

        public IInputField GetInputField(string path)
        {
            var result = InputFieldDictionary.ContainsKey(path) ? InputFieldDictionary[path] : null;
            return result;
        }

        public IListView GetListView(string path)
        {
            var result = ListViewDictionary.ContainsKey(path) ? ListViewDictionary[path] : null;
            return result;
        }

        #endregion

    }
}
