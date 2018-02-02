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
    /// 视图脚本创建器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170913 12:05:29")]
    [ClassPurposeDesc("视图脚本创建器", "视图脚本创建器")]
#endif
    public class ViewScriptCreater
    {
        protected GameObject _selectedGameObject;
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

        protected static Dictionary<string, GameObject> viewWidgetsDictionary 
            = new Dictionary<string, GameObject>();

        public ViewScriptCreater()
        {
            viewWidgetsDictionary.Clear();
            _selectedGameObject = Selection.activeGameObject;

            switch (_selectedGameObject.name)
            {
                case "view_background_root":
                case "view_normal_root":
                case "view_parasitic_root":
                case "view_popup_root":
                case "view_top_root":
                case "view_mask_root":
                    sIsError = true;
                    EditorUtility.DisplayDialog("错误", "不能选择视图分层挂载根对象", "确定");
                    break;
            }

            SaveAllViewWidgets(_selectedGameObject, viewWidgetsDictionary);
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
            var keys = viewWidgetsDictionary.Keys.ToList();
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
