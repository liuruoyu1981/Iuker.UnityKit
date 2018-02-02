using System;
using System.Collections.Generic;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;

public class ReactData<T>
{
    private T mValue;
    public T Value
    {
        get { return mValue; }
        set
        {
            var type = typeof(T);
            if (type.IsValueType)
            {
                if (mValue.Equals(value)) return;
                Update(value);
            }
            else if (!ReferenceEquals(mValue, value))
            {
                Update(value);
            }
        }
    }

    private void Update(T value)
    {
        mValue = value;
        mUpdateDelegates.ForEach(del => del(mValue));
    }

    public string Key { get; private set; }
    private readonly List<Action<T>> mUpdateDelegates = new List<Action<T>>();

    private ReactData() { }

    public static ReactData<T> Create(string key, T initValue)
    {
        var instance = new ReactData<T> { Key = key, Value = initValue };
        return instance;
    }

    public void Watch(params Action<T>[] updateDels)
    {
        mUpdateDelegates.AddRange(updateDels);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class ReactDataDictionary<T>
{
    private readonly Dictionary<string, ReactData<T>> mDatas = new Dictionary<string, ReactData<T>>();

    public void SetData(string key, T data)
    {
        if (mDatas.ContainsKey(key))
        {
            mDatas[key].Value = data;
        }
        else
        {
            mDatas[key] = ReactData<T>.Create(key, data);
        }
    }

    public void WatchData(string key, params Action<T>[] updateDels)
    {
        if (!mDatas.ContainsKey(key)) return;

        mDatas[key].Watch(updateDels);
    }
}

public class NewDataModule
{
    public string ModuleName { get { return ModuleType.Data.ToString(); } }

    #region 字段

    private readonly ReactDataDictionary<int> mIntDic = new ReactDataDictionary<int>();
    private readonly ReactDataDictionary<long> mLongDic = new ReactDataDictionary<long>();
    private readonly ReactDataDictionary<float> mFloatDic = new ReactDataDictionary<float>();
    private readonly ReactDataDictionary<double> mDoubleDic = new ReactDataDictionary<double>();
    private readonly ReactDataDictionary<string> mStringDic = new ReactDataDictionary<string>();

    #endregion

    public void Init(IFrame frame)
    {
        throw new NotImplementedException();
    }

    public void SetData<T>(string key, T newData)
    {
        throw new NotImplementedException();
    }

    public void SetInt(string key, int newData)
    {
        mIntDic.SetData(key, newData);
    }

    public void SetLong(string key, long newData)
    {
        throw new NotImplementedException();
    }

    public void SetString(string key, string newData)
    {
        throw new NotImplementedException();
    }

    public void SetFloat(string key, float newData)
    {
        throw new NotImplementedException();
    }

    public void SetDouble(string key, double newData)
    {
        throw new NotImplementedException();
    }

    public int GetInt(string key)
    {
        throw new NotImplementedException();
    }

    public long GetLong(string key)
    {
        throw new NotImplementedException();
    }

    public string GetString(string key)
    {
        throw new NotImplementedException();
    }

    public float GetFloat(string key)
    {
        throw new NotImplementedException();
    }

    public double GetDouble(string key)
    {
        throw new NotImplementedException();
    }

    public void WatchInt(string key, Action<int> update, Action<int> init = null)
    {
        throw new NotImplementedException();
    }

    public void WatchString(string key, Action<string> update, Action<string> init = null)
    {
        throw new NotImplementedException();
    }

    public void WatchFloat(string key, Action<float> update, Action<float> init = null)
    {
        throw new NotImplementedException();
    }

    public void WatchData<T>(string key, Action<T> onDataChanged)
    {
        throw new NotImplementedException();
    }

    public T GetData<T>(string key) where T : class, new()
    {
        throw new NotImplementedException();
    }
}
