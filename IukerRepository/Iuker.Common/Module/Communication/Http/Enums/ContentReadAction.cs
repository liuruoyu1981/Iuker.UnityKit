using Iuker.Common.Base;

namespace Iuker.Common.Module.Communication.Http.Enums
{
#if DEBUG
    /// <summary>
    /// Http内容读取操作类型
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 13:07:50")]
    [EmumPurposeDesc("Http内容读取操作类型", "Http内容读取操作类型")]
#endif
    public enum ContentReadAction
    {
        Multi,

        /// <summary>
        /// 字节数组
        /// </summary>
        ByteArray,

        /// <summary>
        /// 内存流
        /// </summary>
        Stream
    }
}
