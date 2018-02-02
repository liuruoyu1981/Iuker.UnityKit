/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 11:16:13
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Iuker.MoonSharp.Interpreter.DataTypes;
using Iuker.MoonSharp.Interpreter.Errors;
using Iuker.MoonSharp.Interpreter.Execution;
using Iuker.MoonSharp.Interpreter.Interop.Converters;

namespace Iuker.MoonSharp.Interpreter
{
    /// <summary>
    /// 动态值对象
    /// 代表lua脚本中的值的类型
    /// </summary>
    public sealed class DynValue
    {
        static int s_RefIDCounter = 0;

        private int m_RefID = ++s_RefIDCounter;
        private int m_HashCode = -1;

        private bool m_ReadOnly;
        private double m_Number;
        private object m_Object;
        private DataType m_Type;

        /// <summary>
        /// 获得Lua动态对象唯一的引用标识符。只有在同一个线程中创建的Lua动态对象是惟一的，因为它不是线程安全的。
        /// </summary>
        public int ReferenceID { get { return m_RefID; } }

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        public DataType Type { get { return m_Type; } }

        /// <summary>
        /// Gets the function (valid only if the <see cref="Type"/> is <see cref="DataType.Function"/>)
        /// </summary>
        public Closure Function { get { return m_Object as Closure; } }

        public double Number { get { return m_Number; } }

        /// <summary>
        /// Gets the values in the tuple (valid only if the <see cref="Type"/> is Tuple).
        /// This field is currently also used to hold arguments in values whose <see cref="Type"/> is <see cref="DataType.TailCallRequest"/>.
        /// </summary>
        public DynValue[] Tuple { get { return m_Object as DynValue[]; } }

        /// <summary>
        /// Gets the coroutine handle. (valid only if the <see cref="Type"/> is Thread).
        /// </summary>
        public Coroutine Coroutine { get { return m_Object as Coroutine; } }

        /// <summary>
        /// Gets the table (valid only if the <see cref="Type"/> is <see cref="DataType.Table"/>)
        /// </summary>
        public Table Table { get { return m_Object as Table; } }

        /// <summary>
        /// Gets the boolean value (valid only if the <see cref="Type"/> is <see cref="DataType.Boolean"/>)
        /// </summary>
        public bool Boolean { get { return Number != 0; } }

        /// <summary>
        /// 得到字符串值 (valid only if the <see cref="Type"/> is <see cref="DataType.String"/>)
        /// </summary>
        public string String { get { return m_Object as string; } }

        /// <summary>
        /// Gets the CLR callback (valid only if the <see cref="Type"/> is <see cref="DataType.ClrFunction"/>)
        /// </summary>
        public CallbackFunction Callback { get { return m_Object as CallbackFunction; } }

        public UserData UserData { get { return m_Object as UserData; } }

        /// <summary>
        /// 判断该实例是否受只读保护，如果是则返回true。
        /// </summary>
        public bool ReadOnly { get { return m_ReadOnly; } }

        /// <summary>
        /// 创建一个新的可写入的值初始化为零。
        /// </summary>
        /// <returns></returns>
        public static DynValue NewNil()
        {
            return new DynValue();
        }

        /// <summary>
        /// 创建一个新的可写入的值，该值被初始化为指定的布尔值。
        /// </summary>
        public static DynValue NewBoolean(bool v)
        {
            return new DynValue()
            {
                m_Number = v ? 1 : 0,
                m_Type = DataType.Boolean,
            };
        }

        /// <summary>
        /// 创建一个新的可写入的值，初始化到指定的数字。
        /// </summary>
        public static DynValue NewNumber(double num)
        {
            return new DynValue()
            {
                m_Number = num,
                m_Type = DataType.Number,
                m_HashCode = -1,
            };
        }

        /// <summary>
		/// 创建一个新的可写入的值，初始化到指定的字符串。
		/// </summary>
		public static DynValue NewString(string str)
        {
            return new DynValue()
            {
                m_Object = str,
                m_Type = DataType.String,
            };
        }

