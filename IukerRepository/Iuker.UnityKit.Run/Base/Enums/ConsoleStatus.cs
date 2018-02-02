using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Base.Enums
{
    /// <summary>
    /// 控制台状态
    /// </summary>
#if DEBUG
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 12:42:17")]
    [EmumPurposeDesc("调试器状态", "调试器状态")]
#endif
    public enum DebuggerStatus
    {
        /// <summary>
        /// 控制台隐藏
        /// </summary>
        Hide,

        /// <summary>
        /// 控制台显示
        /// </summary>
        Show,
    }
}
