using System;

namespace Iuker.Common.Base
{
    public interface IukEnum
    {
        /// <summary>
        /// 枚举值用途说明文字
        /// </summary>
        string EnumValueExplain { get; }

        /// <summary>
        /// 基础类型
        /// </summary>
        Type BaseValueType { get; }

        /// <summary>
        /// 类型名
        /// </summary>
        string TypeName { get; }
    }
}
