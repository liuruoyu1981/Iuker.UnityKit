using Iuker.Common.Base;

namespace Iuker.Common.Module.Net.Enums
{
    /// <summary>
    /// 项目网络通信模块码基础枚举类型
    /// </summary>
    [IsEnum("RyModuleCode")]
    [EnumTypeName("RyModuleCode")]
    public class ModuleCode : EnumByte
    {
        static ModuleCode()
        {

        }

        protected ModuleCode(byte index, string name, string instanceEnumExplain)
        : base(index, "ModuleCode", name, instanceEnumExplain)
        {
        }

        protected ModuleCode(string name, string instanceEnumExplain)
        : base("ModuleCode", name, instanceEnumExplain)
        {
        }

        protected ModuleCode() { }

        private static void StaticInit()
        {
            var temp = new ModuleCode();
        }

        // 枚举项列表，在这里写入枚举选项。
        // 例如：public static readonly MyEnum Common_Login = new MyEnum("Common_Login","普通_登录"
        // 字段必须使用静态只读修饰符！
        // 字段变量名必须和构造函数的第一个参数一致！


        /// <summary>
        /// 登录模块
        /// </summary>
        [EnumExplain("登录模块")]
        public static readonly ModuleCode Login = new ModuleCode("Login", "登录模块");

        /// <summary>
        /// 心跳模块
        /// </summary>
        [EnumExplain("心跳模块")]
        public static readonly ModuleCode Heartbeat = new ModuleCode("Heartbeat", "心跳模块");

        /// <summary>
        /// 循环更新模块
        /// </summary>
        [EnumExplain("循环更新模块")]
        public static readonly ModuleCode Loop = new ModuleCode("Loop", "循环更新模块");

        /// <summary>
        /// 用户模块
        /// </summary>
        [EnumExplain("用户模块")]
        public static readonly ModuleCode User = new ModuleCode("User", "用户模块");

        /// <summary>
        /// 每日任务模块
        /// </summary>
        [EnumExplain("每日任务模块")]
        public static readonly ModuleCode Taskday = new ModuleCode("Taskday", "每日任务模块");

        /// <summary>
        /// 邮件模块
        /// </summary>
        [EnumExplain("邮件模块")]
        public static readonly ModuleCode Mail = new ModuleCode("Mail", "邮件模块");

        /// <summary>
        /// 好友模块
        /// </summary>
        [EnumExplain("好友模块")]
        public static readonly ModuleCode Friend = new ModuleCode("Friend", "好友模块");

        /// <summary>
        /// 排行榜模块
        /// </summary>
        [EnumExplain("排行榜模块")]
        public static readonly ModuleCode Billboard = new ModuleCode("Billboard", "排行榜模块");

        /// <summary>
        /// 签到模块
        /// </summary>
        [EnumExplain("签到模块")]
        public static readonly ModuleCode CheckIn = new ModuleCode("CheckIn", "签到模块");




        /// <summary>
        /// 强制转换为byte
        /// </summary>
        public static explicit operator ModuleCode(byte sourceIndex)
        {
            // 这里temp+1代表类型实例字典的长度溢出值
            var temp = SonTypeInstanceIndexDictionary["ModuleCode"] + 1;
            byte count = (byte)temp;
            if (sourceIndex > count) return null;
            return SonTypeInstanceListDictionary["ModuleCode"][sourceIndex] as ModuleCode;
        }

        public override string TypeName { get { return "ModuleCode"; } }
    }
}
