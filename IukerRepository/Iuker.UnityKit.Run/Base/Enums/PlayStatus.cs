using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Base.Enums
{
    /// <summary>
    /// 视频播放状态
    /// </summary>
#if DEBUG
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 12:51:20")]
    [EmumPurposeDesc("视频播放状态", "视频播放状态")]
#endif
    public enum PlayStatus
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,

        /// <summary>
        /// 播放中
        /// </summary>
        Playing,

        /// <summary>
        /// 暂停
        /// </summary>
        Pause,

        /// <summary>
        /// 播放结束
        /// </summary>
        Over,
    }
}
