using System;
using System.Text;
using Iuker.Common;
using Iuker.UnityKit.Run.Base.Config;
using Iuker.UnityKit.Run.Base.Config.Develop;

namespace Iuker.UnityKit.Editor.Protobuf.ResponserCreater
{
    /// <summary>
    /// 通信答复处理器脚本自动创建器
    /// </summary>
    public class CommunicationResponserCreater
    {
        private readonly SonProject mSon;

        public CommunicationResponserCreater(SonProject son)
        {
            mSon = son;
        }

        private void WriteFileInfo(StringBuilder sb, string noteText = null)
        {
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(string.Format("Author：{0}", RootConfig.GetSonClientCoder().Name == null ? null : RootConfig.GetSonClientCoder().Name));
            sb.AppendLine("CreateDate: " + DateTime.Now);
            sb.AppendLine(string.Format("Email: {0}", RootConfig.GetSonClientCoder().Email == null ? null : RootConfig.GetSonClientCoder().Email));
            sb.AppendLine("***********************************************************************************************/");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/*");
            sb.AppendLine("Socket通信处理器脚本");
            sb.AppendLine(noteText ?? "在该脚本中处理对应协议的通信答复！");
            sb.AppendLine("*/");
            sb.AppendLine();
        }

        /// <summary>
        /// 获得通讯处理器脚本的内容
        /// </summary>
        /// <param name="protoDesc"></param>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public string GetScriptContent(ProtobufClassDesc protoDesc, string scriptName)
        {
            var sb = new StringBuilder();
            WriteFileInfo(sb);
            WriteNameSpace(sb);
            sb.AppendLine("namespace " + RootConfig.GetCurrentProject().ProjectName);
            sb.AppendLine("{");
            sb.AppendLine(string.Format("    public class {0} : ICommunicationResponser", scriptName));
            sb.AppendLine("    {");
            sb.AppendLine("        private IU3dFrame mU3DFrame;");
            sb.AppendLine();

            WriteInit(sb);
            sb.AppendCsharpNote("在这里处理该协议的业务逻辑", null, null, "        ");
            WriteProcessMessage(sb, protoDesc);
            sb.AppendCsharpNote("在这里实现该协议的单元测试", null, null, "        ");
            WriteCheckProcessResult(sb);
            WriteCommandId(sb, protoDesc);
            sb.AppendCsharpNote("在这里实现该协议的自定义异常处理", null, null, "        ");
            WriteProcessException(sb);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public string GetTsScriptContent(ProtobufClassDesc protoDesc)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("namespace {0} ", mSon.CompexName) + "{");
            sb.AppendLine();

            sb.AppendLine(string.Format("    export class {0}", protoDesc.ProtoName + "Requester_jint ") + "{");
            sb.AppendLine();

            sb.AppendLine("        //   该函数做一次性的初始化。");
            sb.AppendLine("        Init() " + "{");
            sb.AppendLine("            ");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        //   该函数用于决定在执行脚本之前是否需要执行脚本自身替换的目标Csharp脚本，执行为True,不执行为False。");
            sb.AppendLine("        IsDoCsharp() " + "{");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        //   在这里处理目标视图行为请求。");
            sb.AppendLine("        ProcessRequest() " + "{");
            sb.AppendLine("            ");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private void WriteNameSpace(StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using Iuker.Common.Module.Communication;");
            sb.AppendLine("using Iuker.Common;");
            sb.AppendLine("using Iuker.UnityKit.Run.Base;");
            sb.AppendLine(string.Format("using {0};", RootConfig.GetCurrentSonProject().NameSapce));
            sb.AppendLine("using Iuker.Common.Base.Interfaces;");
            sb.AppendLine();
        }

        private void WriteInit(StringBuilder sb)
        {
            sb.AppendLine("        public void Init(IFrame frame)");
            sb.AppendLine("        {");
            sb.AppendLine("            mU3DFrame = frame as IU3dFrame;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        private static void WriteProcessMessage(StringBuilder sb, ProtobufClassDesc protocolBase)
        {
            sb.AppendLine("        public void ProcessMessage(byte[] socketMessage)");
            sb.AppendLine("        {");
            sb.AppendLine(string.Format(
                "           var message = mU3DFrame.Serializer.DeSerialize<{0}>(socketMessage);",
                protocolBase.ProtobufsTable.ProtocolName));
            sb.AppendLine("            mU3DFrame.EventModule.IssueEvent(U3dEventCode.Net_MessageArrived.Literals, null, message);");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        private void WriteCheckProcessResult(StringBuilder sb)
        {
            sb.AppendLine("        public bool CheckProcessResult()");
            sb.AppendLine("        {");
            sb.AppendLine("            return true;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        private void WriteCommandId(StringBuilder sb, ProtobufClassDesc protoBase)
        {
            sb.AppendLine("        public int CommandId { get { return  " + protoBase.ProtobufsTable.ServerId + ";}}");
            sb.AppendLine();
        }

        private void WriteProcessException(StringBuilder sb)
        {
            sb.AppendLine("        public void ProcessException(Exception ex)");
            sb.AppendLine("        {");
            sb.AppendLine();
            sb.AppendLine("        }");
            sb.AppendLine();
        }

    }
}