using System.Globalization;
using System.Reflection;
using Iuker.Js.Native;

namespace Iuker.Js.Runtime.Descriptors.Specialized
{
    /// <summary>
    /// 属性信息描述符
    /// </summary>
    public class PropertyInfoDescriptor : PropertyDescriptor
    {
        private readonly Engine _engine;
        private readonly PropertyInfo _propertyInfo;
        private readonly object _item;

        public PropertyInfoDescriptor(Engine engine, PropertyInfo propertyInfo, object item)
        {
            _engine = engine;
            _propertyInfo = propertyInfo;
            _item = item;

            Writable = propertyInfo.CanWrite;
        }

        public override JsValue Value
        {
            get
            {
                return JsValue.FromObject(_engine, _propertyInfo.GetValue(_item, null));
            }

            set
            {
                var currentValue = value;
                object obj;
                if (_propertyInfo.PropertyType == typeof(JsValue))
                {
                    obj = currentValue;
                }
                else
                {
                    // attempt to convert the JsValue to the target type
                    obj = currentValue.ToObject();
                    if (obj != null && obj.GetType() != _propertyInfo.PropertyType)
                    {
                        obj = _engine.ClrTypeConverter.Convert(obj, _propertyInfo.PropertyType, CultureInfo.InvariantCulture);
                    }
                }

                _propertyInfo.SetValue(_item, obj, null);
            }
        }



    }
}