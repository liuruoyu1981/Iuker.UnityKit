using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Iuker.Common.Utility;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Ts
{
    /// <summary>
    /// Unity类型typescript接口脚本生成器
    /// </summary>
    public class TsUnityInterfaceCreater
    {
        private string mOutDir;
        private List<Type> mExportTypes = new List<Type>();

        private void AddExportType()
        {
            mExportTypes.Add(typeof(GameObject));
            //            mExportTypes.Add(typeof(Transform));
            //            mExportTypes.Add(typeof(JintButton));
        }

        public TsUnityInterfaceCreater(string outDir)
        {
            mOutDir = outDir;
            AddExportType();
        }

        public static void Export(string outDir)
        {
            var ct = new TsUnityInterfaceCreater(outDir);
            ct.ExportInterfaces();
        }

        private void ExportInterfaces()
        {
            foreach (var type in mExportTypes)
            {
                var tsInterface = new TsInterfaceSb(type, mOutDir);
                tsInterface.Export();
            }
        }

        private class TsInterfaceSb
        {
            private readonly StringBuilder mSb = new StringBuilder();
            private readonly string mOutPutDir;
            private string OutPutPath { get { return mOutPutDir + mType.Name + "_Declare.ts"; } }
            private readonly Type mType;
            private List<FieldInfo> mFields;
            private List<PropertyInfo> mPropertys;
            private List<MethodInfo> mMethods;

            public TsInterfaceSb(Type type, string outPutDir)
            {
                mType = type;
                mOutPutDir = outPutDir;
                GetAllField();
                GetAllProperty();
                GetAllMethod();
            }

            private void GetAllField()
            {
                mFields = mType.GetFields().ToList();
            }

            private void GetAllProperty()
            {
                mPropertys = mType.GetProperties(BindingFlags.Public).ToList();
            }

            private void GetAllMethod()
            {
                mMethods = mType.GetMethods().ToList();
            }

            private void AppendField(FieldInfo info)
            {
                mSb.AppendLine(string.Format("    {0}: {1};", info.Name, info.FieldType.GetTypeStr()));
            }

            private void AppendProperty(PropertyInfo info)
            {
                mSb.AppendLine(string.Format("    {0}: {1};", info.Name, info.PropertyType.GetTypeStr()));
            }

            private void AppendMethod(MethodInfo info)
            {
                mSb.AppendLine(string.Format("    {0}: {1};", info.Name, info.ReturnType.GetTypeStr()));
            }

            public void Export()
            {
                mFields.ForEach(AppendField);
                mPropertys.ForEach(AppendProperty);
                mMethods.ForEach(AppendMethod);

                FileUtility.WriteAllText(OutPutPath, mSb.ToString());
            }
        }
    }

    public static class TypeScriptExtension
    {
        public static string TsInterfaceName(this Type type)
        {
            return "I" + type.Name;
        }

        public static string GetTypeStr(this Type type)
        {
            if (type.FullName == typeof(string).FullName) return "string";
            if (type.FullName == typeof(int).FullName) return "number";
            if (type.FullName == typeof(long).FullName) return "number";
            if (type.FullName == typeof(byte).FullName) return "number";
            if (type.FullName == typeof(float).FullName) return "number";
            if (type.FullName == typeof(double).FullName) return "number";

            return type.TsInterfaceName();
        }




    }
}