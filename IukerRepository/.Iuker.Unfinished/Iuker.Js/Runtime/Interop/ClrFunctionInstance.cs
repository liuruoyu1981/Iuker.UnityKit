using System;
using Iuker.Js.Native;
using Iuker.Js.Native.Function;

namespace Iuker.Js.Runtime.Interop
{
    /// <summary>
    /// 将一个Clr函数包装为JS的函数实例
    /// </summary>
    public sealed class ClrFunctionInstance : FunctionInstance
    {
        private readonly Func<JsValue, JsValue[], JsValue> _func;

        public ClrFunctionInstance(Engine engine, Func<JsValue, JsValue[], JsValue> func, int length = 0)
            : base(engine, null, null, false)
        {
            _func = func;
            Prototype = engine.Function.PrototypeObject;
            FastAddProperty("length", length, false, false, false);
            Extensible = true;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            try
            {
                var result = _func(thisObject, arguments);
                return result;
            }
            catch (InvalidCastException)
            {
                throw new JavaScriptException(Engine.TypeError);
            }
        }



    }
}