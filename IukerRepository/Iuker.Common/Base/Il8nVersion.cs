using System.Collections.Generic;

namespace Iuker.Common.Base
{
    /// <summary>
    /// 可继承枚举-多语言版本
    /// </summary>
    [EnumTypeName("Il8nVersion")]
    public class Il8nVersion : EnumByte
    {
        static Il8nVersion()
        {

        }

        protected Il8nVersion(byte index, string name, string instanceEnumExplain)
            : base(index, "Il8nVersion", name, instanceEnumExplain)
        {
        }

        protected Il8nVersion(string name, string instanceEnumExplain)
            : base("Il8nVersion", name, instanceEnumExplain)
        {
        }

        protected Il8nVersion() { }

        private static void StaticInit()
        {
            var temp = new Il8nVersion();
        }



        // 枚举项列表，在这里写入枚举选项。
        // 例如：public static readonly MyEnum Common_Login = new MyEnum("Common_Login","普通_登录"
        // 字段必须使用静态只读修饰符！
        // 字段变量名必须和构造函数的第一个参数一致！


        /// <summary></summary>
        [EnumExplain("中文版本")]
        public static readonly Il8nVersion Chinese = new Il8nVersion("Chinese", "中文版本");

        [EnumExplain("英语版本")]
        public static readonly Il8nVersion English = new Il8nVersion("English", "英语版本");

        [EnumExplain("日语版本")]
        public static readonly Il8nVersion Japanese = new Il8nVersion("Japanese", "日语版本");

        [EnumExplain("法语版本")]
        public static readonly Il8nVersion France = new Il8nVersion("France", "法语版本");

        [EnumExplain("德语版本")]
        public static readonly Il8nVersion Germany = new Il8nVersion("Germany", "德语版本");

        [EnumExplain("韩语版本")]
        public static readonly Il8nVersion Korea = new Il8nVersion("Korea", "韩语版本");

        [EnumExplain("西班牙语版本")]
        public static readonly Il8nVersion Spain = new Il8nVersion("Spain", "西班牙语版本");

        [EnumExplain("意大利语版本")]
        public static readonly Il8nVersion Italy = new Il8nVersion("Italy", "意大利语版本");


        /// <summary>
        /// 强制转换
        /// </summary>
        public static explicit operator Il8nVersion(byte sourceIndex)
        {
            var count = SonTypeInstanceIndexDictionary["Il8nVersion"] + 1;
            if (sourceIndex > count) return null;
            return SonTypeInstanceListDictionary["Il8nVersion"][sourceIndex] as Il8nVersion;
        }

        private static List<Il8nVersion> mAllField;

        /// <summary>
        /// 可继承枚举的实例枚举列表
        /// </summary>
        private static List<Il8nVersion> AllField
        {
            get
            {
                return mAllField ?? (mAllField = new List<Il8nVersion>());
            }
        }

        /// <summary>
        /// 获得Il8nVersion的所有枚举实例列表
        /// </summary>
        /// <returns></returns>
        public static List<Il8nVersion> GetAllField()
        {
            AllField.Add(Chinese);
            AllField.Add(English);
            AllField.Add(Japanese);
            AllField.Add(France);
            AllField.Add(Germany);
            AllField.Add(Korea);
            AllField.Add(Spain);
            AllField.Add(Italy);

            return AllField;
        }

        public override string TypeName { get { return "Il8nVersion"; } }

        public override string ToString() { return Literals; }
    }
}