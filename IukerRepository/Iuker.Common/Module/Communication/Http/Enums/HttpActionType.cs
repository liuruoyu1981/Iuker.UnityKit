using Iuker.Common.Base;

namespace Iuker.Common.Module.Communication.Http.Enums
{
#if DEBUG
    /// <summary>
    /// Http行为类型
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 12:59:01")]
    [EmumPurposeDesc("Http行为类型", "Http行为类型")]
#endif
    public enum HttpActionType
    {
        Delete,
        Get,
        Patch,
        Post,
        Put
    }
}
