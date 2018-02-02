/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/19 21:03
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System.Collections.Generic;
using System.Text;

namespace Iuker.UnityKit.Editor.Protobuf
{
    /// <summary>
    /// Protobuf协议描述类
    /// </summary>
    public class ProtobufClassDesc
    {
        /// <summary>
        /// 双端协议描述实例所持有的源protobuf协议数据对象
        /// </summary>
        public LD_ProtobufsTable ProtobufsTable { get; private set; }

        /// <summary>
        /// 协议名
        /// 可能是父协议也可能是嵌套协议
        /// 这里返回实际的协议名而不是统一返回父协议
        /// </summary>
        public string ProtoName { get; private set; }

        /// <summary>
        /// 字段描述列表
        /// </summary>
        public List<ProtobufFieldDesc> FieldDescs { get; private set; }

        public int ProtoId
        {
            get
            {
                if (ProtoName.StartsWith("STC")) return ProtobufsTable.ServerId;
                if (ProtoName.StartsWith("CTS")) return ProtobufsTable.ClientId;
                return -9999;
            }
        }

        /// <summary>
        /// 经过覆盖修正之后的协议用途
        /// 防止服务器导出协议表时有协议嵌套存在
        /// 通过覆写修正为正确的协议用途描述文字
        /// </summary>
        private string mFixedProtoPurpose;

        /// <summary>
        /// 内部嵌套协议列表
        /// </summary>
        public readonly List<ProtobufClassDesc> InternalProtos =
        new List<ProtobufClassDesc>();

        /// <summary>
        /// 协议用途
        /// </summary>
        private string ProtoPurpose { get { return mFixedProtoPurpose ?? ProtobufsTable.Desc; } }

        /// <summary>
        /// 构建一个双端protobuf协议描述实例
        /// </summary>
        /// <param name="common"></param>
        /// <param name="protobufsTable"></param>
        /// <param name="protoname"></param>
        public ProtobufClassDesc(List<ProtobufFieldDesc> common, LD_ProtobufsTable protobufsTable, string protoname)
        {
            FieldDescs = common;
            ProtobufsTable = protobufsTable;
            ProtoName = protoname;
        }

        private string mProtoContent;

        /// <summary>
        /// 获得协议文字内容
        /// </summary>
        /// <returns></returns>
        public string GetProtoContent()
        {
            if (mProtoContent != null) return mProtoContent;

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("协议编号 {0}", ProtoId));
            sb.AppendLine(ProtoPurpose.Trim());
            sb.AppendLine();
            sb.AppendLine(string.Format("message {0}", ProtoName));
            sb.AppendLine("{");
            for (var i = 0; i < FieldDescs.Count; i++)
            {
                var desc = FieldDescs[i];
                sb.AppendLine("    " + desc.Note);
                sb.Append("    " + desc.ProtobufFiledType + " ");
                sb.Append(desc.DataType + " ");
                var index = i + 1;
                sb.Append(desc.FieldName + " = " + index + ";\n");
                sb.AppendLine();
            }
            sb.AppendLine("}");
            sb.AppendLine();

            mProtoContent = sb.ToString();

            return mProtoContent;
        }

        public List<string> GetInternalContents()
        {
            var result = new List<string>();
            foreach (var desc in InternalProtos)
            {
                result.Add(desc.GetProtoContent());
            }
            return result;
        }

    }
}