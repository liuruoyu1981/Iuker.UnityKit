namespace Iuker.Js.Native
{
    /// <summary>
    /// 原始类型实例接口
    /// </summary>
    public interface IPrimitiveInstance
    {
        Iuker.Js.Runtime.Types Type { get; }

        /// <summary>
        /// 原始类型实例
        /// </summary>
        JsValue PrimitiveValue { get; }

    }
}
