/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 23:31:44
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Hierarchy
{
    /// <summary>
    /// 辅助功能
    /// </summary>
    public class AssistWindow : EditorWindow
    {
        private static AssistWindow _window;
        private static List<GameObject> _selectGameObjectList;

        /// <summary>
        /// 要替换的文本
        /// </summary>
        private static string _replaceText;

        /// <summary>
        /// 要替换的目标文本
        /// </summary>
        private static string _targetText;


        public static void ShowWindow()
        {
            _selectGameObjectList = Selection.gameObjects.ToList();
            _window = GetWindow<AssistWindow>();
            _window.titleContent.text = "游戏对象名批量替换";
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            GUIStyle labelGuiStyle = new GUIStyle();
            labelGuiStyle.fontStyle = FontStyle.Bold;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("要替换的文本", labelGuiStyle);
            _replaceText = EditorGUILayout.TextField(_replaceText);
            EditorGUILayout.LabelField("要替换的目标文本", labelGuiStyle);
            _targetText = EditorGUILayout.TextField(_targetText);
            if (GUILayout.Button("替换"))
            {
                ReplaceText();
            }
        }


        private static void ReplaceText()
        {
            foreach (var go in _selectGameObjectList)
            {
                var sourceName = go.name;
                go.name = sourceName.Replace(_replaceText, _targetText);
            }

            _window.Close();
            EditorUtility.DisplayDialog(null, "替换完成", "确定");
        }

    }
}
