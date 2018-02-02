//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Iuker.Common;
//using Iuker.Common.Base;
//using Iuker.Common.Constant;
//using Iuker.Common.Utility;
//using Iuker.UnityKit.Editor.Protobuf;
//using Iuker.UnityKit.Editor.Protobuf.ResponserCreater;
//using Iuker.UnityKit.Run.Base;
//using Iuker.UnityKit.Run.Base.Config;
//using Iuker.UnityKit.Run.Base.Config.Develop;
//using ProtoBuf;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;

//namespace Iuker.UnityKit.Editor.Refactor
//{
//    public class MyProtobufCreaterWindow : EditorWindow
//    {
//        [MenuItem("Iuker/快捷菜单/Protobuf构造工厂脚本创建测试")]
//        private static void MyTest()
//        {
//            CurrentSon = RootConfig.GetCurrentSonProject();
//            GetWindow<MyProtobufCreaterWindow>();
//        }

//        #region Common

//        private const float MaxWidth = 300.0f;
//        private ProtobufClassDesc mCurrentDesc;
//        public static SonProject CurrentSon;
//        public List<ProtobufClassDesc> mProtobufDescs;
//        public List<ProtobufClassDesc> mStcProtobufDescs;
//        public List<ProtobufClassDesc> mCtsProtobufDescs;
//        public GUIStyle mHeaderStyle;
//        public int mProtoIndex;
//        public string[] mProtobufNameArray;
//        public string[] mStcProtobufNameArray;
//        public int mStcIndex;
//        public string[] mCtsProtobufNameArray;
//        public int mCtsIndex;
//        public string mProtoContent;
//        private List<string> mInternalContents;
//        private List<Type> mProtobufTypes;
//        private readonly Il8nString il8n_exportProtoFile = Il8nString.Create("IukerInspectorWindow", "一键导出", "One step create");
//        private readonly Il8nString il8n_createRequest = Il8nString.Create("IukerInspectorWindow", "创建通信请求处理脚本", "Create Communication Requester");
//        private readonly Il8nString il8n_createResponser = Il8nString.Create("IukerInspectorWindow", "创建通信答复处理脚本", "Create Communication Responser");
//        private readonly Il8nString il8n_locationScript = Il8nString.Create("IukerInspectorWindow", "定位脚本", "Location Script");

//        private bool mIsInited;
//        private void TryInit()
//        {
//            if (mIsInited) return;

//            mHeaderStyle = new GUIStyle(GUI.skin.label)
//            {
//                fontSize = 12,
//                fontStyle = FontStyle.Bold
//            };

//            var parse = new ProtobufParser();
//            mProtobufDescs = parse.InitProtocols().OrderBy(r => r.ProtoId).ToList();
//            mStcProtobufDescs = mProtobufDescs.FindAll(r => r.ProtoName.StartsWith("STC")).OrderBy(r => r.ProtoId)
//                .ToList();
//            mCtsProtobufDescs = mProtobufDescs.FindAll(r => r.ProtoName.StartsWith("CTS")).OrderBy(r => r.ProtoId)
//                .ToList();

//            mProtobufNameArray = mProtobufDescs.Select(p => p.ProtoId + "@" + p.ProtoName).ToArray();
//            mStcProtobufNameArray = mProtobufDescs.FindAll(r => r.ProtoName.StartsWith("STC")).OrderBy(r => r.ProtoId)
//                .Select(r => r.ProtoId + "@" + r.ProtoName).ToArray();    //  服务器端协议
//            mCtsProtobufNameArray = mProtobufDescs.FindAll(r => r.ProtoName.StartsWith("CTS")).OrderBy(r => r.ProtoId)
//                .Select(r => r.ProtoId + "@" + r.ProtoName).ToArray();

//            mProtoContent = !mProtobufDescs.IsNullOrZero() ? mProtobufDescs[mProtoIndex].GetProtoContent() : string
//                .Format("当前子项目{0}的协议表是空的！", RootConfig.GetCurrentSonProject().ProjectName);

