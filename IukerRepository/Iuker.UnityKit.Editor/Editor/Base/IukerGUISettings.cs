using Iuker.UnityKit.Editor.Setting;
using Iuker.UnityKit.Run.Base;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor
{
    /// <summary>
    /// 编辑器设置项
    /// </summary>
    public class IukerGUISettings
    {
        private static IukerGUISettings mInstance;

        private static IukerGUISettings Instance
        {
            get
            {
                return mInstance ?? (mInstance = new IukerGUISettings());
            }
        }

        /// <summary>
        /// 当前Unity编辑器的主题样式是否为专业版
        /// </summary>
        protected static bool IsProSkin
        {
            get
            {
                return EditorGUIUtility.isProSkin;
            }
        }

        /// <summary>
        /// 获得经过动态修正后最新的编辑器设置实例
        /// </summary>
        /// <returns></returns>
        public static IukerGUISettings GetFixedSettings()
        {
            return Instance;
        }

        #region Iuker Inspector Window

        public readonly SkinColor CommonSplitLineColor = new SkinColor
            (
                IukerColor.DarkGoldenrod,
                IukerColor.DarkGoldenrod
            );

        /// <summary>
        /// 中部分割线宽度
        /// </summary>
        public int MiddleSplitLineWidth = 1;

        private readonly SkinColor mLeftColor = new SkinColor(new Color(0.18f, 0.18f, 0.18f), new Color(0.4f, 0.4f, 0.4f));

        /// <summary>
        /// 左部颜色
        /// </summary>
        public Color LeftColor
        {
            get
            {
                return mLeftColor.Color;
            }
        }

        #endregion

        #region ExactComponent

        public readonly SkinGuiStyle CommonTextStyle = new SkinGuiStyle(
            new GUIStyle
            {
                alignment = TextAnchor.MiddleLeft,
                normal = new GUIStyleState
                {
                    textColor = Color.grey
                }
            },
            new GUIStyle
            {
                alignment = TextAnchor.MiddleLeft,
                normal = new GUIStyleState
                {
                    textColor = Color.black
                }
            });

        #region Title


        public readonly int FirstTitleHeight = 25;
        public readonly int FirstTitleLeftMarkWidth = 10;
        public Color FirstTitleColor = new Color(0.25f, 0.25f, 0.25f);


        #endregion

        #region ToggleManual

        /// <summary>
        /// 选中材质贴图
        /// </summary>
        public readonly Texture2D CheckBoxChecked = EditorResources.Instance.GetTexture(EditorTextureType.QCheckBoxChecked);

        /// <summary>
        /// 未选中材质贴图
        /// </summary>
        public readonly Texture2D CheckBoxUnchecked =
            EditorResources.Instance.GetTexture(EditorTextureType.QCheckBoxUnchecked);

        /// <summary>
        /// 开关组进左部缩进
        /// </summary>
        public int ToggleLeftIndent = 10;

        public int ToggleHeight = 30;

        public int ToggleContentIndent = 30;

        private readonly SkinColor mToggleFrameColor = new SkinColor(new Color(0.22f, 0.22f, 0.22f), new Color(0.78f, 0.78f, 0.78f));

        public Color ToggleFrameColor
        {
            get
            {
                return mToggleFrameColor.Color;
            }
        }

        #endregion

        #region Tab

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public GUIStyle TabSelectStyle =
         new GUIStyle
         {
             alignment = TextAnchor.MiddleLeft,
             //fontSize = 14,
             normal = new GUIStyleState { textColor = Color.black }
         };

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public GUIStyle TabUnSelectStyle =
            new GUIStyle
            {
                alignment = TextAnchor.MiddleRight,
                //fontSize = 14,
                normal = new GUIStyleState { textColor = Color.white }
            };

        public int TabOffest = 4;

        /// <summary>
        /// Tab角标的高度
        /// </summary>
        public int TabHeight = 20;

        /// <summary>
        /// Tab角标的宽度
        /// </summary>
        public int TabWidth = 100;

        /// <summary>
        /// Tab与窗口顶部的间隔距离
        /// </summary>
        public int TabTopInterval = 45;

        /// <summary>
        /// Tab被选中的颜色
        /// </summary>
        public Color TabSelectColor = new Color(1.00f, 0.90f, 0.40f);

        /// <summary>
        /// Tab未被选中时的颜色
        /// </summary>
        public Color TabUnSelectColor = new Color(0.25f, 0.25f, 0.25f);


        #endregion

        #region TextPopup

        public readonly SkinGuiStyle TextPopupTitleStyle = new SkinGuiStyle
        (
            new GUIStyle
            {
                alignment = TextAnchor.MiddleLeft,
                normal = new GUIStyleState
                {
                    textColor = Color.grey
                }
            },
            new GUIStyle
            {
                alignment = TextAnchor.MiddleLeft,
                normal = new GUIStyleState
                {
                    textColor = Color.black
                }
            });




        #endregion

        #endregion


    }
}