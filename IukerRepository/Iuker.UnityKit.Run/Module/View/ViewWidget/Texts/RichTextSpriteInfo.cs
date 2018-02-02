using System;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget.Texts
{
    [Serializable]
    public class RichTextSpriteInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 名称
        /// </summary>
        public string name;

        /// <summary>
        /// 中心点
        /// </summary>
        public Vector2 pivot;

        /// <summary>
        ///坐标及宽高
        /// </summary>
        public Rect rect;

        /// <summary>
        /// 精灵
        /// </summary>
        public Sprite sprite;

        /// <summary>
        /// 标签
        /// </summary>
        public string tag;

        /// <summary>
        /// uv
        /// </summary>
        public Vector2[] uv;
    }
}