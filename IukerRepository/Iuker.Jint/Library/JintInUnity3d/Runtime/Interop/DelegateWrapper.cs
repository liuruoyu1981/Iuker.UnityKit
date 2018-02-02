using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Jint.Native;
using Jint.Native.Function;

namespace Jint.Runtime.Interop
{
    /// <summary>
    /// Represents a FunctionInstance wrapper around a CLR method. This is used by user to pass
    /// custom methods to the engine.
    /// </summary>
    public sealed class DelegateWrapper : FunctionInstance
    {
        private readonly Delegate _d;
        private ParameterInfo[] parameterInfos;
        private bool delegateContainsParamsArgument;
        private int delegateArgumentsCount;
        private int delegateNonParamsArgumentsCount;
        private int jsArgumentsCount;
        private int jsArgumentsWithoutParamsCount;
        private object[] parameters;

        private void Init()
        {
            parameterInfos = _d.GetMethodInfo().GetParameters();
            delegateContainsParamsArgument = parameterInfos.Any(p => p.HasAttribute<ParamArrayAttribute>());
            delegateArgumentsCount = parameterInfos.Length;
            delegateNonParamsArgumentsCount = delegateContainsParamsArgument ? delegateArgumentsCount - 1 : delegateArgumentsCount;
        }

        public DelegateWrapper(Engine engine, Delegate d) : base(engine, null, null, false)
        {
            _d = d;
            Prototype = engine.Function.PrototypeObject;
            Init();
        }

        //public override JsValue Call(JsValue thisObject, JsValue[] jsArguments)
        //{
        //    //  todo
        //    return JsValue.FromObject(Engine, GoSetName.Invoke(thisObject, jsArguments));

        //    jsArgumentsCount = jsArguments.Length;
        //    jsArgumentsWithoutParamsCount = Math.Min(jsArgumentsCount, delegateNonParamsArgumentsCount);

        //    parameters = new object[delegateArgumentsCount];

        //    // convert non params parameter to expected types
        //    for (var i = 0; i < jsArgumentsWithoutParamsCount; i++)
        //    {
        //        var parameterType = parameterInfos[i].ParameterType;

        //        if (parameterType == typeof(JsValue))
        //        {
        //            parameters[i] = jsArguments[i];
        //        }
        //        else
        //        {
        //            parameters[i] = Engine.ClrTypeConverter.Convert(
        //                jsArguments[i].ToObject(),
        //                parameterType,
        //                CultureInfo.InvariantCulture);
        //        }
        //    }

        //    // assign null to parameters not provided
        //    for (var i = jsArgumentsWithoutParamsCount; i < delegateNonParamsArgumentsCount; i++)
        //    {
        //        if (parameterInfos[i].ParameterType.IsValueType())
        //        {
        //            parameters[i] = Activator.CreateInstance(parameterInfos[i].ParameterType);
        //        }
        //        else
        //        {
        //            parameters[i] = null;
        //        }
        //    }

        //    // assign params to array and converts each objet to expected type
        //    if (delegateContainsParamsArgument)
        //    {
        //        int paramsArgumentIndex = delegateArgumentsCount - 1;
        //        int paramsCount = Math.Max(0, jsArgumentsCount - delegateNonParamsArgumentsCount);

        //        object[] paramsParameter = new object[paramsCount];
        //        var paramsParameterType = parameterInfos[paramsArgumentIndex].ParameterType.GetElementType();

        //        for (var i = paramsArgumentIndex; i < jsArgumentsCount; i++)
        //        {
        //            var paramsIndex = i - paramsArgumentIndex;

        //            if (paramsParameterType == typeof(JsValue))
        //            {
        //                paramsParameter[paramsIndex] = jsArguments[i];
        //            }
        //            else
        //            {
        //                paramsParameter[paramsIndex] = Engine.ClrTypeConverter.Convert(
        //                    jsArguments[i].ToObject(),
        //                    paramsParameterType,
        //                    CultureInfo.InvariantCulture);
        //            }
        //        }
        //        parameters[paramsArgumentIndex] = paramsParameter;
        //    }
        //    try
        //    {
        //        return JsValue.FromObject(Engine, _d.DynamicInvoke(parameters));
        //    }
        //    catch (TargetInvocationException exception)
        //    {
        //        var meaningfulException = exception.InnerException ?? exception;
        //        var handler = Engine.Options._ClrExceptionsHandler;

        //        if (handler != null && handler(meaningfulException))
        //        {
        //            throw new JavaScriptException(Engine.Error, meaningfulException.Message);
        //        }

        //        throw meaningfulException;
        //    }
        //}

        public override JsValue Call(JsValue thisObject, JsValue[] jsArguments)
        {
            jsArgumentsCount = jsArguments.Length;
            jsArgumentsWithoutParamsCount = Math.Min(jsArgumentsCount, delegateNonParamsArgumentsCount);

            parameters = new object[delegateArgumentsCount];

            // convert non params parameter to expected types
            for (var i = 0; i < jsArgumentsWithoutParamsCount; i++)
            {
                var parameterType = parameterInfos[i].ParameterType;

                if (parameterType == typeof(JsValue))
                {
                    parameters[i] = jsArguments[i];
                }
                else
                {
                    parameters[i] = Engine.ClrTypeConverter.Convert(
                        jsArguments[i].ToObject(),
                        parameterType,
                        CultureInfo.InvariantCulture);
                }
            }

            // assign null to parameters not provided
            for (var i = jsArgumentsWithoutParamsCount; i < delegateNonParamsArgumentsCount; i++)
            {
                if (parameterInfos[i].ParameterType.IsValueType())
                {
                    parameters[i] = Activator.CreateInstance(parameterInfos[i].ParameterType);
                }
                else
                {
                    parameters[i] = null;
                }
            }

            // assign params to array and converts each objet to expected type
            if (delegateContainsParamsArgument)
            {
                int paramsArgumentIndex = delegateArgumentsCount - 1;
                int paramsCount = Math.Max(0, jsArgumentsCount - delegateNonParamsArgumentsCount);

                object[] paramsParameter = new object[paramsCount];
                var paramsParameterType = parameterInfos[paramsArgumentIndex].ParameterType.GetElementType();

                for (var i = paramsArgumentIndex; i < jsArgumentsCount; i++)
                {
                    var paramsIndex = i - paramsArgumentIndex;

                    if (paramsParameterType == typeof(JsValue))
                    {
                        paramsParameter[paramsIndex] = jsArguments[i];
                    }
                    else
                    {
                        paramsParameter[paramsIndex] = Engine.ClrTypeConverter.Convert(
                            jsArguments[i].ToObject(),
                            paramsParameterType,
                            CultureInfo.InvariantCulture);
                    }
                }
                parameters[paramsArgumentIndex] = paramsParameter;
            }
            try
            {
                return JsValue.FromObject(Engine, _d.DynamicInvoke(parameters));
            }
            catch (TargetInvocationException exception)
            {
                var meaningfulException = exception.InnerException ?? exception;
                var handler = Engine.Options._ClrExceptionsHandler;

                if (handler != null && handler(meaningfulException))
                {
                    throw new JavaScriptException(Engine.Error, meaningfulException.Message);
                }

                throw meaningfulException;
            }
        }
    }
}
