using Iuker.Js.Runtime.Interop;

namespace Iuker.Js.Native.Object
{
    public sealed class ObjectPrototype : ObjectInstance
    {
        private ObjectPrototype(Engine engine) : base(engine)
        {
        }

        public static ObjectPrototype CreatePrototypeObject(Engine engine, ObjectConstructor objectConstructor)
        {
            var obj = new ObjectPrototype(engine) { Extensible = true };

            obj.FastAddProperty("constructor", objectConstructor, true, false, true);

            return obj;
        }

        public void Configure()
        {
            FastAddProperty("toString", new ClrFunctionInstance(Engine, ToObjectString), true, false, true);
            FastAddProperty("toLocaleString", new ClrFunctionInstance(Engine, ToLocaleString), true, false, true);
            FastAddProperty("valueOf", new ClrFunctionInstance(Engine, ValueOf), true, false, true);
            FastAddProperty("hasOwnProperty", new ClrFunctionInstance(Engine, HasOwnProperty, 1), true, false, true);
            FastAddProperty("isPrototypeOf", new ClrFunctionInstance(Engine, IsPrototypeOf, 1), true, false, true);
            FastAddProperty("propertyIsEnumerable", new ClrFunctionInstance(Engine, PropertyIsEnumerable, 1), true, false, true);
        }





















    }
}
