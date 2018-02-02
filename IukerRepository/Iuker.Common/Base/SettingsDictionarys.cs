/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2017/12/31 上午 11:36:56 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Utility;

namespace Iuker.Common.Base
{
    [Serializable]
    public class SettingsDictionarys
    {
        private readonly Dictionary<string, int> IntValues = new Dictionary<string, int>();
        private readonly Dictionary<string, long> LongValues = new Dictionary<string, long>();
        private readonly Dictionary<string, float> FloatValus = new Dictionary<string, float>();
        private readonly Dictionary<string, string> StringValues = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> BoolValues = new Dictionary<string, bool>();
        private readonly Dictionary<string, byte[]> ObjectValues = new Dictionary<string, byte[]>();

        public void InternalSetInt(string key, int value, Action callback = null)
        {
            if (!IntValues.ContainsKey(key))
            {
                IntValues.Add(key, value);
            }
            else
            {
                IntValues[key] = value;
            }

            if (callback != null)
            {
                callback();
            }
        }

        public void InternalSetLong(string key, long value, Action callback = null)
        {
            if (!LongValues.ContainsKey(key))
            {
                LongValues.Add(key, value);
            }
            else
            {
                LongValues[key] = value;
            }

            if (callback != null)
            {
                callback();
            }
        }

        public long InternalGetLong(string key)
        {
            return LongValues.ContainsKey(key) ? LongValues[key] : long.MaxValue;
        }

        public int InternalGetInt(string key)
        {
            return IntValues.ContainsKey(key) ? IntValues[key] : int.MaxValue;
        }

        public int InternalGetInt(string key, int defaultValue)
        {
            if (InternalGetInt(key) != int.MaxValue) return InternalGetInt(key);

            InternalSetInt(key, defaultValue);
            return defaultValue;
        }

        public void InternalSetFloat(string key, float value, Action callback = null)
        {
            if (!FloatValus.ContainsKey(key))
            {
                FloatValus.Add(key, value);
            }
            else
            {
                FloatValus[key] = value;
            }

            if (callback != null)
            {
                callback();
            }
        }

        public float InternalGetFloat(string key)
        {
            return FloatValus.ContainsKey(key) ? FloatValus[key] : float.MaxValue;
        }

        public float InternalGetFloat(string key, float defaultValue)
        {
            if (InternalGetFloat(key).EqualFloat(defaultValue)) return InternalGetFloat(key);

            InternalSetFloat(key, defaultValue);
            return defaultValue;
        }

        public List<string> GetAllFloatKey()
        {
            return FloatValus.Keys.ToList();
        }

        public List<float> GetAllFloatValue()
        {
            return FloatValus.Values.ToList();
        }

        public void InternalSetString(string key, string value, Action callback = null)
        {
            if (!StringValues.ContainsKey(key))
            {
                StringValues.Add(key, value);
            }
            else
            {
                StringValues[key] = value;
            }

            if (callback != null)
            {
                callback();
            }
        }

        public string InternalGetString(string key)
        {
            return StringValues.ContainsKey(key) ? StringValues[key] : null;
        }

        public string InternalGetString(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(defaultValue)) return null;

            if (InternalGetString(key) != null) return InternalGetString(key);
            InternalSetString(key, defaultValue);
            return defaultValue;
        }

        public List<string> GetAllStringValue()
        {
            return StringValues.Values.ToList();
        }

        public List<string> GetAllStringKey()
        {
            return StringValues.Keys.ToList();
        }

        public void InternalSetBool(string key, bool value, Action callback = null)
        {
            if (!BoolValues.ContainsKey(key))
            {
                BoolValues.Add(key, value);
            }
            else
            {
                BoolValues[key] = value;
            }

            if (callback != null)
            {
                callback();
            }
        }

        public bool InternalGetBool(string key)
        {
            return BoolValues.ContainsKey(key) && BoolValues[key];
        }

        public bool InternalGetBool(string key, bool defaultValue)
        {
            if (InternalGetBool(key)) return InternalGetBool(key);
            InternalSetBool(key, defaultValue);
            return defaultValue;
        }

        public List<string> GetAllBoolKey()
        {
            return BoolValues.Keys.ToList();
        }

        public List<bool> GetAllBoolValue()
        {
            return BoolValues.Values.ToList();
        }

        public void InternalSetObject<T>(string key, T instance, Action callback = null)
        {
            var bytes = SerializeUitlity.Serialize(instance);
            if (ObjectValues.ContainsKey(key))
            {
                ObjectValues[key] = bytes;
            }
            else
            {
                ObjectValues.Add(key, bytes);
            }
            if (callback != null)
            {
                callback();
            }
        }

        public T InternalGetObject<T>(string key) where T : class, new()
        {
            T t;

            if (ObjectValues.ContainsKey(key))
            {
                var bytes = ObjectValues[key];
                t = SerializeUitlity.DeSerialize<T>(bytes);
            }
            else
            {
                t = default(T);
            }

            return t;
        }

        public bool InternalHasKey(string key)
        {
            if (IntValues.ContainsKey(key))
            {
                return true;
            }
            if (FloatValus.ContainsKey(key))
            {
                return true;
            }
            return StringValues.ContainsKey(key) || BoolValues.ContainsKey(key);
        }

        public void InternalDeleteKey(string key, Action callbakc)
        {
            if (IntValues.ContainsKey(key))
            {
                IntValues.Remove(key);
                return;
            }
            if (FloatValus.ContainsKey(key))
            {
                FloatValus.Remove(key);
                return;
            }
            if (StringValues.ContainsKey(key))
            {
                StringValues.Remove(key);
                return;
            }
            if (BoolValues.ContainsKey(key))
            {
                BoolValues.Remove(key);
            }

            callbakc();
        }

        public void InternalDeleteAll(Action callback)
        {
            IntValues.Clear();
            FloatValus.Clear();
            StringValues.Clear();
            BoolValues.Clear();
            callback();
        }
    }
}
