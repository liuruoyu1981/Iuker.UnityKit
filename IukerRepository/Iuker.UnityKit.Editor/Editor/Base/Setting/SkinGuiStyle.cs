using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Setting
{
    public class SkinGuiStyle
    {
        private readonly GUIStyle mNotProStyle;
        private readonly GUIStyle mProStyle;

        public SkinGuiStyle(GUIStyle pro, GUIStyle notPro)
        {
            mProStyle = pro;
            mNotProStyle = notPro;
        }

        public GUIStyle CurrentStyle
        {
            get
            {
                return EditorGUIUtility.isProSkin ? mProStyle : mNotProStyle;
            }
        }
    }
}