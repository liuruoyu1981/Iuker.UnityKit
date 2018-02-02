/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 8/23/2017 4:19:32 PM
Email: 35490136@qq.com
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
using System.IO;
using System.Linq;
using System.Reflection;
using Iuker.Common;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Socket;
using Iuker.Common.Utility;
using Iuker.Jint.JsViewWidget;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Context;
using Iuker.UnityKit.Run.Module.Asset;
using Iuker.UnityKit.Run.Module.JavaScript;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Jint;
using Jint.Native;
using Jint.Native.String;
using Jint.Runtime;
using Jint.Runtime.Interop;
using UnityEngine;


namespace Iuker.Jint
{
    /// <summary>
    /// 默认的Javascript模块
    /// </summary>
    public class DefaultU3dModule_JavaScript : AbsU3dModule, IU3dJavaScriptModule
    {
        #region 字段和属性

        private Engine mJsEngine;
        public object Engine { get { return mJsEngine; } }

        private IU3dAssetModule mAssetModule;

        /// <summary>
        /// Js脚本路径字典
        /// </summary>
        private readonly Dictionary<string, string> mJsPaths = new Dictionary<string, string>();

        /// <summary>
        /// Js脚本的Md5字典
        /// 用于支持热重载快速开发
        /// </summary>
        private readonly Dictionary<string, string> mJsMd5s = new Dictionary<string, string>();

        /// <summary>
        /// 运行或模拟移动设备时使用的脚本字符串字典
        /// </summary>
        private readonly Dictionary<string, string> mJsScripts = new Dictionary<string, string>();

        /// <summary>
        /// 当前已经加载过的脚本名集合
        /// </summary>
        private readonly HashSet<string> mLoadedScripts = new HashSet<string>();

        #endregion

        #region 基类实现

        public override string ModuleName
        {
            get
            {
                return ModuleType.JavaScript.ToString();
            }
        }

        public override void Init(IFrame frame)
        {
            base.Init(frame);

            var asms = new List<Assembly> { typeof(GameObject).Assembly, Assembly.GetExecutingAssembly() };
            mJsEngine = new Engine(cfg => cfg.AllowClr(asms.ToArray()));
            mJsEngine.Execute(@"
var UnityEngine = importNamespace('UnityEngine');
");
            InitBaseContext();
        }

        protected override void OnHotUpdateComplete()
        {
            base.OnHotUpdateComplete();

            mAssetModule = U3DFrame.AssetModule;
            InitJsContext();
            InitJsScirpts();
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.LateUpdate.ToString(), InitAllJsObjectByCoroutine);
        }

        private bool mIsReady;

        public override bool IsReady
        {
            get { return mIsReady; }
        }

        /// <summary>
        /// 获取全局对象
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public object GetGlobalValue(string scriptName)
        {
            return mJsEngine.GetValue(scriptName);
        }

        public object GetJsValueByNameSpace(string name, string nameSpace)
        {
            var nsValue = mJsEngine.GetValue(nameSpace);
            return nsValue.GetJsValue(name);
        }

        public bool IsMd5Change(string scriptName)
        {
            var oldMd5 = mJsMd5s[scriptName];
            var newMd5 = Md5Uitlity.GetFileMd5(mJsPaths[scriptName]);
            var isChange = oldMd5 != newMd5;
            if (!isChange) return false;

            Debug.Log(string.Format("目标脚本{0}当前已更新!！", scriptName));
            mJsMd5s[scriptName] = newMd5;
            return true;
        }

        public bool Exist(string scriptName)
        {
            return U3DFrame.AppContext.DevelopContextType == DevelopContextType.Editor ?
                mJsPaths.ContainsKey(scriptName) :
                mJsScripts.ContainsKey(scriptName);
        }

        public void DoFile(string scriptName)
        {
#if UNITY_EDITOR || DEBUG
            try
            {
#endif
                if (U3DFrame.AppContext.DevelopContextType == DevelopContextType.Device)
                {
                    DoFileByDevice(scriptName);
                }
                else
                {
                    DoFileByEditor(scriptName);
                }
#if UNITY_EDITOR || DEBUG
            }
            catch (JavaScriptException jsException)
            {
                LogException(jsException.StackTrace);
            }
#endif
        }

