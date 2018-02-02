﻿using System.Globalization;
using System.Reflection;
using Iuker.Js.Native;

namespace Iuker.Js.Runtime.Descriptors.Specialized
{
    /// <summary>
    /// 字段信息描述符
    /// </summary>
    public sealed class FieldInfoDescriptor : PropertyDescriptor
    {
        private readonly Engine _engine;
        private readonly FieldInfo _fieldInfo;
        private readonly object _item;

        public FieldInfoDescriptor(Engine engine, FieldInfo fieldInfo, object item)
        {
            _engine = engine;
            _fieldInfo = fieldInfo;
            _item = item;

            //Writable = !fieldInfo.Attributes.HasFlag(FieldAttributes.InitOnly); // don't write to fields marked as readonly
            Writable = !fieldInfo.IsInitOnly;
        }

        public override JsValue Value
        {
            get
            {
                return JsValue.FromObject(_engine, _fieldInfo.GetValue(_item));
            }

            set
            {
                var currentValue = value;
                object obj;
                if (_fieldInfo.FieldType == typeof(JsValue))
                {
                    obj = currentValue;
                }
                else
                {
                    // attempt to convert the JsValue to the target type
                    obj = currentValue.ToObject();
                    if (obj.GetType() != _fieldInfo.FieldType)
                    {
                        obj = _engine.ClrTypeConverter.Convert(obj, _fieldInfo.FieldType, CultureInfo.InvariantCulture);
                    }
                }

                _fieldInfo.SetValue(_item, obj);
            }
        }

    }
}