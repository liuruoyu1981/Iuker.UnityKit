/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/28 12:43:29
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
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Event;
using Iuker.UnityKit.Run.Base;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.InputResponse
{
    /// <summary>
    /// 默认输入响应模块
    /// </summary>
    public class DefaultU3dModule_InputResponse : AbsU3dModule, IU3dInputResponseModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.InputResponse.ToString();
            }
        }

        public override void Init(IFrame frame)
        {
            base.Init(frame);
            InitInputCheck();

            U3DFrame.EventModule.WatchU3dAppEvent(AppEventType.Update.ToString(), CallInputCheck);
        }

        private List<IInputChecker> inputCheckers;

        private void InitInputCheck()
        {
            inputCheckers = new List<IInputChecker>();
            inputCheckers.Add(new PcInputChecker().Init(this));    // 桌面平台
        }

        private void CallInputCheck()
        {
            foreach (var inputChecker in inputCheckers)
            {
                inputChecker.InputCheck();
            }
        }

        /// <summary>
        /// 输入输出事件处理器调用者字典
        /// </summary>
        private readonly Dictionary<InputEventType, ITallyEventHandlerCaller> inputEventCallerDictionary = new Dictionary<InputEventType, ITallyEventHandlerCaller>();

        /// <summary>
        /// 观察输入输出控制事件
        /// </summary>
        /// <param name="ryInputEventType">输入输出控制事件类型</param>
        /// <param name="action">事件处理委托</param>
        /// <param name="executeCount">可执行次数，默认为-1即无限执行</param>
        public void WatchInputEvent(InputEventType ryInputEventType, Action action, int executeCount = -1)
        {
            if (inputEventCallerDictionary.ContainsKey(ryInputEventType) == false)
            {
                inputEventCallerDictionary.Add(ryInputEventType, new TallyEventHandlerCaller());
                var caller = inputEventCallerDictionary[ryInputEventType];
                caller.AddHandler(action, executeCount);
            }
            else
            {
                var caller = inputEventCallerDictionary[ryInputEventType];
                caller.AddHandler(action, executeCount);
            }
        }

        public void RemoveInputEvent(InputEventType ryInputEventType, Action action)
        {
            if (inputEventCallerDictionary.ContainsKey(ryInputEventType))
            {
                inputEventCallerDictionary[ryInputEventType].RemoveHandler(action);
            }
        }

        public InputStatus InputStatus { get; set; }

        public GameObject CurrentClick { get; set; }

        /// <summary>
        /// 触发输入事件
        /// </summary>
        /// <param name="ryInputEventType"></param>
        public void IssueInputEvent(InputEventType ryInputEventType)
        {
            if (inputEventCallerDictionary.ContainsKey(ryInputEventType) == false)
            {
                return;
            }
            var caller = inputEventCallerDictionary[ryInputEventType];
            caller.CallEventHanlder();
        }


    }
}
