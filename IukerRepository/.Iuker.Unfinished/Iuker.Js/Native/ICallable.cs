namespace Iuker.Js.Native
{
    /// <summary>
    /// 可调用Javascript对象
    /// </summary>
    public interface ICallable
    {
        JsValue Call(JsValue thisObject, JsValue[] arguments);
    }
}
