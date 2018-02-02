using Iuker.Common;
using Iuker.Common.Constant;
using Iuker.UnityKit.Run.Module.View.WidgetCreaters;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA
{
    /// <summary>
    /// MVDA视图空间创建插件
    /// </summary>
    public class WidgetCreater
    {
        private static GameObject _selectedGameObject;

        [MenuItem("GameObject/Iuker/视图控件/Input Filed")]
        private static void MenuIukerInputField()
        {
            _selectedGameObject = Selection.activeGameObject;
            var iukInputField = InputFieldCreater.Create();
            iukInputField.DependentGo.transform.SetParent(_selectedGameObject.transform);
            iukInputField.DependentGo.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/Button")]
        private static void MenuButton()
        {
            _selectedGameObject = Selection.activeGameObject;
            var button = ButtonCreater.Create();
            button.DependentGo.transform.SetParent(_selectedGameObject.transform);
            button.DependentGo.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/Image")]
        private static void MenuImage()
        {
            _selectedGameObject = Selection.activeGameObject;
            var image = ImageCreater.Create();
            image.DependentGo.transform.SetParent(_selectedGameObject.transform);
            image.DependentGo.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/RawImage")]
        private static void MenuRawImage()
        {
            _selectedGameObject = Selection.activeGameObject;
            var rawImage = RawImageCreater.Create();
            rawImage.DependentGo.transform.SetParent(_selectedGameObject.transform);
            rawImage.DependentGo.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/Text")]
        private static void MenuText()
        {
            _selectedGameObject = Selection.activeGameObject;
            var text = TextCreater.Create();
            text.DependentGo.transform.SetParent(_selectedGameObject.transform);
            text.DependentGo.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/Toggles")]
        private static void MenuToggle()
        {
            _selectedGameObject = Selection.activeGameObject;
            var toggle = ToggleCreater.Create();
            toggle.DependentGo.transform.SetParent(_selectedGameObject.transform);
            toggle.DependentGo.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/Dropdown")]
        private static void MenuDropdown()
        {
            _selectedGameObject = Selection.activeGameObject;
            var dropdown = DropdownCreater.Create();
            dropdown.DependentGo.transform.SetParent(_selectedGameObject.transform);
            dropdown.DependentGo.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/Slider")]
        private static void MenuSlider()
        {
            _selectedGameObject = Selection.activeGameObject;
            var slider = SliderCreater.Create();
            slider.transform.SetParent(_selectedGameObject.transform);
            slider.transform.localScale = Vector3.one;
        }

        [MenuItem("GameObject/Iuker/视图控件/ListView")]
        private static void MenuCreateListView()
        {
            _selectedGameObject = Selection.activeGameObject;
            var listView = Resources.Load("iuker_listview").As<GameObject>();
            var newListView = Object.Instantiate(listView);
            newListView.name = "listview_" + Constant.GetTimeToken;
            newListView.transform.SetParent(_selectedGameObject.transform);
            newListView.transform.localScale = Vector3.one;
        }

    }
}
