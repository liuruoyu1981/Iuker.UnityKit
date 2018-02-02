using Iuker.Common.Base;

namespace Iuker.Common.Module.Net.Enums
{
    /// <summary>
    /// 可继承枚举—逻辑码
    /// </summary>
    [EnumTypeName("LogicCode")]
    public class LogicCode : EnumUshort
    {
        static LogicCode()
        {

        }

        protected LogicCode(ushort index, string name, string instanceEnumExplain)
        : base(index, "LogicCode", name, instanceEnumExplain)
        {
        }

        protected LogicCode(string name, string instanceEnumExplain)
        : base("LogicCode", name, instanceEnumExplain)
        {
        }

        protected LogicCode() { }

        private static void StaticInit()
        {
            var temp = new LogicCode();
        }



        public override string TypeName { get { return "LogicCode"; } }

        // 枚举项列表，在这里写入枚举选项。
        // 例如：public static readonly MyEnum Common_Login = new MyEnum("Common_Login","普通_登录"
        // 字段必须使用静态只读修饰符！
        // 字段变量名必须和构造函数的第一个参数一致！

        #region 登录

        /// <summary>
        /// 从服务器获取运行时配置信息
        /// </summary>
        [EnumExplain("从服务器获取运行时配置信息")]
        public static readonly LogicCode Login_ConfigByServer = new LogicCode("Login_ConfigByServer", "从服务器获取运行时配置信息");

        /// <summary>
        /// 获取公告
        /// </summary>
        [EnumExplain("获取公告")]
        public static readonly LogicCode Login_Notice = new LogicCode("Login_Notice", "获取公告");

        /// <summary>
        /// 登录
        /// </summary>
        [EnumExplain("登录")]
        public static readonly LogicCode Login_Login = new LogicCode("Login_Login", "登录");

        /// <summary>
        /// 注册
        /// </summary>
        [EnumExplain("注册")]
        public static readonly LogicCode Login_Register = new LogicCode("Login_Register", "注册");

        /// <summary>
        /// 进入游戏并下发进入游戏所需的所有数据
        /// </summary>
        [EnumExplain("进入游戏并下发进入游戏所需的所有数据")]
        public static readonly LogicCode Login_EnterGame = new LogicCode("Login_EnterGame", "进入游戏并下发进入游戏所需的所有数据");

        /// <summary>
        /// 用户离线
        /// </summary>
        [EnumExplain("用户离线")]
        public static readonly LogicCode Login_OffLine = new LogicCode("Login_OffLine", "用户离线");

        #endregion

        /// <summary>
        /// 心跳连接保持确认
        /// </summary>
        [EnumExplain("心跳连接保持确认")]
        public static readonly LogicCode Heartbeat_Alive = new LogicCode("Heartbeat_Alive", "心跳连接保持确认");


        /// <summary>
        /// 强制转换为ushort
        /// </summary>
        public static explicit operator LogicCode(ushort sourceIndex)
        {
            var temp = SonTypeInstanceIndexDictionary["LogicCode"] + 1;
            ushort count = (ushort)temp;
            if (sourceIndex > count) return null;
            return SonTypeInstanceListDictionary["LogicCode"][sourceIndex] as LogicCode;
        }
    }
}
