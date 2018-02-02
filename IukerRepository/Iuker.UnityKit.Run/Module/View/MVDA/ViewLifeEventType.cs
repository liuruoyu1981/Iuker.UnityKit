/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 07:07:55
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
    /// 视图生命周期事件类型
    /// </summary>
    public enum ViewLifeEventType
    {
        /// <summary>
        /// 视图初始化可执行一次
        /// </summary>
        BeforeCreat = 0,

        /// <summary>
        /// 视图初始化完成可执行一次
        /// </summary>
        OnCreated,

        /// <summary>
        /// 视图隐藏可多次执行
        /// </summary>
        BeforeHide,

        /// <summary>
        /// 视图隐藏完成可多次执行
        /// </summary>
        OnHided,

        /// <summary>
        /// 视图激活可多次执行
        /// </summary>
        BeforeActive,

        /// <summary>
        /// 视图激活完成可多次执行
        /// </summary>
        OnActived,

        /// <summary>
        /// 视图关闭之前可执行一次
        /// </summary>
        BeforeClose,

        /// <summary>
        /// 视图关闭完成可执行一次
        /// </summary>
        OnClosed,
    }
}