        /// <summary>
        /// 为指定的StringBuilder创建一个新的可写值，类型为字符串。
        /// </summary>
        public static DynValue NewString(StringBuilder sb)
        {
            return new DynValue()
            {
                m_Object = sb.ToString(),
                m_Type = DataType.String,
            };
        }

        /// <summary>
        /// 使用字符串创建一个新的可写入的值，并将其初始化为指定的字符串。格式(如语法
        /// Creates a new writable value initialized to the specified string using String.Format like syntax
        /// </summary>
        public static DynValue NewString(string format, params object[] args)
        {
            return new DynValue()
            {
                m_Object = string.Format(format, args),
                m_Type = DataType.String,
            };
        }

        /// <summary>
        /// 创建一个新的可写入的值初始化为指定的协同程序。
        /// 只用于内部使用，如要外部使用，请参阅Script.CoroutineCreate。
        /// </summary>
        /// <param name="coroutine">The coroutine object.</param>
        /// <returns></returns>
        public static DynValue NewCoroutine(Coroutine coroutine)
        {
            return new DynValue()
            {
                m_Object = coroutine,
                m_Type = DataType.Thread
            };
        }

        /// <summary>
        /// 创建一个新的可写入的值初始化为指定的闭包(函数)。
        /// </summary>
        public static DynValue NewClosure(Closure function)
        {
            return new DynValue()
            {
                m_Object = function,
                m_Type = DataType.Function,
            };
        }


        /// <summary>
        /// Creates a new writable value initialized to the specified CLR callback.
        /// </summary>
        public static DynValue NewCallback(Func<ScriptExecutionContext, CallbackArguments, DynValue> callBack, string name = null)
        {
            return new DynValue()
            {
                //m_Object = new CallbackFunction(callBack, name),
                m_Type = DataType.ClrFunction,
            };
        }

        /// <summary>
        /// 创建一个新的可写入的值初始化为指定的CLR回调。
        /// See also CallbackFunction.FromDelegate and CallbackFunction.FromMethodInfo factory methods.
        /// </summary>
        public static DynValue NewCallback(CallbackFunction function)
        {
            return new DynValue()
            {
                m_Object = function,
                m_Type = DataType.ClrFunction,
            };
        }

        /// <summary>
        /// Creates a new writable value initialized to the specified table.
        /// </summary>
        public static DynValue NewTable(Table table)
        {
            return new DynValue()
            {
                m_Object = table,
                m_Type = DataType.Table,
            };
        }

        /// <summary>
        /// Creates a new writable value initialized to an empty prime table (a 
        /// prime table is a table made only of numbers, strings, booleans and other
        /// prime tables).
        /// </summary>
        public static DynValue NewPrimeTable()
        {
            return NewTable(new Table(null));
        }

        /// <summary>
        /// Creates a new writable value initialized to an empty table.
        /// </summary>
        public static DynValue NewTable(Script script)
        {
            return NewTable(new Table(script));
        }

        /// <summary>
        /// Creates a new writable value initialized to with array contents.
        /// </summary>
        public static DynValue NewTable(Script script, params DynValue[] arrayValues)
        {
            return NewTable(new Table(script, arrayValues));
        }

        /// <summary>
        /// Creates a new request for a tail call. This is the preferred way to execute Lua/MoonSharp code from a callback,
        /// although it's not always possible to use it. When a function (callback or script closure) returns a
        /// TailCallRequest, the bytecode processor immediately executes the function contained in the request.
        /// By executing script in this way, a callback function ensures it's not on the stack anymore and thus a number
        /// of functionality (state savings, coroutines, etc) keeps working at full power.
        /// </summary>
        /// <param name="tailFn">The function to be called.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static DynValue NewTailCallReq(DynValue tailFn, params DynValue[] args)
        {
            return new DynValue()
            {
                m_Object = new TailCallData()
                {
                    Args = args,
                    Function = tailFn,
                },
                m_Type = DataType.TailCallRequest,
            };
        }


