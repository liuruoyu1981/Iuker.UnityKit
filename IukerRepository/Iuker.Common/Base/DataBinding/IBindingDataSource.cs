/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/24 23:27
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

namespace Iuker.Common.Base.DataBinding
{
    /// <summary>
    /// 可绑定的数据源
    /// </summary>
    public interface IBindingDataSource<T>
    {
        /// <summary>
        /// 和可绑定的控件进行绑定
        /// </summary>
        /// <param name="widget"></param>
        void Binding(IBindingWidget<T> widget);

        /// <summary>
        /// 接收控件的变化
        /// </summary>
        /// <param name="widget"></param>
        void ReceiveUpdate(IBindingWidget<T> widget);

        /// <summary>
        /// 数据源所拥有的数据值
        /// </summary>
        T DataValue { get; }

        /// <summary>
        /// 提供外部观察自身的清理摧毁或回收事件
        /// 使得和自身绑定的控件可以执行相应的清理操作
        /// </summary>
        /// <param name="cleanAction"></param>
        void WatchClose(Action<IBindingDataSource<T>> cleanAction);
    }

}