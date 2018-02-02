using System;
using UnityEditor;
using UnityEngine;

public class IukerLighting : EditorWindow
{
    #region 静态成员

    private static IukerLighting ExistWindow;
    private const float WINDOW_WIDTH = 800;
    private const float WINDOW_HEIGHT = 100;
    private const string IUKER_LIGHTING_POSITION_X = "IUKER_LIGHTING_POSITION_X";
    private const string IUKER_LIGHTING_POSITION_Y = "IUKER_LIGHTING_POSITION_Y";

    [MenuItem("Tools/IukerLighting/Config %i")]
    private static void OpenConfigTag()
    {
        ShowWindow(LightingTag.Config);
    }

    #endregion


    #region 实例成员

    private string Title;
    private LightingTag tag;


    #endregion



    private static void ShowWindow(LightingTag tag)
    {
        if (ExistWindow == null)
        {
            ExistWindow = CreateInstance<IukerLighting>();

            switch (tag)
            {
                case LightingTag.Config:
                    ExistWindow.DrawConfig();
                    break;
                case LightingTag.Explore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("tag", tag, null);
            }
        }
    }

    private void DrawConfig()
    {
        ExistWindow.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        ExistWindow.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        var potision = PositionRect(110);
        potision.y -= potision.height;
        ExistWindow.ShowAsDropDown(potision, new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT - 1));
    }

    private static Rect PositionRect(float windowHeight)
    {
        var positionX = EditorPrefs.GetFloat(IUKER_LIGHTING_POSITION_X,
            (float)Screen.currentResolution.width / 2 - WINDOW_WIDTH / 2);
        var positionY = EditorPrefs.GetFloat(IUKER_LIGHTING_POSITION_Y, (float)Screen.currentResolution
                                                                            .height / 2 - 100);
        return new Rect(positionX, positionY, WINDOW_WIDTH, windowHeight);
    }

    private enum LightingTag
    {
        Config,

        Explore,


    }
}