        /// <summary>
        /// Creates a new request for a tail call. This is the preferred way to execute Lua/MoonSharp code from a callback,
        /// although it's not always possible to use it. When a function (callback or script closure) returns a
        /// TailCallRequest, the bytecode processor immediately executes the function contained in the request.
        /// By executing script in this way, a callback function ensures it's not on the stack anymore and thus a number
        /// of functionality (state savings, coroutines, etc) keeps working at full power.
        /// </summary>
        /// <param name="tailCallData">The data for the tail call.</param>
        /// <returns></returns>
        public static DynValue NewTailCallReq(TailCallData tailCallData)
        {
            return new DynValue()
            {
                m_Object = tailCallData,
                m_Type = DataType.TailCallRequest,
            };
        }

        /// <summary>
        /// Creates a new request for a yield of the current coroutine.
        /// </summary>
        /// <param name="args">The yield argumenst.</param>
        /// <returns></returns>
        public static DynValue NewYieldReq(DynValue[] args)
        {
            return new DynValue()
            {
                m_Object = new YieldRequest() { ReturnValues = args },
                m_Type = DataType.YieldRequest,
            };
        }

        /// <summary>
        /// Creates a new request for a yield of the current coroutine.
        /// </summary>
        /// <param name="args">The yield argumenst.</param>
        /// <returns></returns>
        internal static DynValue NewForcedYieldReq()
        {
            return new DynValue()
            {
                m_Object = new YieldRequest() { Forced = true },
                m_Type = DataType.YieldRequest,
            };
        }

        /// <summary>
        /// 创建一个新的元祖并初始化为指定的值。
        /// </summary>
        public static DynValue NewTuple(params DynValue[] values)
        {
            if (values.Length == 0)
                return DynValue.NewNil();

            if (values.Length == 1)
                return values[0];

            return new DynValue()
            {
                m_Object = values,
                m_Type = DataType.Tuple,
            };
        }

        /// <summary>
        /// 创建一个新的元祖并初始化为指定的值。
        /// Creates a new tuple initialized to the specified values - which can be potentially other tuples
        /// </summary>
        public static DynValue NewTupleNested(params DynValue[] values)
        {
            if (!values.Any(v => v.Type == DataType.Tuple))
                return NewTuple(values);

            if (values.Length == 1)
                return values[0];

            List<DynValue> vals = new List<DynValue>();

            foreach (var v in values)
            {
                if (v.Type == DataType.Tuple)
                    vals.AddRange(v.Tuple);
                else
                    vals.Add(v);
            }

            return new DynValue()
            {
                m_Object = vals.ToArray(),
                m_Type = DataType.Tuple,
            };
        }

        /// <summary>
        /// 创建一个新的用户数据值
        /// Creates a new userdata value
        /// </summary>
        public static DynValue NewUserData(UserData userData)
        {
            return new DynValue()
            {
                m_Object = userData,
                m_Type = DataType.UserData,
            };
        }

        /// <summary>
        /// 返回实例的只读副本，如果该实例当前是只读的则返回实例自身，否则返回实例的只读克隆副本。
        /// </summary>
        public DynValue AsReadOnly()
        {
            if (ReadOnly)
                return this;
            else
            {
                return Clone(true);
            }
        }

        /// <summary>
        /// 克隆该实例。
        /// </summary>
        /// <returns></returns>
        public DynValue Clone()
        {
            return Clone(this.ReadOnly);
        }

        /// <summary>
        /// 克隆这个实例，重写“readonly”状态。
        /// </summary>
        /// <param name="readOnly">if set to <c>true</c> the new instance is set as readonly, or writeable otherwise.</param>
        /// <returns></returns>
        public DynValue Clone(bool readOnly)
        {
            DynValue v = new DynValue();
            v.m_Object = this.m_Object;
            v.m_Number = this.m_Number;
            v.m_HashCode = this.m_HashCode;
            v.m_Type = this.m_Type;
            v.m_ReadOnly = readOnly;
            return v;
        }

        /// <summary>
        /// 克隆该实例，返回一个可写的副本。
        /// </summary>
        /// <exception cref="System.ArgumentException">Can't clone Symbol values</exception>
        public DynValue CloneAsWritable()
        {
            return Clone(false);
        }

        /// <summary>
        /// 只读的实例,相当于空白
        /// </summary>
        public static DynValue Void { get; private set; }

        /// <summary>
        ///  一个被初始化为0并且只读的动态值实例，相当于Lua中的Nil。
        /// </summary>
        public static DynValue Nil { get; private set; }

