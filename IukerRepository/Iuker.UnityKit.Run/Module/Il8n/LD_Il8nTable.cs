/***********************************************************************************************
Author：
CreateDate: 7/23/2017 7:33:09 AM
Email: 
Blog: 
***********************************************************************************************/


/*
该文件由工具自动生成，请勿做任何修改！！！！！！！！！
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Module.LocalData;
using Iuker.Common.Utility;
using Iuker.Common.Constant;

namespace RyThreeKindoms
{
    /// <summary>
    /// 本地数据表_Il8n
    /// </summary>
    [Serializable]
    public class LD_Il8nTable : IDeepCopyLocalData<LD_Il8nTable>
    {
        /// <summary>
        /// 索引编号
        /// </summary>
        public int Index;

        /// <summary>
        /// 文本用途
        /// </summary>
        public string Purpose;

        /// <summary>
        /// 中文版本
        /// </summary>
        public string Chinese;

        /// <summary>
        /// 英语版本
        /// </summary>
        public string English;

        /// <summary>
        /// 日语版本
        /// </summary>
        public string Japanese;

        /// <summary>
        /// 法语版本
        /// </summary>
        public string France;

        /// <summary>
        /// 德语版本
        /// </summary>
        public string Germany;

        /// <summary>
        /// 韩语版本
        /// </summary>
        public string Korea;

        /// <summary>
        /// 西班牙语版本
        /// </summary>
        public string Spain;

        /// <summary>
        /// 意大利语版本
        /// </summary>
        public string Italy;


        /// <summary>
        /// 从单行转换为实体数据类
        /// </summary>
        /// <param name="row">单行数据</param>
        public LD_Il8nTable CreateEntity(List<string> row)
        {
            LD_Il8nTable instance = new LD_Il8nTable();
            instance.Index = Convert.ToInt32(row[0]);
            instance.Purpose = row[1];
            instance.Chinese = row[2];
            instance.English = row[3];
            instance.Japanese = row[4];
            instance.France = row[5];
            instance.Germany = row[6];
            instance.Korea = row[7];
            instance.Spain = row[8];
            instance.Italy = row[9];
            return instance;
        }

        public List<LD_Il8nTable> CreateEntitys(List<string> listObj)
        {
            var result = new List<LD_Il8nTable>();
            foreach (var list in listObj)
            {
                var entityListText = list.Split(Constant.TxtSeparators,StringSplitOptions.None).ToList();
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
            string entityString = string.Empty;
            entityString = entityString + Index + "[__]";
            entityString = entityString + Purpose + "[__]";
            entityString = entityString + Chinese + "[__]";
            entityString = entityString + English + "[__]";
            entityString = entityString + Japanese + "[__]";
            entityString = entityString + France + "[__]";
            entityString = entityString + Germany + "[__]";
            entityString = entityString + Korea + "[__]";
            entityString = entityString + Spain + "[__]";
            entityString = entityString + Italy + "[__]";
            entityString = entityString.Remove(entityString.Length - 1);
            return entityString;
        }

        /// <summary>
        /// 获得本地数据的一份深度复制副本
        /// </summary>
        public LD_Il8nTable DeepCopy()
        {
            var buff = SerializeUitlity.Serialize(this);
            LD_Il8nTable entity = SerializeUitlity.DeSerialize<LD_Il8nTable>(buff);
            return entity;
        }
    }
}
