using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Iuker.Js.Runtime.Interop;

namespace Iuker.Js
{
    /// <summary>
    /// Iuker.Js设置
    /// </summary>
    public class Options
    {
        private bool _discardGlobal;
        private bool _strict;
        private bool _allowDebuggerStatement;
        private bool _debugMode;
        private bool _allowClr;
        private readonly List<IObjectConverter> _objectConverters = new List<IObjectConverter>();
        private int _maxStatements;
        private int _maxRecursionDepth = -1;
        private TimeSpan _timeoutInterval;
        private CultureInfo _culture = CultureInfo.CurrentCulture;
        //private TimeZoneInfo _localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");
        private List<Assembly> _lookupAssemblies = new List<Assembly>();
        private Predicate<Exception> _clrExceptionsHandler;

        /// <summary>
        /// 调用后将不会初始化全局作用域
        /// 可以用在轻量级脚本中以提高性能
        /// </summary>
        /// <param name="discard"></param>
        /// <returns></returns>
        public Options DiscardGlobal(bool discard = true)
        {
            _discardGlobal = discard;
            return this;
        }

        /// <summary>
        /// 在严格模式下运行脚本
        /// </summary>
        /// <param name="strict"></param>
        /// <returns></returns>
        public Options Strict(bool strict = true)
        {
            _strict = strict;
            return this;
        }

        /// <summary>
        /// 允许调试语句
        /// </summary>
        /// <param name="allowDebuggerStatement"></param>
        /// <returns></returns>
        public Options AllowDebuggerStatement(bool allowDebuggerStatement = true)
        {
            _allowDebuggerStatement = allowDebuggerStatement;
            return this;
        }

        /// <summary>
        /// 允许在调试模式下运行脚本
        /// </summary>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public Options DebugMode(bool debugMode = true)
        {
            _debugMode = debugMode;
            return this;
        }

        /// <summary>
        /// 添加一个用于转换CLR类型的类型转换器
        /// </summary>
        /// <param name="objectConverter"></param>
        /// <returns></returns>
        public Options AddObjectConverter(IObjectConverter objectConverter)
        {
            _objectConverters.Add(objectConverter);
            return this;
        }

        /// <summary>
        /// 允许脚本直接调用CRL类型
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public Options AllowClr(params Assembly[] assemblies)
        {
            _allowClr = true;
            _lookupAssemblies.AddRange(assemblies);
            _lookupAssemblies = _lookupAssemblies.Distinct().ToList();
            return this;
        }


        public Options CatchClrExceptions()
        {
            CatchClrExceptions(_ => true);
            return this;
        }

        public Options CatchClrExceptions(Predicate<Exception> hanlder)
        {
            _clrExceptionsHandler = hanlder;
            return this;
        }

        public Options MaxStatements(int maxStatements = 0)
        {
            _maxStatements = maxStatements;
            return this;
        }

        /// <summary>
        /// 设置超时间隔
        /// </summary>
        /// <param name="timeoutInterval"></param>
        /// <returns></returns>
        public Options TimeoutInterval(TimeSpan timeoutInterval)
        {
            _timeoutInterval = timeoutInterval;
            return this;
        }

        /// <summary>
        /// 设置允许递归的最大深度
        /// 允许递归的情况
        /// a) In case max depth is zero no recursion is allowed.
        /// b) In case max depth is equal to n it means that in one scope function can be called no more than n times.
        /// </summary>
        /// <param name="maxRecursionDepth"></param>
        /// <returns></returns>
        public Options LimitRecursion(int maxRecursionDepth = 0)
        {
            _maxRecursionDepth = maxRecursionDepth;
            return this;
        }

        public Options Culture(CultureInfo cultureInfo)
        {
            _culture = cultureInfo;
            return this;
        }


        public Options LocalTimeZone(TimeZoneInfo timeZoneInfo)
        {
            return this;
        }


        internal bool _IsGlobalDiscarded => _discardGlobal;

        internal bool _IsStrict => _strict;

        internal bool _IsDebuggerStatementAllowed => _allowDebuggerStatement;

        internal bool _IsDebugMode => _debugMode;

        internal bool _IsClrAllowed => _allowClr;

        internal Predicate<Exception> _ClrExceptionsHandler => _clrExceptionsHandler;

        internal IList<Assembly> _LookupAssemblies => _lookupAssemblies;

        internal IEnumerable<IObjectConverter> _ObjectConverters => _objectConverters;

        internal int _MaxStatements => _maxStatements;

        internal int _MaxRecursionDepth => _maxRecursionDepth;

        internal TimeSpan _TimeoutInterval => _timeoutInterval;

        internal CultureInfo _Culture => _culture;

        //internal TimeZoneInfo _LocalTimeZone => _localTimeZone;




    }
}