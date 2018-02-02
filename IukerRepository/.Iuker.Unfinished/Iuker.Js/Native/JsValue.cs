using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Iuker.Js.Native.Array;
using Iuker.Js.Native.Boolean;
using Iuker.Js.Native.Date;
using Iuker.Js.Native.Function;
using Iuker.Js.Native.Number;
using Iuker.Js.Native.Object;
using Iuker.Js.Native.RegExp;
using Iuker.Js.Native.String;
using Iuker.Js.Runtime;
using Iuker.Js.Runtime.Interop;
using System.Threading;

namespace Iuker.Js.Native
{
    /// <summary>
    /// Javascript动态对象，该对象为JavaScript的根对象
    /// </summary>
    [DebuggerTypeProxy(typeof(JsValueDebugView))]
    public class JsValue : IEquatable<JsValue>
    {
        /// <summary>
        /// Js静态实例，用来代表Undefined值。
        /// </summary>
        public static readonly JsValue Undefined = new JsValue(Types.Undefined);

        /// <summary>
        /// Js静态实例，用来代表Null。
        /// </summary>
        public static readonly JsValue Null = new JsValue(Types.Null);

        /// <summary>
        /// Js静态实例，用来代表False值。
        /// </summary>
        public static readonly JsValue False = new JsValue(false);

        /// <summary>
        /// Js静态实例，用来代表True值。
        /// </summary>
        public static readonly JsValue True = new JsValue(true);

        /// <summary>
        /// 构建一个布尔值Js对象实例
        /// </summary>
        /// <param name="value"></param>
        public JsValue(bool value)
        {
            _double = value ? 1.0 : 0.0;
            _object = null;
            _type = Types.Boolean;
        }

        /// <summary>
        /// 构建一个整型Byte值Js对象实例
        /// </summary>
        /// <param name="value"></param>
        public JsValue(double value)
        {
            _object = null;
            _type = Types.Number;
            _double = value;
        }

        public JsValue(string value)
        {
            _double = double.NaN;
            _object = value;
            _type = Types.String;
        }

        public JsValue(ObjectInstance value)
        {
            _double = double.NaN;
            _type = Types.Object;
            _object = value;
        }

        /// <summary>
        /// 构建一个jsValue对象并设置为指定的类型 
        /// </summary>
        /// <param name="type"></param>
        private JsValue(Types type)
        {
            _double = double.NaN;
            _object = null;
            _type = type;
        }

        private readonly double _double;
        private readonly object _object;
        private readonly Types _type;

        /// <summary>
        /// 是否为基础类型
        /// </summary>
        /// <returns></returns>
        [Pure]
        public bool IsPrimitive()
        {
            return _type != Types.Object && _type != Types.None;
        }

        /// <summary>
        /// 是否为Undefined类型
        /// </summary>
        /// <returns></returns>
        [Pure]
        public bool IsUndefined()
        {
            return _type == Types.Undefined;
        }

        [Pure]
        public bool IsArray()
        {
            return IsObject() && AsObject() is ArrayInstance;
        }

        [Pure]
        public bool IsDate()
        {
            return IsObject() && AsObject() is DateInstance;
        }

        [Pure]
        public bool IsRegExp()
        {
            return IsObject() && AsObject() is RegExpInstance;
        }

        [Pure]
        public bool IsObject()
        {
            return _type == Types.Object;
        }

        [Pure]
        public bool IsString()
        {
            return _type == Types.String;
        }

        [Pure]
        public bool IsNumber()
        {
            return _type == Types.Number;
        }

        [Pure]
        public bool IsBoolean()
        {
            return _type == Types.Boolean;
        }

        [Pure]
        public bool IsNull()
        {
            return _type == Types.Null;
        }

        /// <summary>
        /// 转换为JavaScript对象
        /// </summary>
        /// <returns></returns>
        [Pure]
        public ObjectInstance AsObject()
        {
            if (_type != Types.Object)
            {
                throw new ArgumentException("The value is not an object");
            }

            return _object as ObjectInstance;
        }

        public ArrayInstance AsArray()
        {
            if (!IsArray())
            {
                throw new ArgumentException("The value is not an array");
            }

            return _object as ArrayInstance;
        }

        public DateInstance AsDate()
        {
            if (!IsDate())
            {
                throw new ArgumentException("The value is not a date");
            }

            return _object as DateInstance;
        }

        [Pure]
        public RegExpInstance AsRegExp()
        {
            if (!IsRegExp())
            {
                throw new ArgumentException("The value is not a date");
            }

            return _object as RegExpInstance;
        }


        /// <summary>
        /// 将一个JsValue对象转换为指定类型，该转换可能失败。
        /// </summary>
        /// <typeparam name="T">将转换的目标类型</typeparam>
        /// <param name="fail">转换失败回调函数</param>
        /// <returns></returns>
        [Pure]
        public T TryCast<T>(Action<JsValue> fail = null) where T : class
        {
            if (IsObject())
            {
                var o = AsObject();
                var t = o as T;
                if (t != null)
                {
                    return t;
                }
            }

            fail?.Invoke(this);

            return null;
        }

