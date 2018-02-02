using System;
using Iuker.Js.Native.Function;

namespace Iuker.Js.Native.Date
{
    public sealed class DateConstructor : FunctionInstance, IConstructor
    {
        internal static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public DateConstructor(Engine engine) : base(engine, null, null, false) { }

        public static DateConstructor CreateDateConstructor(Engine engine)
        {
            var obj = new DateConstructor(engine);
            obj.Extensible = true;

            // The value of the [[Prototype]] internal property of the Date constructor is the Function prototype object 
            //obj.Prototype = engine.Function.PrototypeObject;
            //obj.PrototypeObject = DatePrototype.CreatePrototypeObject(engine, obj);

            //obj.FastAddProperty("length", 7, false, false, false);

            //// The initial value of Date.prototype is the Date prototype object
            //obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);

            return obj;
        }


















    }
}
