using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Base
{
    /// <summary>
    /// 绘制工具
    /// </summary>
    public static class GraphicUtility
    {
        public static Color ColorFrom256(int r, int g, int b, int a) { return new Color(r / 255f, g / 255f, b / 255f, a / 255f); }
        public static Color ColorFrom256(int r, int g, int b) { return new Color(r / 255f, g / 255f, b / 255f); }

        public static void PrintManifestResources()
        {
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            Debug.Log("------------------");
            Debug.Log("Manifest Resources");
            Debug.Log("------------------");
            for (int i = 0; i < resourceNames.Length; i++) Debug.Log(i + 1 + ") " + resourceNames[i]);
            Debug.Log("--------END-------");
        }

        public static void SetGlobalTintColor(Color darkSkinColor, Color lightSkinColor)
        {
            GUI.color = EditorGUIUtility.isProSkin ? darkSkinColor : lightSkinColor;
        }

        public static void SetGlobalTintColor(Color color) { GUI.color = color; }

        public static void SetTextColor(Color darkSkinColor, Color lightSkinColor)
        {
            GUI.contentColor = EditorGUIUtility.isProSkin ? darkSkinColor : lightSkinColor;
        }

        public static void SetTextColor(Color color) { GUI.contentColor = color; }

        public static void SetBackgroundColor(Color darkSkinColor, Color lightSkinColor)
        {
            GUI.backgroundColor = EditorGUIUtility.isProSkin ? darkSkinColor : lightSkinColor;
        }

        public static void SetBackgroundColor(Color color) { GUI.backgroundColor = color; }


    }
}