        private void DoFileByEditor(string scriptName)
        {
            if (!mJsPaths.ContainsKey(scriptName))
            {
                Debug.LogError(string.Format("目标脚本{0}不存在，请检查脚本目录！", scriptName));
                return;
            }

            if (!mJsMd5s.ContainsKey(scriptName))   //  第一次加载
            {
                var md5 = Md5Uitlity.GetFileMd5(mJsPaths[scriptName]);
                mJsMd5s.Add(scriptName, md5);
                LoadJsObj(scriptName);
                return;
            }

            LoadJsObj(scriptName);
        }

        private void DoFileByDevice(string scriptName)
        {
            if (!mJsScripts.ContainsKey(scriptName))
            {
                Debug.LogError(string.Format("目标脚本{0}不存在，请检查脚本目录！", scriptName));
                return;
            }

            if (mLoadedScripts.Contains(scriptName)) return;

            var code = mJsScripts[scriptName];
            mJsEngine.Execute(code);
            mLoadedScripts.Add(scriptName);
        }

        private void LoadJsObj(string scriptName)
        {
            var code = File.ReadAllText(mJsPaths[scriptName]);
            mJsEngine.Execute(code);
        }

        public void DoString(string code)
        {
#if UNITY_EDITOR || DEBUG
            try
            {
#endif
                mJsEngine.Execute(code);
#if UNITY_EDITOR || DEBUG
            }
            catch (JavaScriptException jsException)
            {
                LogError(string.Format("执行Js语句时发生错误，错误提示为{0}，源码为{1}",
                   jsException.Error, code));
            }
#endif
        }

        #endregion

        #region 执行环境初始化

        private void InitJsContext()
        {
            InjectSocketInterchanger();
            InjectModuleMethods();
        }

        /// <summary>
        /// 初始化基础执行环境
        /// </summary>
        private void InitBaseContext()
        {
            mJsEngine.Execute("var exports = {};");
            mJsEngine.Global.FastAddProperty("require", new ClrFunctionInstance(mJsEngine, Require, 1), true, false, true);
            InJectDebuger();
        }

        private void InJectDebuger()
        {
            mJsEngine.SetValue("Debuger_Log", new Action<string>(Log));
            mJsEngine.SetValue("Debuger_LogWarning", new Action<string>(LogWarning));
            mJsEngine.SetValue("Debuger_LogError", new Action<string>(LogError));
            mJsEngine.SetValue("Debuger_LogException", new Action<string>(LogException));
        }

        private readonly Action<string> Log = s => Debug.Log("[JS] " + s);
        private readonly Action<string> LogError = s => Debug.LogError("[JS] " + s);
        private readonly Action<string> LogWarning = s => Debug.LogWarning("[JS] " + s);
        private readonly Action<string> LogException = s => Debug.LogException(new Exception("[JS] " + s));

        private void InitJsScirpts()
        {
            //  移动设备或移动模拟
            if (U3DFrame.AppContext.DevelopContextType == DevelopContextType.Device)
            {
                var scripts = mAssetModule.LoadType<TextAsset>("Jint");
                if (scripts == null) return;

                foreach (var pair in scripts)
                {
                    mJsScripts.Add(pair.Key.AssetName, pair.Value.text);
                    mJsQueue.Enqueue(pair.Key.AssetName);
                }
            }
            else
            {
                var sons = Bootstrap.Instance.TotalSons;
                sons.ForEach(LoadBaseJavaScript);
                sons.ForEach(LoadSonJavaScript);
            }
        }

        private bool mIsLoadBase;
        private void LoadBaseJavaScript(SonProject son)
        {
            if (mIsLoadBase) return;

            var dir = son.TsBaseRelizeDir;
            if (!Directory.Exists(dir)) return;

            var paths = FileUtility.GetFilePathDictionary(dir, s => !s.Contains(".meta") && s.EndsWith(".js"))
                .FilePathDictionary;
            mJsPaths.Combin(paths);
            paths.Keys.ToList().ForEach(p => mJsQueue.Enqueue(p));
            mIsLoadBase = true;
        }

        private void LoadSonJavaScript(SonProject son)
        {
            if (!Directory.Exists(son.TsProjectBuildOutPutDir)) return;

            var paths = FileUtility
                .GetFilePathDictionary(son.TsProjectBuildOutPutDir, s => !s.Contains(".meta") && s.EndsWith(".js"))
                .FilePathDictionary;
            paths.Keys.ToList().ForEach(p => mJsQueue.Enqueue(p));
            mJsPaths.Combin(paths);
        }

        #endregion

        #region Node特性