//            mIsInited = true;
//        }

//        #endregion

//        #region Draw

//        private void OnGUI()
//        {
//            TryInit();

//            using (new IukHorizontalLayout())
//            {
//                using (new IukVerticalLayout(GUILayout.MinWidth(MaxWidth)))
//                {
//                    GUILayout.Space(5);
//                    DrawAllProto();
//                    DrawCtsProto();
//                    DraStcProto();
//                    DrawOneStep(); //  点一次完成所有流程
//                }
//            }

//            DrawSelectedProto();
//        }

//        private void DrawSelectedProto()
//        {
//            GUILayout.BeginVertical("IN GameObjectHeader"); // 选中协议展示
//            using (new IukHorizontalLayout())
//            {
//                using (new IukVerticalLayout(GUILayout.MaxWidth(300)))
//                {
//                    GUILayout.Label("外部协议");
//                    GUILayout.Label(new GUIContent(mProtoContent), GUILayout.MaxWidth(300));
//                }
//                if (mInternalContents != null && mInternalContents.Count > 0)
//                {
//                    foreach (var content in mInternalContents)
//                    {
//                        using (new IukVerticalLayout(GUILayout.MaxWidth(300)))
//                        {
//                            GUILayout.Label("内部嵌套协议");
//                            GUILayout.Label(new GUIContent(content), GUILayout.MaxWidth(300));
//                        }
//                    }
//                }
//            }
//            GUILayout.EndVertical();
//        }

//        private void DrawOneStep()
//        {
//            if (GUILayout.Button(il8n_exportProtoFile.Content))
//            {
//                DoOnStepByCsharp();
//                DoOnStepByTypeScript();
//            }
//        }

//        private void DoOnStepByCsharp()
//        {
//            ExportProtoFile();
//            //            CompileProtoScript();
//            CreateCsharpProtoConstructorFactoryScript();
//            //            CreateProtoClassNameConstants();
//            //            CreateProtoIdScript();
//            //            CreateProtobufSerializerProxy();
//            Close();
//        }

//        private void DoOnStepByTypeScript()
//        {

//        }


//        private void DrawAllProto()
//        {
//            using (new IukHorizontalLayout())
//            {
//                GUILayout.Label("All Protocols");
//                var oldCommonIndex = mProtoIndex;
//                mProtoIndex = EditorGUILayout.Popup(mProtoIndex, mProtobufNameArray);
//                if (oldCommonIndex != mProtoIndex)
//                {
//                    mCurrentDesc = mProtobufDescs[mProtoIndex];
//                    UpdateProtoDraw();
//                }
//            }
//        }

//        private void DrawCtsProto()
//        {
//            using (new IukHorizontalLayout())
//            {
//                GUILayout.Label("Cts Protocols");
//                var oldCtsIndex = mCtsIndex;
//                mCtsIndex = EditorGUILayout.Popup(mCtsIndex, mCtsProtobufNameArray, GUILayout.MinWidth(250));
//                if (oldCtsIndex != mCtsIndex)
//                {
//                    mCurrentDesc = mCtsProtobufDescs[mCtsIndex];
//                    UpdateProtoDraw();
//                }
//                if (GUILayout.Button(il8n_createRequest.Content))
//                {
//                    var protobufDesc = mCtsProtobufDescs[mCtsIndex];
//                    CreateRequestScript(protobufDesc);
//                }
//                if (GUILayout.Button(il8n_locationScript.Content))    //  自动定位Unity编辑器中对应的脚本目录
//                {
//                    // 查找当前选中的协议所对应的通信请求处理器脚本所在目录
//                    //                    LocationRequestScript();
//                }
//            }
//        }

