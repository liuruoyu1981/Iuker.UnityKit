using System;

namespace Iuker.Common.Base
{
    /// <summary>
    /// 枚举标识特性，被标识了该特性的类约定为可继承枚举类型
    /// </summary>
    public class IsEnumAttribute : Attribute
    {
        /// <summary>
        /// 可继承枚举类的类名
        /// </summary>
        protected string enumClassName;

        public IsEnumAttribute(string enumClassName)
        {
            this.enumClassName = enumClassName;
        }

        public string EnumClassName
        {
            get { return enumClassName; }
            set { enumClassName = value; }
        }
    }
}
