using Iuker.Common.Base;

namespace Iuker.Common.Module.Communication.Http.Enums
{
#if DEBUG
    /// <summary>
    /// Http行为操作设置
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 12:59:31")]
    [EmumPurposeDesc("Http行为操作设置", "Http行为操作设置")]
#endif
    public enum HttpCompletionOption
    {
        /// <summary>
        /// 阅读所有的响应内容而不更新进度
        /// Read all response content without raising progress updates
        /// </summary>
        AllResponseContent,

        /// <summary>
        /// 在块中读取响应内容并更新进度
        /// </summary>
        StreamResponseContent
    }
}
