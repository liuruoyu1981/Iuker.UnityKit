/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/26 下午1:44:19
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


namespace Iuker.Common.Module.Data
{
    /// <summary>
    /// 链式数据操作
    /// 实现了该接口的对象可以以链式编程的方式连续追加或者获取数据
    /// </summary>
    public interface IDataChainOperateNull<out T>
    {
        /// <summary>
        /// 源对象
        /// </summary>
        T Origin { get; }

        /// <summary>
        /// 获取一个布尔数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool? GetBool(string key);

        /// <summary>
        /// 获取一个int数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int? GetInt(string key);

        /// <summary>
        /// 获取一个float数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        float? GetFloat(string key);

        /// <summary>
        /// 获取一个double数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        double? GetDouble(string key);

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// 追加一个字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperateNull<T> AppendData(string key, string data, Action<string> onDataChanged = null);

        /// <summary>
        /// 追加一个float数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperateNull<T> AppendData(string key, float? data, Action<float?> onDataChanged = null);

        /// <summary>
        /// 追加一个double数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperateNull<T> AppendData(string key, double? data, Action<double?> onDataChanged = null);

        /// <summary>
        /// 追加一个int数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperateNull<T> AppendData(string key, int? data, Action<int?> onDataChanged = null);

        /// <summary>
        /// 追加一个布尔数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperateNull<T> AppendData(string key, bool? data, Action<bool?> onDataChanged = null);

    }

    public interface IDataChainOperate<out T>
    {
        /// <summary>
        /// 源对象
        /// </summary>
        T Origin { get; }

        /// <summary>
        /// 获取一个布尔数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool GetBool(string key);

        /// <summary>
        /// 获取一个int数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int GetInt(string key);

        /// <summary>
        /// 获取一个float数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        float GetFloat(string key);

        /// <summary>
        /// 获取一个double数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        double GetDouble(string key);

        /// <summary>
        /// 获取一个字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// 追加一个字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperate<T> AppendData(string key, string data, Action<string> onDataChanged = null);

        /// <summary>
        /// 追加一个float数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperate<T> AppendData(string key, float data, Action<float> onDataChanged = null);

        /// <summary>
        /// 追加一个double数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperate<T> AppendData(string key, double data, Action<double> onDataChanged = null);

        /// <summary>
        /// 追加一个int数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperate<T> AppendData(string key, int data, Action<int> onDataChanged = null);

        /// <summary>
        /// 追加一个布尔数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="onDataChanged"></param>
        /// <returns></returns>
        IDataChainOperate<T> AppendData(string key, bool data, Action<bool> onDataChanged = null);


    }
}