//        private void DraStcProto()
//        {
//            using (new IukHorizontalLayout())
//            {
//                GUILayout.Label("Stc Protocols");
//                var oldStcIndex = mStcIndex;
//                mStcIndex = EditorGUILayout.Popup(mStcIndex, mStcProtobufNameArray, GUILayout.MinWidth(250));
//                if (oldStcIndex != mStcIndex)
//                {
//                    mCurrentDesc = mStcProtobufDescs[mStcIndex];
//                    UpdateProtoDraw();
//                }
//                if (GUILayout.Button(il8n_createResponser.Content))
//                {
//                    var protobufDesc = mStcProtobufDescs[mStcIndex];
//                    CreateResponserScript(protobufDesc);
//                }
//                if (GUILayout.Button(il8n_locationScript.Content))    //  自动定位Unity编辑器中对应的脚本目录
//                {
//                    // 查找当前选中的协议所对应的通信答复处理器脚本所在目录
//                    //                    LocationResponserScript();
//                }
//            }
//        }

//        private void CreateResponserScript(ProtobufClassDesc protoClassDesc)
//        {
//            var targetDir = CurrentSon.CsCommunicationResponsersDir + protoClassDesc.ProtobufsTable.ProtocolName + "/";
//            if (!Directory.Exists(targetDir))
//            {
//                Directory.CreateDirectory(targetDir);   //  每个协议对应一个脚本目录
//            }

//            var scriptName = protoClassDesc.ProtobufsTable.ProtocolName + "_Responser" + Constant.GetTimeToken;
//            var targetPath = targetDir + scriptName + ".cs";

//            var creater = new CommunicationResponserCreater(RootConfig.GetCurrentSonProject());
//            var content = creater.GetScriptContent(protoClassDesc, scriptName);
//            File.WriteAllText(targetPath, content);
//            AssetDatabase.Refresh();
//            InternalEditorUtility.OpenFileAtLineExternal(targetPath, 1);
//        }


//        private void UpdateProtoDraw()
//        {
//            mProtoContent = mCurrentDesc.GetProtoContent();
//            mInternalContents = mCurrentDesc.GetInternalContents();
//        }


//        #endregion

//        #region Csharp

//        private void CreateProtoIdScript()
//        {
//            var sb = new StringBuilder();
//            sb.WriteFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail, "Protobuf消息编号解析脚本\r此脚本由框架自动生成，请勿做任何手动修改！");
//            sb.AppendLine(string.Format("using {0};", CurrentSon.ProtobufNameSpace));
//            sb.AppendLine("using System.Collections.Generic;");
//            sb.AppendLine("using Iuker.Common.Module.Socket;");
//            sb.AppendLine("using UnityEngine;");
//            sb.AppendLine("using System;");
//            sb.AppendLine();
//            sb.AppendLine(string.Format("namespace {0}", CurrentSon.ParentName));
//            sb.AppendLine("{");
//            sb.AppendLine(string.Format("    public class {0} : IProtoIdResolver", CurrentSon.ProtoIdResolverName));
//            sb.AppendLine("    {");

//            sb.AppendLine("        private readonly Dictionary<string, int> mProtoIdDictionary = new Dictionary<string, int>();");
//            sb.AppendLine("        private readonly Dictionary<int, string> mProtoNameDictionary = new Dictionary<int, string>();");
//            sb.AppendLine();

//            sb.AppendLine("        public void Init()");
//            sb.AppendLine("        {");

//            sb.AppendLine("            try");
//            sb.AppendLine("            {");
//            foreach (var protobufDesc in mProtobufDescs)
//            {
//                if (protobufDesc.ProtoId != -1 && (protobufDesc.ProtoName.Contains("STC") || protobufDesc.ProtoName.Contains("CTS")) && protobufDesc.ProtoName == protobufDesc.ProtobufsTable.ProtocolName)
//                {
//                    sb.AppendLine(string.Format("                mProtoIdDictionary.Add(\"{0}\", {1});",
//                        protobufDesc.ProtoName, protobufDesc.ProtoId));
//                }
//            }
//            sb.AppendLine();
//            foreach (var protobufDesc in mProtobufDescs)
//            {
//                if (protobufDesc.ProtoId != -1 && (protobufDesc.ProtoName.Contains("STC") || protobufDesc.ProtoName.Contains("CTS")) && protobufDesc.ProtoName == protobufDesc.ProtobufsTable.ProtocolName)
//                {
//                    sb.AppendLine(string.Format("                mProtoNameDictionary.Add({0}, \"{1}\");",
//                        protobufDesc.ProtoId, protobufDesc.ProtoName));
//                }
//            }

