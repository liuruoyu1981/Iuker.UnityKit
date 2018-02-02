using Iuker.Js.Native;
using Iuker.Js.Native.Object;
using Iuker.Js.Runtime.Descriptors;

namespace Iuker.Js.Runtime.Interop
{
    public class NamespaceReference : ObjectInstance, ICallable
    {
        private readonly string _path;

        public NamespaceReference(Engine engine, string path)
            : base(engine)
        {
            _path = path;
        }

        public override bool DefineOwnProperty(string propertyName, PropertyDescriptor desc, bool throwOnError)
        {
            if (throwOnError)
            {
                throw new JavaScriptException(Engine.TypeError, "Can't define a property of a NamespaceReference");
            }

            return false;
        }

        public override bool Delete(string propertyName, bool throwOnError)
        {
            if (throwOnError)
            {
                throw new JavaScriptException(Engine.TypeError, "Can't delete a property of a NamespaceReference");
            }

            return false;
        }

























    }
}