using System;
using System.Collections.Generic;
#if UNITY_IOS || UNITY_ANDROID
using System.Text;
#endif
using Iuker.Common;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Module.Debugger;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Enums;
using Run.Iuker.UnityKit.Run.Module.Debugger;
using UnityEngine;
using LogType = Iuker.Common.Base.Enums.LogType;

namespace Iuker.UnityKit.Run.Module.Debugger
{
    /// <summary>
    /// 默认调试器模块
    /// </summary>
    public class DefaultU3dModule_Debugger : AbsU3dModule, IU3dDebuggerModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Debugger.ToString();
            }
        }

        public override void Init(IFrame frame)
        {
            base.Init(frame);
            InitLogerDictionary();  //  读取配置初始化日志输出器字典

            InitWidthHigh();
            separatorStyle.normal.textColor = Color.black;
            UnityEngine.Application.logMessageReceived += ReceiveLog;
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), Update);
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.OnGUI.ToString(), OnGUI);

            mHeaderStyle = new GUIStyle
            {
                fontSize = 20,
                //fontStyle = FontStyle.Bold
            };
        }

        protected override void onFrameInited()
        {
            base.onFrameInited();

            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.OnGUI.ToString(), PrintTipsToScreen);
            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), Update);
        }

        private BaseU3dProfiler mBaseU3DProfiler;

        /// <summary>
        /// 将当前所有的提示信息打印到屏幕上
        /// </summary>
        private void PrintTipsToScreen()
        {
            if (!Bootstrap.Instance.IsRelease)
            {
                //  基础调试信息显示
                if (mBaseU3DProfiler != null)
                {
                    mBaseU3DProfiler.Render();
                }
            }

            OnGUI();
        }

        private float ByteToM(long byteCount)
        {
            return byteCount / (1024.0f * 1024.0f);
        }

        /// <summary>
        /// 日志输出器字典
        /// </summary>
        private readonly Dictionary<string, ILoger> mLogerDictionary = new Dictionary<string, ILoger>();

        private void InitLogerDictionary()
        {
            var logsConfig = U3DFrame.UnityAppConfig.ProjectBaseConfig.Logers;
            foreach (var owner in logsConfig)
            {
                var loger = new DefaultU3dLogger();
                loger.Init(owner);
                mLogerDictionary.Add(owner, loger);
            }
        }

        /// <summary>
        /// 获得指定创建者的日志输出器
        /// </summary>
        /// <param name="creater"></param>
        /// <returns></returns>
        public ILoger GetLoger(string creater)
        {
            if (!mLogerDictionary.ContainsKey(creater))
            {
                throw new Exception(string.Format("没有名为{0}的日志输出器！", creater));
            }

            var loger = mLogerDictionary[creater];
            return loger;
        }


        private string wirtePath;
        private byte filterLevel;
        private Rect windowRect;
        private Vector2 logSrc;
        private int width = 1280;
        private int height = 720;

        private string title;

        /// <summary>
        /// 用于在屏幕上打印的日志信息列表
        /// </summary>
        private readonly List<Log> LogScreenList = new List<Log>();

        /// <summary>
        /// 用于写入日志文件的日志信息列表
        /// </summary>
        private readonly List<Log> allLogList = new List<Log>();

        private readonly List<Log> infoLogList = new List<Log>();

        private readonly List<Log> warningLogList = new List<Log>();

        private readonly List<Log> errorLogList = new List<Log>();

        private readonly List<Log> ExceptionLogList = new List<Log>();

        private GUIStyle currentLoGuiStyle;
        private readonly GUIStyle separatorStyle = new GUIStyle();
        private Iuker.Common.Base.Enums.LogType currentShowLogType = LogType.Log;
        private Log lastLog;

        private string logServerIP = "127.0.0.1";
        private int port = 60000;



        private void InitWidthHigh()
        {
            width = Screen.width - 50;
            height = Screen.height - 50;

            windowRect = new Rect(20, 20, width, height);
        }

        public DebuggerStatus Status { get; protected set; }
        private bool isSendExceptionMail;

        private bool IsOpen { get; set; }
        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetKeyUp(KeyCode.F1))
            {
                InitWidthHigh();
                IsOpen = !IsOpen;

                if (IsOpen)
                {
                    Status = DebuggerStatus.Show;
                    U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.OnGUI.ToString(), OnGUI);
                    title = "实时控制台—日志";
                }
                else
                {
                    Status = DebuggerStatus.Hide;
                    U3DFrame.EventModule.RemoveAppEvent(AppEventType.OnGUI.ToString(), OnGUI);
                }
            }
#endif

#if UNITY_IOS || UNITY_ANDROID

            if (Input.touchCount == 5)
            {
                InitWidthHigh();
                IsOpen = !IsOpen;

                if (IsOpen)
                {
                    U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.OnGUI.ToString(), OnGUI);
                    title = "实时控制台—日志";
                }
                else
                {
                    U3DFrame.EventModule.RemoveAppEvent(AppEventType.OnGUI.ToString(), OnGUI);
                }
            }
