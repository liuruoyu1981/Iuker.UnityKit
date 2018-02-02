using Iuker.Js.Native.Object;
using Iuker.Js.Runtime;

namespace Iuker.Js.Native.Boolean
{
    public class BooleanInstance : ObjectInstance, IPrimitiveInstance
    {
        public BooleanInstance(Engine engine)
            : base(engine)
        {

        }

        public override string Class => "Boolean";

        Types IPrimitiveInstance.Type => Types.Boolean;

        JsValue IPrimitiveInstance.PrimitiveValue => PrimitiveValue;

        public JsValue PrimitiveValue { get; set; }


    }
}
