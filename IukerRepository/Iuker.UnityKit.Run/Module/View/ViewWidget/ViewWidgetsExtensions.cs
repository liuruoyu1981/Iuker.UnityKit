using System.Collections.Generic;
using Iuker.UnityKit.Run.LinqExtensions;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    /// <summary>
    /// MVDA视图控件扩展
    /// </summary>
    public static class ViewWidgetsExtensions
    {
        public static IButton AddTo(this IButton button, string path, Dictionary<string, IButton> dictionary)
        {
            dictionary.Add(path, button);
            return button;
        }

        public static IText AddTo(this IText text, string path, Dictionary<string, IText> dictionary)
        {
            dictionary.Add(path, text);
            return text;
        }

        public static IInputField AddTo(this IInputField inputField, string path, Dictionary<string, IInputField> dictionary)
        {
            dictionary.Add(path, inputField);
            return inputField;
        }

        public static IImage AddTo(this IImage image, string path, Dictionary<string, IImage> dictionary)
        {
            dictionary.Add(path, image);
            return image;
        }

        public static IToggle AddTo(this IToggle toggle, string path, Dictionary<string, IToggle> dictionary)
        {
            dictionary.Add(path, toggle);
            return toggle;
        }

        public static IRawImage AddTo(this IRawImage rawImage, string path, Dictionary<string, IRawImage> dictionary)
        {
            dictionary.Add(path, rawImage);
            return rawImage;
        }

        public static ISlider AddTo(this ISlider toggle, string path, Dictionary<string, ISlider> dictionary)
        {
            dictionary.Add(path, toggle);
            return toggle;
        }

        public static ITabGroup AddTo(this ITabGroup tabGroup, string path, Dictionary<string, ITabGroup> dictionary)
        {
            dictionary.Add(path, tabGroup);
            return tabGroup;
        }

        public static IToggleGroup AddTo(this IToggleGroup toggleGroup, string path,
            Dictionary<string, IToggleGroup> dictionary)
        {
            dictionary.Add(path, toggleGroup);
            return toggleGroup;
        }

        public static IListView AddTo(this IListView listView, string path, Dictionary<string, IListView>
            dictionary)
        {
            dictionary.Add(path, listView);
            return listView;
        }

        public static GameObject AddTo(this GameObject go, string path, Dictionary<string, GameObject> dictionary)
        {
            dictionary.Add(path, go);
            return go;
        }

        /// <summary>
        /// 查找视图根游戏对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="targert"></param>
        public static void FindViewRoot(this GameObject parent, out GameObject targert)
        {
            while (true)
            {
                var parentName = parent.name;
                var grandfatherName = parent.Parent().name;
                if (!parentName.Contains("view"))
                {
                    FindViewRoot(parent.Parent(), out targert);
                }
                if (!ViewRootHashSet.Contains(grandfatherName))
                {
                    parent = parent.Parent();
                    continue;
                }
                targert = parent;
                break;
            }
        }

        /// <summary>
        /// 视图根集合
        /// </summary>
        private static readonly HashSet<string> ViewRootHashSet = new HashSet<string>
        {
            "view_background_root",
            "view_normal_root",
            "view_parasitic_root",
            "view_popup_root",
            "view_top_root",
            "view_mask_root",
        };
    }
}
