using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Iuker.Common.Base;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Setting
{
    /// <summary>
    /// Unity编辑器常量序列化器，用于替代Unity自身的EditorPrefs。
    /// </summary>
#if DEBUG
    [ClassPurposeDesc("", "Unity编辑器常量序列化器，用于替代Unity自身的EditorPrefs。")]
#endif
    public static class IukerEditorPrefs
    {
        private static string SavePath
        {
            get { return Application.dataPath.Replace("Assets", "") + "ProjectSettings/IukerEditorPrefs.bytes"; }
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

        public static List<string> GetAllFloatKey()
        {
            return _basePrefs.GetAllFloatKey();
        }

        public static List<string> GetAllFloatValue()
        {
            return _basePrefs.GetAllFloatValue().Select(v => v.ToString(CultureInfo.InvariantCulture))
.ToList();
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

        public static List<string> GetAllStringValue()
        {
            return _basePrefs.GetAllStringValue();
        }

        public static List<string> GetAllStrintKey()
        {
            return _basePrefs.GetAllStringKey();
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

        public static List<string> GetAllBoolKey()
        {
            return _basePrefs.GetAllBoolKey();
        }

        public static List<string> GetAllBoolValue()
        {
            return _basePrefs.GetAllBoValue().Select(v => v.ToString()).ToList();
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