/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/24 23:21
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
    /// 可绑定的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindingWidget<T>
    {
        /// <summary>
        /// 和可绑定的数据源对象进行绑定
        /// </summary>
        /// <param name="dataSource"></param>
        void Binding(IBindingDataSource<T> dataSource);

        /// <summary>
        /// 接收数据源的变化
        /// </summary>
        /// <param name="dataSource"></param>
        void ReceiveUpdate(IBindingDataSource<T> dataSource);

        /// <summary>
        /// 控件所拥有的数据值
        /// </summary>
        T WidgetValue { get; }

        /// <summary>
        /// 提供外部观察自身的清理摧毁或回收事件
        /// 使得和自身绑定的数据源可以执行相应的清理操作
        /// </summary>
        /// <param name="cleanAction"></param>
        void WatchClose(Action<IBindingWidget<T>> cleanAction);

    }
}