using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Utility;
using Iuker.UnityKit.Editor.Excel;
using Iuker.UnityKit.Editor.Protobuf.ResponserCreater;
using Iuker.UnityKit.Editor.Setting;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;

namespace Iuker.UnityKit.Editor.Protobuf
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProtobufEditorWindow : EditorWindow
    {
        const float MaxWidth = 300.0f;

        public static ProtobufEditorWindow Instance;

        public static void ShowWindow()
        {
            Init();
        }

        private static SonProject sCurrentSonProject;

        private static void Init()
        {
            IukerEditorPrefs.SetBool("Compiled", false);

            sCurrentSonProject = RootConfig.GetCurrentSonProject();
            // 创建.proto文件之前先从协议定义Excel文件中导出proto的本地数据txt文件到指定目录下
            if (!File.Exists(sCurrentSonProject.ProtobufExcelPath))
            {
                EditorUtility.DisplayDialog("错误",
                    string.Format("当前项目为{0},没有找到对应的Protobuf目录或Protobuf协议定义Excel文件，请检查后重试！",
                        RootConfig.Instance.CurrentProjectName), "确定");
                return;
            }

            ExcelUtility.ExportLocalDataTxt(sCurrentSonProject.ProtobufExcelPath, sCurrentSonProject.ProtobufRootDir, sCurrentSonProject.CompexName + "_Protobufs");
            if (Instance != null)
            {
                Instance.Close();
            }
            Instance = GetWindow<ProtobufEditorWindow>("Protobuf工具");
            Instance.titleContent = new GUIContent("Protobuf工具");
            Instance.Focus();
            Instance.InitProtoContext();
        }

        #region Il8n


        private readonly Il8nString il8n_exportProtoFile = Il8nString.Create("IukerInspectorWindow", "一键导出", "One step create");
        private readonly Il8nString il8n_createRequest = Il8nString.Create("IukerInspectorWindow", "创建Cs通信请求处理脚本", "Create Communication Requester");
        private readonly Il8nString il8n_createResponser = Il8nString.Create("IukerInspectorWindow", "创建Cs通信答复处理脚本", "Create Communication Responser");
        private readonly Il8nString il8n_locationScript = Il8nString.Create("IukerInspectorWindow", "定位脚本", "Location Script");

        #endregion

        private void OnDestroy()
        {
            Instance = null;
        }

        /// <summary>
        /// 协议列表
        /// </summary>
        private List<ProtobufClassDesc> mAllProtocols;

        /// <summary>
        /// 由服务器发起的协议类型描述列表
        /// </summary>
        private List<ProtobufClassDesc> mStcProtocols;

        /// <summary>
        /// 由客户端发起的协议类型描述列表
        /// </summary>
        private List<ProtobufClassDesc> mCtsProtocols;

        private GUIStyle mHeaderStyle;
        //        private bool mIsInit;

        /// <summary>
        /// 当前选中的协议的数组索引
        /// </summary>
        private int mProtoIndex;

        /// <summary>
        /// 协议名数组
        /// </summary>
        private string[] mAllProtoArray;

        /// <summary>
        /// 服务器发起的协议类型数组
        /// </summary>
        private string[] STCProtoArray;

        /// <summary>
        /// 当前选中的由服务器发起的协议索引
        /// </summary>
        private int mStcIndex;

        /// <summary>
        /// 客户端发起的协议类型数组
        /// </summary>
        private string[] CtsProtoArray;

        /// <summary>
        /// 当前选中的由客户端 发起的协议索引
        /// </summary>
        private int mCtsIndex;

        private static bool isCompiling = true;

        private void Update()
        {
            if (!EditorApplication.isCompiling && isCompiling)
            {
                Debug.Log("Window context reload!");
                InitProtoContext();
            }

            isCompiling = EditorApplication.isCompiling;
        }

        private void OnGUI()
        {
            InitStyle();
            using (new IukHorizontalLayout())
            {
                DrawProtoCreate();
            }

            GUILayout.BeginVertical("IN GameObjectHeader"); // 选中协议展示
            using (new IukHorizontalLayout())
            {
                using (new IukVerticalLayout(GUILayout.MaxWidth(300)))
                {
                    GUILayout.Label("外部协议");
                    GUILayout.Label(new GUIContent(mProtoContent), GUILayout.MaxWidth(300));
                }
                if (mInternalContent != null && mInternalContent.Count > 0)
                {
                    foreach (var content in mInternalContent)
                    {
                        using (new IukVerticalLayout(GUILayout.MaxWidth(300)))
                        {
                            GUILayout.Label("内部嵌套协议");
                            GUILayout.Label(new GUIContent(content), GUILayout.MaxWidth(300));
                        }
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private void InitStyle()
        {
            if (mHeaderStyle == null)
            {
                mHeaderStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold
                };
            }
        }

        private void DrawProtoCreate()
        {
            using (new IukVerticalLayout(GUILayout.MinWidth(MaxWidth)))
            {
                GUILayout.Space(5);
                AllProtoOnGUI();
                CtsProtoOnGUI();
                StcProtoOnGUI();
                OneStepOnGUI(); //  点一次完成所有流程
            }
        }

        private void OneStepOnGUI()
        {
            if (GUILayout.Button(il8n_exportProtoFile.Content))
            {
                DoOnStep();
            }
        }

        /// <summary>
        /// 执行一键自动生成Protobuf所有相关资源
        /// 1. 程序集
        /// 2. 脚本
        /// 3. 文本资源
        /// </summary>
        public void DoOnStep()
        {
            ExportProtoFile();
            CompileProtoScript();
            CreateCsharpProtoConstructorFactoryScript();
            CreateProtoClassNameConstants();
            CreateProtoIdScript();
            CreateProtobufSerializerProxy();
            CreateCommonProtobufEntityScript();
            Close();
        }

        private void ExportProtoFile()
        {
            var instance = new ProtobufParser();
            // 创建proto后缀的协议文件
            instance.ExportProtoFile();
            AssetDatabase.Refresh();
            // 将生成的协议定义proto文件拷贝到protogen工具所在目录
            var sourcePath = sCurrentSonProject.ProtoExportPath;
            var targetPath = U3dConstants.ProtobufModelProtogenDir + sCurrentSonProject.ProtoFullName;
            File.Copy(sourcePath, targetPath, true);
        }

        private void CompileProtoScript()
        {
            var switchDiskCmd = Application.dataPath.Split('/').First(); //  切换到当前Unity3D项目所在盘符的Dos命令字符串
            var protoGenDir = U3dConstants.ProtobufModelProtogenDir;
            var protoGenStr = string.Format("protogen -i:{0} -o:{1}", sCurrentSonProject.ProtoFullName,
                sCurrentSonProject.ProtoClassPath);
            var ProtoModelCmd = string.Format(
                "C:/Windows/Microsoft.NET/Framework/v4.0.30319/Csc.exe /noconfig /nowarn:1701,1702 /nostdlib+ /errorreport:prompt /warn:4 /define:TRACE /reference:C:/Windows/Microsoft.NET/Framework/v2.0.50727/mscorlib.dll /reference:{0}/1_Iuker.UnityKit/IukerRepository/.Iuker.ProtobufModel/protobuf-net.dll /reference:C:/Windows/Microsoft.NET/Framework/v2.0.50727/System.dll /debug:pdbonly /filealign:512 /optimize+ /out:{1} /target:library /utf8output {2}",
                Application.dataPath, sCurrentSonProject.ProtobufMessageDllPath, sCurrentSonProject.ProtoClassPath);
            var precompileCmd = string.Format("{0}precompile.exe {1} -o:{2} -t:{3}_ProtobufSerializer",
                U3dConstants.ProtobufModelPrecompileDir, sCurrentSonProject.ProtobufMessageDllPath,
                sCurrentSonProject.ProtobufSerializerDllPath, sCurrentSonProject.CompexName);

            // 拷贝protobuf-net程序集文件到目标子项目的ProtobufScript目录下
            FileUtility.TryCreateDirectory(sCurrentSonProject.CsProtobufDir);
            FileUtility.TryCopy(U3dConstants.ProtobufNetDllPath, sCurrentSonProject.Protobuf_Temp_Net_DllPath);

            if (!Directory.Exists(sCurrentSonProject.CsProtobufDir))    //   如果protobuf协议生成脚本存放目录不存在则创建
            {
                Directory.CreateDirectory(sCurrentSonProject.CsProtobufDir);
            }
            CmdUtility.ExcuteDosCommand
                (
                Debug.Log,
                switchDiskCmd,
                "cd " + protoGenDir,
                protoGenStr,
                "cd..",
                ProtoModelCmd,
                precompileCmd
                );

            // 删掉临时使用的protobuf-net.dll文件
            File.Delete(sCurrentSonProject.Protobuf_Temp_Net_DllPath);
            AssetDatabase.Refresh();
        }

        private void CreateCsharpProtoConstructorFactoryScript()
        {
            var son = RootConfig.GetCurrentSonProject();
            var projeceAssembly = Assembly.LoadFile(son.ProtobufMessageDllPath);
            var types = projeceAssembly.GetTypes().ToList();
            var stcProtoTypes = types.Where(t => t.Namespace == son.ProtobufNameSpace && t.Name.StartsWith("STC")).Select(t => t.Name).ToList();
            var ctsProtoTypes = types.Where(t => t.Namespace == son.ProtobufNameSpace && t.Name.StartsWith("CTS")).Select(t => t.Name).ToList();
            var commonProtoTypes =
                types.Where(t =>
                        t.Namespace == son.ProtobufNameSpace && !t.Name.StartsWith("CTS") && !t.Name.StartsWith("STC"))
                    .Select(t => t.Name).ToList();
            var protoClassList = new List<string>();
            protoClassList.AddRange(ctsProtoTypes);
            protoClassList.AddRange(commonProtoTypes);
            protoClassList.AddRange(stcProtoTypes);

            var sb = new StringBuilder();
            WriteConstructorFileInfo(sb);
            sb.AppendLine(string.Format("using {0};", sCurrentSonProject.ProtobufNameSpace));
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
            sb.AppendLine("{");
            sb.AppendLine(string.Format("    public class {0}", sCurrentSonProject.ProtoConstructorFactoryName));
            sb.AppendLine("    {");

            foreach (var protoname in protoClassList)
            {
                var type = types.Find(t => t.Name == protoname);
                if (type != null)
                {
                    AppendCsharpProtoConstructor(sb, protoname, type);
                }
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            // 创建protobuf全参构建函数脚本文件
            var scriptContent = sb.ToString();
            File.WriteAllText(sCurrentSonProject.ProtoConstructorFactoryPath, scriptContent);
            AssetDatabase.Refresh();
        }

        //private void CreateTypeScriptProtoConstructorFactoryProxyScript()
        //{
        //    var protoClassList = mAllProtoArray.ToList();
        //    var sb = new StringBuilder();
        //    sb.AppendLine($"namespace {RootConfig.GetCurrentProject().ProjectName} " + "{");
        //    sb.AppendLine($"    public class {sCurrentSonProject.ProtoConstructorFactoryName}");
        //    sb.AppendLine("    {");

        //    foreach (var protoname in protoClassList)
        //    {
        //        var type = stcProtoTypes.Find(t => t.Name == protoname);
        //        if (type != null)
        //        {
        //            AppendCsharpProtoConstructor(sb, protoname, type);
        //        }
        //    }
        //    sb.AppendLine("    }");
        //    sb.AppendLine("}");

        //    // 创建protobuf全参构建函数脚本文件
        //    var scriptContent = sb.ToString();
        //    File.WriteAllText(sCurrentSonProject.ProtoConstructorFactoryPath, scriptContent);
        //    AssetDatabase.Refresh();
        //    InternalEditorUtility.OpenFileAtLineExternal(sCurrentSonProject.ProtoConstructorFactoryPath, 1);
        //}

        /// <summary>
        /// 创建Protobuf协议脚本名常量脚本
        /// </summary>
        private void CreateProtoClassNameConstants()
        {
            var sb = new StringBuilder();

            WriteConstantFileInfo(sb);
            var classname = sCurrentSonProject.CompexName + "_ProtobufConstant";
            var targetPath = sCurrentSonProject.CsProtobufDir + classname + ".cs";

            sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
            sb.AppendLine("{");
            sb.AppendLine(string.Format("    public class {0}", classname));
            sb.AppendLine("    {");

            foreach (var protoname in mAllProtoArray)
            {
                var splitedProtoName = protoname.Split('@').Last();
                sb.AppendLine(string.Format("        public static readonly string {0} = ", splitedProtoName) + "\"" + splitedProtoName + "\"" +
                              ";");
                sb.AppendLine();
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(targetPath, sb.ToString());
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 追加一个protobuf协议类的全参构造函数
        /// </summary>
        private void AppendCsharpProtoConstructor(StringBuilder sb, string protoname, Type type)
        {
            var properties = type.GetProperties();
            if (properties.Length > 0)
            {
                AppendTypeScriptBridgeConstructorCode(sb, protoname);
            }
            AppendProtoConstructor_Argulist(sb, protoname, properties); // 追加参数列表
            AppendProtoConstructor_Body(sb, protoname, properties); // 追加函数体协议字段赋值
        }

        private void AppendTypeScriptBridgeConstructorCode(StringBuilder sb, string protoname)
        {
            sb.AppendLine(string.Format("        public static {0} Create{1}() {{ return new {2}(); }}", protoname, protoname,
                protoname));
            sb.AppendLine();
        }

        /// <summary>
        /// 追加参数列表
        /// </summary>
        private void AppendProtoConstructor_Argulist(StringBuilder sb, string protoname, PropertyInfo[] properties)
        {
            sb.Append(properties.Length > 0
                ? string.Format("        public static {0} Create{1}(", protoname, protoname)
                : string.Format("        public static {0} Create{1}()", protoname, protoname));

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];
                var argType = GetArguTypeStr(propertyInfo);
                var arguname = propertyInfo.Name.ToLower();
                if (i < properties.Length - 1)
                {
                    if (propertyInfo.PropertyType.IsGenericType)
                    {
                        sb.Append(string.Format("{0} {1},", argType, arguname));
                    }
                    else
                    {
                        sb.Append(string.Format("{0} {1},", argType, arguname));
                    }
                }
                else
                {
                    sb.Append(string.Format("{0} {1})\r\n", argType, arguname));    //  最后一个参数则写入一个换行符
                }
            }
        }

        /// <summary>
        /// 追加函数体
        /// 协议字段赋值
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="protoname"></param>
        /// <param name="properties"></param>
        private void AppendProtoConstructor_Body(StringBuilder sb, string protoname, PropertyInfo[] properties)
        {
            sb.AppendLine("        {");

            sb.AppendLine(string.Format("            var message = new {0}();", protoname));
            foreach (var fieldInfo in properties)
            {
                sb.AppendLine(fieldInfo.PropertyType.IsGenericType
                    ? string.Format("            message.{0}.AddRange({1});", fieldInfo.Name, fieldInfo.Name.ToLower())
                    : string.Format("            message.{0} = {1};", fieldInfo.Name, fieldInfo.Name.ToLower()));
            }

            sb.AppendLine("            return message;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        /// <summary>
        /// 获得参数类型的字符串
        /// </summary>
        private string GetArguTypeStr(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsGenericType)
            {
                var tempname = propertyInfo.PropertyType.ToString();
                var tempArray = tempname.Split('.').ToList();
                tempname = tempArray.Last();
                tempname = tempname.Substring(0, tempname.Length - 1);
                if (baseTypeStrDictionary.ContainsKey(tempname))
                {
                    tempname = baseTypeStrDictionary[tempname]; //  列表中是基础类型则替换为IDE的小写关键字字符串
                }
                //                tempname = $"List<{tempname}>";
                tempname = string.Format("{0}[]", tempname);
                return tempname;
            }

            var typename = propertyInfo.PropertyType.ToString();
            if (baseTypeStrDictionary.ContainsKey(typename))
            {
                var swithname = baseTypeStrDictionary[typename];
                return swithname;
            }

            return typename;
        }

        private readonly Dictionary<string, string> baseTypeStrDictionary = new Dictionary<string, string>
        {
            { "System.Int32","int"},
            { "System.Boolean","bool"},
            { "System.Int64","long"},
            { "System.String","string"},
            { "Int32","int"},
            { "Boolean","bool"},
            { "Int64","long"},
            { "String","string"},
        };

        private void WriteConstructorFileInfo(StringBuilder sb)
        {
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(string.Format("Author：{0}", RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Name));
            sb.AppendLine("CreateDate: " + DateTime.Now);
            sb.AppendLine(string.Format("Email: {0}", RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Email));
            sb.AppendLine("***********************************************************************************************/");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/*");
            sb.AppendLine("Protobuf消息构造函数工厂脚本\r此脚本由框架自动生成，请勿做任何手动修改！");
            sb.AppendLine("*/");
            sb.AppendLine();
        }

        private void WriteConstantFileInfo(StringBuilder sb)
        {
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(string.Format("Author：{0}", RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Name));
            sb.AppendLine("CreateDate: " + DateTime.Now);
            sb.AppendLine(string.Format("Email: {0}", RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Email));
            sb.AppendLine("***********************************************************************************************/");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/*");
            sb.AppendLine("Protobuf消息常量脚本\r此脚本由框架自动生成，请勿做任何手动修改！");
            sb.AppendLine("*/");
            sb.AppendLine();
        }

        private void WriteIdResolverFileInfo(StringBuilder sb)
        {
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(string.Format("Author：{0}", RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Name));
            sb.AppendLine("CreateDate: " + DateTime.Now);
            sb.AppendLine(string.Format("Email: {0}", RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Email));
            sb.AppendLine("***********************************************************************************************/");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/*");
            sb.AppendLine("Protobuf消息编号解析脚本\r此脚本由框架自动生成，请勿做任何手动修改！");
            sb.AppendLine("*/");
            sb.AppendLine();
        }

        #region 请求及答复处理器脚本创建

        /// <summary>
        /// 创建通信答复处理器脚本
        /// </summary>
        /// <param name="protoClassDesc"></param>
        private void CreateResponserScript(ProtobufClassDesc protoClassDesc)
        {
            var targetDir = sCurrentSonProject.CsCommunicationResponsersDir;
            var scriptName = protoClassDesc.ProtobufsTable.ProtocolName + "_Responser";
            var targetPath = targetDir + scriptName + ".cs";
            if (File.Exists(targetPath))
            {
                EditorUtility.DisplayDialog("警告", "目标脚本已存在！", "确定");
                return;
            }

            var creater = new CommunicationResponserCreater(RootConfig.GetCurrentSonProject());
            var content = creater.GetScriptContent(protoClassDesc, scriptName);
            FileUtility.WriteAllText(targetPath, content);
            AssetDatabase.Refresh();
        }

        private void CreateTsResponserScript(ProtobufClassDesc protoClassDesc)
        {
            var son = RootConfig.GetCurrentSonProject();
            var targetPath = son.TsCommunicationResponserDir + protoClassDesc.ProtoName.ToLower() + "_responser_jint.ts";
            if (File.Exists(targetPath))
            {
                EditorUtility.DisplayDialog("警告", "目标脚本已存在！", "确定");
                return;
            }

            var creater = new CommunicationResponserCreater(son);
            var content = creater.GetTsScriptContent(protoClassDesc);
            FileUtility.WriteAllText(targetPath, content);
            TsProj.AddLine(protoClassDesc.ProtoName.ToLower() + "_responser_jint", son, "Project/Communication/Responser").UpdateToFile(son.TsProjPath);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建通信请求处理器脚本
        /// </summary>
        /// <param name="protoClassDesc"></param>
        private void CreateRequestScript(ProtobufClassDesc protoClassDesc)
        {
            var son = RootConfig.GetCurrentSonProject();
            var targetDir = sCurrentSonProject.CsCommunicationRequestDir;
            var scriptName = protoClassDesc.ProtobufsTable.ProtocolName + "_Requester";
            var targetPath = targetDir + scriptName + ".cs";
            if (File.Exists(targetPath))
            {
                EditorUtility.DisplayDialog("警告", "目标脚本已存在！", "确定");
                return;
            }

            var creater = new CommunicationRequesterCreater(son);
            var content = creater.GetScriptContent(protoClassDesc, scriptName);
            FileUtility.WriteAllText(targetPath, content);
            AssetDatabase.Refresh();
        }

        private void CreateTsRequestScript(ProtobufClassDesc protoClassDesc)
        {
            var son = RootConfig.GetCurrentSonProject();
            var targetPath = son.TsCommunicationRequesterDir + protoClassDesc.ProtoName.ToLower() + "_requester_jint.ts";
            if (File.Exists(targetPath))
            {
                EditorUtility.DisplayDialog("警告", "目标脚本已存在！", "确定");
                return;
            }

            var creater = new CommunicationRequesterCreater(son);
            var content = creater.GetTsScriptContent(protoClassDesc);
            FileUtility.WriteAllText(targetPath, content);
            TsProj.AddLine(protoClassDesc.ProtoName.ToLower() + "_requester_jint", son, "Project/Communication/Requester").UpdateToFile(son.TsProjPath);
            AssetDatabase.Refresh();
        }

        #endregion

        private void AllProtoOnGUI()
        {
            using (new IukHorizontalLayout())
            {
                GUILayout.Label("All Protocols");
                var oldCommonIndex = mProtoIndex;
                mProtoIndex = EditorGUILayout.Popup(mProtoIndex, mAllProtoArray);
                if (oldCommonIndex != mProtoIndex)
                {
                    _mCurrentProtobufClassDesc = mAllProtocols[mProtoIndex];
                    OnProtoUpdate();
                }
            }
        }

        private ProtobufClassDesc _mCurrentProtobufClassDesc;

        private void OnProtoUpdate()
        {
            mProtoContent = _mCurrentProtobufClassDesc.GetProtoContent();
            mInternalContent = _mCurrentProtobufClassDesc.GetInternalContents();
        }

        private void CtsProtoOnGUI()
        {
            using (new IukHorizontalLayout())
            {
                GUILayout.Label("Cts Protocols");
                var oldCtsIndex = mCtsIndex;
                mCtsIndex = EditorGUILayout.Popup(mCtsIndex, CtsProtoArray, GUILayout.MinWidth(250));
                if (oldCtsIndex != mCtsIndex)
                {
                    _mCurrentProtobufClassDesc = mCtsProtocols[mCtsIndex];
                    OnProtoUpdate();
                }
                if (GUILayout.Button(il8n_createRequest.Content))
                {
                    var protobufDesc = mCtsProtocols[mCtsIndex];
                    CreateRequestScript(protobufDesc);
                }
                if (GUILayout.Button("创建Js通信请求处理器脚本"))
                {
                    var protobufDesc = mCtsProtocols[mCtsIndex];
                    CreateTsRequestScript(protobufDesc);
                }
                if (GUILayout.Button(il8n_locationScript.Content))    //  自动定位Unity编辑器中对应的脚本目录
                {
                    // 查找当前选中的协议所对应的通信请求处理器脚本所在目录
                    LocationRequestScript();
                }
            }
        }

        private void StcProtoOnGUI()
        {
            using (new IukHorizontalLayout())
            {
                GUILayout.Label("Stc Protocols");
                var oldStcIndex = mStcIndex;
                mStcIndex = EditorGUILayout.Popup(mStcIndex, STCProtoArray, GUILayout.MinWidth(250));
                if (oldStcIndex != mStcIndex)
                {
                    _mCurrentProtobufClassDesc = mStcProtocols[mStcIndex];
                    OnProtoUpdate();
                }
                if (GUILayout.Button(il8n_createResponser.Content))
                {
                    var protobufDesc = mStcProtocols[mStcIndex];
                    CreateResponserScript(protobufDesc);
                }
                if (GUILayout.Button("创建Js通信答复处理器脚本"))
                {
                    var protobufDesc = mStcProtocols[mStcIndex];
                    CreateTsResponserScript(protobufDesc);
                }
                if (GUILayout.Button(il8n_locationScript.Content))    //  自动定位Unity编辑器中对应的脚本目录
                {
                    // 查找当前选中的协议所对应的通信答复处理器脚本所在目录
                    LocationResponserScript();
                }
            }
        }

        /// <summary>
        /// 创建协议id解析器脚本
        /// </summary>
        private void CreateProtoIdScript()
        {
            StringBuilder sb = new StringBuilder();
            WriteIdResolverFileInfo(sb);
            sb.AppendLine(string.Format("using {0};", sCurrentSonProject.ProtobufNameSpace));
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Iuker.Common.Module.Socket;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
            sb.AppendLine("{");
            sb.AppendLine(string.Format("    public class {0} : IProtoIdResolver",
                sCurrentSonProject.ProtoIdResolverName));
            sb.AppendLine("    {");

            sb.AppendLine("        private readonly Dictionary<string, int> mProtoIdDictionary = new Dictionary<string, int>();");
            sb.AppendLine("        private readonly Dictionary<int, string> mProtoNameDictionary = new Dictionary<int, string>();");
            sb.AppendLine();

            sb.AppendLine("        public void Init()");
            sb.AppendLine("        {");

            sb.AppendLine("            try");
            sb.AppendLine("            {");
            foreach (var protobufDesc in mAllProtocols)
            {
                if (protobufDesc.ProtoId != -1 && (protobufDesc.ProtoName.Contains("STC") || protobufDesc.ProtoName.Contains("CTS")) && protobufDesc.ProtoName == protobufDesc.ProtobufsTable.ProtocolName)
                {
                    sb.AppendLine(string.Format("                mProtoIdDictionary.Add(\"{0}\", {1});",
                        protobufDesc.ProtoName, protobufDesc.ProtoId));
                }
            }
            sb.AppendLine();
            foreach (var protobufDesc in mAllProtocols)
            {
                if (protobufDesc.ProtoId != -1 && (protobufDesc.ProtoName.Contains("STC") || protobufDesc.ProtoName.Contains("CTS")) && protobufDesc.ProtoName == protobufDesc.ProtobufsTable.ProtocolName)
                {
                    sb.AppendLine(string.Format("                mProtoNameDictionary.Add({0}, \"{1}\");",
                        protobufDesc.ProtoId, protobufDesc.ProtoName));
                }
            }

            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                Debug.LogException(new Exception(ex.Message));");
            sb.AppendLine("            }");

            sb.AppendLine();
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendCsharpNote("获取Protobuf协议编号", null, null, "        ");
            sb.AppendLine("        public int GetProtoId(string protoname)");
            sb.AppendLine("        {");
            sb.AppendLine("            if(mProtoIdDictionary.ContainsKey(protoname))");
            sb.AppendLine("            {");
            sb.AppendLine("                return mProtoIdDictionary[protoname];");
            sb.AppendLine("            }");
            sb.AppendLine("            return int.MaxValue;");
            sb.AppendLine("        }");

            sb.AppendLine();

            sb.AppendCsharpNote("获取Protobuf协议名", null, null, "        ");
            sb.AppendLine("        public string GetProtoName(int commandId)");
            sb.AppendLine("        {");
            sb.AppendLine("            if(mProtoNameDictionary.ContainsKey(commandId))");
            sb.AppendLine("            {");
            sb.AppendLine("                return mProtoNameDictionary[commandId];");
            sb.AppendLine("            }");
            sb.AppendLine("            return null;");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            var scriptContent = sb.ToString();
            File.WriteAllText(sCurrentSonProject.ProtoIdResolverPath, scriptContent);
            AssetDatabase.Refresh();
            //InternalEditorUtility.OpenFileAtLineExternal(RootConfig.ProtoIdResolverPath, 1);
        }

        /// <summary>
        /// 用于展示的协议内容
        /// 当前选中的协议
        /// </summary>
        private string mProtoContent;

        /// <summary>
        /// 当前选中协议的内部协议内容
        /// </summary>
        private List<string> mInternalContent;

        private void InitProtoContext()
        {
            InitProtocolList();    // 初始化当前项目的各协议类型列表
            InitProtoArray();
            InitShowProtoContent(); //  初始化用于展示的协议定义内容文本字符串
        }

        private void InitShowProtoContent()
        {
            mProtoContent = !mAllProtocols.IsNullOrZero() ? mAllProtocols[mProtoIndex].GetProtoContent() : string
                .Format("当前子项目{0}的协议表是空的！", RootConfig.GetCurrentSonProject().ProjectName);
        }

        /// <summary>
        /// 初始化各类型协议显示用数组
        /// </summary>
        private void InitProtoArray()
        {
            mAllProtoArray = mAllProtocols.Select(protocol => protocol.ProtoId + "@" + protocol.ProtoName).ToArray();    //  所有协议
            STCProtoArray = mAllProtocols.FindAll(r => r.ProtoName.StartsWith("STC")).OrderBy(r => r.ProtoId)
                .Select(r => r.ProtoId + "@" + r.ProtoName).ToArray();    //  服务器端协议
            CtsProtoArray = mAllProtocols.FindAll(r => r.ProtoName.StartsWith("CTS")).OrderBy(r => r.ProtoId)
                .Select(r => r.ProtoId + "@" + r.ProtoName).ToArray();
        }

        /// <summary>
        /// 初始化当前项目的各协议类型列表
        /// </summary>
        private void InitProtocolList()
        {
            var parser = new ProtobufParser();
            mAllProtocols = parser.InitProtocols().OrderBy(r => r.ProtoId).ToList();
            mStcProtocols = mAllProtocols.FindAll(r => r.ProtoName.StartsWith("STC")).OrderBy(r => r.ProtoId).ToList();
            mCtsProtocols = mAllProtocols.FindAll(r => r.ProtoName.StartsWith("CTS")).OrderBy(r => r.ProtoId).ToList();
        }

        /// <summary>
        /// 查找当前选中的协议所对应的通信答复处理器脚本所在目录
        /// </summary>
        private void LocationResponserScript()
        {
            var protobufDesc = mStcProtocols[mStcIndex];

            var targetDir = sCurrentSonProject.CsCommunicationResponsersDir + protobufDesc.ProtoName + "/";
            if (FileUtility.TryCreateDirectory(targetDir) == false)
            {
                EditorUtility.DisplayDialog("警告", string.Format("协议{0}的处理器脚本当前不存在！", protobufDesc.ProtoName), "确定");
                return;
            }

            var files = Directory.GetFiles(targetDir).Where(r => !r.Contains("meta")).OrderBy(r => r).ToList();
            if (files.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", string.Format("协议{0}的处理器脚本当前不存在！", protobufDesc.ProtoName), "确定");
                return;
            }

            var targetScriptPath = files.Last();
            targetScriptPath = targetScriptPath.Replace("\\", "/").Replace(Application.dataPath, "");
            targetScriptPath = "Assets" + targetScriptPath;
            var scriptObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(targetScriptPath);
            EditorGUIUtility.PingObject(scriptObject.GetInstanceID());
        }

        /// <summary>
        /// 查找当前选中的协议所对应的通信请求处理器脚本所在目录
        /// </summary>
        private void LocationRequestScript()
        {
            var protobufDesc = mCtsProtocols[mCtsIndex];

            var targetDir = sCurrentSonProject.CsCommunicationRequestDir + protobufDesc.ProtoName + "/";
            if (FileUtility.TryCreateDirectory(targetDir) == false)
            {
                EditorUtility.DisplayDialog("警告", string.Format("协议{0}的处理器脚本当前不存在！", protobufDesc.ProtoName), "确定");
                return;
            }

            var files = Directory.GetFiles(targetDir).Where(r => !r.Contains("meta")).OrderBy(r => r).ToList();
            if (files.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", string.Format("协议{0}的处理器脚本当前不存在！", protobufDesc.ProtoName), "确定");
                return;
            }

            var targetScriptPath = files.Last();
            targetScriptPath = targetScriptPath.Replace("\\", "/").Replace(Application.dataPath, "");
            targetScriptPath = "Assets" + targetScriptPath;
            var scriptObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(targetScriptPath);
            EditorGUIUtility.PingObject(scriptObject.GetInstanceID());
        }

        /// <summary>
        /// 创建当前子项目的Protobuf序列化器代理脚本
        /// </summary>
        private void CreateProtobufSerializerProxy()
        {
            StringBuilder sb = new StringBuilder();
            var sonProject = RootConfig.GetCurrentSonProject();

            var classname = sonProject.CompexName + "_ProtobufSerializer_Proxy";
            var proxyClassname = sonProject.CompexName + "_ProtobufSerializer";
            var selfNamespace = sonProject.NameSapce;
            sb.AppendCsahrpFileInfo(RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Name,
                RootConfig.GetSonClientCoder().Email == null ? null : RootConfig.GetSonClientCoder().Email, "Protobuf协议序列化器实现");
            sb.WriteNameSpace
                (
                    "using System.IO;",
                    "using Iuker.Common.Serialize;",
                    "using ProtoBuf.Meta;"
                );

            sb.AppendLine(string.Format("namespace {0}", selfNamespace));
            sb.AppendLine("{");
            sb.AppendLine(string.Format("public class {0} : ISerializer", classname));
            sb.AppendLine("{");
            sb.WriteFiledOrProperty("        private readonly TypeModel mTypeModel;");
            sb.AppendLine(string.Format("        public {0}()", classname));
            sb.AppendLine("        {");
            sb.AppendLine(string.Format("            mTypeModel = new {0}();", proxyClassname));
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        public byte[] Serialize(object value)");
            sb.AppendLine("        {");
            sb.AppendLine("            using (MemoryStream ms = new MemoryStream())");
            sb.AppendLine("            {");
            sb.AppendLine("                mTypeModel.Serialize(ms, value);");
            sb.AppendLine("                ms.Position = 0;");
            sb.AppendLine("                int length = (int)ms.Length;");
            sb.AppendLine("                var buffer = new byte[length];");
            sb.AppendLine("                ms.Read(buffer, 0, length);");
            sb.AppendLine("                return buffer;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        public T DeSerialize<T>(byte[] messageBytes) where T : class, new()");
            sb.AppendLine("        {");
            sb.AppendLine("            using (MemoryStream ms = new MemoryStream(messageBytes))");
            sb.AppendLine("            {");
            sb.AppendLine("                var message = mTypeModel.Deserialize(ms, null, typeof(T)) as T;");
            sb.AppendLine("                return message;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        }");
            sb.AppendLine("        }");

            var scriptContent = sb.ToString();
            var scriptPath = sCurrentSonProject.CsProtobufDir + classname + ".cs";
            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建CSharp后端使用的Csharp格式的protobuf协议脚本
        /// </summary>
        private void CreateCommonProtobufEntityScript()
        {
            var son = RootConfig.GetCurrentSonProject();
            foreach (var protobufClassDesc in mAllProtocols)
            {
                var sb = new StringBuilder();
                var index = 1;
                sb.AppendLine("using ProtoBuf;");
                sb.AppendLine();
                sb.AppendLine(string.Format("namespace {0}", son.ProjectName));
                sb.AppendLine("{");
                sb.AppendLine("    [ProtoContract]");
                sb.AppendLine(string.Format("    public class {0}", protobufClassDesc.ProtoName));
                sb.AppendLine("    {");

                foreach (var fieldDesc in protobufClassDesc.FieldDescs)
                {
                    sb.AppendCsharpNote(fieldDesc.Note, null, null, "        ");
                    sb.AppendLine(string.Format("        [ProtoMember({0})]", index));
                    index++;
                    sb.AppendLine(string.Format("        public {0} {1} {2}", ProtobufToCsType(fieldDesc.DataType),
                        fieldDesc.FieldName, "{ get; set; }"));
                    sb.AppendLine();
                }

                sb.AppendLine("    }");
                sb.AppendLine("}");
                var path = son.CsBackProtobufDir + protobufClassDesc.ProtoName + ".cs";
                FileUtility.WriteAllText(path, sb.ToString());
            }

            AssetDatabase.Refresh();
        }

        private string ProtobufToCsType(string protoType)
        {
            switch (protoType)
            {
                case "int16":
                    return "short";
                case "int32":
                    return "int";
                case "int64":
                    return "long";
                case "bytes":
                    return "byte[]";
                default:
                    return protoType;
            }
        }

    }
}