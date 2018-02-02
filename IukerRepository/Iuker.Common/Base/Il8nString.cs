/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 8/5/2017 8:13
Email: liuruoyu1981@gmail.com
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

namespace Iuker.Common.Base
{
    /// <summary>
    /// 多语言字符串
    /// </summary>
    public class Il8nString
    {
        /// <summary>
        /// 关键字多语言字符串列表
        /// </summary>
        private static readonly Dictionary<string, List<Il8nString>> sKeyIl8nDictionary = new Dictionary<string, List<Il8nString>>();

        /// <summary>
        /// 多语言字符串列表
        /// </summary>
        private static readonly List<Il8nString> sAllIl8NStrings = new List<Il8nString>();

        /// <summary>
        /// 自身当前语言版本
        /// </summary>
        private Il8nType mVersion = Il8nType.Chinese;

        /// <summary>
        /// 自身多语言内容版本
        /// </summary>
        private readonly Dictionary<Il8nType, string> mSelfDictionary = new Dictionary<Il8nType, string>();

        public string Content { get { return mSelfDictionary[mVersion]; } }

        private void ChangeVersion(Il8nType il8NVersion) { mVersion = il8NVersion; }

        /// <summary>
        /// 设置指定版本的语言内容
        /// 如果修改内容和当前内容相同则不会做任何变更
        /// </summary>
        /// <param name="conent"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public Il8nString SetContent(string conent, Il8nType version = Il8nType.Chinese)
        {
            if (conent != null && mSelfDictionary[version] != conent)
            {
                mSelfDictionary[version] = conent;
            }
            return this;
        }

        private Il8nString() { }

        public static Il8nString Create(string key, string chinese = null, string english = null)
        {
            var il8NString = new Il8nString();
            il8NString.mSelfDictionary[Il8nType.Chinese] = chinese;
            il8NString.mSelfDictionary[Il8nType.English] = english;

            if (sKeyIl8nDictionary.ContainsKey(key))
            {
                sKeyIl8nDictionary[key].Add(il8NString);
            }
            else
            {
                var newIl8NStrings = new List<Il8nString> { il8NString };
                sKeyIl8nDictionary.Add(key, newIl8NStrings);
            }

            sAllIl8NStrings.Add(il8NString);

            return il8NString;
        }


        public static void ChangeVersion(Il8nType il8NVersion, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                sAllIl8NStrings.ForEach(il8n => il8n.ChangeVersion(il8NVersion));
            }
            else
            {
                var keyIl8ns = sKeyIl8nDictionary[key];
                keyIl8ns.ForEach(il8n => il8n.ChangeVersion(il8NVersion));
            }
        }
















    }
}