#endif
        }

        ///// <summary>
        ///// 更新日志，并将日志写入日志文件
        ///// </summary>
        //private IEnumerator UpdateLog()
        //{
        //    if (allLogList.Count > 0)
        //    {
        //        var temp = allLogList.ToArray();
        //        foreach (var Log in temp)
        //        {
        //            allLogList.Remove(Log);
        //        }
        //    }
        //    else
        //    {
        //        yield return new WaitForSeconds(1f);
        //    }
        //}

        /// <summary>
        /// 接收日志
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        protected virtual void ReceiveLog(string logMessage, string stackTrace, UnityEngine.LogType type)
        {
            Log Log = new Log(logMessage, type.ToString().AsEnum<LogType>(), stackTrace);
            allLogList.Add(Log);
            if (allLogList.Count > 20)
            {
                allLogList.RemoveAt(0);
            }

            CacheLogByType(Log);

            if (type == UnityEngine.LogType.Exception)
            {
                SendExceptionMail(Log);
            }
        }

        private GUIStyle mHeaderStyle;

        private void CacheLogByType(Log log)
        {
            switch (log.LogType)
            {
                case LogType.Log:
                    infoLogList.Add(log);
                    break;
                case LogType.Warning:
                    warningLogList.Add(log);
                    break;
                case LogType.Error:
                    errorLogList.Add(log);
                    break;
                case LogType.Exception:
                    ExceptionLogList.Add(log);
                    break;
                case LogType.Assert:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool isSendExMail;

        private void SendExceptionMail(Log Log)
        {
#if UNITY_IOS || UNITY_ANDROID

            // 异常邮件只发送一次
            if (isSendExMail) return;

            StringBuilder sb = new StringBuilder();
            sb.Append("异常日期： " + DateTime.Now + "\r\n");
            sb.Append("调用堆栈： " + Log.StackTrace);
            isSendExMail = true;
#endif
        }

        /// <summary>
        /// 绘制日志
        /// </summary>
        protected virtual void OnGUI()
        {
            //windowRect = GUILayout.Window(0, windowRect, ConsoleWindow, title);
            //DrawLog();
        }

        private void DrawLog()
        {
            for (int i = 0; i < ExceptionLogList.Count; i++)
            {
                var Log = ExceptionLogList[i];
                GUILayout.Label(Log.LogMessage, mHeaderStyle);
            }
        }

        private void ConsoleWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, width, 20));
            DrawSonModuleTags();
            LogWindow();
        }

        /// <summary>
        /// 绘制控制台子模块分页标签按钮
        /// </summary>
        private void DrawSonModuleTags()
        {
            //GUILayout.BeginHorizontal();
            //if (GUILayout.Button("日志"))
            //{
            //    OnLogTagClick();
            //}
            //if (GUILayout.Button("性能"))
            //{
            //    OnXingTagClick();
            //}
            //if (GUILayout.Button("热命令"))
            //{
            //    OnHotCommandTagClick();
            //}
            //if (GUILayout.Button("帮助"))
            //{
            //    OnHelpTagClick();
            //}
            //if (GUILayout.Button("关闭"))
            //{
            //    OnCloseTagClick();
            //}

            //GUILayout.EndHorizontal();
        }

        private void OnXingTagClick()
        {
        }

        private void OnHotCommandTagClick()
        {
        }

        private void OnHelpTagClick()
        {
        }

        #region 日志子功能

        private void LogWindow()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("信息")) OnLogTagClick();
            if (GUILayout.Button("警告")) OnLogWarningTagClick();
            if (GUILayout.Button("错误")) OnLogErrorTagClick();
            if (GUILayout.Button("异常")) OnLogExceptionTagClick();

            GUILayout.EndHorizontal();
            DragCurrentLogList();
            DragLogStatus();
        }

        private void DragCurrentLogList()
        {
            GUILayout.BeginScrollView(logSrc, false, false, GUILayout.Width(width), GUILayout.Height(height - 150));

            currentLoGuiStyle = new GUIStyle { fontSize = 14 };
            switch (currentShowLogType)
            {
                case LogType.Error:
                    currentLoGuiStyle.normal.textColor = Color.red;
                    DrawLog(errorLogList, currentLoGuiStyle);
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    currentLoGuiStyle.normal.textColor = Color.yellow;
                    DrawLog(warningLogList, currentLoGuiStyle);
                    break;
                case LogType.Log:
                    currentLoGuiStyle.normal.textColor = Color.white;
                    DrawLog(infoLogList, currentLoGuiStyle);
                    break;
                case LogType.Exception:
                    currentLoGuiStyle.normal.textColor = Color.magenta;
                    DrawLog(ExceptionLogList, currentLoGuiStyle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GUILayout.EndScrollView();
            GUILayout.Label("_________________________________________________________________", separatorStyle);
        }

        private void DragLogStatus()
        {
            GUILayout.Label("当前显示日志等级： " + currentShowLogType);
            if (lastLog != null)
            {
                GUILayout.Label("最新日志消息： " + lastLog.LogMessage, currentLoGuiStyle);
            }
        }

        private void DrawLog(List<Log> list, GUIStyle guiStyle)
        {
            if (list == null || list.Count == 0) return;

            for (int i = list.Count - 1; i >= 0; --i)
            {
                lastLog = list[i];

                GUILayout.Label(lastLog.LogMessage, guiStyle);
            }
        }

        private void OnLogTagClick()
        {
            currentShowLogType = LogType.Log;
        }

        private void OnLogWarningTagClick()
        {
            currentShowLogType = LogType.Warning;
        }

        private void OnLogErrorTagClick()
        {
            currentShowLogType = LogType.Error;
        }

        private void OnLogExceptionTagClick()
        {
            currentShowLogType = LogType.Exception;
        }




        #endregion





















    }
}