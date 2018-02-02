/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/21 11:55
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

namespace Iuker.Common.DataTypes.ReactiveDatas
{
    /// <summary>
    /// 响应式值数据
    /// </summary>
    public interface IReactiveStruct<T>
    {
        /// <summary>
        /// 添加一个数据初始化事件委托
        /// </summary>
        /// <param name="onInit"></param>
        /// <param name="executeCount"></param>
        /// <returns></returns>
        IReactiveStruct<T> AddOnInitAction(Action<T> onInit, int executeCount = -1);

        /// <summary>
        /// 添加一个数据更新事件委托
        /// </summary>
        /// <param name="onUpdate"></param>
        /// <param name="executeCount"></param>
        /// <returns></returns>
        IReactiveStruct<T> AddOnUpdateAction(Action<T> onUpdate, int executeCount = -1);

        /// <summary>
        /// 获得响应式数据内部的值
        /// 如果要对内部数据赋值则需要调用Assign方法
        /// </summary>
        T Value { get; }

        /// <summary>
        /// 初始化
        /// 这将会触发响应式树的Init事件
        /// </summary>
        /// <param name="sourceValue"></param>
        /// <returns></returns>
        IReactiveStruct<T> Init(T sourceValue);

        /// <summary>
        /// 赋值这将会触发响应式数据的Update事件
        /// </summary>
        /// <param name="newData"></param>
        void Assign(T newData);

        /// <summary>
        /// 回收数据
        /// </summary>
        void Close();
    }
}