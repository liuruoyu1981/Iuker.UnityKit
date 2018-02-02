using System;

namespace Iuker.Common.Base.Attributes
{
#if DEBUG
    /// <summary>
    ///数据Id
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 16:35:11")]
    [ClassPurposeDesc("数据Id", "数据Id")]
#endif
    public class DataIdDefineAttribute : Attribute
    {
        /// <summary>
        /// 数据id字段定义的目标数据类型
        /// </summary>
        public Type DataType { get; private set; }

        /// <summary>
        /// 数据的字符串Id
        /// </summary>
        public string DataId { get; private set; }

        public DataIdDefineAttribute(Type targetDataType, string dataId)
        {
            DataType = targetDataType;
            DataId = dataId;
        }
    }
}
