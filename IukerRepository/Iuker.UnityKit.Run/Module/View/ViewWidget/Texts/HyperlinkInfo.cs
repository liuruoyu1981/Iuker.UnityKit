using System.Collections.Generic;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget.Texts
{
    /// <summary>
    /// 超链接信息
    /// </summary>
    public class HyperlinkInfo
    {
        public int Id;

        public int StartIndex;

        public int EndIndex;

        public string Name;

        public readonly List<Rect> Boxes = new List<Rect>();
    }
}