        public bool Is<T>()
        {
            return IsObject() && AsObject() is T;
        }

        public T As<T>() where T : ObjectInstance
        {
            return _object as T;
        }

        [Pure]
        public bool AsBoolean()
        {
            if (_type != Types.Boolean)
            {
                throw new ArgumentException("The value is not a boolean");
            }

            return _double != 0;
        }

        [Pure]
        public string AsString()
        {
            if (_type != Types.String)
            {
                throw new ArgumentException("The value is not a string");
            }

            if (_object == null)
            {
                throw new ArgumentException("The value is not defined");
            }

            return _object as string;
        }

        [Pure]
        public double AsNumber()
        {
            if (_type != Types.Number)
            {
                throw new ArgumentException("The value is not a number");
            }

            return _double;
        }

        public bool Equals(JsValue other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (_type != other._type)
            {
                return false;
            }

            switch (_type)
            {
                case Types.None:
                    return false;
                case Types.Undefined:
                    return true;
                case Types.Null:
                    return true;
                case Types.Boolean:
                case Types.Number:
                    return _double == other._double;
                case Types.String:
                case Types.Object:
                    return _object == other._object;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Creates a valid <see cref="JsValue"/> instance from any <see cref="Object"/> instance
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JsValue FromObject(Engine engine, object value)
        {
            if (value == null)
            {
                return Null;
            }

            foreach (var converter in engine.Options._ObjectConverters)
            {
                JsValue result;
                if (converter.TryConvert(value, out result))
                {
                    return result;
                }
            }

            var valueType = value.GetType();

            var typeMappers = Engine.TypeMappers;

            Func<Engine, object, JsValue> typeMapper;
            if (typeMappers.TryGetValue(valueType, out typeMapper))
            {
                return typeMapper(engine, value);
            }

            // if an ObjectInstance is passed directly, use it as is
            var instance = value as ObjectInstance;
            if (instance != null)
            {
                // Learn conversion.
                // Learn conversion, racy, worst case we'll try again later
                Interlocked.CompareExchange(ref Engine.TypeMappers, new Dictionary<Type, Func<Engine, object, JsValue>>(typeMappers)
                {
                    [valueType] = (Engine e, object v) => new JsValue((ObjectInstance)v)
                }, typeMappers);
                return new JsValue(instance);
            }

            var a = value as System.Array;
            if (a != null)
            {
                Func<Engine, object, JsValue> convert = (Engine e, object v) =>
                {
                    var array = (System.Array)v;

                    var jsArray = engine.Array.Construct(Arguments.Empty);
                    foreach (var item in array)
                    {
                        var jsItem = JsValue.FromObject(engine, item);
                        engine.Array.PrototypeObject.Push(jsArray, Arguments.From(jsItem));
                    }

                    return jsArray;
                };
                // racy, we don't care, worst case we'll catch up later
                Interlocked.CompareExchange(ref Engine.TypeMappers, new Dictionary<Type, Func<Engine, object, JsValue>>(typeMappers)
                {
                    [valueType] = convert
                }, typeMappers);
                return convert(engine, a);
            }

            var d = value as Delegate;
            if (d != null)
            {
                return new DelegateWrapper(engine, d);
            }


            if (value.GetType().IsEnum)
            {
                return new JsValue((Int32)value);
            }

            // if no known type could be guessed, wrap it as an ObjectInstance
            return new ObjectWrapper(engine, value);
        }

        /// <summary>
        /// Converts a <see cref="JsValue"/> to its underlying CLR value.
        /// </summary>
        /// <returns>The underlying CLR value of the <see cref="JsValue"/> instance.</returns>
        public object ToObject()
        {
            switch (_type)
            {
                case Types.None:
                case Types.Undefined:
                case Types.Null:
                    return null;
                case Types.String:
                    return _object;
                case Types.Boolean:
                    return System.Math.Abs(_double) > 0;
                case Types.Number:
                    return _double;
                case Types.Object:
                    if (_object is IObjectWrapper wrapper)
                    {
                        return wrapper.Target;
                    }

                    if (_object is ObjectInstance objectInstance)
                        switch (objectInstance.Class)
                        {
                            case "Array":
                                if (_object is ArrayInstance arrayInstance)
                                {
                                    var len = TypeConverter.ToInt32(arrayInstance.Get("length"));
                                    var result = new object[len];
                                    for (var k = 0; k < len; k++)
                                    {
                                        var pk = k.ToString();
                                        var kpresent = arrayInstance.HasProperty(pk);
                                        if (kpresent)
                                        {
                                            var kvalue = arrayInstance.Get(pk);
                                            result[k] = kvalue.ToObject();
                                        }
                                        else
                                        {
                                            result[k] = null;
                                        }
                                    }
                                    return result;
                                }
                                break;

                            case "String":
                                if (_object is StringInstance stringInstance)
                                {
                                    return stringInstance.PrimitiveValue.AsString();
                                }

                                break;

                            case "Date":
                                if (_object is DateInstance dateInstance)
                                {
                                    return dateInstance.ToDateTime();
                                }

                                break;

                            case "Boolean":
                                if (_object is BooleanInstance booleanInstance)
                                {
                                    return booleanInstance.PrimitiveValue.AsBoolean();
                                }

                                break;

                            case "Function":
                                if (_object is FunctionInstance function)
                                {
                                    return (Func<JsValue, JsValue[], JsValue>)function.Call;
                                }

                                break;

                            case "Number":
                                if (_object is NumberInstance numberInstance)
                                {
                                    return numberInstance.PrimitiveValue.AsNumber();
                                }

                                break;

                            case "RegExp":
                                if (_object is RegExpInstance regeExpInstance)
                                {
                                    return regeExpInstance.Value;
                                }

                                break;

                            case "Arguments":
                            case "Object":
#if __IOS__
                                IDictionary<string, object> o = new Dictionary<string, object>();
#else
                                // todo fixed
                                IDictionary<string, object> o = new Dictionary<string, object>();
#endif

                                foreach (var p in objectInstance.GetOwnProperties())
                                {
                                    if (!p.Value.Enumerable.HasValue || p.Value.Enumerable.Value == false)
                                    {
                                        continue;
                                    }

                                    o.Add(p.Key, objectInstance.Get(p.Key).ToObject());
                                }

                                return o;
                        }


                    return _object;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        /// <summary>
        /// 以函数的形式调用当前Js实例。
        /// </summary>
        /// <param name="arguments">作为函数调用的参数数组（Js实例数组）。</param>
        /// <returns>函数调用的返回值。</returns>
        public JsValue Invoke(params JsValue[] arguments)
        {
            return Invoke(Undefined, arguments);
        }

        /// <summary>
        /// 以函数的形式调用当前Js实例。
        /// </summary>
        /// <param name="thisObj">作为函数调用的Js实例。</param>
        /// <param name="arguments">作为函数调用的参数数组（Js实例数组）。</param>
        /// <returns>函数调用的返回值。</returns>
        private JsValue Invoke(JsValue thisObj, JsValue[] arguments)
        {
            var callable = TryCast<ICallable>();

            if (callable == null)
            {
                throw new ArgumentException("Can only invoke functions");
            }

            return callable.Call(thisObj, arguments);
        }

        public override string ToString()
        {
            switch (Type)
            {
                case Types.None:
                    return "None";
                case Types.Undefined:
                    return "undefined";
                case Types.Null:
                    return "null";
                case Types.Boolean:
                    return System.Math.Abs(_double) > 0 ? bool.TrueString : bool.FalseString;
                case Types.Number:
                    return _double.ToString(CultureInfo.InvariantCulture);
                case Types.String:
                case Types.Object:
                    return _object.ToString();
                default:
                    return string.Empty;
            }
        }

        #region 重载操作符

        public static bool operator ==(JsValue a, JsValue b)
        {
            if ((object)a == null)
            {
                if ((object)b == null)
                {
                    return true;
                }

                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(JsValue a, JsValue b)
        {
            if ((object)a == null)
            {
                if ((object)b == null)
                {
                    return false;
                }

                return true;
            }

            return !a.Equals(b);
        }

        public static implicit operator JsValue(double value)
        {
            return new JsValue(value);
        }

        public static implicit operator JsValue(bool value)
        {
            return new JsValue(value);
        }

        public static implicit operator JsValue(string value)
        {
            return new JsValue(value);
        }

        public static implicit operator JsValue(ObjectInstance value)
        {
            return new JsValue(value);
        }

        #endregion

        public Types Type => _type;

        private class JsValueDebugView
        {
            private string Value;
            public JsValueDebugView(JsValue value)
            {

                switch (value.Type)
                {
                    case Types.None:
                        Value = "None";
                        break;
                    case Types.Undefined:
                        Value = "undefined";
                        break;
                    case Types.Null:
                        Value = "null";
                        break;
                    case Types.Boolean:
                        Value = value.AsBoolean() + " (bool)";
                        break;
                    case Types.String:
                        Value = value.AsString() + " (string)";
                        break;
                    case Types.Number:
                        Value = value.AsNumber() + " (number)";
                        break;
                    case Types.Object:
                        Value = value.AsObject().GetType().Name;
                        break;
                    default:
                        Value = "Unknown";
                        break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            var a = obj as JsValue;
            return a != null && Equals(a);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                hashCode = (hashCode * 397) ^ _double.GetHashCode();
                hashCode = (hashCode * 397) ^ (_object != null ? _object.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)_type;
                return hashCode;
            }
        }


    }
}
