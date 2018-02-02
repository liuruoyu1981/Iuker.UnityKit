using System.Collections.Generic;
using System.IO;
using Iuker.Common.Utility;

namespace Iuker.Common.Base
{
#if DEBUG
    /// <summary>
    /// 配置持久化基类
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170907 15:03:31")]
    [ClassPurposeDesc("配置持久化基类", "配置持久化基类")]
#endif
    public class BasePrefs
    {
        public readonly SettingsDictionarys Settings;

        private readonly string mSavePath;

        public BasePrefs(string savePath)
        {
            mSavePath = savePath;
            if (File.Exists(mSavePath))
            {
                try
                {
                    var bytes = File.ReadAllBytes(mSavePath);
                    Settings = SerializeUitlity.DeSerialize<SettingsDictionarys>(bytes);
                }
                catch
                {
                    Settings = new SettingsDictionarys();
                }
            }
            else
            {
                Settings = new SettingsDictionarys();
            }
        }

        private void Save()
        {
            File.WriteAllBytes(mSavePath, SerializeUitlity.Serialize(Settings));
        }

        public void SetInt(string key, int value)
        {
            Settings.InternalSetInt(key, value, Save);
        }

        public int GetInt(string key)
        {
            return Settings.InternalGetInt(key);
        }

        public int GetInt(string key, int defaultValue)
        {
            return Settings.InternalGetInt(key, defaultValue);
        }

        public long GetLong(string key)
        {
            return Settings.InternalGetLong(key);
        }

        public void SetLong(string key, long value)
        {
            Settings.InternalSetLong(key, value);
        }

        public void SetFloat(string key, float value)
        {
            Settings.InternalSetFloat(key, value, Save);
        }

        public float GetFloat(string key)
        {
            return Settings.InternalGetFloat(key);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return Settings.InternalGetFloat(key, defaultValue);
        }

        public List<string> GetAllFloatKey()
        {
            return Settings.GetAllFloatKey();
        }

        public List<float> GetAllFloatValue()
        {
            return Settings.GetAllFloatValue();
        }


        public void SetString(string key, string value)
        {
            Settings.InternalSetString(key, value, Save);
        }

        public string GetString(string key)
        {
            return Settings.InternalGetString(key);
        }

        public string GetString(string key, string defaultValue)
        {
            return Settings.InternalGetString(key, defaultValue);
        }

        public List<string> GetAllStringValue()
        {
            return Settings.GetAllStringValue();
        }

        public List<string> GetAllStringKey()
        {
            return Settings.GetAllStringKey();
        }

        public void SetBool(string key, bool value)
        {
            Settings.InternalSetBool(key, value, Save);
        }

        public bool GetBool(string key)
        {
            return Settings.InternalGetBool(key);
        }

        public bool GetBool(string key, bool defaultValue)
        {
            return Settings.InternalGetBool(key, defaultValue);
        }

        public List<string> GetAllBoolKey()
        {
            return Settings.GetAllBoolKey();
        }

        public List<bool> GetAllBoValue()
        {
            return Settings.GetAllBoolValue();
        }

        public void SetObject<T>(string key, T obj)
        {
            Settings.InternalSetObject(key, obj, Save);
        }

        public T GetObject<T>(string key) where T : class, new()
        {
            return Settings.InternalGetObject<T>(key);
        }

        public bool HasKey(string key)
        {
            return Settings.InternalHasKey(key);
        }

        public void DeleteKey(string key)
        {
            Settings.InternalDeleteKey(key, Save);
        }

        public void DeleteAll()
        {
            Settings.InternalDeleteAll(Save);
        }


    }
}