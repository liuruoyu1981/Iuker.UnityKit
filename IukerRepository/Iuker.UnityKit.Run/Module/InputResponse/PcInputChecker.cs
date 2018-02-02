/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/02 10:50:33
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


using UnityEngine;

namespace Iuker.UnityKit.Run.Module.InputResponse
{
    /// <summary>
    /// 桌面平台类型输入检测器
    /// </summary>
    public class PcInputChecker : IInputChecker
    {
        public IU3dInputResponseModule InputResponseModule { get; protected set; }

        private bool isPressStart;
        private bool isPress;
        private Vector3 lastMousePosition;
        private bool isDragStart;
        private bool isDrag;
        private int pressCount;
        private bool isLongPressStart;

        /// <summary>
        /// 单击或触摸触发后到目前的实际时间
        /// </summary>
        private float clickTimeAtNow;

        /// <summary>
        /// 指定时间内为止的点击触发次数
        /// </summary>
        private int clickCountAtNow;

        /// <summary>
        /// 双击生效时间单位
        /// </summary>
        private float doubleClickTime = 0.2f;

        /// <summary>
        /// 当前是否已经点击
        /// </summary>
        private bool isClicked = false;


        public IInputChecker Init(IU3dInputResponseModule inputResponseModule)
        {
            InputResponseModule = inputResponseModule;
            return this;
        }

        public void InputCheck()
        {
            if (GetMouseButtonUp(MouseClickType.Left))
            {
                InputResponseModule.IssueInputEvent(InputEventType.Click);
                //Debuger.Log("单击抬起");
            }

            if (clickCountAtNow >= 2 && clickTimeAtNow < doubleClickTime)   // 如果当前已连续单击两次且过渡时间在双击判定时间内则触发双击事件
            {
                InputResponseModule.IssueInputEvent(InputEventType.DoubleClick);
                //Debuger.Log("双击");
                clickCountAtNow = 0;
                clickTimeAtNow = 0f;
                isClicked = false;
            }

            if (GetMouseButtonDown(MouseClickType.Left))
            {
                clickCountAtNow++;
                isClicked = true;
                ClickTargetRaycastHitCheck();    // 检测是否有点击到物体
                InputResponseModule.IssueInputEvent(InputEventType.Click); // 发出单击事件
            }

            LeftUpCheck();  // 检测左键是否抬起
            DragCheck();    // 检测是否发生拖拽

            if (isClicked)
            {
                clickTimeAtNow = clickTimeAtNow + Time.deltaTime;
            }

            if (GetMouseButtonDown(MouseClickType.Left) && clickTimeAtNow >= doubleClickTime)  // 超出双击判断时间则判定为再次触发点击事件
            {
                // 检测是否有点击到物体
                ClickTargetRaycastHitCheck();
                InputResponseModule.IssueInputEvent(InputEventType.Click);
                //Debuger.Log("单击");
                clickCountAtNow = 0;
                clickTimeAtNow = 0f;
                isClicked = false;
            }
        }


        private void DragCheck()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                pressCount++;
                if (pressCount <= 10) return;

                isPress = true;

                if (pressCount >= 50 && isLongPressStart == false)
                {
                    isLongPressStart = true;
                    InputResponseModule.IssueInputEvent(InputEventType.LongPressStart);
                    //Debuger.Log("LongPressStart");
                }

                if (isPressStart == false)
                {
                    isPressStart = true;
                    lastMousePosition = Input.mousePosition;
                    InputResponseModule.IssueInputEvent(InputEventType.PressStart);
                    //Debuger.Log("PressStart");
                }
                else
                {
                    InputResponseModule.IssueInputEvent(InputEventType.Press);
                    InputResponseModule.InputStatus = InputStatus.Press;
                    if (lastMousePosition != Input.mousePosition)
                    {
                        if (isDragStart == false)
                        {
                            isDragStart = true;
                            InputResponseModule.IssueInputEvent(InputEventType.DragStart);
                            //Debuger.Log("DragStart");
                        }
                        isDrag = true;
                        InputResponseModule.IssueInputEvent(InputEventType.Drag);
                        lastMousePosition = UnityEngine.Input.mousePosition;
                    }
                }
            }
        }

        /// <summary>
        /// 非移动平台检测左键是否抬起
        /// </summary>
        private void LeftUpCheck()
        {
            if (Input.GetMouseButtonUp(0))
            {
                InputResponseModule.InputStatus = InputStatus.None;
                pressCount = 0;
                if (isPress)
                {
                    isPress = isPressStart = isLongPressStart = false;
                    pressCount = 0;
                }

                if (isDrag)
                {
                    InputResponseModule.IssueInputEvent(InputEventType.DragEnd);
                    //Debuger.Log("DragEnd");
                    isDragStart = isDrag = false;
                }
            }
        }


        /// <summary>
        /// 获取鼠标各个键位的抬起情况
        /// </summary>
        /// <param name="mouseClickType"></param>
        /// <returns></returns>
        private bool GetMouseButtonUp(MouseClickType mouseClickType)
        {
            int type = (int)mouseClickType;
            return Input.GetMouseButtonUp(type);
        }

        /// <summary>
        /// 获取鼠标各个键位的按下情况
        /// </summary>
        /// <param name="mouseClickType"></param>
        /// <returns></returns>
        private bool GetMouseButtonDown(MouseClickType mouseClickType)
        {
            int type = (int)mouseClickType;
            return Input.GetMouseButtonDown(type);
        }

        /// <summary>
        /// 当前点击目标射线检测，如果有点击到带有碰撞器的物体则记录该物体
        /// </summary>
        private void ClickTargetRaycastHitCheck()
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hitInfo;
            InputResponseModule.CurrentClick = Physics.Raycast(ray, out hitInfo) ? hitInfo.collider.gameObject : null;
        }
    }
}
