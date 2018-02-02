/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 11:42:16
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
using System.Linq;
using Iuker.Common;
using Iuker.Common.Util;

namespace Iuker.MoonSharp.Interpreter.Loaders
{
    /// <summary>
    /// 一个可以从Unity3D资源加载脚本的程序
    /// MoonSharp在Unity3D中应用时会使用这个默认的脚本加载器
    /// 脚本应该被保存为.txt文件并放置在Assets/Resources的子目录下
    /// </summary>
    public class UnityAssetsScriptLoader : ScriptLoaderBase
    {
        Dictionary<string, string> m_Resources = new Dictionary<string, string>();

        /// <summary>
        /// 脚本应该被存储的默认路径(如果没有更改的话)
        /// </summary>
        public static string DEFAULT_PATH = "/MoonSharp/Scripts/";

        private static bool isInitAssetPath = false;

        public static void InitDefaultAssetPath(string path)
        {
            // todo monosharp fixde
            if (isInitAssetPath)
                throw new Exception("UntiyAssetScriptLoader is inited once!");
            DEFAULT_PATH = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityAssetsScriptLoader"/> class.
        /// </summary>
        /// <param name="assetsPath">The path, relative to Assets/Resources. For example
        /// if your scripts are stored under Assets/Resources/Scripts, you should
        /// pass the value "Scripts". If null, "MoonSharp/Scripts" is used. </param>
        public UnityAssetsScriptLoader(string assetsPath = null)
        {
            // todo monosharp fixde
            if (DEFAULT_PATH == null)
                throw new Exception("UnityAssetScriptLoader is not init!");
            assetsPath = assetsPath ?? DEFAULT_PATH;
#if UNITY_5
            LoadResourcesUnityNative(assetsPath);
#else
			LoadResourcesWithReflection(assetsPath);
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityAssetsScriptLoader"/> class.
        /// </summary>
        /// <param name="scriptToCodeMap">A dictionary mapping filenames to the proper Lua script code.</param>
        public UnityAssetsScriptLoader(Dictionary<string, string> scriptToCodeMap)
        {
            m_Resources = scriptToCodeMap;
        }

#if UNITY_5
        void LoadResourcesUnityNative(string assetsPath)
        {
            try
            {
                var luaDc = FileUtility.GetFilePathDictionary(assetsPath, s => !s.Contains("meta")).FilePathDictionary;
                foreach (var kv in luaDc)
                {
                    var text = System.IO.File.ReadAllText(kv.Value);
                    m_Resources.Add(kv.Key, text);
                }
            }
            catch (Exception ex)
            {
                Debuger.LogError(string.Format("Error initializing UnityScriptLoader : {0}", ex));
            }
        }

#else

		void LoadResourcesWithReflection(string assetsPath)
		{
			try
			{
				Type resourcesType = Type.GetType("UnityEngine.Resources, UnityEngine");
				Type textAssetType = Type.GetType("UnityEngine.TextAsset, UnityEngine");

				MethodInfo textAssetNameGet = Framework.Do.GetMethod(Framework.Do.GetProperty(textAssetType, "name"));
				MethodInfo textAssetTextGet = Framework.Do.GetMethod(Framework.Do.GetProperty(textAssetType, "text"));

				MethodInfo loadAll = Framework.Do.GetMethod(resourcesType, "LoadAll",
					new Type[] { typeof(string), typeof(Type) });

				Array array = (Array)loadAll.Invoke(null, new object[] { assetsPath, textAssetType });

				for (int i = 0; i < array.Length; i++)
				{
					object o = array.GetValue(i);

					string name = textAssetNameGet.Invoke(o, null) as string;
					string text = textAssetTextGet.Invoke(o, null) as string;

					m_Resources.Add(name, text);
				}
			}
			catch (Exception ex)
			{
#if !(PCL || ENABLE_DOTNET || NETFX_CORE)
				Console.WriteLine("Error initializing UnityScriptLoader : {0}", ex);
#endif
				System.Diagnostics.Debug.WriteLine(string.Format("Error initializing UnityScriptLoader : {0}", ex));
			}
		}
#endif

        private string GetFileName(string filename)
        {
            int b = Math.Max(filename.LastIndexOf('\\'), filename.LastIndexOf('/'));

            if (b > 0)
                filename = filename.Substring(b + 1);

            return filename;
        }

        /// <summary>
        /// Opens a file for reading the script code.
        /// It can return either a string, a byte[] or a Stream.
        /// If a byte[] is returned, the content is assumed to be a serialized (dumped) bytecode. If it's a string, it's
        /// assumed to be either a script or the output of a string.dump call. If a Stream, autodetection takes place.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="globalContext">The global context.</param>
        /// <returns>
        /// A string, a byte[] or a Stream.
        /// </returns>
        /// <exception cref="System.Exception">UnityAssetsScriptLoader.LoadFile : Cannot load  + file</exception>
        public override object LoadFile(string file, Table globalContext)
        {
            file = GetFileName(file);

            if (m_Resources.ContainsKey(file))
                return m_Resources[file];
            else
            {
                var error = string.Format(
@"Cannot load script '{0}'. By default, scripts should be .txt files placed under a Assets/Resources/{1} directory.
If you want scripts to be put in another directory or another way, use a custom instance of UnityAssetsScriptLoader or implement
your own IScriptLoader (possibly extending ScriptLoaderBase).", file, DEFAULT_PATH);

                throw new Exception(error);
            }
        }

        /// <summary>
        /// 检查给定的文件是否存在
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public override bool ScriptFileExists(string file)
        {
            file = GetFileName(file);
            return m_Resources.ContainsKey(file);
        }


        /// <summary>
        /// 获取已加载脚本的文件名列表(用于调试目的)。
        /// </summary>
        /// <returns></returns>
        public string[] GetLoadedScripts()
        {
            return m_Resources.Keys.ToArray();
        }
    }
}
