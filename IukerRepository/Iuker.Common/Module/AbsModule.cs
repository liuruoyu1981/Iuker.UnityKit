/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2/12/2017 15:29:43
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 模块基类
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using Iuker.Common.Base.Interfaces;

namespace Iuker.Common
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public abstract class AbsModule : IModule
    {
        public abstract string ModuleName { get; }

        public abstract void Init(IFrame frame);

        public virtual bool IsReady
        {
            get { return true; }
        }

        protected virtual void RegisterEvent()
        {
        }

        protected virtual void onFrameInited()
        {
        }

        protected virtual void OnHotUpdateComplete()
        {

        }
    }
}