/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 7/19/2017 20:29:25 PM
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

namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 默认会话视图数据
    /// </summary>
    public class DefaultViewDiaLog : IViewDiaLog
    {
        public string DialogContent { get; private set; }
        public Action EnsureAction { get; private set; }
        public bool IsShowCancel { get; private set; }

        public IViewDiaLog Init(string content, Action ensure, bool isShowCancel = false)
        {
            DialogContent = content;
            EnsureAction = ensure;
            IsShowCancel = isShowCancel;

            return this;
        }

        public void TryExecuteEnsure()
        {
            if (EnsureAction != null)
            {
                EnsureAction();
            }
        }
    }
}