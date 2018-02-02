using System.IO;
using Iuker.Common.Base;
using Iuker.Common.Module.Communication.Http.Enums;

namespace Run.Iuker.Common.Module.Communication.Http.HttpContent
{
#if DEBUG
    /// <summary>
    /// Http内容
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 13:04:00")]
    [InterfacePurposeDesc("Http内容", "Http内容")]
#endif
    public interface IHttpContent
    {
        /// <summary>
        /// http内容操作类型
        /// </summary>
        ContentReadAction ContentReadAction { get; }

        /// <summary>
        /// 获得http内容长度
        /// </summary>
        /// <returns></returns>
        long GetContentLength();

        /// <summary>
        /// 获得http内容类型
        /// </summary>
        /// <returns></returns>
        string GetContentType();

        /// <summary>
        /// 以字节数组形式读取
        /// 返回字节数组
        /// </summary>
        /// <returns></returns>
        byte[] ReadAsByteArray();

        /// <summary>
        /// 以内存流读取
        /// 返回流对象
        /// </summary>
        /// <returns></returns>
        Stream ReadAsStream();
    }
}
