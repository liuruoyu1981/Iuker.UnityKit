/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/13 14:03
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config.Develop;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Protobuf
{
    /// <summary>
    /// protobuf自动化工具
    /// 基于excel表自动构建以下
    /// 1. proto格式文件
    /// 2. protobuf协议脚本
    /// 3. protobuf消息全参构造函数工厂类
    /// 4. socket通讯消息处理器脚本
    /// </summary>
    public class ProtobufParser
    {
        #region 字段

        /// <summary>
        /// Protobuf描述实体列表
        /// </summary>
        private List<LD_ProtobufsTable> mProtobufsTables;

        /// <summary>
        /// protobuf协议字段描述列表字典
        /// </summary>
        private readonly Dictionary<string, ProtobufClassDesc> sProtobufDescDictionary = new Dictionary<string, ProtobufClassDesc>();

        /// <summary>
        /// 当前协议解析环境是否已结束
        /// </summary>
        private bool mIsEnd;

        /// <summary>
        /// 临时注释
        /// </summary>
        private string mTempComment;

        /// <summary>
        /// 注释列表
        /// </summary>
        private readonly List<string> mComments = new List<string>();

        /// <summary>
        /// 当前协议的文本内容
        /// </summary>
        private string mFieldContent;

        /// <summary>
        /// 词法分析索引
        /// </summary>
        private int mCurrentIndex;

        /// <summary>
        /// 
        /// </summary>
        private char mCurrentChar { get { return mFieldContent[mCurrentIndex]; } }

        /// <summary>
        /// 
        /// </summary>
        private readonly List<string> mFieldDescContent = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        private char NextChar { get { return mFieldContent[mCurrentIndex + 1]; } }

        /// <summary>
        /// 
        /// </summary>
        private string mTempField;

        #endregion

        /// <summary>
        /// 导出当前项目的proto定义文件
        /// </summary>
        public void ExportProtoFile()
        {
            InitProtocols();
            var sb = new StringBuilder();
            var protobufDescs = sProtobufDescDictionary.Values.ToList();
            foreach (var protobufFieldDesc in protobufDescs)
            {
                if (protobufFieldDesc == protobufDescs.Last())
                {

                }
                WriteOneProto(sb, protobufFieldDesc.FieldDescs, protobufFieldDesc.ProtoName);
            }

            // 创建proto文本文档
            var protoContent = sb.ToString();
            File.WriteAllText(RootConfig.GetCurrentSonProject().ProtoExportPath, protoContent);
        }

        /// <summary>
        /// 获得当前项目所有协议的描述实例列表
        /// </summary>
        /// <returns></returns>
        public List<ProtobufClassDesc> InitProtocols()
        {
            sProtobufDescDictionary.Clear();

            var protoTxt = File.ReadAllText(RootConfig.GetCurrentSonProject().ProtobufTxtPath);
            var sourceList = protoTxt.Split(Environment.NewLine.ToCharArray()).ToList();
            var listObj = LdUitlity.ConvertLocalDataTxt(sourceList, false);
            var protoTabel = new LD_ProtobufsTable();
            mProtobufsTables = protoTabel.CreateEntitys(listObj);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < mProtobufsTables.Count; i++)
            {
                var protobufsTable = mProtobufsTables[i];
                var protobufFieldDescs = ParseProtobufTable(protobufsTable);
                foreach (var fieldDesc in protobufFieldDescs)
                {
                    if (!sProtobufDescDictionary.ContainsKey(fieldDesc.ProtoName))
                    {
                        sProtobufDescDictionary.Add(fieldDesc.ProtoName, fieldDesc);
                    }
                }
            }

            return sProtobufDescDictionary.Values.ToList();
        }

        private static void WriteOneProto(StringBuilder sb, List<ProtobufFieldDesc> fieldDescs, string protoName)
        {
            if (fieldDescs == null) return;

            sb.AppendLine(string.Format("message {0}", protoName));
            sb.AppendLine("{");
            for (var i = 0; i < fieldDescs.Count; i++)
            {
                var desc = fieldDescs[i];
                sb.Append("    " + desc.ProtobufFiledType + " ");
                sb.Append(desc.DataType + " ");
                var index = i + 1;
                sb.Append(desc.FieldName + " = " + index + ";\n");
            }
            sb.AppendLine("}");
            sb.AppendLine();
        }

        /// <summary>
        /// 将一条protobuf协议描述记录解析为双端协议字段描述实例
        /// </summary>
        /// <param name="protobufsTable"></param>
        /// <returns></returns>
        private IEnumerable<ProtobufClassDesc> ParseProtobufTable(LD_ProtobufsTable protobufsTable)
        {
            var fieldContent = protobufsTable.CommonField;
            var messageContent = fieldContent.Split('}').ToList();
            var messageArray = messageContent.Where(r => r != " " && r != "").ToList().Select(r => r.TrimStart(' '))
                .ToList();
            var desces = new List<ProtobufClassDesc>();

            for (var i = 0; i < messageArray.Count; i++)
            {
                var message = messageArray[i];
                var desc = i == 0 ? ParseMesssage(message, protobufsTable) : ParseMesssage(message, protobufsTable, true);

                if (i != 0)
                {
                    desces[0].InternalProtos.Add(desc);
                }
                desces.Add(desc);
            }
            return desces;
        }

        /// <summary>
        /// 解析一段protobuf消息定义文本
        /// </summary> 
        private ProtobufClassDesc ParseMesssage(string messageContent, LD_ProtobufsTable protobufsTable, bool isInternal = false)
        {
            CleanContext();
            var splitArray = messageContent.Split('{');
            splitArray = splitArray.Where(r => r != "").ToArray();
            var protoname = splitArray.First().Split(' ')[1];   //  由于有内部嵌套协议定义，因此需拆分拿到实际的消息名
            mFieldContent = splitArray.Last();
            mFieldContent = mFieldContent.Trim();
            Scan();

            var fieldDescs = new List<ProtobufFieldDesc>();
            ParseParentAndInternal(fieldDescs, protobufsTable, isInternal);
            var protobufDesc = new ProtobufClassDesc(fieldDescs, protobufsTable, protoname);
            return protobufDesc;
        }

        private void ParseParentAndInternal(List<ProtobufFieldDesc> fieldDescs, LD_ProtobufsTable protobufsTable, bool isInternal = false)
        {
            try
            {
                if (!isInternal)
                {
                    ParseParent(fieldDescs);
                }
                else
                {
                    ParseInternal(fieldDescs);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("解析协议表{0}时发生异常，异常信息为{1}！", protobufsTable.ProtocolName, ex.Message));
            }
        }

        private void CleanContext()
        {
            mIsEnd = false;
            mCurrentIndex = 0;
            mComments.Clear();
            mFieldDescContent.Clear();
        }

        private void ParseParent(List<ProtobufFieldDesc> fieldDescs)
        {
            for (var i = 0; i < mFieldDescContent.Count; i++)
            {
                var comment = string.Empty;
                if (mComments.Count > i) comment = mComments[i];
                var fieldContent = mFieldDescContent[i];
                fieldContent = fieldContent.Substring(0, fieldContent.Length - 1);
                var desc = ParseOneLineProto(fieldContent, comment);
                fieldDescs.Add(desc);
            }
        }

        private void ParseInternal(List<ProtobufFieldDesc> fieldDescs)
        {
            foreach (var fieldDesc in mFieldDescContent)
            {
                var tempArray = fieldDesc.Split(';').Where(r => r != "").ToArray();
                foreach (var s in tempArray)
                {
                    var desc = ParseOneLineProto(s);
                    fieldDescs.Add(desc);
                }
            }
        }

        private ProtobufFieldDesc ParseOneLineProto(string code, string comment = null)
        {
            var tempArr = code.Split('=');

            var index = Convert.ToInt32(tempArr.Last());
            var tempArr2 = tempArr.First().Split(' ');
            var desc = ProtobufFieldDesc.CreateProtobufMessageDesc(tempArr2, index, comment);
            return desc;
        }

        private void Scan()
        {
            while (mIsEnd == false)
            {
                if (mCurrentIndex == mFieldContent.Length)
                {
                    mIsEnd = true;
                    return;
                }

                if (mCurrentChar == '/' && NextChar == '/')
                {
                    mCurrentIndex += 2;
                    ReadComment();
                    continue;
                }

                ReadProtoField();
            }
        }



        private void ReadComment()
        {
            while (true)
            {
                if (mCurrentChar == 'o' && IsProtoKey("optional") || mCurrentChar == 'r' && IsProtoKey("repeated"))
                {
                    mComments.Add(mTempComment);
                    mTempComment = null;
                    return;
                }

                mTempComment += mCurrentChar;
                mCurrentIndex++;
            }
        }

        private void ReadProtoField()
        {
            while (true)
            {
                if (mCurrentChar == '/' && NextChar == '/')
                {
                    mFieldDescContent.Add(mTempField);
                    mTempField = null;
                    return;
                }

                if (mCurrentChar == ';')
                {
                    mTempField += mCurrentChar;
                    mCurrentIndex++;
                    mFieldDescContent.Add(mTempField);
                    mTempField = null;
                    return;
                }

                mTempField += mCurrentChar;
                mCurrentIndex++;
                if (mCurrentIndex != mFieldContent.Length) continue;

                mIsEnd = true;
                mFieldDescContent.Add(mTempField);
                mTempField = null;
                return;
            }
        }

        private bool IsProtoKey(string key)
        {
            if (mCurrentIndex + 8 > mFieldContent.Length)
            {
                Debug.Log("已到达协议字段末尾！");
                mIsEnd = true;
                return false;
            }

            var value = mFieldContent.Substring(mCurrentIndex, 8);
            return value == key;
        }

    }
}