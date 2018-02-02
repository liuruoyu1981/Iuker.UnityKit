/***********************************************************************************************
Author：
CreateDate: 1/6/2018 11:47:25 AM
Email: 
***********************************************************************************************/


/*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Module.LocalData;
using Iuker.Common.Constant;
using Iuker.Common.Utility;

namespace Iuker.UnityKit.Editor.Protobuf
{
    /// <summary>
    /// 本地数据表_LD_ProtobufsTable
    /// </summary>
    [Serializable]
    public class LD_ProtobufsTable : IDeepCopyLocalData<LD_ProtobufsTable>
    {
        /// <summary>
        /// 客户端协议编号
        /// </summary>
        public int ClientId;

        /// <summary>
        /// 服务器协议编号
        /// </summary>
        public int ServerId;

        /// <summary>
        /// 模块
        /// </summary>
        public int module;

        /// <summary>
        /// 协议名
        /// </summary>
        public string ProtocolName;

        /// <summary>
        /// 通用协议字段
        /// </summary>
        public string CommonField;

        /// <summary>
        /// 协议用途描述
        /// </summary>
        public string Desc;

        /// <summary>
        /// 处理类名
        /// </summary>
        public string className;

        /// <summary>
        /// 对应服务端protobuf类
        /// </summary>
        public string stcProtobuf;

        /// <summary>
        /// 对应客户端protobuf类
        /// </summary>
        public string ctsProtobuf;

        public LD_ProtobufsTable CreateEntity(List<string> row)
        {
            LD_ProtobufsTable entity = new LD_ProtobufsTable();
            entity.ClientId = Convert.ToInt32(row[0]);
            entity.ServerId = Convert.ToInt32(row[1]);
            entity.module = Convert.ToInt32(row[2]);
            entity.ProtocolName = row[3];
            entity.CommonField = row[4];
            entity.Desc = row[5];
            entity.className = row[6];
            entity.stcProtobuf = row[7];
            entity.ctsProtobuf = row[8];
            return entity;
        }

        public List<LD_ProtobufsTable> CreateEntitys(List<string> listObj)
        {
            var result = new List<LD_ProtobufsTable>();
            foreach (var list in listObj)
            {
                var entityListText = list.Split(Constant.TxtSeparators, StringSplitOptions.None).ToList();
                var entity = CreateEntity(entityListText);
                result.Add(entity);
            }
            return result;
        }

        /// <summary>
        /// 将本地数据对象转换为txt源数据字符串
        /// </summary>
        public string ToTxt()
        {
            string entityStr = string.Empty;
            entityStr = entityStr + ClientId + "[__]";
            entityStr = entityStr + ServerId + "[__]";
            entityStr = entityStr + module + "[__]";
            entityStr = entityStr + ProtocolName + "[__]";
            entityStr = entityStr + CommonField + "[__]";
            entityStr = entityStr + Desc + "[__]";
            entityStr = entityStr + className + "[__]";
            entityStr = entityStr + stcProtobuf + "[__]";
            entityStr = entityStr + ctsProtobuf + "[__]";
            entityStr = entityStr.Remove(entityStr.Length - 1);
            return entityStr;
        }

        /// <summary>
        /// 获得本地数据的一份深度复制副本
        /// </summary>
        public LD_ProtobufsTable DeepCopy()
        {
            var buff = SerializeUitlity.Serialize(this);
            var entity = SerializeUitlity.DeSerialize<LD_ProtobufsTable>(buff);
            return entity;
        }
    }
}
