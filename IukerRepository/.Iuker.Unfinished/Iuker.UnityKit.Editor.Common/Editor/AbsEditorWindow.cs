using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit
{
    /// <summary>
    /// 编辑器窗口基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbsEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        protected abstract Vector2 WindowSize { get; }

        [ThreadStatic]
        private static T mInstance;

        public static T Instance
        {
            get { return mInstance ?? (mInstance = GetWindow<T>()); }
            set { mInstance = value; }
        }


        /// <summary>
        /// 绘制顶部各编辑器窗口通用信息
        /// </summary>
        protected virtual void DrawTopCommonInfo()
        {


        }

        private void OnGUI()
        {
            DrawTopCommonInfo();
            DrawSelf();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        #region 常用编辑器绘制方法

        /// <summary>
        /// 绘制子类自身
        /// </summary>
        protected abstract void DrawSelf();

        /// <summary>
        /// 使用给定的文本及GUI样式绘制一个两边有竖线的按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        protected bool GetButtonClamped(string text, GUIStyle style) => GUILayout.Button(text, style,
            GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));

        protected bool GetToggleClamped(bool state, string text, GUIStyle style) => GUILayout.Toggle(state, text, style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));

        protected bool GetToggleClamped(bool state, GUIContent content, GUIStyle style) => GUILayout.Toggle(state,
            content, style, GUILayout.MaxWidth(style.CalcSize(content).x));

        protected void DrawToggle(ref bool source, string text, GUIStyle style, Action<bool> action)
        {
            var old = source;
            source = GUILayout.Toggle(source, text, style, GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));
            if (old != source)
            {
                action?.Invoke(source);
            }
        }

        protected void DrawToggle(ref bool source, GUIContent content, GUIStyle style, Action<bool> action)
        {
            var old = source;
            source = GUILayout.Toggle(source, content, style, GUILayout.MaxWidth(style.CalcSize(content).x));
            if (old != source)
            {
                action?.Invoke(source);
            }
        }

        /// <summary>
        /// 绘制一个下拉菜单
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="menuList"></param>
        /// <param name="menuToggles"></param>
        /// <param name="menuActions"></param>
        protected void DrawDropDown(Rect rect, string text, List<string> menuList, List<bool> menuToggles, List<GenericMenu.MenuFunction> menuActions)
        {
            if (menuList == null || menuActions == null) return;
            if (menuList.Count != menuActions.Count) return;

            if (GUI.Button(rect, text, EditorStyles.toolbarDropDown))
            {
                var menu = new GenericMenu();
                for (int i = 0; i < menuList.Count; i++)
                {
                    var menuContent = menuList[i];
                    var action = menuActions[i];
                    menu.AddItem(new GUIContent(menuContent), menuToggles[i], action);
                }
                menu.DropDown(rect);
            }
        }

        /// <summary>
        /// 绘制一个带有关闭小按钮的搜索框
        /// </summary>
        /// <param name="searchStr"></param>
        /// <param name="style"></param>
        /// <param name="action"></param>
        /// <param name="closeAction"></param>
        /// <param name="closeStyle"></param>
        /// <param name="options"></param>
        protected void DrawSearch(string searchStr, GUIStyle style, Action<string> action, Action closeAction, GUIStyle closeStyle, params GUILayoutOption[] options)
        {
            var old = searchStr;
            searchStr = EditorGUILayout.TextArea(searchStr, style, options);
            if (searchStr != old)
            {
                action?.Invoke(searchStr);
            }
            if (GUILayout.Button("", closeStyle))
            {
                closeAction?.Invoke();
            }
        }


        #endregion



    }
}