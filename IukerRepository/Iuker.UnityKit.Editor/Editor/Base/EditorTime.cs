using UnityEditor;

namespace Iuker.UnityKit.Editor.Base
{
    [InitializeOnLoad]
    public class EditorTime
    {
        public static double DeltaTime { get; private set; }
        private static double mTime;

        static EditorTime()
        {
            EditorApplication.update += UpdateTime;
        }

        private static void UpdateTime()
        {
            var oldTime = mTime;
            mTime = EditorApplication.timeSinceStartup;
            DeltaTime = mTime - oldTime;
        }



    }
}