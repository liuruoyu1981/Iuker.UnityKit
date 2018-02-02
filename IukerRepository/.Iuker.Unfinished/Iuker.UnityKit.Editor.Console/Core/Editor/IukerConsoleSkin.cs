/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/18/2017 14:40
Email: liuruoyu1981@gmail.com
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Console.Core.Editor
{
    /// <summary>
    /// 控制台皮肤设置
    /// </summary>
    public class IukerConsoleSkin
    {
        public static GUIStyle EvenBackStyle => GUI.skin.FindStyle("CN EntryBackEven");

        public static GUIStyle BoxStyle => GUI.skin.FindStyle("CN Box");

        public static GUIStyle CollapseStyle => GUI.skin.FindStyle("CN CountBadge");

        public static GUIStyle ButtonStyle => GUI.skin.FindStyle("Button");

        public static GUIStyle ToolbarButtonStyle => GUI.skin.FindStyle("ToolbarButton");

        public static GUIStyle ToolbarSearchTextFieldStyle => GUI.skin.FindStyle("ToolbarSeachTextField");

        public static GUIStyle ToolbarSearchCancelButtonStyle => GUI.skin.FindStyle("ToolbarSeachCancelButton");

        public static GUIStyle MessageStyle => GUI.skin.FindStyle("CN Message");

        public static GUIStyle LogInfoStyle => GUI.skin.FindStyle("CN EntryInfo");

        public static GUIStyle LogWarnStyle => GUI.skin.FindStyle("CN EntryWarn");

        public static GUIStyle LogErrorStyle => GUI.skin.FindStyle("CN EntryError");

        public static GUIStyle ToolbarStyle => GUI.skin.FindStyle("Toolbar");


        public static Texture2D EvenBackTexture => EvenBackStyle.normal.background;

        public static GUIStyle OddBackStyle => GUI.skin.FindStyle("CN EntryBackOdd");

        public static Texture2D OddBackTexture => OddBackStyle.normal.background;

        public static Texture2D InfoIcon => EditorGUIUtility.FindTexture("d_console.infoicon");

        public static Texture2D InfoIconSmall => EditorGUIUtility.FindTexture("d_console.infoicon.sml");

        public static Texture2D WarningIcon => EditorGUIUtility.FindTexture("d_console.warnicon");

        public static Texture2D WarningIconSmall => EditorGUIUtility.FindTexture("d_console.warnicon.sml");

        public static Texture2D ErrorIcon => EditorGUIUtility.FindTexture("d_console.erroricon");

        public static Texture2D ErrorIconSmall => EditorGUIUtility.FindTexture("d_console.erroricon.sml");
    }
}