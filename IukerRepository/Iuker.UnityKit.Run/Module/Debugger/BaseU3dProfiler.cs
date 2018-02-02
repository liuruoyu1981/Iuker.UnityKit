using Iuker.Common.Base;
using UnityEngine;

namespace Run.Iuker.UnityKit.Run.Module.Debugger
{
#if DEBUG
    /// <summary>
    /// Unity3d基础性能分析器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170911 17:54:19")]
    [ClassPurposeDesc("Unity3d基础性能分析器", "Unity3d基础性能分析器")]
#endif
    public class BaseU3dProfiler
    {
        // 帧率计算频率
        private const float calcRate = 0.5f;
        // 本次计算频率下帧数
        private int frameCount = 0;
        // 频率时长
        private float rateDuration = 0f;
        // 显示帧率
        private int fps = 0;

        private void UpdateFrame()
        {
            ++frameCount;
            rateDuration += Time.deltaTime;
            if (rateDuration > calcRate)
            {
                // 计算帧率
                fps = (int)(frameCount / rateDuration);
                frameCount = 0;
                rateDuration = 0f;
            }
        }

        /// <summary>
        /// 基础调试信息显示
        /// </summary>
        public void Render()
        {
            UpdateFrame();
#pragma warning disable 618
            GUILayout.TextField("总内存：" + ByteToM(UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory()).ToString("F") + "M");
#pragma warning restore 618
#pragma warning disable 618
            GUILayout.TextField("堆内存：" + ByteToM(UnityEngine.Profiling.Profiler.GetMonoUsedSize()).ToString("F") + "M");
#pragma warning restore 618
            GUILayout.TextField("FPS：" + fps);
        }

        private float ByteToM(long byteCount)
        {
            return byteCount / (1024.0f * 1024.0f);
        }
    }
}
