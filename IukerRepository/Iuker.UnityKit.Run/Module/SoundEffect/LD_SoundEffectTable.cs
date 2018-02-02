/***********************************************************************************************
Author：
CreateDate: 7/27/2017 5:35:06 PM
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

/// <summary>
/// 本地数据表_SoundEffect
/// </summary>
[Serializable]
public class LD_SoundEffectTable : IDeepCopyLocalData<LD_SoundEffectTable>
{
    /// <summary>
    /// 索引
    /// </summary>
    public int ID;

    /// <summary>
    /// 组件根对象名（用于和组件拼合为唯一Token）
    /// </summary>
    public string ComponentRootName;

    /// <summary>
    /// 需要播放音效的组件名
    /// </summary>
    public string ComponentName;

    /// <summary>
    /// 组件行为名（点击、按压、滑入等）
    /// </summary>
    public string ActionName;

    /// <summary>
    /// 音效资源名
    /// </summary>
    public string SoundEffectName;

    /// <summary>
    /// 音效说明
    /// </summary>
    public string Desc;


    /// <summary>
    /// 从单行转换为实体数据类
    /// </summary>
    /// <param name="row">单行数据</param>
    public LD_SoundEffectTable CreateEntity(List<string> row)
    {
        LD_SoundEffectTable instance = new LD_SoundEffectTable();
        instance.ID = Convert.ToInt32(row[0]);
        instance.ComponentRootName = row[1];
        instance.ComponentName = row[2];
        instance.ActionName = row[3];
        instance.SoundEffectName = row[4];
        instance.Desc = row[5];
        return instance;
    }

    public List<LD_SoundEffectTable> CreateEntitys(List<string> listObj)
    {
        var result = new List<LD_SoundEffectTable>();
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
        string entityString = string.Empty;
        entityString = entityString + ID + "[__]";
        entityString = entityString + ComponentRootName + "[__]";
        entityString = entityString + ComponentName + "[__]";
        entityString = entityString + ActionName + "[__]";
        entityString = entityString + SoundEffectName + "[__]";
        entityString = entityString + Desc + "[__]";
        entityString = entityString.Remove(entityString.Length - 1);
        return entityString;
    }

    /// <summary>
    /// 获得本地数据的一份深度复制副本
    /// </summary>
    public LD_SoundEffectTable DeepCopy()
    {
        var buff = SerializeUitlity.Serialize(this);
        LD_SoundEffectTable entity = SerializeUitlity.DeSerialize<LD_SoundEffectTable>(buff);
        return entity;
    }
}
