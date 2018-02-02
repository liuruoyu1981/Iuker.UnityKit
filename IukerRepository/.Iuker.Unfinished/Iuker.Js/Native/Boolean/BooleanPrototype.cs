namespace Iuker.Js.Native.Boolean
{
    public sealed class BooleanPrototype : BooleanInstance
    {
        private BooleanPrototype(Engine engine)
            : base(engine)
        {

        }

        public static BooleanPrototype CreatePrototypeObject(Engine engine, BooleanConstructor booleanConstructor)
        {
            var obj = new BooleanPrototype(engine)
            {
                Prototype = engine.Object.PrototypeObject,
                PrimitiveValue = false,
                Extensible = true
            };

            obj.FastAddProperty("constructor", booleanConstructor, true, false, true);

            return obj;
        }



    }
}
