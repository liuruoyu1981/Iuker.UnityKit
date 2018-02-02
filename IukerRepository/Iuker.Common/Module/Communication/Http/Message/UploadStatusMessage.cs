using Iuker.Common.Base;

namespace Iuker.Common.Module.Communication.Http.Message
{
#if DEBUG
    /// <summary>
    /// Http上传消息(表示上传状态)
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 13:28:54")]
    [ClassPurposeDesc("Http上传消息(表示上传状态)", "Http上传消息")]
#endif
    public class UploadStatusMessage
    {
        /// <summary>
        /// 上传的内容长度
        /// </summary>
        public long ContentLength
        {
            get; set;
        }

        /// <summary>
        /// 到目前为止上传了多少内容
        /// </summary>
        public long TotalContentUploaded
        {
            get; set;
        }

        /// <summary>
        /// 自上次上传状态消息后，上传了多少内容
        /// </summary>
        public long ContentUploadedThisRound
        {
            get; set;
        }

        /// <summary>
        /// 上传完成百分比
        /// </summary>
        public int PercentageComplete
        {
            get { return (int)(((double)TotalContentUploaded / ContentLength) * 100); }
        }
    }
}
