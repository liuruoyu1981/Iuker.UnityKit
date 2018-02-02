using System;
using System.Collections.Generic;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget.Texts
{
    [Serializable]
    public class RechTextSpriteInfoGroup
    {
        public string Tag = "";

        public List<RichTextSpriteInfo> RichSpriteInfos = new List<RichTextSpriteInfo>();

        public float Width = 1.0f;

        public float Size = 24.0f;

    }
}