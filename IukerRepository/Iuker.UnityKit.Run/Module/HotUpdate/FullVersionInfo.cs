using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Base;


namespace Iuker.UnityKit.Run.Module.HotUpdate
{
#if DEBUG
    /// <summary>
    /// 整包更新形式的版本数据
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170912 18:11:02")]
    [ClassPurposeDesc("整包更新形式的版本数据", "整包更新形式的版本数据")]
#endif
    [Serializable]
    public class FullApkInfo
    {
        public int VersionId;
        public string CreateDate;

        public FullApkInfo Next()
        {
            var info = new FullApkInfo();
            info.VersionId = VersionId + 1;
            info.CreateDate = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
            return info;
        }

        /// <summary>
        /// 创建一个新的整包更新版本数据实例
        /// 版本号将为1
        /// </summary>
        public FullApkInfo()
        {
            VersionId = 1;
            CreateDate = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
        }
    }

    [Serializable]
    public class FullApkInfos
    {
        public List<FullApkInfo> AllInfos;
        public FullApkInfo Last { get { return AllInfos.Last(); } }

        /// <summary>
        /// 创建一个新的整包更新数据
        /// </summary>
        public FullApkInfo CreateNew()
        {
            FullApkInfo newInfo = null;

            if (AllInfos == null)
            {
                AllInfos = new List<FullApkInfo>();
            }

            if (AllInfos.Count > 0)
            {
                newInfo = Last.Next();
                AllInfos.Add(newInfo);
            }
            else
            {
                newInfo = new FullApkInfo();
                AllInfos.Add(newInfo);
            }

            Debuger.Log("创建了新的整包更新数据");
            return newInfo;
        }

    }











}