        private JsValue Require(JsValue thisObject, JsValue[] arguments)
        {
            var inputString = TypeConverter.ToString(arguments.At(0));
            inputString = StringPrototype.TrimEx(inputString);
            var fn = inputString.Contains("/") ? inputString.Split('/').Last() : inputString;
            DoFile(fn);
            var exports = mJsEngine.GetValue("exports");
            return exports;
        }

        #endregion

        #region Socket数据交换机注入

        private void InjectSocketInterchanger()
        {
            mJsEngine.SetValue("PushSendMessage", new Action<string, object, int>(U3dSocketInterchanger.PushSendMessage));
            mJsEngine.SetValue("GetProtoByJs", new Func<string, string, object>(U3dSocketInterchanger.GetProtoByJs));
        }

        #endregion

        #region 模块方法注入

        /// <summary>
        /// 注入模块的接口方法
        /// </summary>
        private void InjectModuleMethods()
        {
            InjectEventModule();
            InjectAssetModule();
            InjectIukerPlayerPrefs();
            InjectDataModule();
            InjectLocalDataModule();
            InjectMusicMethod();
            InjectInputResponseModule();
            InjectViewMethod();
        }

        private void InjectEventModule()
        {
            var eventModule = U3DFrame.EventModule.As<U3dJintModule_Event>();
            mJsEngine.SetValue("Iuker_EventModule_WatchEventByJint",
                new Action<string, JsValue, int>(eventModule.WatchEventByJint));
            mJsEngine.SetValue("Iuker_EventModule_WatchEventByJintAsData",
                new Action<string, JsValue, int>(eventModule.WatchEventByJintAsData));
            mJsEngine.SetValue("Iuker_Event_IssueEventByJint",
                new Action<string, JsValue, object>(eventModule.IssueEventByJint));
        }

        private void InjectAssetModule()
        {
            var assetModule = U3DFrame.AssetModule;
            mJsEngine.SetValue("Iuker_AssetModule_LoadTextAsset",
                new Func<string, TextAssetRef>(assetModule.LoadTextAsset));
        }

        private void InjectLocalDataModule()
        {
            var localModule = U3DFrame.LocalDataModule.As<U3dJintModule_LocalData>();
            mJsEngine.SetValue("Iuker_LocalDataModule_GetEntityStrLines",
                new Func<string, string[]>(localModule.GetEntityStrLines));
        }

        private void InjectMusicMethod()
        {
            var musicModule = U3DFrame.MusicModule;
            mJsEngine.SetValue("Iuker_MusicModule_Play", new Action<string, bool, bool>(musicModule.Play));
            mJsEngine.SetValue("Iuker_MusicModule_Stop", new Action(musicModule.Stop));
            mJsEngine.SetValue("Iuker_MusicModule_ChangeVolume", new Action<float>(musicModule.ChangeVolume));
            mJsEngine.SetValue("Iuker_MusicModule_Pause", new Action(musicModule.Pause));
            mJsEngine.SetValue("Iuker_MusicModule_Open", new Action(musicModule.Open));
            mJsEngine.SetValue("Iuker_MusicModule_Volume", new Func<float>(GetMusicVolume));
        }

        float GetMusicVolume()
        {
            return U3DFrame.MusicModule.Volume;
        }

        private void InjectViewMethod()
        {
            var viewModule = U3DFrame.ViewModule.As<U3dJintModule_View>();
            mJsEngine.SetValue("Iuker_ViewModule_GetView", new Func<string, IView>(viewModule.GetView));
            mJsEngine.SetValue("Iuker_ViewModule_CreateView", new Action<string, string, bool>(viewModule.CreateView));
            mJsEngine.SetValue("Iuker_ViewModule_CloseView", new Action<string>(viewModule.CloseView));
            mJsEngine.SetValue("Iuker_ViewModule_GetJintView", new Func<string, JintView>(viewModule.GetJintView));
            mJsEngine.SetValue("Iuker_ViewModule_WatchViewLiefEvent", new Action<string, string, JsValue>(
                viewModule.WatchViewLifeEvent));
            mJsEngine.SetValue("Iuker_ViewModule_OpenDialog", new Action<string, string, JsValue, bool>(
                viewModule.OpenDialogByJint));
        }

