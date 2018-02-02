using System.IO;
using Iuker.Common.Base;
using Iuker.Common.Module.Communication.Http.Enums;
using Run.Iuker.Common.Module.Communication.Http.HttpContent;

namespace Iuker.Common.Module.Communication.Http.HttpContent
{
#if DEBUG
    /// <summary>
    /// Http二进制类型内容
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 13:17:46")]
    [ClassPurposeDesc("Http二进制类型内容", "Http二进制类型内容")]
#endif
    public class ByteArrayContent : IHttpContent
    {
        private readonly byte[] mContent;
        private readonly string mMdediaType;

        public ByteArrayContent(byte[] content, string mediaType)
        {
            mContent = content;
            mMdediaType = mediaType;
        }

        public ContentReadAction ContentReadAction
        {
            get
            {
                return ContentReadAction.ByteArray;
            }
        }

        public long GetContentLength()
        {
            return mContent.Length;
        }

        public string GetContentType()
        {
           return mMdediaType;
        }

        public byte[] ReadAsByteArray()
        {
            return mContent;
        }

        public Stream ReadAsStream()
        {
            return new MemoryStream(mContent);
        }


    }
}