        /// <summary>
        /// 只读的实例,相当于true
        /// </summary>
        public static DynValue True { get; private set; }

        /// <summary>
        /// 只读的实例,相当于false
        /// </summary>
        public static DynValue False { get; private set; }

        static DynValue()
        {
            Nil = new DynValue() { m_Type = DataType.Nil }.AsReadOnly();
            Void = new DynValue() { m_Type = DataType.Void }.AsReadOnly();
            True = DynValue.NewBoolean(true).AsReadOnly();
            False = DynValue.NewBoolean(false).AsReadOnly();
        }

        /// <summary>
        /// Returns a string which is what it's expected to be output by the print function applied to this value.
        /// </summary>
        public string ToPrintString()
        {
            if (this.m_Object != null && this.m_Object is RefIdObject)
            {
                RefIdObject refid = (RefIdObject)m_Object;

                string typeString = this.Type.ToLuaTypeString();

                if (m_Object is UserData)
                {
                    UserData ud = (UserData)m_Object;
                    string str = ud.Descriptor.AsString(ud.Object);
                    if (str != null)
                        return str;
                }

                return refid.FormatTypeString(typeString);
            }

            switch (Type)
            {
                case DataType.String:
                    return String;
                case DataType.Tuple:
                    return string.Join("\t", Tuple.Select(t => t.ToPrintString()).ToArray());
                case DataType.TailCallRequest:
                    return "(TailCallRequest -- INTERNAL!)";
                case DataType.YieldRequest:
                    return "(YieldRequest -- INTERNAL!)";
                default:
                    return ToString();
            }
        }