//            sb.AppendLine("            }");
//            sb.AppendLine("            catch (Exception ex)");
//            sb.AppendLine("            {");
//            sb.AppendLine("                Debug.LogException(new Exception(ex.Message));");
//            sb.AppendLine("            }");

//            sb.AppendLine();
//            sb.AppendLine("        }");
//            sb.AppendLine();

//            sb.AppendCsharpNote("获取Protobuf协议编号", null, null, "        ");
//            sb.AppendLine("        public int GetProtoId(string protoname)");
//            sb.AppendLine("        {");
//            sb.AppendLine("            if(mProtoIdDictionary.ContainsKey(protoname))");
//            sb.AppendLine("            {");
//            sb.AppendLine("                return mProtoIdDictionary[protoname];");
//            sb.AppendLine("            }");
//            sb.AppendLine("            return int.MaxValue;");
//            sb.AppendLine("        }");

//            sb.AppendLine();

//            sb.AppendCsharpNote("获取Protobuf协议名", null, null, "        ");
//            sb.AppendLine("        public string GetProtoName(int commandId)");
//            sb.AppendLine("        {");
//            sb.AppendLine("            if(mProtoNameDictionary.ContainsKey(commandId))");
//            sb.AppendLine("            {");
//            sb.AppendLine("                return mProtoNameDictionary[commandId];");
//            sb.AppendLine("            }");
//            sb.AppendLine("            return null;");
//            sb.AppendLine("        }");

//            sb.AppendLine("    }");
//            sb.AppendLine("}");

//            var scriptContent = sb.ToString();
//            File.WriteAllText(CurrentSon.ProtoIdResolverPath, scriptContent);
//            AssetDatabase.Refresh();
//        }

//        private void CreateProtoClassNameConstants()
//        {
//            var sb = new StringBuilder();
//            sb.WriteFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail, "Protobuf消息常量脚本\r此脚本由框架自动生成，请勿做任何手动修改！");
//            var classname = CurrentSon.CompexName + "_ProtobufConstant";
//            var targetPath = CurrentSon.CsProtobufDir + classname + ".cs";

//            sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
//            sb.AppendLine("{");
//            sb.AppendLine(string.Format("    public class {0}", classname));
//            sb.AppendLine("    {");

//            foreach (var protoname in mProtobufNameArray)
//            {
//                var splitedProtoName = protoname.Split('@').Last();
//                sb.AppendLine(string.Format("        public static readonly string {0} = ", splitedProtoName) + "\"" + splitedProtoName + "\"" +
//                              ";");
//                sb.AppendLine();
//            }

//            sb.AppendLine("    }");
//            sb.AppendLine("}");

//            File.WriteAllText(targetPath, sb.ToString());
//            AssetDatabase.Refresh();
//        }

//        private void CreateRequestScript(ProtobufClassDesc protoClassDesc)
//        {
//            var targetDir = CurrentSon.CsCommunicationRequestDir + protoClassDesc.ProtobufsTable.ProtocolName + "/";
//            if (!Directory.Exists(targetDir))
//            {
//                Directory.CreateDirectory(targetDir);   //  每个协议对应一个脚本目录
//            }

//            var scriptName = protoClassDesc.ProtobufsTable.ProtocolName + "_Requester" + Constant.GetTimeToken;
//            var targetPath = targetDir + scriptName + ".cs";
//            var content = CommunicationRequesterCreater.GetScriptContent(protoClassDesc, scriptName);
//            File.WriteAllText(targetPath, content);
//            AssetDatabase.Refresh();
//            InternalEditorUtility.OpenFileAtLineExternal(targetPath, 1);
//        }

