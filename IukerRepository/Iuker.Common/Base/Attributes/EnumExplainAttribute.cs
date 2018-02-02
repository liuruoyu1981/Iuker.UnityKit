namespace Iuker.Common.Base
{
    /// <summary>
    /// 可继承枚举说明特性
    /// </summary>
    public class EnumExplainAttribute : System.Attribute
    {
        /// <summary>
        /// 字段说明
        /// </summary>
        public string FieldExplain { get; private set; }

        public EnumExplainAttribute(string fieldExplain)
        {
            FieldExplain = fieldExplain;
        }

    }
}