        /// <summary>
        /// Returns a string which is what it's expected to be output by debuggers.
        /// </summary>
        public string ToDebugPrintString()
        {
            if (this.m_Object != null && this.m_Object is RefIdObject)
            {
                RefIdObject refid = (RefIdObject)m_Object;

                string typeString = this.Type.ToLuaTypeString();

                if (m_Object is UserData)
                {
                    UserData ud = (UserData)m_Object;
                    string str = ud.Descriptor.AsString(ud.Object);
                    if (str != null)
                        return str;
                }

                return refid.FormatTypeString(typeString);
            }

            switch (Type)
            {
                case DataType.Tuple:
                    return string.Join("\t", Tuple.Select(t => t.ToPrintString()).ToArray());
                case DataType.TailCallRequest:
                    return "(TailCallRequest)";
                case DataType.YieldRequest:
                    return "(YieldRequest)";
                default:
                    return ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            switch (Type)
            {
                case DataType.Void:
                    return "void";
                case DataType.Nil:
                    return "nil";
                case DataType.Boolean:
                    return Boolean.ToString().ToLower();
                case DataType.Number:
                    return Number.ToString(CultureInfo.InvariantCulture);
                case DataType.String:
                    return "\"" + String + "\"";
                case DataType.Function:
                    return string.Format("(Function {0:X8})", Function.EntryPointByteCodeLocation);
                case DataType.ClrFunction:
                    return string.Format("(Function CLR)", Function);
                case DataType.Table:
                    return "(Table)";
                case DataType.Tuple:
                    return string.Join(", ", Tuple.Select(t => t.ToString()).ToArray());
                case DataType.TailCallRequest:
                    return "Tail:(" + string.Join(", ", Tuple.Select(t => t.ToString()).ToArray()) + ")";
                case DataType.UserData:
                    return "(UserData)";
                case DataType.Thread:
                    return string.Format("(Coroutine {0:X8})", this.Coroutine.ReferenceID);
                default:
                    return "(???)";
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            if (m_HashCode != -1)
                return m_HashCode;

            int baseValue = ((int)(Type)) << 27;

            switch (Type)
            {
                case DataType.Void:
                case DataType.Nil:
                    m_HashCode = 0;
                    break;
                case DataType.Boolean:
                    m_HashCode = Boolean ? 1 : 2;
                    break;
                case DataType.Number:
                    m_HashCode = baseValue ^ Number.GetHashCode();
                    break;
                case DataType.String:
                    m_HashCode = baseValue ^ String.GetHashCode();
                    break;
                case DataType.Function:
                    m_HashCode = baseValue ^ Function.GetHashCode();
                    break;
                case DataType.ClrFunction:
                    m_HashCode = baseValue ^ Callback.GetHashCode();
                    break;
                case DataType.Table:
                    m_HashCode = baseValue ^ Table.GetHashCode();
                    break;
                case DataType.Tuple:
                case DataType.TailCallRequest:
                    m_HashCode = baseValue ^ Tuple.GetHashCode();
                    break;
                case DataType.UserData:
                case DataType.Thread:
                default:
                    m_HashCode = 999;
                    break;
            }

            return m_HashCode;
        }

        /// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
        {
            DynValue other = obj as DynValue;

            if (other == null) return false;    // 比较实例为null则返回false

            // 如果自身和比较实例互为void和nil类型则返回true
            if ((other.Type == DataType.Nil && this.Type == DataType.Void)
                || (other.Type == DataType.Void && this.Type == DataType.Nil))
                return true;

            if (other.Type != this.Type) return false;  //  数据类型不相同则返回false


            switch (Type)
            {
                case DataType.Void:
                case DataType.Nil:
                    return true;
                case DataType.Boolean:
                    return Boolean == other.Boolean;
                case DataType.Number:
                    return Number == other.Number;
                case DataType.String:
                    return String == other.String;
                case DataType.Function:
                    return Function == other.Function;
                case DataType.ClrFunction:
                    return Callback == other.Callback;
                case DataType.Table:
                    return Table == other.Table;
                case DataType.Tuple:
                case DataType.TailCallRequest:
                    return Tuple == other.Tuple;
                case DataType.Thread:
                    return Coroutine == other.Coroutine;
                case DataType.UserData:
                    {
                        UserData ud1 = this.UserData;
                        UserData ud2 = other.UserData;

                        if (ud1 == null || ud2 == null)
                            return false;

                        if (ud1.Descriptor != ud2.Descriptor)
                            return false;

                        if (ud1.Object == null && ud2.Object == null)
                            return true;

                        if (ud1.Object != null && ud2.Object != null)
                            return ud1.Object.Equals(ud2.Object);

                        return false;
                    }
                default:
                    return object.ReferenceEquals(this, other);
            }
        }

        /// <summary>
        /// 将此值转换为字符串，如果类型为number，则使用强制。
        /// Casts this DynValue to string, using coercion if the type is number.
        /// </summary>
        /// <returns>The string representation, or null if not number, not string.</returns>
        public string CastToString()
        {
            DynValue rv = ToScalar();
            if (rv.Type == DataType.Number)
            {
                return rv.Number.ToString();
            }
            else if (rv.Type == DataType.String)
            {
                return rv.String;
            }
            return null;
        }

        /// <summary>
        /// 将DynValue转换为double值，使用强制字符串。
        /// Casts this DynValue to a double, using coercion if the type is string.
        /// </summary>
        /// <returns>The string representation, or null if not number, not string or non-convertible-string.</returns>
        public double? CastToNumber()
        {
            DynValue rv = ToScalar();
            if (rv.Type == DataType.Number)
            {
                return rv.Number;
            }
            else if (rv.Type == DataType.String)
            {
                double num;
                if (double.TryParse(rv.String, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
                    return num;
            }
            return null;
        }

        /// <summary>
        /// Casts this DynValue to a bool
        /// </summary>
        /// <returns>False if value is false or nil, true otherwise.</returns>
        public bool CastToBool()
        {
            DynValue rv = ToScalar();
            if (rv.Type == DataType.Boolean)
                return rv.Boolean;
            else return (rv.Type != DataType.Nil && rv.Type != DataType.Void);
        }

        /// <summary>
        /// Returns this DynValue as an instance of <see cref="IScriptPrivateResource"/>, if possible,
        /// null otherwise
        /// </summary>
        /// <returns>False if value is false or nil, true otherwise.</returns>
        public IScriptPrivateResource GetAsPrivateResource()
        {
            return m_Object as IScriptPrivateResource;
        }

        /// <summary>
        /// 将元组转换为标量值。如果它已经是一个标量值,这个函数返回实例自身。
        /// </summary>
        public DynValue ToScalar()
        {
            if (Type != DataType.Tuple)
                return this;

            if (Tuple.Length == 0)
                return DynValue.Void;

            return Tuple[0].ToScalar();
        }

        /// <summary>
        /// 执行一个赋值，用指定的值重写值。
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ScriptRuntimeException">If the value is readonly.</exception>
        public void Assign(DynValue value)
        {
            if (this.ReadOnly)
                throw new ScriptRuntimeException("Assigning on r-value");

            this.m_Number = value.m_Number;
            this.m_Object = value.m_Object;
            this.m_Type = value.Type;
            this.m_HashCode = -1;
        }

        /// <summary>
        /// 获取字符串或表值的长度。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ScriptRuntimeException">Value is not a table or string.</exception>
        public DynValue GetLength()
        {
            if (this.Type == DataType.Table)
                return DynValue.NewNumber(this.Table.Length);
            if (this.Type == DataType.String)
                return DynValue.NewNumber(this.String.Length);

            throw new ScriptRuntimeException("Can't get length of type {0}", this.Type);
        }

        /// <summary>
        /// 确定这个实例是nil还是void的
        /// </summary>
        public bool IsNil()
        {
            return this.Type == DataType.Nil || this.Type == DataType.Void;
        }


        /// <summary>
        /// 判断一个实例不为Nil并且不为void
        /// </summary>
        /// <returns></returns>
        public bool IsNotNil()
        {
            return this.Type != DataType.Nil && this.Type != DataType.Void;
        }

        /// <summary>
        /// 判断实例是否是void类型
        /// </summary>
        public bool IsVoid()
        {
            return this.Type == DataType.Void;
        }

        /// <summary>
        /// 判断实例不是void类型
        /// </summary>
        public bool IsNotVoid()
        {
            return this.Type != DataType.Void;
        }

        /// <summary>
        /// 判断一个DynValue对象是否为nil、void或NaN。
        /// 这样的值不能用于做为table的key。
        /// </summary>
        /// <returns></returns>
        public bool IsNilOrNan()
        {
            return (Type == DataType.Nil) || (Type == DataType.Void) ||
                   (Type == DataType.Number && double.IsNaN(Number));
        }

        /// <summary>
        ///  更改数字值的数值。
        /// </summary>
        /// <param name="num"></param>
        internal void AssignNumber(double num)
        {
            if (this.ReadOnly)
                throw new InternalErrorException(null, "Writing on r-value");

            if (this.Type != DataType.Number)
                throw new InternalErrorException("Can't assign number to type {0}", this.Type);

            this.m_Number = num;
        }

        public static DynValue FromObject(Script script, object obj)
        {
            //return ClrToScriptConversions.ObjectToDynValue(script, obj);
            // todo 
            return null;
        }

        /// <summary>
        /// 将DynValue转换为CLR类型。
		/// </summary>
		public object ToObject()
        {
            return ScriptToClrConversions.DynValueToObject(this);
        }

        /// <summary>
        /// 将DynValue转换为指定的CLR类型。
        /// </summary>
        public object ToObject(Type desiredType)
        {
            //Contract.Requires(desiredType != null);
            return ScriptToClrConversions.DynValueToObjectOfType(this, desiredType, null, false);
        }

        /// <summary>
        /// 将DynValue转换为指定的CLR类型。
        /// </summary>
        public T ToObject<T>()
        {
            return (T)ToObject(typeof(T));
        }

#if HASDYNAMIC
		/// <summary>
		/// Converts this MoonSharp DynValue to a CLR object, marked as dynamic
		/// </summary>
		public dynamic ToDynamic()
		{
			return MoonSharp.Interpreter.Interop.Converters.ScriptToClrConversions.DynValueToObject(this);
		}
#endif

        public DynValue CheckType(string funcName, DataType desiredType, int argNum = -1,
            TypeValidationFlags flags = TypeValidationFlags.Default)
        {



            return null;
        }

        public T CheckUserDataType<T>(string funcName, int argNum = -1,
            TypeValidationFlags flags = TypeValidationFlags.Default)
        {
            return default(T);
        }







    }
}