//        private void ExportProtoFile()
//        {
//            var instance = new ProtobufParser();
//            // 创建proto后缀的协议文件
//            instance.ExportProtoFile();
//            AssetDatabase.Refresh();
//            // 将生成的协议定义proto文件拷贝到protogen工具所在目录
//            var sourcePath = CurrentSon.ProtoExportPath;
//            var targetPath = U3dConstants.ProtobufModelProtogenDir + CurrentSon.ProtoFullName;
//            File.Copy(sourcePath, targetPath, true);
//        }

//        private void CompileProtoScript()
//        {
//            var switchDiskCmd = Application.dataPath.Split('/').First(); //  切换到当前Unity3D项目所在盘符的Dos命令字符串
//            var protoGenDir = U3dConstants.ProtobufModelProtogenDir;
//            var protoGenStr = string.Format("protogen -i:{0} -o:{1}", CurrentSon.ProtoFullName,
//                CurrentSon.ProtoClassPath);
//            var ProtoModelCmd = string.Format(
//                "C:/Windows/Microsoft.NET/Framework/v4.0.30319/Csc.exe /noconfig /nowarn:1701,1702 /nostdlib+ /errorreport:prompt /warn:4 /define:TRACE /reference:C:/Windows/Microsoft.NET/Framework/v2.0.50727/mscorlib.dll /reference:{0}/1_Iuker.UnityKit/IukerRepository/.Iuker.ProtobufModel/protobuf-net.dll /reference:C:/Windows/Microsoft.NET/Framework/v2.0.50727/System.dll /debug:pdbonly /filealign:512 /optimize+ /out:{1} /target:library /utf8output {2}",
//                Application.dataPath, CurrentSon.ProtobufMessageDllPath, CurrentSon.ProtoClassPath);
//            var precompileCmd = string.Format("{0}precompile.exe {1} -o:{2} -t:{3}_ProtobufSerializer",
//                U3dConstants.ProtobufModelPrecompileDir, CurrentSon.ProtobufMessageDllPath,
//                CurrentSon.ProtobufSerializerDllPath, CurrentSon.CompexName);

//            // 拷贝protobuf-net程序集文件到目标子项目的ProtobufScript目录下
//            FileUtility.TryCreateDirectory(CurrentSon.CsProtobufDir);
//            FileUtility.TryCopy(U3dConstants.ProtobufNetDllPath, CurrentSon.Protobuf_Temp_Net_DllPath);

//            if (!Directory.Exists(CurrentSon.CsProtobufDir))    //   如果protobuf协议生成脚本存放目录不存在则创建
//            {
//                Directory.CreateDirectory(CurrentSon.CsProtobufDir);
//            }
//            CmdUtility.ExcuteDosCommand
//                (
//                Debug.Log,
//                switchDiskCmd,
//                "cd " + protoGenDir,
//                protoGenStr,
//                "cd..",
//                ProtoModelCmd,
//                precompileCmd
//                );

//            // 删掉临时使用的protobuf-net.dll文件
//            File.Delete(CurrentSon.Protobuf_Temp_Net_DllPath);
//            AssetDatabase.Refresh();
//        }

//        private string mSplitedName;
//        private void CreateCsharpProtoConstructorFactoryScript()
//        {
//            var protoClassList = new List<string>();
//            protoClassList.AddRange(mProtobufNameArray);
//            var projeceAssembly = Assembly.LoadFile(CurrentSon.ProtobufMessageDllPath);
//            mProtobufTypes = ReflectionUitlity.GetTypeList<IExtensible>(projeceAssembly);

//            var sb = new StringBuilder();
//            sb.WriteFileInfo(EditorConstant.HostClientName, EditorConstant.HostClientEmail, "Protobuf消息构造函数工厂脚本\r此脚本由框架自动生成，请勿做任何手动修改！");
//            sb.AppendLine(string.Format("using {0};", CurrentSon.ProtobufNameSpace));
//            sb.AppendLine("using System.Collections.Generic;");
//            sb.AppendLine();
//            sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
//            sb.AppendLine("{");
//            sb.AppendLine(string.Format("    public class {0}", CurrentSon.ProtoConstructorFactoryName));
//            sb.AppendLine("    {");


