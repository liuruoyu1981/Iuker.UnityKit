/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:46:19
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
using Iuker.MoonSharp.Interpreter.Loaders;
using Iuker.MoonSharp.Loaders;

namespace Iuker.MoonSharp.Interpreter.Platforms
{
    /// <summary>
    /// 用于自动检测系统/平台细节的静态类
    /// </summary>
    public static class PlatformAutoDetector
    {
        private static bool? m_IsRunningOnAOT = null;

        private static bool m_AutoDetectionsDone = false;

        /// <summary>
        /// 获取一个值，指示这个实例是否在mono上运行。
        /// </summary>
        public static bool IsRunningOnMono { get; private set; }

        /// <summary>
        /// 获取一个值，指示这个实例是否在一个CLR4兼容的实现上运行
        /// </summary>
        public static bool IsRunningOnClr4 { get; private set; }

        /// <summary>
        /// 获取一个值，指示这个实例是否在unity3d上运行。
        /// </summary>
        public static bool IsRunningOnUnity { get; private set; }

        /// <summary>
        /// 获取一个值，指示该实例是否已作为一个可移植的类库构建
        /// </summary>
        public static bool IsPortableFramework { get; private set; }

        /// <summary>
        /// 获取一个值，指示这个实例是否已经在Unity中本地编译(与导入DLL相反)。
        /// </summary>
        public static bool IsUnityNative { get; private set; }

        /// <summary>
        /// 获取一个值，指示这个实例是否已经在Unity中使用IL2CPP本地编译。
        /// </summary>
        public static bool IsUnityIL2CPP { get; private set; }


        public static bool IsRunningOnAOT
        {
            get
            {
#if UNITY_WEBGL || UNITY_IOS || UNITY_TVOS || ENABLE_IL2CPP
				return true;
#else
                if (!m_IsRunningOnAOT.HasValue)
                {
                    try
                    {
                        System.Linq.Expressions.Expression e =
                            System.Linq.Expressions.Expression.Constant(5, typeof(int));
                        var lambda = System.Linq.Expressions.Expression.Lambda<Func<int>>(e);
                        lambda.Compile();
                        m_IsRunningOnAOT = false;
                    }
                    catch (Exception)   // 通过捕获C#动态编译异常来确定当前是否可以使用动态编译
                    {
                        m_IsRunningOnAOT = true;
                    }
                }

                return m_IsRunningOnAOT.Value;
#endif
            }
        }

        private static void AutoDetectPlatformFlags()
        {
            if(m_AutoDetectionsDone)
                return;

#if PCL
			IsPortableFramework = true;
#if ENABLE_DOTNET
			IsRunningOnUnity = true;
			IsUnityNative = true;
#endif
#else
#if UNITY_5
			IsRunningOnUnity = true;
			IsUnityNative = true;

#if ENABLE_IL2CPP
					IsUnityIL2CPP = true;
#endif
#elif !(NETFX_CORE)
            // todo 
            //IsRunningOnUnity = AppDomain.CurrentDomain
            //    .GetAssemblies()
            //    .SelectMany(a => a.SafeGetTypes())
            //    .Any(t => t.FullName.StartsWith("UnityEngine."));
#endif
#endif

            IsRunningOnMono = (Type.GetType("Mono.Runtime") != null);

            IsRunningOnClr4 = (Type.GetType("System.Lazy`1") != null);

            m_AutoDetectionsDone = true;
        }

        internal static IPlatformAccessor GetDefaultPlatform()
        {
            AutoDetectPlatformFlags();

#if PCL || ENABLE_DOTNET
			return new LimitedPlatformAccessor();
#else
            if (IsRunningOnUnity)
                return new LimitedPlatformAccessor();

#if DOTNET_CORE
			return new DotNetCorePlatformAccessor();
#else
            return new StandardPlatformAccessor();
#endif
#endif
        }

        internal static IScriptLoader GetDefaultScriptLoader()
        {
            AutoDetectPlatformFlags();

            if (IsRunningOnUnity)
                return new UnityAssetsScriptLoader();
            else
            {
#if (DOTNET_CORE)
				return new FileSystemScriptLoader();
#elif (PCL || ENABLE_DOTNET || NETFX_CORE)
				return new InvalidScriptLoader("Portable Framework");
#else
                return new FileSystemScriptLoader();
#endif
            }
        }

    }
}
