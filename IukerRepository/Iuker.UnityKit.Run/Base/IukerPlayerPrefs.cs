using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Base
{
#if DEBUG
    /// <summary>
    /// 运行时配置持久化工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170907 15:15:01")]
    [ClassPurposeDesc("运行时配置持久化工具", "运行时配置持久化工具")]
#endif
    public static class IukerPlayerPrefs
    {
        private static string SavePath
        {
            get
            {
                return UnityEngine.Application.persistentDataPath + "/IukerPlayerPrefs.bytes";
            }
        }

        private static readonly BasePrefs _basePrefs = new BasePrefs(SavePath);

        public static void SetInt(string key, int value)
        {
            _basePrefs.SetInt(key, value);
        }

        public static int GetInt(string key)
        {
            return _basePrefs.GetInt(key);
        }

        public static int GetInt(string key, int defaultValue)
        {
            return _basePrefs.GetInt(key, defaultValue);
        }

        public static long GetLong(string key)
        {
            return _basePrefs.GetLong(key);
        }

        public static void SetLong(string key, long value)
        {
            _basePrefs.SetLong(key, value);
        }

        public static void SetFloat(string key, float value)
        {
            _basePrefs.SetFloat(key, value);
        }

        public static float GetFloat(string key)
        {
            return _basePrefs.GetFloat(key);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            return _basePrefs.GetFloat(key, defaultValue);
        }

        public static void SetString(string key, string value)
        {
            _basePrefs.SetString(key, value);
        }

        public static string GetString(string key)
        {
            return _basePrefs.GetString(key);
        }

        public static string GetString(string key, string defaultValue)
        {
            return _basePrefs.GetString(key, defaultValue);
        }

        public static void SetBool(string key, bool value)
        {
            _basePrefs.SetBool(key, value);
        }

        public static bool GetBool(string key)
        {
            return _basePrefs.GetBool(key);
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            return _basePrefs.GetBool(key, defaultValue);
        }

        public static void SetObject<T>(string key, T obj)
        {
            _basePrefs.SetObject(key, obj);
        }

        public static T GetObject<T>(string key) where T : class, new()
        {
            return _basePrefs.GetObject<T>(key);
        }

        public static bool HasKey(string key)
        {
            return _basePrefs.HasKey(key);
        }

        public static void DeleteKey(string key)
        {
            _basePrefs.DeleteKey(key);
        }

        public static void DeleteAll()
        {
            _basePrefs.DeleteAll();
        }
    }
}
