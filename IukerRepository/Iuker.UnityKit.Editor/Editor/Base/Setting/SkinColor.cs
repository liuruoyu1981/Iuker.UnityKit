using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Setting
{
    public class SkinColor
    {
        private readonly Color mNotProColor;
        private readonly Color mProColor;

        public SkinColor(Color pro, Color notPro)
        {
            mNotProColor = notPro;
            mProColor = pro;
        }

        public Color Color
        {
            get
            {
                return EditorGUIUtility.isProSkin ? mProColor : mNotProColor;
            }
        }
    }
}