//            foreach (var protoname in protoClassList)
//            {
//                mSplitedName = protoname.Split('@').Last();
//                var type = mProtobufTypes.Find(t => t.Name == mSplitedName);
//                if (type != null)
//                {
//                    AppendCsharpProtoConstructor(sb, mSplitedName, type);
//                }
//            }
//            sb.AppendLine("    }");
//            sb.AppendLine("}");

//            // 创建protobuf全参构建函数脚本文件
//            var scriptContent = sb.ToString();
//            File.WriteAllText(CurrentSon.ProtoConstructorFactoryPath, scriptContent);
//            AssetDatabase.Refresh();
//            InternalEditorUtility.OpenFileAtLineExternal(CurrentSon.ProtoConstructorFactoryPath, 1);
//        }

//        private void AppendCsharpProtoConstructor(StringBuilder sb, string protoname, Type type)
//        {
//            var properties = type.GetProperties();
//            AppendProtoConstructor_Argulist(sb, protoname, properties); // 追加参数列表
//            AppendProtoConstructor_Body(sb, protoname, properties); // 追加函数体协议字段赋值
//        }

//        private void AppendProtoConstructor_Argulist(StringBuilder sb, string protoname, PropertyInfo[] properties)
//        {
//            if (protoname == "CTS_L_GameConfigMsg")
//            {

//            }

//            sb.Append(string.Format("        public static {0} Create{1}(", protoname, protoname));
//            for (int i = 0; i < properties.Length; i++)
//            {
//                var propertyInfo = properties[i];
//                var argType = GetCsharpArguTypeStr(propertyInfo);
//                var arguname = propertyInfo.Name.ToLower();
//                if (i < properties.Length - 1)
//                {
//                    if (propertyInfo.PropertyType.IsGenericType)
//                    {
//                        sb.Append(string.Format("{0} {1},", argType, arguname));
//                    }
//                    else
//                    {
//                        sb.Append(string.Format("{0} {1},", argType, arguname));
//                    }
//                }
//                else
//                {
//                    sb.Append(string.Format("{0} {1})\r\n", argType, arguname));    //  最后一个参数则写入一个换行符
//                }
//            }
//        }

//        private string GetCsharpArguTypeStr(PropertyInfo propertyInfo)
//        {
//            if (propertyInfo.PropertyType.IsGenericType)
//            {
//                var tempname = propertyInfo.PropertyType.ToString();
//                var tempArray = tempname.Split('.').ToList();
//                tempname = tempArray.Last();
//                tempname = tempname.Substring(0, tempname.Length - 1);
//                if (mBaseCsharpTypeMap.ContainsKey(tempname))
//                {
//                    tempname = mBaseCsharpTypeMap[tempname]; //  列表中是基础类型则替换为IDE的小写关键字字符串
//                }
//                tempname = string.Format("List<{0}>", tempname);
//                return tempname;
//            }

//            var typename = propertyInfo.PropertyType.ToString();
//            if (mBaseCsharpTypeMap.ContainsKey(typename))
//            {
//                var swithname = mBaseCsharpTypeMap[typename];
//                return swithname;
//            }

//            return typename;
//        }

//        private readonly Dictionary<string, string> mBaseCsharpTypeMap = new Dictionary<string, string>
//        {
//            { "System.Int32","int"},
//            { "System.Boolean","bool"},
//            { "System.Int64","long"},
//            { "System.String","string"},
//            { "Int32","int"},
//            { "Boolean","bool"},
//            { "Int64","long"},
//            { "String","string"},
//        };

//        private void AppendProtoConstructor_Body(StringBuilder sb, string protoname, PropertyInfo[] properties)
//        {
//            sb.AppendLine("        {");

