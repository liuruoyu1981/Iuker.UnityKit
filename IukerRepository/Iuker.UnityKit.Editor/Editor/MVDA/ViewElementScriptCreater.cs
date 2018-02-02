using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Base;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA
{
#if DEBUG
    /// <summary>
    /// 视图元素（视图，视图碎片等）脚本创建器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170913 12:04:08")]
    [ClassPurposeDesc("视图元素（视图，视图碎片等）脚本创建器", "视图元素（视图，视图碎片等）脚本创建器")]
#endif
    public class ViewScriptCreaterBase
    {
        /// <summary>
        /// 视图根集合
        /// </summary>
        public static readonly HashSet<string> ViewRootHashSet = new HashSet<string>
        {
            "view_background_root",
            "view_normal_root",
            "view_parasitic_root",
            "view_popup_root",
            "view_top_root",
            "view_mask_root",
        };
        protected GameObject _selectedGameObject;
        protected readonly List<WidgetPathInfo> _viewWidgetGetPathList = new List<WidgetPathInfo>();
        protected bool sIsError;
        protected readonly Dictionary<string, Type> UnityDefualtNameDictionary = new Dictionary<string, Type>()
        {
            {"Text",typeof( UnityEngine.UI.Text) },
            {"Button",typeof( UnityEngine.UI.Button) },
            {"InputField",typeof(UnityEngine.UI.InputField) },
            {"Toggles",typeof(UnityEngine.UI.Toggle) },
            {"Image",typeof(UnityEngine.UI.Image) },
            {"RawImage",typeof(UnityEngine.UI.RawImage) },
            {"Slider",typeof(UnityEngine.UI.Slider) },
            {"Dropdown",typeof(UnityEngine.UI.Dropdown) },
        };

        protected List<string> _containerList;
        protected List<string> _buttonList;
        protected List<string> _textList;
        protected List<string> _inputFieldList;
        protected List<string> _imageList;
        protected List<string> _rawImageList;
        protected List<string> _toggleList;
        protected List<string> _sliderList;
        protected List<string> _tabgroupList;

        protected readonly Dictionary<string, GameObject> ViewWidgetsDictionary
            = new Dictionary<string, GameObject>();

        protected ViewScriptCreaterBase()
        {
            _selectedGameObject = Selection.activeGameObject;

            if (ViewRootHashSet.Contains(_selectedGameObject.name))
            {
                sIsError = true;
                EditorUtility.DisplayDialog("错误", "不能选择视图分层挂载根对象", "确定");
                return;
            }

            SaveAllViewWidgets(_selectedGameObject, ViewWidgetsDictionary);
        }

        protected void SaveAllViewWidgets(GameObject root, Dictionary<string, GameObject> elementDictionary)
        {
            Transform trm = root.transform;
            int i = 0;
            int childCount = trm.childCount;
            while (i < childCount)
            {
                GameObject currentGo = trm.GetChild(i).gameObject;
                if (!elementDictionary.ContainsKey(currentGo.name)
                    && !UnityDefualtNameDictionary.ContainsKey(currentGo.name) // 不是unity3d UI组件的默认名
                )   // 不是容器
                {
                    elementDictionary.Add(currentGo.name, currentGo.gameObject);
                }
                SaveAllViewWidgets(currentGo.gameObject, elementDictionary);
                i++;
            }
            var keys = ViewWidgetsDictionary.Keys.ToList();
            // 分别保存所有类型控件名至对应列表
            _containerList = keys.FindAll(r => r.StartsWith("container"));
            _buttonList = keys.FindAll(r => r.StartsWith("button") && r != "button_root");
            _textList = keys.FindAll(r => r.StartsWith("text"));
            _inputFieldList = keys.FindAll(r => r.StartsWith("inputfield"));
            _imageList = keys.FindAll(r => r.StartsWith("image"));
            _rawImageList = keys.FindAll(r => r.StartsWith("rawimage"));
            _toggleList = keys.FindAll(r => r.StartsWith("toggle"));
            _sliderList = keys.FindAll(r => r.StartsWith("slider"));
            _tabgroupList = keys.FindAll(r => r.StartsWith("tabgroup"));
        }
    }
}
