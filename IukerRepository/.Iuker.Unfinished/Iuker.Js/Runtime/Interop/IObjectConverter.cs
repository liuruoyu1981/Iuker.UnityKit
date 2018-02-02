using Iuker.Js.Native;

namespace Iuker.Js.Runtime.Interop
{
    /// <summary>
    /// 对象转换器
    /// </summary>
    public interface IObjectConverter
    {
        /// <summary>
        /// 尝试将一个对象转换为js实例
        /// 成功返回真，失败返回假。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryConvert(object value, out JsValue result);
    }
}