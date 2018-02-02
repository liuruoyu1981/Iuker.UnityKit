/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/06 07:34:22
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

using System.IO;
using System.Reflection;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// unity3d通用常用常量
    /// </summary>
    public static class U3dConstants
    {
        /// <summary>
        /// Unity项目的根目录
        /// Assets的上级目录
        /// </summary>
        public static string UnityRootDir
        {
            get
            {
                return Application.dataPath.Replace("Assets", "");
            }
        }

        public static string IukerUnityKitDir
        {
            get
            {
                return string.Format("{0}/1_Iuker.UnityKit/", Application.dataPath);
            }
        }

        /// <summary>
        /// Unity项目的开发根目录
        /// 即Assets目录
        /// </summary>
        public static string UnityAssetsDir
        {
            get
            {
                return Application.dataPath + "/";
            }
        }

        public static string ProjectSettingsDir
        {
            get
            {
                return Application.dataPath.Replace("Assets", "") + "ProjectSettings/";
            }
        }

        public static string OriginProjectSettingsDir
        {
            get
            {
                return string.Format("{0}.ProjectSettings/", ProjectTemplateDir);
            }
        }

        private static string ProjectTemplateDir
        {
            get
            {
                return string.Format("{0}UnityProjectTemplate/", IukerUnityKitDir);
            }
        }

        /// <summary>
        /// Unity项目StreamingAssets目录（带反斜线）
        /// </summary>
        public static string StreamingAssetsDir
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        public static string SandboxDir
        {
            get
            {
                return Application.persistentDataPath + "/";
            }
        }

        /// <summary>
        /// protobuf-net项目Protogen工具所在目录
        /// </summary>
        public static string ProtobufModelProtogenDir
        {
            get
            {
                return IukerUnityKitDir + "IukerRepository/.Iuker.ProtobufModel/ProtoGen/";
            }
        }

        /// <summary>
        /// protobuf-net项目Precompile工具所在目录
        /// </summary>
        public static string ProtobufModelPrecompileDir
        {
            get
            {
                return IukerUnityKitDir + "IukerRepository/.Iuker.ProtobufModel/Precompile/";
            }
        }

        public static string ProtobufModelRootDir
        {
            get
            {
                return IukerUnityKitDir + "IukerRepository/.Iuker.ProtobufModel/";
            }
        }

        public static string ProtobufNetDllPath
        {
            get
            {
                return IukerUnityKitDir + "IukerRepository/.Iuker.ProtobufModel/protobuf-net.dll";
            }
        }

        /// <summary>
        /// 当前项目程序集文件路径
        /// </summary>
        public static string AssemblyCSharpPath
        {
            get
            {
                return Directory.GetParent(Application.dataPath) +
                       "/Library/ScriptAssemblies/Assembly-CSharp.dll";
            }
        }

        public static Assembly AssemblyCSharp
        {
            get
            {
                var asm = Assembly.LoadFile(AssemblyCSharpPath);
                return asm;
            }
        }

        /// <summary>
        /// 根目录下的Resources目录
        /// </summary>
        public static string RootResourcesDir
        {
            get
            {
                return Application.dataPath + "/Resources/";
            }
        }

        /// <summary>
        /// 框架提供的视图模板预制件路径
        /// </summary>
        public static string ViewTemplatePath
        {
            get
            {
                return string.Format("{0}view_template.prefab", ProjectTemplateDir);
            }
        }

        #region Profiler

        public static readonly string Profiler_DefaultU3dEventModule = "Profiler_DefaultU3dEventModule";
        public static readonly string Profiler_DefaultU3dTimerModule = "Profiler_DefaultU3dTimerModule";
        public static readonly string Profiler_GameObjectPoolCount = "Profiler_GameObjectPoolCount";


        #endregion

        #region 热更新

        /// <summary>
        /// 资源及脚本更新服务器地址
        /// </summary>
        public static string HotUpdateHttpServerUrl = "HotUpdateHttpServerUrl";

        #region 本地http服务器

        /// <summary>
        /// 本地主机Http服务器目录
        /// </summary>
        public static string LocalHttpDir
        {
            get
            {
                return Application.dataPath + "/2_LocalHostHttp/";
            }
        }

        #endregion




        #endregion

        #region TypeScript

        public static string TsProjectDir
        {
            get
            {
                return string.Format("{0}IukerRepository/Iuker.UnityKit.Ts/", IukerUnityKitDir);
            }
        }

        public static string TsProjectFilePath
        {
            get
            {
                return string.Format("{0}IukerRepository/Iuker.UnityKit.Ts/Iuker.UnityKit.Ts.njsproj", IukerUnityKitDir);
            }
        }


        public static string TsIukerDir { get { return TsProjectDir + "Iuker/"; } }


        #endregion


    }
}
