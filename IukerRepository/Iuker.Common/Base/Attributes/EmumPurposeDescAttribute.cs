﻿using System;

namespace Iuker.Common.Base
{
    /// <summary>
    /// 用于生成枚举用途描述元数据的自描述特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class EmumPurposeDescAttribute : SelfDescAttribute
    {
        public EmumPurposeDescAttribute(string english, string chinese = null, string japenese = null, string france = null, string germany = null, string korea = null, string spain = null, string italy = null) : base(english, chinese, japenese, france, germany, korea, spain, italy)
        {
        }
    }
}