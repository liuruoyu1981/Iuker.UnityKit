/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/3/27 下午10:39:02
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
    /// 数据模块
    /// 放置及取出数据
    /// </summary>
    public interface IDataModule : IModule
    {
        /// <summary>
        /// 通过key放置一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        void SetData<T>(string key, T newData);

        /// <summary>
        /// 通过key设置一个新的int32数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        void SetInt(string key, int newData);

        /// <summary>
        /// 通过key设置一个新的int64数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        void SetLong(string key, long newData);

        /// <summary>
        /// 通过key设置一个新的字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        void SetString(string key, string newData);

        /// <summary>
        /// 通过key设置一个新的浮点数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        void SetFloat(string key, float newData);

        void SetDouble(string key, double newData);

        /// <summary>
        /// 通过key获取一个int32数据
        /// 如果没有指定的key所对应的数据则会返回int32的最大范围值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int GetInt(string key);

        /// <summary>
        /// 通过key获取一个int64数据
        /// 如果没有指定的key所对应的数据则会返回int64的最大范围值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long GetLong(string key);

        /// <summary>
        /// 通过key获取一个字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetString(string key);

        /// <summary>
        /// 通过key获取一个浮点数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        float GetFloat(string key);

        double GetDouble(string key);

        /// <summary>
        /// 通过key观察一个int32整型数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="update"></param>
        /// <param name="init"></param>
        void WatchInt(string key, Action<int> update, Action<int> init = null);

        /// <summary>
        /// 通过key观察一个int64整型数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="update"></param>
        /// <param name="init"></param>
        void WatchLong(string key, Action<long> update, Action<long> init = null);

        /// <summary>
        /// 通过key观察一个字符串数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="update"></param>
        /// <param name="init"></param>
        void WatchString(string key, Action<string> update, Action<string> init = null);

        /// <summary>
        /// 通过key观察一个浮点数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="update"></param>
        /// <param name="init"></param>
        void WatchFloat(string key, Action<float> update, Action<float> init = null);

        /// <summary>
        /// 通过key观察一个数据
        /// 当前数据发生改变时会通知到所有关注了该事件的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="onDataChanged"></param>
        void WatchData<T>(string key, Action<T> onDataChanged);

        /// <summary>
        /// 通过key得到一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetData<T>(string key) where T : class, new();



    }

}
