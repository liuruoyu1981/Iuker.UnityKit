using System;
using Iuker.Js.Native.Function;
using Iuker.Js.Native.Object;

namespace Iuker.Js.Native.Error
{
    /// <summary>
    /// javascript错误构造器
    /// </summary>
    public class ErrorConstructor : FunctionInstance, IConstructor
    {
        private string _name;

        public ErrorConstructor(Engine engine) : base(engine, null, null, false)
        {

        }

        public static ErrorConstructor CreatErrorConstructor(Engine engine, string name)
        {
            var obj = new ErrorConstructor(engine);
            obj.Extensible = true;
            obj._name = name;

            return null;
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            throw new NotImplementedException();
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
            var instance = new ErrorInstance(Engine, _name);
            //instance.Prototype = PrototypeObject;
            //instance.Extensible = true;

            //if (arguments.At(0) != Undefined.Instance)
            //{
            //    instance.Put("message", TypeConverter.ToString(arguments.At(0)), false);
            //}

            return instance;
        }








    }
}
