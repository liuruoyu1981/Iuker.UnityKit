using UnityEditor;

namespace Iuker.UnityKit.Utility
{
    public static class IukEditorUtility
    {
        /// <summary>
        /// 显示一个unity提示窗口并显示指定的提示内容
        /// </summary>
        /// <param name="message"></param>
        public static void DisplayDialog(string message)
        {
            EditorUtility.DisplayDialog("提示", message, "确定");
        }
    }
}