using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Iuker.Common.Module.Socket;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Ts
{
    public class ProtobufTypeScriptCreater
    {
        private Assembly mProtobufAssembly;
        private List<Type> mAllTypes;
        private List<Type> mStcTypes;
        private List<Type> mCtsTypes;
        private List<Type> mNormalTypes;

        public static void ExportTypeScriptProtubufProxy(SonProject son)
        {
            var creater = new ProtobufTypeScriptCreater();
            creater.Export(son);
        }

        private void Export(SonProject son)
        {
            mProtobufAssembly = Assembly.LoadFile(son.ProtobufMessageDllPath);
            mAllTypes = mProtobufAssembly.GetTypes().ToList();
            mStcTypes = mAllTypes.Where(t => t.Name.StartsWith("STC")).ToList();
            mCtsTypes = mAllTypes.Where(t => t.Name.StartsWith("CTS")).ToList();
            mNormalTypes = mAllTypes.Where(t => !t.Name.StartsWith("CTS") && !t.Name.StartsWith("STC")).ToList();

            //  将协议按照业务逻辑类型重新排序
            mAllTypes.Clear();
            mAllTypes.AddRange(mNormalTypes);
            mAllTypes.AddRange(mCtsTypes);
            mAllTypes.AddRange(mStcTypes);

            CreateProtoBridge(son);
            ExportTypeScriptInterface(son);
            AssetDatabase.Refresh();
        }

        private void ExportTypeScriptInterface(SonProject son)
        {
            var appender = new TypeScriptProtobufAppender(mAllTypes, son);
            appender.CreateProxyScript();
            TsProj.AddLine(son.TsProtobufInterfaceName, son, "Project/Interface").UpdateToFile(son.TsProjPath);
        }

        private void CreateProtoBridge(SonProject son)
        {
            var bridgeCreater = new TypeScriptProtobufBridgeCreater(son, mStcTypes, mAllTypes);
            bridgeCreater.CreateDeSerializeProxyScript();
        }

        private class TypeScriptProtobufAppender
        {
            private readonly StringBuilder mSb = new StringBuilder();
            private readonly List<Type> mTypes;
            private readonly SonProject mSon;
            private List<PropertyInfo> mPropertys;

            public TypeScriptProtobufAppender(List<Type> types, SonProject son)
            {
                mTypes = types;
                mSon = son;
            }

            public void CreateProxyScript()
            {
                foreach (var type in mTypes)
                {
                    mSb.AppendLine(string.Format("declare function Create{0}(): I{1};", type.Name, type.Name));
                }
                mSb.AppendLine();

                foreach (var mType in mTypes)
                {
                    mPropertys = mType.GetProperties().ToList();
                    mSb.AppendLine(string.Format("interface I{0} ", mType.Name) + "{");
                    mPropertys.ForEach(AppendFieldCode);
                    mSb.AppendLine("}");
                    mSb.AppendLine();
                }

                var path = mSon.TsProtobufInterfacePath;
                FileUtility.WriteAllText(path, mSb.ToString());
            }

            private void AppendFieldCode(PropertyInfo info)
            {
                if (info.PropertyType.IsGenericType)
                {
                    var genericTypeStr = info.PropertyType.ToString();
                    genericTypeStr = genericTypeStr.Split('[').Last();
                    genericTypeStr = genericTypeStr.Substring(0, genericTypeStr.Length - 1);
                    AppendGenericTypeFiledCode(info, genericTypeStr);
                    return;
                }

                var typeStr = info.PropertyType.ToString();
                if (typeStr.Contains("[]"))
                {
                    mSb.AppendLine(string.Format("        {0}: {1};", info.Name, GetArrayTypeStr(typeStr)));
                    return;
                }

                AppendBaseTypeFiledCode(info, typeStr);
            }

            private string GetArrayTypeStr(string sourceTypeStr)
            {
                switch (sourceTypeStr)
                {
                    case "System.Byte[]":
                        return "number[]";
                    default:
                        return null;
                }
            }

            private void AppendBaseTypeFiledCode(PropertyInfo info, string typeStr)
            {
                switch (typeStr)
                {
                    case "System.Int8":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Float":
                    case "System.Double":
                    case "System.Byte":
                        mSb.AppendLine(string.Format("    {0}: number;", info.Name));
                        break;
                    case "System.String":
                        mSb.AppendLine(string.Format("    {0}: string;", info.Name));
                        break;
                    case "System.Boolean":
                        mSb.AppendLine(string.Format("    {0}: boolean;", info.Name));
                        break;
                    default:
                        var notBaseType = typeStr.Split('.').Last();
                        mSb.AppendLine(string.Format("    {0}: I{1};", info.Name, notBaseType));
                        break;
                }
            }

            private void AppendGenericTypeFiledCode(PropertyInfo info, string typeStr)
            {
                switch (typeStr)
                {
                    case "System.Int8":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Float":
                    case "System.Double":
                    case "System.Byte":
                        mSb.AppendLine(string.Format("    {0}: IList<number>;", info.Name));
                        break;
                    case "System.String":
                        mSb.AppendLine(string.Format("    {0}: IList<string>;", info.Name));
                        break;
                    case "System.Boolean":
                        mSb.AppendLine(string.Format("    {0}: IList<boolean>;", info.Name));
                        break;
                    default:
                        var notBaseType = typeStr.Split('.').Last();
                        mSb.AppendLine(string.Format("    {0}: IList<I{1}>;", info.Name, notBaseType));
                        break;
                }
            }

        }

        private class TypeScriptProtobufBridgeCreater
        {
            private readonly StringBuilder mSb = new StringBuilder();
            private readonly SonProject mSon;
            private readonly List<Type> mStcTypes;
            //            private readonly List<Type> mCtsTypes;
            private readonly List<Type> mAllTypes;

            public TypeScriptProtobufBridgeCreater(SonProject son, List<Type> stcTypes, List<Type> allTypes)
            {
                mSon = son;
                mStcTypes = stcTypes;
                mAllTypes = allTypes;
            }

            private void AppendInjectMessageCreateFunctionCode()
            {
                mSb.AppendLine("        public void InjectMessageCreateFunction()");
                mSb.AppendLine("        {");
                foreach (var type in mAllTypes)
                {
                    mSb.AppendLine("            mJsEngine.SetValue(" + "\"" + string.Format("Create{0}", type.Name) + "\"" +
                                   string.Format(", new Func<object>({0}_ProtoConstructorFactory.Create{1}));",
                                       mSon.CompexName, type.Name));
                }
                mSb.AppendLine("        }");
                mSb.AppendLine();
            }

            public void CreateDeSerializeProxyScript()
            {
                AppendRefNameSpaceCode();
                AppendHeadCode();
                AppendFieldCode();
                AppendInitCode();
                AppendInjectMessageCreateFunctionCode();
                AppendDeSerializeCode();
                AppendFooterCode();
                FileUtility.WriteAllText(mSon.ProtoBridgePath, mSb.ToString());
            }

            private void AppendRefNameSpaceCode()
            {
                mSb.AppendLine("using System;");
                mSb.AppendLine("using Iuker.Common;");
                mSb.AppendLine("using Iuker.Common.Serialize;");
                mSb.AppendLine("using Iuker.UnityKit.Run.Base;");
                mSb.AppendLine("using Iuker.UnityKit.Run.Module.Communication;");
                mSb.AppendLine("using Iuker.UnityKit.Run.Module.JavaScript;");
                mSb.AppendLine("using Jint;");
                mSb.AppendLine();
            }

            private void AppendHeadCode()
            {
                mSb.AppendLine(string.Format("namespace {0}", mSon.ParentName));
                mSb.AppendLine("{");
                mSb.AppendLine(string.Format("    public class {0}_ProtobufBridge : IProtobufBridge", mSon.CompexName));
                mSb.AppendLine("    {");
            }

            private void AppendFieldCode()
            {
                mSb.AppendLine("        private IU3dFrame mU3dFrame;");
                mSb.AppendLine("        private ISerializer mSerializer;");
                mSb.AppendLine("        private Engine mJsEngine;");
                mSb.AppendLine();
            }

            private void AppendInitCode()
            {
                mSb.AppendLine("        public void Init(IU3dFrame frame)");
                mSb.AppendLine("        {");
                mSb.AppendLine("            mU3dFrame = frame;");
                mSb.AppendLine("            mSerializer = mU3dFrame.Serializer;");
                mSb.AppendLine("            mJsEngine = mU3dFrame.GetModule<IU3dJavaScriptModule>().Engine.As<Engine>();");
                mSb.AppendLine("        }");
                mSb.AppendLine();
            }

            private void AppendDeSerializeCode()
            {
                GetProtoIdResolver();
                mSb.AppendLine("        public object DeSerialize(int protoId, byte[] messageBytes)");
                mSb.AppendLine("        {");
                mSb.AppendLine("            switch (protoId)");
                mSb.AppendLine("            {");

                foreach (var type in mStcTypes)
                {
                    var fullName = type.FullName;
                    var protoname = type.Name;
                    var protoId = mProtoIdResolver.GetProtoId(protoname);
                    mSb.AppendLine("                case " + protoId + ":");
                    mSb.AppendLine(string.Format(
                        "                    return mSerializer.DeSerialize<{0}>(messageBytes);", fullName));
                }
                mSb.AppendLine("            default:");
                mSb.AppendLine("                return null;");

                mSb.AppendLine("            }");
                mSb.AppendLine("        }");
                mSb.AppendLine();
            }

            private IProtoIdResolver mProtoIdResolver;

            private void GetProtoIdResolver()
            {
                var typeName = mSon.ProtoIdResolverTypeName;
                var asm = Assembly.LoadFile(U3dConstants.AssemblyCSharpPath);
                var types = ReflectionUitlity.GetTypeList<IProtoIdResolver>(asm);
                var targetType = types.Find(t => t.Name == typeName);
                if (targetType == null)
                {
                    Debug.LogError(string.Format("目标子项目{0}的Protobuf协议编号解析器脚本没有找到！", mSon.CompexName));
                    return;
                }

                mProtoIdResolver = Activator.CreateInstance(targetType) as IProtoIdResolver;
                if (mProtoIdResolver == null)
                {
                    Debug.LogError(string.Format("目标子项目{0}的Protobuf协议编号解析器创建实例失败！", mSon.CompexName));
                    return;
                }

                mProtoIdResolver.Init();
            }

            private void AppendFooterCode()
            {
                mSb.AppendLine("    }");
                mSb.AppendLine("}");
            }
        }
    }
}
