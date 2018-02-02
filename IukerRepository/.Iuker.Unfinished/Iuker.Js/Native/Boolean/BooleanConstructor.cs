using Iuker.Js.Native.Function;

namespace Iuker.Js.Native.Boolean
{
    public class BooleanConstructor : FunctionInstance, IConstructor
    {
        private BooleanConstructor(Engine engine) : base(engine, null, null, false)
        {
        }

        public static BooleanConstructor CreateBooleanConstructor(Engine engine)
        {
            var obj = new BooleanConstructor(engine);
            obj.Extensible = true;

            // The value of the [[Prototype]] internal property of the Boolean constructor is the Function prototype object 
            obj.Prototype = engine.Function.PrototypeObject;
            obj.PrototypeObject = BooleanPrototype.CreatePrototypeObject(engine, obj);

            obj.FastAddProperty("length", 1, false, false, false);

            // The initial value of Boolean.prototype is the Boolean prototype object
            obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);

            return obj;
        }

        public BooleanPrototype PrototypeObject { get; private set; }
    }
}
