using System;
using UnityEngine;

namespace Iuker.UnityKit
{
    public class IukScrollView : IDisposable
    {
        public IukScrollView(Rect rect1, Vector2 vector2, Rect rect2)
        {
            UnityEngine.GUI.BeginScrollView(rect1, vector2, rect2);
        }

        public void Dispose()
        {
            GUI.EndScrollView();
        }
    }
}