        private void InjectIukerPlayerPrefs()
        {
            mJsEngine.SetValue("IukerPlayerPrefs_SetInt", new Action<string, int>(IukerPlayerPrefs.SetInt));
            mJsEngine.SetValue("IukerPlayerPrefs_GetInt", new Func<string, int>(IukerPlayerPrefs.GetInt));

            mJsEngine.SetValue("IukerPlayerPrefs_SetLong", new Action<string, long>(IukerPlayerPrefs.SetLong));
            mJsEngine.SetValue("IukerPlayerPrefs_GetLong", new Func<string, long>(IukerPlayerPrefs.GetLong));

            mJsEngine.SetValue("IukerPlayerPrefs_SetString", new Action<string, string>(IukerPlayerPrefs.SetString));
            mJsEngine.SetValue("IukerPlayerPrefs_GetString", new Func<string, string>(IukerPlayerPrefs.GetString));

            mJsEngine.SetValue("IukerPlayerPrefs_SetFloat", new Action<string, float>(IukerPlayerPrefs.SetFloat));
            mJsEngine.SetValue("IukerPlayerPrefs_GetFloat", new Func<string, float>(IukerPlayerPrefs.GetFloat));
        }

        private void InjectDataModule()
        {
            var dataModule = U3DFrame.DataModule;
            mJsEngine.SetValue("Iuker_DataModule_SetInt", new Action<string, int>(dataModule.SetInt));
            mJsEngine.SetValue("Iuker_DataModule_GetInt", new Func<string, int>(dataModule.GetInt));

            mJsEngine.SetValue("Iuker_DataModule_SetLong", new Action<string, long>(dataModule.SetLong));
            mJsEngine.SetValue("Iuker_DataModule_GetLong", new Func<string, long>(dataModule.GetLong));

            mJsEngine.SetValue("Iuker_DataModule_SetString", new Action<string, string>(dataModule.SetString));
            mJsEngine.SetValue("Iuker_DataModule_GetString", new Func<string, string>(dataModule.GetString));

            mJsEngine.SetValue("Iuker_DataModule_SetFloat", new Action<string, float>(dataModule.SetFloat));
            mJsEngine.SetValue("Iuker_DataModule_GetFloat", new Func<string, float>(dataModule.GetFloat));
        }

        private void InjectInputResponseModule()
        {
            var inputResponserModule = U3DFrame.InputResponseModule.As<U3dJintModule_InputResponser>();
            mJsEngine.SetValue("Iuker_InputResponseModule_WatchInputEventByJint",
                new Action<string, string, JsValue, int>(inputResponserModule.WatchInputEventByJint));
            mJsEngine.SetValue("Iuker_InputResponseModule_RemoveInputEventByJint",
                new Action<string, string>(inputResponserModule.RemoveInputEventByJint));

        }

        #endregion

        #region Js对象缓存及分帧加载

        //        private readonly Dictionary<string, JsValue> mJsValueDictionary = new Dictionary<string, JsValue>();
        private readonly Queue<string> mJsQueue = new Queue<string>();

        private void InitAllJsObjectByCoroutine()
        {
            if (mJsQueue.Count == 0)
            {
                U3DFrame.EventModule.RemoveAppEvent(AppEventType.LateUpdate.ToString(), InitAllJsObjectByCoroutine);
                mIsReady = true;
                InitJsManager();
                U3DFrame.TryEnterLoginView();
                return;
            }

            var name = mJsQueue.Dequeue();
            DoFile(name);
        }


        #endregion

        #region JsManager

        private JsValue mJsIuker;
        private JsValue mJsLogicManagers;
        private readonly Dictionary<string, JsValue> mJsManagers = new Dictionary<string, JsValue>();

        /// <summary>
        /// 查找名字以manager结尾的js脚本并执行Init方法
        /// </summary>
        private void InitJsManager()
        {
            var managerTypes = U3DFrame.AppContext.CsManagerTypes;

            foreach (var name in managerTypes)
            {
                var jsScriptName = name.ToLower();
                if (!Exist(jsScriptName)) continue;

                var jsManagerType = name;
                var code = string.Format("Iuker.LogicManagers.Managers.{0} = new Iuker_Project.{0}();", jsManagerType);
                DoString(code);
                if (mJsIuker == null)
                {
                    mJsIuker = GetGlobalValue("Iuker").As<JsValue>();
                    mJsLogicManagers = mJsIuker.GetJsValue("LogicManagers");
                }

                var managers = mJsLogicManagers.GetJsValue("Managers");
                var jsManager = managers.GetJsValue(jsManagerType);
                mJsManagers.Add(jsManagerType, jsManager);
                var InitFunc = jsManager.GetJsValue("Init");
                InitFunc.InvokeByThis(jsManager);
            }
        }

        #endregion
    }
}







