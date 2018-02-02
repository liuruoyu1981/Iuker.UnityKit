using Iuker.Common.Base;

namespace Iuker.Common.Module.Net.Enums
{
    /// <summary>
    /// 可继承枚举—通信状态码
    /// </summary>
    [IsEnum("StateCode")]
    [EnumTypeName("StateCode")]
    public class StateCode : EnumUshort
    {
        static StateCode()
        {

        }


        protected StateCode(ushort index, string name, string instanceEnumExplain)
            : base(index, "StateCode", name, instanceEnumExplain)
        {

        }

        protected StateCode(string name, string instanceEnumExplain)
        : base("StateCode", name, instanceEnumExplain)
        {

        }

        protected StateCode() { }

        private static void StaticInit()
        {
            Debuger.Log("This is RyerrorCode StaticInit!");
            var temp = new StateCode();
        }



        public override string TypeName { get { return "StateCode"; } }

        [EnumExplain("通信成功")]
        public static readonly StateCode Scuuess = new StateCode("Scuuess", "通信成功");

        [EnumExplain("账号在其他地方登录")]
        public static readonly StateCode OtherLogin = new StateCode("OtherLogin", "账号在其他地方登录");

        [EnumExplain("没有这样的逻辑码")]
        public static readonly StateCode LigicCodeDontExist = new StateCode("LigicCodeDontExist", "没有这样的逻辑码");

        [EnumExplain("没有这样的处理模块")]
        public static readonly StateCode ModuleDontExist = new StateCode("ModuleDontExist", "没有这样的处理模块");

        [EnumExplain("邮箱地址错误")]
        public static readonly StateCode EmailState = new StateCode("EmailState", "邮箱地址错误");

        [EnumExplain("重设密码校验码错误")]
        public static readonly StateCode ResetVerifyState = new StateCode("ResetVerifyState", "重设密码校验码错误");

        [EnumExplain("重设密码校验码过期")]
        public static readonly StateCode ResetVerifyOut = new StateCode("ResetVerifyOut", "重设密码校验码过期");

        [EnumExplain("重设密码太频繁")]
        public static readonly StateCode ResetTooMuch = new StateCode("ResetTooMuch", "重设密码太频繁");

        [EnumExplain("账号不存在")]
        public static readonly StateCode AccountDontExist = new StateCode("AccountDontExist", "账号不存在");

        [EnumExplain("密码不匹配")]
        public static readonly StateCode PasswordDontMatch = new StateCode("PasswordDontMatch", "密码不匹配");

        public static explicit operator StateCode(ushort sourceIndex)
        {
            var temp = SonTypeInstanceIndexDictionary["StateCode"] + 1;
            ushort count = (ushort)temp;
            if (sourceIndex > count) return null;
            return SonTypeInstanceListDictionary["StateCode"][sourceIndex] as StateCode;
        }





    }
}