//            var tempInstanc = "_" + protoname.ToLower();
//            sb.AppendLine(string.Format("            var {0} = new {1}();", tempInstanc, protoname));
//            foreach (var fieldInfo in properties)
//            {
//                sb.AppendLine(fieldInfo.PropertyType.IsGenericType
//                    ? string.Format("            {0}.{1}.AddRange({2});", tempInstanc, fieldInfo.Name,
//                        fieldInfo.Name.ToLower())
//                    : string.Format("            {0}.{1} = {2};", tempInstanc, fieldInfo.Name,
//                        fieldInfo.Name.ToLower()));
//            }

//            sb.AppendLine(string.Format("            return _{0};", protoname.ToLower()));
//            sb.AppendLine("        }");
//            sb.AppendLine();
//        }

//        /// <summary>
//        /// 创建当前子项目的Protobuf序列化器代理脚本
//        /// </summary>
//        private void CreateProtobufSerializerProxy()
//        {
//            var sb = new StringBuilder();
//            var sonProject = RootConfig.GetCurrentSonProject();

//            var classname = sonProject.CompexName + "_ProtobufSerializer_Proxy";
//            var proxyClassname = sonProject.CompexName + "_ProtobufSerializer";
//            var selfNamespace = sonProject.NameSapce;
//            sb.AppendCsahrpFileInfo(RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Name,
//                RootConfig.GetSonClientCoder().Email == null ? null : RootConfig.GetSonClientCoder().Email, "Protobuf协议序列化器实现");
//            sb.WriteNameSpace
//                (
//                    "using System.IO;",
//                    "using Iuker.Common.Serialize;",
//                    "using ProtoBuf.Meta;"
//                );

//            sb.AppendLine(string.Format("namespace {0}", selfNamespace));
//            sb.AppendLine("{");
//            sb.AppendLine(string.Format("public class {0} : ISerializer", classname));
//            sb.AppendLine("{");
//            sb.WriteFiledOrProperty("        private readonly TypeModel mTypeModel;");
//            sb.AppendLine(string.Format("        public {0}()", classname));
//            sb.AppendLine("        {");
//            sb.AppendLine(string.Format("            mTypeModel = new {0}();", proxyClassname));
//            sb.AppendLine("        }");
//            sb.AppendLine();

//            sb.AppendLine("        public byte[] Serialize(object value)");
//            sb.AppendLine("        {");
//            sb.AppendLine("            using (MemoryStream ms = new MemoryStream())");
//            sb.AppendLine("            {");
//            sb.AppendLine("                mTypeModel.Serialize(ms, value);");
//            sb.AppendLine("                ms.Position = 0;");
//            sb.AppendLine("                int length = (int)ms.Length;");
//            sb.AppendLine("                var buffer = new byte[length];");
//            sb.AppendLine("                ms.Read(buffer, 0, length);");
//            sb.AppendLine("                return buffer;");
//            sb.AppendLine("            }");
//            sb.AppendLine("        }");
//            sb.AppendLine();

//            sb.AppendLine("        public T DeSerialize<T>(byte[] messageBytes) where T : class, new()");
//            sb.AppendLine("        {");
//            sb.AppendLine("            using (MemoryStream ms = new MemoryStream(messageBytes))");
//            sb.AppendLine("            {");
//            sb.AppendLine("                var message = mTypeModel.Deserialize(ms, null, typeof(T)) as T;");
//            sb.AppendLine("                return message;");
//            sb.AppendLine("            }");
//            sb.AppendLine("        }");
//            sb.AppendLine("        }");
//            sb.AppendLine("        }");

//            var scriptContent = sb.ToString();
//            var scriptPath = CurrentSon.CsProtobufDir + classname + ".cs";
//            File.WriteAllText(scriptPath, scriptContent);
//            AssetDatabase.Refresh();
//        }


//        #region 构造工厂




//        #endregion

//        #region 字符串常量




//        #endregion

//        #region 协议Id解析器




//        #endregion

//        #endregion

//        #region TypeScript

//        #region 代理构造工厂





//        #endregion

//        #endregion

//    }
//}
