/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 08:51:13
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

using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.JavaScript;
using UnityEngine;

namespace Iuker.UnityKit.Run
{
    /// <summary>
    /// Mono组件对象基类
    /// </summary>
    public abstract class IukMono : MonoBehaviour
    {
        protected IU3dFrame U3DFrame { get; private set; }

        /// <summary>
        /// 自身挂载对象，即根元素
        /// </summary>
        public GameObject Root { get; protected set; }

        /// <summary>
        /// 附着目标及自身所依附的游戏对象
        /// </summary>
        public GameObject Attcher { get; protected set; }

        protected string JintId { get { return GetType().Name + "_jint"; } }

        public virtual void Init(GameObject root, GameObject attcher, IU3dFrame iu3DFrame)
        {
            Root = root;
            Attcher = attcher;
            U3DFrame = iu3DFrame;
            var jsModule = iu3DFrame.GetModule<IU3dJavaScriptModule>();

            if (jsModule.Exist(JintId))
            {
                //                var code = string.Format("")
            }
            else
            {
                DoLogic();
            }
        }

        protected virtual void DoLogic()
        {

        }

        private void InvokeJintMono()
        {

        }

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void OnDestroy()
        {

        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnEnable()
        {

        }
    }
}
