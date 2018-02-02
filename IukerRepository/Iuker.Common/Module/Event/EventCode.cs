using Iuker.Common.Base;

namespace Iuker.Common.Event
{
    /// <summary>
    /// 可继承枚举—事件码
    /// </summary>
    [EnumTypeName("EventCode")]
    public class EventCode : EnumUshort
    {
        static EventCode()
        {

        }

        protected EventCode(ushort index, string name, string instanceEnumExplain)
        : base(index, "EventCode", name, instanceEnumExplain)
        {
        }

        protected EventCode(string name, string instanceEnumExplain)
        : base("EventCode", name, instanceEnumExplain)
        {
        }

        protected EventCode() { }

        private static void StaticInit()
        {
            var temp = new EventCode();
        }

        public override string TypeName { get { return "EventCode"; } }

        // 枚举项列表，在这里写入枚举选项。
        // 例如：public static readonly MyEnum Common_Login = new MyEnum("Common_Login","普通_登录"
        // 字段必须使用静态只读修饰符！
        // 字段变量名必须和构造函数的第一个参数一致！

        /// <summary>
        /// 强制转换为ushort
        /// </summary>
        public static explicit operator EventCode(ushort sourceIndex)
        {
            var temp = SonTypeInstanceIndexDictionary["EventCode"] + 1;
            var count = (ushort)temp;
            if (sourceIndex > count) return null;
            return SonTypeInstanceListDictionary["EventCode"][sourceIndex] as EventCode;
        }
    }
}
