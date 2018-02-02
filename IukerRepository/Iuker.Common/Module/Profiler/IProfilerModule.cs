using Iuker.Common.Base;

namespace Iuker.Common.Module.Profiler
{
#if DEBUG
    /// <summary>
    /// 性能分析模块，用于动态调整App的性能参数。
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170910 15:58:27")]
    [InterfacePurposeDesc("性能分析模块，用于动态调整App的性能参数。", "性能分析模块，用于动态调整App的性能参数。")]
#endif
    public interface IProfilerModule : IModule
    {
        /// <summary>
        /// 对象池容量超出
        /// </summary>
        /// <param name="poolKey"></param>
        /// <param name="poolCount"></param>
        void PoolOut(string poolKey, int poolCount);
    }
}
