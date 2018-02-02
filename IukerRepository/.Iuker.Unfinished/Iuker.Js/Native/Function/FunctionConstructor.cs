namespace Iuker.Js.Native.Function
{
    public sealed class FunctionConstructor : FunctionInstance, IConstructor
    {
        private FunctionConstructor(Engine engine) : base(engine, null, null, false)
        { }


        public static FunctionConstructor CreatgeFunctionConstructor(Engine engine)
        {
            var obj = new FunctionConstructor(engine);
            obj.Extensible = true;

            // The initial value of Function.prototype is the standard built-in Function prototype object
            obj.PrototypeObject = FunctionPrototype.CreatePrototypeObject(engine);

            // The value of the [[Prototype]] internal property of the Function constructor is the standard built-in Function prototype object 
            obj.Prototype = obj.PrototypeObject;

            obj.FastAddProperty("prototype", obj.PrototypeObject, false, false, false);

            obj.FastAddProperty("length", 1, false, false, false);

            return obj;
        }


















        public FunctionPrototype PrototypeObject { get; private set; }

    }
}
