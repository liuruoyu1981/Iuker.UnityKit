/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/25 下午7:09:38
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


namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 视图行为处理请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewActionRequest<T> : IViewActionRequest<T>
    {
        public string ActionToken { get; set; }

        public IViewActionRequester<T> ActionRequester { get; private set; }

        public IViewActionRequest<T> Init(IViewActionRequester<T> requester, string actionToken, ViewScriptType viewScriptType)
        {
            ActionRequester = requester;
            ActionToken = actionToken;
            ViewScriptType = viewScriptType;
            return this;
        }

        public ViewScriptType ViewScriptType { get; private set; }
    }
}
