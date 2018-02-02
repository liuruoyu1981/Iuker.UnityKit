using Iuker.Js.Native;

namespace Iuker.Js.Runtime
{
    /// <summary>
    /// 参数，用于表示Js虚拟机运行时的函数参数数组
    /// </summary>
    public static class Arguments
    {
        public static JsValue[] Empty = new JsValue[0];

        public static JsValue[] From(params JsValue[] o) => o;


        public static JsValue At(this JsValue[] args, int index, JsValue undefinedValue)
        {
            return args.Length > index ? args[index] : undefinedValue;
        }

        /// <summary>
        /// 获得指定索引位置的函数参数
        /// 如果指定索引超出实际参数数组长度则会返回Undefind。
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static JsValue At(this JsValue[] args, int index)
        {
            return At(args, index, Undefined.Instance);
        }
    }
}