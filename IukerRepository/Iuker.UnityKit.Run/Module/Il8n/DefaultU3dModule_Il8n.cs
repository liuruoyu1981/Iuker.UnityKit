/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/27 22:31:32
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
using System.Linq;
using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Base.Enums;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config;
using RyThreeKindoms;

namespace Iuker.UnityKit.Run.Module.Il8n
{
    public class DefaultU3dModule_Il8n : AbsU3dModule, IU3dIl8nModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Il8N.ToString();
            }
        }

        /// <summary>
        /// 当前的多语言版本
        /// </summary>
        private Il8nVersion mCurrentVersion;

        /// <summary>
        /// 当前的多语言版本字符串字典
        /// </summary>
        private Dictionary<int, string> CurrentIl8NDictionary
        {
            get
            {
                return mIl8NVersionDictionary[mCurrentVersion];
            }
        }

        /// <summary>
        /// 当前的多语言版本字符串列表
        /// </summary>
        private List<string> CurrentIl8nList
        {
            get
            {
                return mIl8NListDictionary[mCurrentVersion];
            }
        }

        /// <summary>
        /// 所有子项目多语言数据按国籍的分类字典
        /// </summary>
        private readonly Dictionary<Il8nVersion, Dictionary<int, string>> mIl8NVersionDictionary = new Dictionary<Il8nVersion, Dictionary<int, string>>();

        /// <summary>
        /// 所有子项目多语言文本内容列表字典
        /// 用于查找指定多语言文本的数据索引
        /// 用于支持UI文本控件的多语言替换
        /// </summary>
        private readonly Dictionary<Il8nVersion, List<string>> mIl8NListDictionary = new Dictionary<Il8nVersion, List<string>>();

        //protected override void onFrameInited()
        //{
        //    base.onFrameInited();

        //    List<LD_Il8nTable> il8nTables = new List<LD_Il8nTable>();
        //    // 获取并合并所有子项目的多语言数据
        //    RootConfig.GetCurrentProject().AllSonProjects.ForEach(son => GetSonProjectIl8nData(son, il8nTables));
        //    // 将多语言数据列表转换为字典
        //    Il8nVersion.GetAllField().ForEach(il8n => SaveTargetVersionToDictionary(il8n, il8nTables));
        //}

        /// <summary>
        /// 保存指定版本的多语言数据到字典中
        /// </summary>
        /// <param name="il8NVersion"></param>
        /// <param name="il8NTables"></param>
        private void SaveTargetVersionToDictionary(Il8nVersion il8NVersion, List<LD_Il8nTable> il8NTables)
        {
            il8NVersion.Swith(il8NVersion.Literals)
                .Case(Il8nVersion.Chinese, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.Chinese, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.Chinese];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.Chinese);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.Chinese, targetVersion.Values.ToList());
                })
                .Case(Il8nVersion.English, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.English, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.English];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.English);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.English, targetVersion.Values.ToList());
                })
                .Case(Il8nVersion.Japanese, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.Japanese, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.Japanese];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.Japanese);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.Japanese, targetVersion.Values.ToList());
                })
                .Case(Il8nVersion.France, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.France, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.France];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.France);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.France, targetVersion.Values.ToList());
                })
                .Case(Il8nVersion.Germany, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.Germany, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.Germany];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.Germany);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.Germany, targetVersion.Values.ToList());
                })
                .Case(Il8nVersion.Korea, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.Korea, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.Korea];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.Korea);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.Korea, targetVersion.Values.ToList());
                })
                .Case(Il8nVersion.Spain, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.Spain, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.Spain];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.Spain);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.Spain, targetVersion.Values.ToList());
                })
                .Case(Il8nVersion.Italy, s =>
                {
                    mIl8NVersionDictionary.Add(Il8nVersion.Italy, new Dictionary<int, string>());
                    var targetVersion = mIl8NVersionDictionary[Il8nVersion.Italy];
                    foreach (var ldIl8NTable in il8NTables)
                    {
                        targetVersion.Add(ldIl8NTable.Index, ldIl8NTable.Italy);
                    }
                    mIl8NListDictionary.Add(Il8nVersion.Italy, targetVersion.Values.ToList());
                })
                .Default();
        }

        private void GetSonProjectIl8nData(SonProject sonProject, List<LD_Il8nTable> ilinIl8NTables)
        {
            var sonIl8nFileName = sonProject.CompexName.ToLower() + "_ldil8n";
            var il8nTables = U3DFrame.LocalDataModule.GetAllRecord<LD_Il8nTable>(sonIl8nFileName);
            if (il8nTables != null)
            {
                ilinIl8NTables.AddRange(il8nTables);
            }
        }

        public string GetTextByIndex(int index)
        {
            if (index < 0) return null;
            if (!CurrentIl8NDictionary.ContainsKey(index)) return null;
            var il8nText = CurrentIl8NDictionary[index];
            return il8nText;
        }

        public int GetIndex(string text)
        {
            if (!CurrentIl8nList.Contains(text)) return Int32.MaxValue;
            var index = CurrentIl8nList.FindIndex(content => content == text);
            return index;
        }

        public void SetVersion(Il8nVersion version)
        {
            mCurrentVersion = version;
        }
    }
}

