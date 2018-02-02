/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/18/2017 11:20
Email: liuruoyu1981@gmail.com
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/


using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Iuker.UnityKit.Editor.Console.Core.UnityLoggerApi;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Console.Core.Editor
{
    public class IukerConsoleEditorWindow : EditorWindow
    {
        private readonly int MAX_LOGS = 999;
        private readonly int MAX_LENGTH_MESSAGE = 999;
        private readonly int MAX_LENGTH_COLLAPSE = 999;

        // Layout variables
        private float _drawYPos;

        // Cache Variables
        private Texture2D _consoleIcon;
        private bool _hasConsoleIcon = true;
        private float _buttonWidth;
        private float _buttonHeight;
        private UnityApiEvents _unityApiEvents;
        private List<IukerLog> _cacheLog = new List<IukerLog>();


        /// <summary>
        /// 缓存日志比较器列表
        /// </summary>
        private List<bool> _cacheLogComparer = new List<bool>();


        private List<string> _stackTraceIgnorePrefixs = new List<string>();
        private IukerLogSettings _settings;
        private int _cacheLogCount = 0;

        // Toolbar Variables
        private string[] _searchStringPatterns;
        private string _searchString = "";

        // LogList Variables
        private Vector2 _logListScrollPosition;
        private int _logListSelectedMessage = -1;
        private double _logListLastTimeClicked = 0.0;
        private int _qtLogs = 0;

        // Resizer
        private float _topPanelHeight;
        private Rect _cursorChangeRect;
        private bool _isResizing = false;

        // LogDetail Variables
        private Vector2 _logDetailScrollPosition;
        private int _logDetailSelectedFrame = -1;
        private double _logDetailLastTimeClicked = 0.0;
        private IukerLog _selectedLog = null;

        // Scroll Logic
        private bool _isFollowScroll = false;
        private bool _hasScrollWheelUp = false;

        //[MenuItem("Iuker/远程控制台")]
        public static void ShowWindow()
        {
            var window = GetWindow<IukerConsoleEditorWindow>("IukerConsole");

            var consoleIcon = window.ConsoleIcon;
            window.titleContent = consoleIcon != null ? new GUIContent("IukerConsole", consoleIcon) : new GUIContent("IukerConsole");

            window._topPanelHeight = window.position.height / 2.0f;
        }

        private void OnEnable()
        {
            _stackTraceIgnorePrefixs = GetStackTraceIgnorePrefixs();
            _stackTraceIgnorePrefixs.AddRange(GetDefaultIgnorePrefixs());

            _settings = GetOrCreateSettings();
            _settings.CacheFilterLower();

            if (_unityApiEvents == null)
            {
                _unityApiEvents = UnityApiEvents.GetOrCreate();
            }

            SetDirtyLogs();
        }

        /// <summary>
        /// 重写编辑器窗口的OnDestroy生命周期事件处理函数
        /// 回收Icon图标
        /// 回收Settings脚本资源
        /// </summary>
        private void OnDestroy()
        {
            if (_consoleIcon != null)
            {
                _hasConsoleIcon = true;
                Resources.UnloadAsset(_consoleIcon);
                Resources.UnloadAsset(_settings);
                _consoleIcon = null;
                _settings = null;
            }
        }

        private void Update()
        {
            _unityApiEvents.OnBeforeCompileEvent -= SetDirtyLogs;
            _unityApiEvents.OnBeforeCompileEvent += SetDirtyLogs;
            _unityApiEvents.OnAfterCompileEvent -= SetDirtyLogs;
            _unityApiEvents.OnAfterCompileEvent += SetDirtyLogs;
            _unityApiEvents.OnBeginPlayEvent -= SetDirtyLogs;
            _unityApiEvents.OnBeginPlayEvent += SetDirtyLogs;
            _unityApiEvents.OnStopPlayEvent -= SetDirtyLogs;
            _unityApiEvents.OnStopPlayEvent += SetDirtyLogs;
        }

        private void InitVariables()
        {
            _buttonWidth = position.width;
            _buttonHeight = IukerConsoleSkin.MessageStyle.CalcSize(new GUIContent("Test")).y + 15.0f;
            _drawYPos = 0f;
        }

        private float DrawTopToolbar()
        {
            float height = IukerConsoleSkin.ToolbarButtonStyle.CalcSize(new GUIContent("Clear")).y;
            GUILayout.BeginHorizontal(IukerConsoleSkin.ToolbarStyle, GUILayout.Height(height));

            if (GetButtonClamped("Clear", IukerConsoleSkin.ToolbarButtonStyle))
            {
                UnityLoggerServer.Clear();
                SetDirtyLogs();
                _logListSelectedMessage = -1;
                _logDetailSelectedFrame = -1;
            }

            GUILayout.Space(6.0f);  //  在Clear按钮旁边插入一个6像素的空白

            bool oldCollapse = UnityLoggerServer.HasFlag(ConsoleWindowFlag.Collapse);   //  获得当前的折叠按钮开关状态
            // 尝试获取折叠按钮的开关状态
            bool newCollapse = GetToggleClamped(oldCollapse, "Collapse", IukerConsoleSkin.ToolbarButtonStyle);
            if (oldCollapse != newCollapse) //  如果折叠日志按钮被点击
            {
                SetDirtyLogs();
            }
            UnityLoggerServer.SetFlag(ConsoleWindowFlag.Collapse, newCollapse); //  更新折叠按钮的状态 

            bool oldClearOnPlay = UnityLoggerServer.HasFlag(ConsoleWindowFlag.ClearOnPlay);
            bool newClearOnPlay = GetToggleClamped(oldClearOnPlay, "Clear on Play",
                IukerConsoleSkin.ToolbarButtonStyle);
            UnityLoggerServer.SetFlag(ConsoleWindowFlag.ClearOnPlay, newClearOnPlay);

            bool oldPauseOnError = UnityLoggerServer.HasFlag(ConsoleWindowFlag.ErrorPause);
            bool newPauseOnError = GetToggleClamped(oldPauseOnError, "Pause on Error", IukerConsoleSkin.ToolbarButtonStyle);
            UnityLoggerServer.SetFlag(ConsoleWindowFlag.ErrorPause, newPauseOnError);

            GUILayout.FlexibleSpace();  //  插入一个可变空白

            // 搜索区域
            var oldString = _searchString;
            _searchString = EditorGUILayout.TextArea(_searchString,
                IukerConsoleSkin.ToolbarSearchTextFieldStyle,
                GUILayout.Width(200.0f));
            if (_searchString != oldString)
            {
                SetDirtyComparer(); //  
            }

            if (GUILayout.Button("", IukerConsoleSkin.ToolbarSearchCancelButtonStyle))  //  紧跟着搜索字段区域绘制一个X形状按钮
            {
                _searchString = "";
                SetDirtyComparer();
                GUI.FocusControl(null); //  还原键盘焦点
            }

            _searchStringPatterns = _searchString.Trim().ToLower().Split(' ');

            GUILayout.Space(10.0f);

            // Info/Warning/Error buttons Area
            int qtNormalLogs = 0, qtWarningLogs = 0, qtErrorLogs = 0;
            //  通过反射获取普通、警告及错误的日志数量
            UnityLoggerServer.GetCount(ref qtNormalLogs, ref qtWarningLogs, ref qtErrorLogs);

            int maxLogs = MAX_LOGS;
            string qtNormalLogsStr = qtNormalLogs.ToString();
            if (qtNormalLogs >= maxLogs)    //  如果普通日志的数量超过了日志最大数量则显示为最大数量+
            {
                qtNormalLogsStr = maxLogs + "+";
            }

            string qtWarningLogsStr = qtWarningLogs.ToString();
            if (qtWarningLogs >= maxLogs)
            {
                qtWarningLogsStr = maxLogs + "+";
            }

            string qtErrorLogsStr = qtErrorLogs.ToString();
            if (qtErrorLogs >= maxLogs)
            {
                qtErrorLogsStr = maxLogs + "+";
            }

            // 普通日志显示控制
            bool oldlsShowNormal = UnityLoggerServer.HasFlag(ConsoleWindowFlag.LogLevelLog);
            bool newlsShowNormal = GetToggleClamped(oldlsShowNormal, GetInfoGUIContent(qtNormalLogsStr),
                IukerConsoleSkin.ToolbarButtonStyle);
            if (oldlsShowNormal != newlsShowNormal)
            {
                SetDirtyLogs();
            }
            UnityLoggerServer.SetFlag(ConsoleWindowFlag.LogLevelLog, newlsShowNormal);

            // 警告日志显示控制
            bool oldIsShowWarning = UnityLoggerServer.HasFlag(ConsoleWindowFlag.LogLevelWarning);
            bool newIsShowWarning = GetToggleClamped(oldIsShowWarning,
                GetWarningGUIContent(qtWarningLogsStr),
                IukerConsoleSkin.ToolbarButtonStyle);
            if (oldIsShowWarning != newIsShowWarning)
                SetDirtyLogs();
            UnityLoggerServer.SetFlag(ConsoleWindowFlag.LogLevelWarning, newIsShowWarning);

            // 错误日志显示控制
            bool oldIsShowError = UnityLoggerServer.HasFlag(ConsoleWindowFlag.LogLevelError);
            bool newIsShowError = GetToggleClamped(oldIsShowError, GetErrorGUIContent(qtErrorLogsStr),
                IukerConsoleSkin.ToolbarButtonStyle);
            if (oldIsShowError != newIsShowError)
            {
                SetDirtyLogs();
            }
            UnityLoggerServer.SetFlag(ConsoleWindowFlag.LogLevelError, newIsShowError);

            GUILayout.EndHorizontal();

            return height;
        }

        private void OnGUI()
        {
            InitVariables();

            DrawResizer();

            _hasScrollWheelUp = Event.current.type == EventType.ScrollWheel && Event.current.delta.y < 0f;  //  delta（增量）

            GUILayout.BeginVertical(GUILayout.Height(_topPanelHeight), GUILayout.MinHeight(MinHeightOfTopAndBottom));

            _drawYPos += DrawTopToolbar();
            _drawYPos += DrawTopToolbal_Remote();
            _drawYPos -= 1f;
            DrawLogList();  //  绘制日志列表

            GUILayout.EndVertical();

            GUILayout.Space(ResizerHeight);

            GUILayout.BeginVertical(GUILayout.Height(WindowHeight - _topPanelHeight - ResizerHeight));
            _drawYPos = _topPanelHeight + ResizerHeight;
            DrawLogDetail();    //  绘制选中日志的日志详情

            GUILayout.EndVertical();

            Repaint();
        }

        private string mServerIp;

        /// <summary>
        /// 绘制顶部的远程日志工具栏
        /// </summary>
        private float DrawTopToolbal_Remote()
        {
            float height = IukerConsoleSkin.ToolbarButtonStyle.CalcSize(new GUIContent("Remote")).y;
            GUILayout.BeginHorizontal(IukerConsoleSkin.ToolbarStyle, GUILayout.Height(height));

            bool oldRemote = UnityLoggerServer.HasFlag(ConsoleWindowFlag.Remote);
            bool newRemote = GetToggleClamped(oldRemote, "Remote", IukerConsoleSkin.ToolbarButtonStyle);
            UnityLoggerServer.SetFlag(ConsoleWindowFlag.Remote, newRemote);

            GUILayout.Space(6f);

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            return height;
        }

        private void DrawResizer()
        {
            var resizerY = _topPanelHeight;

            _cursorChangeRect = new Rect(0, resizerY - 2f, position.width, ResizerHeight + 3f);
            var cursorChangeCenterRect = new Rect(0, resizerY, position.width, 1.0f);

            if (IsRepaintEvent)
            {
                IukerConsoleSkin.BoxStyle.Draw(cursorChangeCenterRect, false, false, false, false);
            }
            EditorGUIUtility.AddCursorRect(_cursorChangeRect, MouseCursor.ResizeVertical);

            if (IsClicked(_cursorChangeRect))
            {
                _isResizing = true;
            }
            else if (Event.current.rawType == EventType.MouseUp)
            {
                _isResizing = false;
            }

            if (_isResizing)
            {
                _topPanelHeight = Event.current.mousePosition.y;
                _cursorChangeRect.Set(_cursorChangeRect.x, resizerY, _cursorChangeRect.width, _cursorChangeRect.height);
            }

            _topPanelHeight = Mathf.Clamp(_topPanelHeight, MinHeightOfTopAndBottom,
                position.height - MinHeightOfTopAndBottom);
        }


        private void SetDirtyLogs()
        {
            _cacheLog.Clear();
            _cacheLogCount = 0;
            SetDirtyComparer();
        }

        private void SetDirtyComparer() => _cacheLogComparer.Clear();

        /// <summary>
        /// 绘制日志列表
        /// </summary>
        private void DrawLogList()
        {
            //  通过反射调用原生控制台的StartGettingEntries方法来获取当前日志列表并返回日志数量 
            _qtLogs = UnityLoggerServer.StartGettingLogs();
            _cacheLogCount = _qtLogs;

            int cntLogs = 0;
            List<int> rows = new List<int>(_qtLogs);
            List<IukerLog> logs = new List<IukerLog>(_qtLogs);
            for (int i = 0; i < _qtLogs; i++)
            {
                var log = GetSimpleLog(i);
                if (HasPattern(log, i))
                {
                    cntLogs++;
                    rows.Add(i);
                    logs.Add(log);
                }
            }

            _qtLogs = cntLogs;

            float windowWidth = WindowWidth;
            float windowHeight = _topPanelHeight - DrawYPos;

            float buttonWidth = ButtonWidth;
            if (_qtLogs * ButtonHeight > windowHeight)
            {
                buttonWidth -= 15f;
            }

            float viewWidth = buttonWidth;
            float viewHeight = _qtLogs * ButtonHeight;

            Rect scrollViewPosition = new Rect(0f, DrawYPos, windowWidth, windowHeight);
            Rect scrollViewRect = new Rect(0f, 0f, viewWidth, viewHeight);

            GUI.DrawTexture(scrollViewPosition, IukerConsoleSkin.EvenBackTexture);

            Vector2 oldScrollPosition = _logListScrollPosition;
            _logListScrollPosition = GUI.BeginScrollView(scrollViewPosition, _logListScrollPosition, scrollViewRect);

            int firstRenderLogIndex = (int)(_logListScrollPosition.y / ButtonHeight);
            firstRenderLogIndex = Mathf.Clamp(firstRenderLogIndex, 0, _qtLogs);

            int lastRenderLogIndex = firstRenderLogIndex + (int)(windowHeight / ButtonHeight) + 2;
            lastRenderLogIndex = Mathf.Clamp(lastRenderLogIndex, 0, _qtLogs);

            float buttonY = firstRenderLogIndex * ButtonHeight;
            bool hasSomeClick = false;

            int cnt = 0;
            for (int i = firstRenderLogIndex; i < lastRenderLogIndex; i++)
            {
                var row = rows[i];
                var log = logs[i];
                var styleBack = GetLogBackStyle(i);

                var styleMessage = GetLogListStyle(log);
                string showMessage = GetTruncatedMessage(GetLogListMessage(log));
                var contentMessage = new GUIContent(showMessage);
                var rectMessage = new Rect(x: 0,
                    y: buttonY,
                    width: viewWidth,
                    height: ButtonHeight);
                bool isSelected = i == _logListSelectedMessage ? true : false;
                DrawBack(rectMessage, styleBack, isSelected);
                if (IsRepaintEvent)
                    styleMessage.Draw(rectMessage, contentMessage, false, false, isSelected, false);

                bool messageClicked = IsClicked(rectMessage);   //  是否发生了鼠标点击事件
                bool isLeftClick = messageClicked && Event.current.button == 0; //  是否为左键点击

                if (UnityLoggerServer.HasFlag(ConsoleWindowFlag.Collapse))
                {
                    int quantity = UnityLoggerServer.GetLogCount(row);
                    var collapseCount = Mathf.Min(quantity, MAX_LENGTH_COLLAPSE);
                    var collapseText = collapseCount.ToString();
                    if (collapseCount >= MAX_LENGTH_COLLAPSE)
                        collapseText += "+";
                    var collapseContent = new GUIContent(collapseText);
                    var collapseSize = IukerConsoleSkin.CollapseStyle.CalcSize(collapseContent);

                    var collapseRect = new Rect(x: viewWidth - collapseSize.x - 5f,
                        y: (buttonY + buttonY + ButtonHeight - collapseSize.y) * 0.5f,
                        width: collapseSize.x,
                        height: collapseSize.y);

                    GUI.Label(collapseRect, collapseContent, IukerConsoleSkin.CollapseStyle);
                }

                if (messageClicked)
                {
                    _selectedLog = GetCompleteLog(row);

                    hasSomeClick = true;

                    if (!isLeftClick && i == _logListSelectedMessage)
                        DrawPopup(Event.current, log);

                    if (isLeftClick && i == _logListSelectedMessage)
                    {
                        if (IsDoubleClickLogListButton)
                        {
                            _logListLastTimeClicked = 0.0f;
                            var completeLog = GetCompleteLog(row);
                            JumpToSource(completeLog, 0);
                        }
                        else
                        {
                            PingLog(_selectedLog);
                            _logListLastTimeClicked = EditorApplication.timeSinceStartup;
                        }
                    }
                    else
                    {
                        PingLog(_selectedLog);
                        _logListSelectedMessage = i;
                    }

                    _logDetailSelectedFrame = -1;
                }

                buttonY += ButtonHeight;
                cnt++;
            }

            UnityLoggerServer.StopGettingsLogs();

            GUI.EndScrollView();

            if (_hasScrollWheelUp || hasSomeClick)
            {
                _isFollowScroll = false;
            }
            else if (_logListScrollPosition != oldScrollPosition)
            {
                _isFollowScroll = false;
                float topOffset = viewHeight - windowHeight;
                if (_logListScrollPosition.y >= topOffset)
                    _isFollowScroll = true;
            }

            if (!IsFollowScroll)
                return;

            float endY = viewHeight - windowHeight;
            _logListScrollPosition.y = endY;
        }

        /// <summary>
        /// 绘制选中的日志的细节
        /// </summary>
        private void DrawLogDetail()
        {
            var windowHeight = WindowHeight - DrawYPos;

            {
                var rect = new Rect(x: 0, y: DrawYPos, width: WindowWidth, height: windowHeight);
                GUI.DrawTexture(rect, IukerConsoleSkin.EvenBackTexture);
            }

            if (_logListSelectedMessage == -1 ||
                _qtLogs == 0 ||
                _logListSelectedMessage >= _qtLogs ||
                _selectedLog == null ||
                _selectedLog.StackTrace == null)
            {
                return;
            }

            var log = _selectedLog;

            var size = log.StackTrace.Count;
            var sizePlus = size + 1;

            float buttonHeight = GetDetailMessageHeight("A", MessageDetailCallstackStyle);
            float buttonWidth = ButtonWidth;
            float firstLogHeight = Mathf.Max(buttonHeight, GetDetailMessageHeight(GetTruncatedMessage(log.Message),
                MessageDetailFirstLogStyle,
                buttonWidth));

            float viewHeight = size * buttonHeight + firstLogHeight;

            if (viewHeight > windowHeight)
                buttonWidth -= 15f;

            // Recalculate it because we decreased the buttonWdith
            firstLogHeight = Mathf.Max(buttonHeight, GetDetailMessageHeight(GetTruncatedMessage(log.Message),
                MessageDetailFirstLogStyle,
                buttonWidth));
            viewHeight = size * buttonHeight + firstLogHeight;

            float viewWidth = buttonWidth;

            Rect scrollViewPosition = new Rect(x: 0f, y: DrawYPos, width: WindowWidth, height: windowHeight);
            Rect scrollViewViewRect = new Rect(x: 0f, y: 0f, width: viewWidth, height: viewHeight);

            _logDetailScrollPosition = GUI.BeginScrollView(position: scrollViewPosition,
                scrollPosition: _logDetailScrollPosition,
                viewRect: scrollViewViewRect);

            // Return if has nothing to show
            if (_logListSelectedMessage == -1 || _qtLogs == 0 || _logListSelectedMessage >= _qtLogs)
            {
                GUI.EndScrollView();
                return;
            }

            float scrollY = _logDetailScrollPosition.y;

            int firstRenderLogIndex = 0;
            if (scrollY <= firstLogHeight)
                firstRenderLogIndex = 0;
            else
                firstRenderLogIndex = (int)((scrollY - firstLogHeight) / buttonHeight) + 1;
            firstRenderLogIndex = Mathf.Clamp(firstRenderLogIndex, 0, sizePlus);

            int lastRenderLogIndex = 0;
            if (firstRenderLogIndex == 0)
            {
                float offsetOfFirstLog = firstLogHeight - scrollY;
                if (windowHeight > offsetOfFirstLog)
                    lastRenderLogIndex = firstRenderLogIndex + (int)((windowHeight - offsetOfFirstLog) / buttonHeight) + 2;
                else
                    lastRenderLogIndex = 2;
            }
            else
            {
                lastRenderLogIndex = firstRenderLogIndex + (int)((windowHeight / buttonHeight)) + 2;
            }
            lastRenderLogIndex = Mathf.Clamp(lastRenderLogIndex, 0, sizePlus);

            float buttonY = 0f;
            if (firstRenderLogIndex > 0)
                buttonY = firstLogHeight + (firstRenderLogIndex - 1) * buttonHeight;

            // Logging first message
            if (firstRenderLogIndex == 0)
            {
                var styleBack = GetLogBackStyle(0);
                var styleMessage = MessageDetailFirstLogStyle;
                var rectButton = new Rect(x: 0, y: buttonY, width: viewWidth, height: firstLogHeight);

                var isSelected = _logDetailSelectedFrame == -2 ? true : false;
                var contentMessage = new GUIContent(GetTruncatedMessage(log.Message));

                DrawBack(rectButton, styleBack, isSelected);
                if (IsRepaintEvent)
                    styleMessage.Draw(rectButton, contentMessage, false, false, isSelected, false);

                bool messageClicked = IsClicked(rectButton);
                if (messageClicked)
                {
                    bool isLeftClick = Event.current.button == 0;

                    if (!isLeftClick && _logDetailSelectedFrame == -2)
                        DrawPopup(Event.current, log);

                    if (isLeftClick && _logDetailSelectedFrame == -2)
                    {
                        if (IsDoubleClickLogDetailButton)
                        {
                            _logDetailLastTimeClicked = 0.0f;
                            JumpToSource(log, 0);
                        }
                        else
                        {
                            _logDetailLastTimeClicked = EditorApplication.timeSinceStartup;
                        }
                    }
                    else
                    {
                        _logDetailSelectedFrame = -2;
                    }
                }

                buttonY += firstLogHeight;
            }

            for (int i = firstRenderLogIndex == 0 ? 0 : firstRenderLogIndex - 1; i + 1 < lastRenderLogIndex; i++)
            {
                var contentMessage = new GUIContent(GetTruncatedMessage(log.StackTrace[i].FrameInformation));

                var styleBack = GetLogBackStyle(0);
                var styleMessage = MessageDetailCallstackStyle;
                var rectButton = new Rect(x: 0, y: buttonY, width: viewWidth, height: buttonHeight);

                var isSelected = i == _logDetailSelectedFrame ? true : false;
                DrawBack(rectButton, styleBack, isSelected);
                if (IsRepaintEvent)
                    styleMessage.Draw(rectButton, contentMessage, false, false, isSelected, false);

                bool messageClicked = IsClicked(rectButton);
                if (messageClicked)
                {
                    bool isLeftClick = Event.current.button == 0;

                    if (isLeftClick && i == _logDetailSelectedFrame)
                    {
                        if (IsDoubleClickLogDetailButton)
                        {
                            _logDetailLastTimeClicked = 0.0f;
                            JumpToSource(log, i);
                        }
                        else
                        {
                            _logDetailLastTimeClicked = EditorApplication.timeSinceStartup;
                        }
                    }
                    else
                    {
                        _logDetailSelectedFrame = i;
                    }
                }

                buttonY += buttonHeight;
            }

            GUI.EndScrollView();
        }

        /// <summary>
        /// 绘制一个弹出菜单
        /// </summary>
        /// <param name="clickEvent"></param>
        /// <param name="log"></param>
        private void DrawPopup(Event clickEvent, IukerLog log)
        {
            void CopyCallbak()
            {
                EditorGUIUtility.systemCopyBuffer = log.Message;
            }

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Copy"), false, CopyCallbak);
            menu.ShowAsContext();

            clickEvent.Use();
        }


        private void DrawBack(Rect rect, GUIStyle style, bool isSelected)
        {
            if (IsRepaintEvent)
            {
                style.Draw(rect, false, false, isSelected, false);
            }
        }

        /// <summary>
        /// 给定一个矩形区域并返回该区域内是否发生了鼠标点击事件的判断结果
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private bool IsClicked(Rect rect) => Event.current.type == EventType.MouseDown &&
                                             rect.Contains(Event.current.mousePosition);


        private IukerLog GetSimpleLog(int row)
        {
            int realCount = _cacheLog.Count;

            if (row < _cacheLogCount && row < realCount)
            {
                return _cacheLog[row];
            }

            var log = UnityLoggerServer.GetSimpleLog(row);

            if (realCount > row)
            {
                _cacheLog[row] = log;
            }
            else
            {
                _cacheLog.Add(log);
            }

            return _cacheLog[row];
        }

        private IukerLog GetCompleteLog(int row)
        {
            var log = UnityLoggerServer.GetCompleteLog(row);
            log.FilterStackTrace(_stackTraceIgnorePrefixs);
            return log;
        }

        private void PingLog(IukerLog log)
        {
            if (log.InstanceID != 0)
            {
                EditorGUIUtility.PingObject(log.InstanceID);
            }
        }


        #region Gets

        private float GetMaxDetailMessageHeight(
            float normalHeight)
        {
            return normalHeight * 5f;
        }

        private string GetTruncatedMessage(
            string message)
        {
            if (message.Length <= MAX_LENGTH_MESSAGE)
                return message;

            return string.Format("{0}... <truncated>", message.Substring(startIndex: 0, length: MAX_LENGTH_MESSAGE));
        }

        private string GetLogListMessage(
            IukerLog log)
        {
            return log.Message.Replace(System.Environment.NewLine, " ");
        }

        private GUIStyle GetLogBackStyle(
            int row)
        {
            return row % 2 == 0 ? IukerConsoleSkin.EvenBackStyle : IukerConsoleSkin.OddBackStyle;
        }

        private GUIStyle GetLogListStyle(
            IukerLog log)
        {
            IukerLogType logType = GetLogType(log);
            switch (logType)
            {
                case IukerLogType.Normal:
                    return IukerConsoleSkin.LogInfoStyle;
                case IukerLogType.Warning:
                    return IukerConsoleSkin.LogWarnStyle;
                case IukerLogType.Error:
                    return IukerConsoleSkin.LogErrorStyle;
            }
            return IukerConsoleSkin.LogInfoStyle;
        }

        private float GetListMessageWidth(
            string message,
            GUIStyle style)
        {
            return style.CalcSize(new GUIContent(message)).x;
        }

        private float GetDetailMessageWidth(
            string message,
            GUIStyle style)
        {
            return style.CalcSize(new GUIContent(message)).x;
        }

        private float GetDetailMessageHeight(
            string message,
            GUIStyle style,
            float width = 0f)
        {
            return style.CalcHeight(new GUIContent(message), width);
        }

        /// <summary>
        /// 获得指定的日志类型的图标贴图
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        private Texture2D GetIcon(
            IukerLogType logType)
        {
            switch (logType)
            {
                case IukerLogType.Normal:
                    return IukerConsoleSkin.InfoIcon;
                case IukerLogType.Warning:
                    return IukerConsoleSkin.WarningIcon;
                case IukerLogType.Error:
                    return IukerConsoleSkin.ErrorIcon;
                default:
                    return IukerConsoleSkin.InfoIcon;
            }
        }

        /// <summary>
        /// 获得指定的日志类型的小图标贴图
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        private Texture2D GetIconSmall(
            IukerLogType logType)
        {
            switch (logType)
            {
                case IukerLogType.Normal:
                    return IukerConsoleSkin.InfoIconSmall;
                case IukerLogType.Warning:
                    return IukerConsoleSkin.WarningIconSmall;
                case IukerLogType.Error:
                    return IukerConsoleSkin.ErrorIconSmall;
                default:
                    return IukerConsoleSkin.InfoIconSmall;
            }
        }

        private IukerLogType GetLogType(IukerLog log)
        {
            int mode = log.Mode;
            if (UnityLoggerServer.HasMode(mode, (ConsoleWindowMode)GetLogMask(IukerLogType.Error)))
            {
                return IukerLogType.Error;
            }
            if (UnityLoggerServer.HasMode(mode, (ConsoleWindowMode)GetLogMask(IukerLogType.Warning)))
            {
                return IukerLogType.Warning;
            }
            return IukerLogType.Normal;
        }

        private int GetLogMask(IukerLogType type)
        {
            switch (type)
            {
                case IukerLogType.Normal:
                    return 1028;
                case IukerLogType.Warning:
                    return 4736;
                default:
                    return 3148115;
            }
        }

        /// <summary>
        /// 跳转到源码
        /// </summary>
        /// <param name="log"></param>
        /// <param name="row"></param>
        private void JumpToSource(IukerLog log, int row)
        {
            var file = "";
            var line = -1;
            var frames = log.StackTrace;

            if (frames.Count == 0)
            {
                file = log.File;
                line = log.Line;
            }
            else if (row < frames.Count)
            {
                file = frames[row].File;
                line = frames[row].Line;
            }

            if (string.IsNullOrEmpty(file) || line == -1)
            {
                return;
            }

            var filename = System.IO.Path.Combine(Directory.GetCurrentDirectory(), file);
            if (File.Exists(filename))
            {
                InternalEditorUtility.OpenFileAtLineExternal(file, line);
            }
        }

        private IukerLogSettings GetOrCreateSettings()
        {
            var path = "BluConsole/IukerLogSettings";
            var settings = Resources.Load<IukerLogSettings>(path);
            return settings ?? CreateInstance<IukerLogSettings>();
        }

        private GUIContent GetInfoGUIContent(
            string text)
        {
            return new GUIContent(text, IukerConsoleSkin.InfoIconSmall);
        }

        private GUIContent GetInfoGUIContent(
            int value)
        {
            return new GUIContent(value.ToString(), IukerConsoleSkin.InfoIconSmall);
        }

        private GUIContent GetWarningGUIContent(
            string text)
        {
            return new GUIContent(text, IukerConsoleSkin.WarningIconSmall);
        }

        private GUIContent GetWarningGUIContent(
            int value)
        {
            return new GUIContent(value.ToString(), IukerConsoleSkin.WarningIconSmall);
        }

        private GUIContent GetErrorGUIContent(
            string text)
        {
            return new GUIContent(text, IukerConsoleSkin.ErrorIconSmall);
        }

        private GUIContent GetErrorGUIContent(
            int value)
        {
            return new GUIContent(value.ToString(), IukerConsoleSkin.ErrorIconSmall);
        }

        /// <summary>
        /// 使用给定的文本及GUI样式绘制一个两边有竖线的按钮
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        private bool GetButtonClamped(string text, GUIStyle style) => GUILayout.Button(text, style,
            GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));

        private bool GetToggleClamped(bool state, string text, GUIStyle style) => GUILayout.Toggle(state, text, style,
            GUILayout.MaxWidth(style.CalcSize(new GUIContent(text)).x));

        private bool GetToggleClamped(bool state, GUIContent content, GUIStyle style) => GUILayout.Toggle(state,
            content, style, GUILayout.MaxWidth(style.CalcSize(content).x));

        private bool HasPattern(IukerLog log, int row)
        {
            if (row < _cacheLogComparer.Count)
            {
                return _cacheLogComparer[row];
            }

            string messageLower = log.MessageLower;

            int size = _searchStringPatterns.Length;
            for (int i = 0; i < size; i++)
            {
                string pattern = _searchStringPatterns[i];
                if (pattern == "")
                {
                    continue;
                }

                if (!messageLower.Contains(pattern))
                {
                    SetLogComparer(row, false);
                    return false;
                }
            }

            if (UnityLoggerServer.IsDebugError(log.Mode))
            {
                SetLogComparer(row, true);
                return true;
            }

            var filters = _settings.FilterLower;
            size = _settings.FilterLower.Count;
            for (int i = 0; i < size; i++)
            {
                var filter = filters[i];

                if (!messageLower.Contains(filter))
                {
                    SetLogComparer(row, false);
                    return false;
                }
            }

            SetLogComparer(row, true);
            return true;
        }

        private void SetLogComparer(
            int row,
            bool value)
        {
            if (row < _cacheLogComparer.Count)
                _cacheLogComparer[row] = value;
            _cacheLogComparer.Add(value);
        }

        private List<IukerLogFrame> FilterLogFrames(
            List<IukerLogFrame> frames)
        {
            var filteredFrames = new List<IukerLogFrame>(frames.Count);
            foreach (var frame in frames)
            {
                bool starts = false;
                foreach (var stackTrace in _stackTraceIgnorePrefixs)
                {
                    if (frame.FrameInformation.StartsWith(stackTrace))
                    {
                        starts = true;
                        break;
                    }
                }
                if (!starts)
                    filteredFrames.Add(frame);
            }
            return filteredFrames;
        }

        private List<string> GetDefaultIgnorePrefixs() => new List<string> { "UnityEngine.Debug" };

        private List<string> GetStackTraceIgnorePrefixs()
        {
            var ret = new List<string>();
            var assembly = Assembly.GetAssembly(typeof(StackTraceIgnore));
            foreach (var type in assembly.GetTypes())
            {
                foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    if (methodInfo.GetCustomAttributes(typeof(StackTraceIgnore), true).Length > 0)
                    {
                        if (methodInfo.DeclaringType != null)
                        {
                            var key = $"{methodInfo.DeclaringType.FullName}:{methodInfo.Name}";
                            ret.Add(key);
                        }
                    }
                }
            }

            return ret;
        }

        #endregion


        #region 属性

        /// <summary>
        /// 控制台图标Icon
        /// 如果Resources目录指定路径下没有Icon图片则会返回Null
        /// </summary>
        private Texture2D ConsoleIcon
        {
            get
            {
                if (_consoleIcon == null && _hasConsoleIcon)
                {
                    string path = "BluConsole/Images/bluconsole-icon";
                    _consoleIcon = Resources.Load<Texture2D>(path);
                    if (_consoleIcon == null)
                        _hasConsoleIcon = false;
                }

                return _consoleIcon;
            }
        }

        private GUIStyle MessageDetailFirstLogStyle
        {
            get
            {
                var style = new GUIStyle(IukerConsoleSkin.MessageStyle)
                {
                    stretchWidth = true,
                    wordWrap = true
                };
                return style;
            }
        }

        private GUIStyle MessageDetailCallstackStyle
        {
            get
            {
                var style = new GUIStyle(MessageDetailFirstLogStyle) { wordWrap = false };
                return style;
            }
        }


        /// <summary>
        /// 当前事件是否为重绘事件
        /// </summary>
        private bool IsRepaintEvent => Event.current.type == EventType.Repaint;

        private bool IsFollowScroll => _isFollowScroll;

        private bool IsDoubleClickLogListButton => (EditorApplication.timeSinceStartup - _logListLastTimeClicked) < 0.3f;

        private bool IsDoubleClickLogDetailButton => (EditorApplication.timeSinceStartup - _logDetailLastTimeClicked) < 0.3f;
        private float DrawYPos => _drawYPos;

        private float WindowWidth => position.width;

        private float WindowHeight => position.height;

        private float ButtonWidth => _buttonWidth;

        private float ButtonHeight => _buttonHeight;

        private int FontSize => 12;
        private float ResizerHeight => 1.0f;
        private float MinHeightOfTopAndBottom => 60.0f;



        #endregion


    }
}