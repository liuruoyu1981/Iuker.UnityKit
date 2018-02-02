﻿/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 下午12:06:35
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
using Iuker.Common;
using UnityEditor;
using UnityEngine;


namespace Iuker.UnityKit.Editor.Hierarchy
{
    public class BatchReplaceSuffixWindow : EditorWindow
    {
        private static BatchReplaceSuffixWindow _window;
        private static List<GameObject> _selectGameObjectList;

        /// <summary>
        /// 要替换的目标文本
        /// </summary>
        private static string _targetText;

        public static void ShowWindow()
        {
            _selectGameObjectList = Selection.gameObjects.ToList();
            _window = GetWindow<BatchReplaceSuffixWindow>();
            _window.titleContent.text = "批量替换后缀";
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            GUIStyle labelGuiStyle = new GUIStyle();
            labelGuiStyle.fontStyle = FontStyle.Bold;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("新前缀", labelGuiStyle);
            _targetText = EditorGUILayout.TextField(_targetText);
            if (GUILayout.Button("替换"))
            {
                ReplaceText();
            }
        }

        private static void ReplaceText()
        {
            foreach (var gameObject in _selectGameObjectList)
            {
                var sourcename = gameObject.name;
                var tempArray = sourcename.Split('_');
                tempArray[tempArray.Length - 1] = _targetText;
                var newname = tempArray.ToUnion("_");
                gameObject.name = newname;
            }

            _window.Close();
            EditorUtility.DisplayDialog(null, "批量替换后缀完成", "确定");
        }


    }
}