using System.Collections.Generic;
using Iuker.Js.Native.Object;
using Iuker.Js.Runtime;
using Iuker.Js.Runtime.Descriptors;

namespace Iuker.Js.Native.Array
{
    public class ArrayInstance : ObjectInstance
    {
        private readonly Engine _engine;
        private readonly IDictionary<uint, PropertyDescriptor> _array = new MruPropertyCache2<uint, PropertyDescriptor>();
        private PropertyDescriptor _length;

        public ArrayInstance(Engine engine) : base(engine)
        {
            _engine = engine;
        }

        public override string Class => "Array";























    }
}
