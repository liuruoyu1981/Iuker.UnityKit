using System;
using Iuker.Js.Native.Object;
using Iuker.Js.Runtime;

namespace Iuker.Js.Native.Number
{
    /// <summary>
    /// JavaScript数字基类
    /// </summary>
    public class NumberInstance : ObjectInstance, IPrimitiveInstance
    {
        private static readonly long NegativeZeroBits = BitConverter.DoubleToInt64Bits(-0.0);

        public NumberInstance(Engine engine)
            : base(engine)
        {

        }

        public override string Class => "Number";

        Types IPrimitiveInstance.Type => Types.Number;

        JsValue IPrimitiveInstance.PrimitiveValue => PrimitiveValue;

        public JsValue PrimitiveValue { get; set; }

        /// <summary>
        /// 数字是否无限接近0的负数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsNegativeZero(double x)
        {
            return System.Math.Abs(x) < 0 && BitConverter.DoubleToInt64Bits(x) == NegativeZeroBits;
        }

        /// <summary>
        /// 数字是否无限接近0的正数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsPositiveZero(double x)
        {
            return System.Math.Abs(x) < 0 && BitConverter.DoubleToInt64Bits(x) != NegativeZeroBits;
        }


    }
}
