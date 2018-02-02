using System;
using Iuker.Js.Native.Object;
using Iuker.Js.Runtime;

namespace Iuker.Js.Native.Date
{
    /// <summary>
    /// JavaScript日期对象
    /// </summary>
    public class DateInstance : ObjectInstance
    {
        // Maximum allowed value to prevent DateTime overflow
        internal static readonly double Max = (DateTime.MaxValue - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        // Minimum allowed value to prevent DateTime overflow
        internal static readonly double Min = -(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) - DateTime.MinValue).TotalMilliseconds;

        public DateInstance(Engine engine)
            : base(engine)
        {
        }

        public override string Class => "Date";

        public DateTime ToDateTime()
        {
            if (double.IsNaN(PrimitiveValue) || PrimitiveValue > Max || PrimitiveValue < Min)
            {
                throw new JavaScriptException(Engine.RangeError);
            }

            return DateConstructor.Epoch.AddMilliseconds(PrimitiveValue);
        }

        /// <summary>
        /// 原始值
        /// </summary>
        public double PrimitiveValue { get; set; }
    }
}
