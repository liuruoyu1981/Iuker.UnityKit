using System.Collections.Generic;

namespace Iuker.Common.Utility
{
    /// <summary>
    /// 本地数据工具
    /// </summary>
    public static class LdUitlity
    {
        /// <summary>
        /// 将一个符合格式的字符串列表转换为双层嵌套的字符串列表列表
        /// </summary>
        /// <param name="sourceList">字符串源列表</param>
        /// <param name="isDecrypt">是否需要加密</param>
        /// <returns></returns>
        public static List<string> ConvertLocalDataTxt(List<string> sourceList, bool isDecrypt = true)
        {
            var dataList = sourceList.RemoveAllLineFeed();

            if (isDecrypt)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    var line = dataList[i];
                    line = EncryptUitlity.Decrypt(line);
                    dataList[i] = line;
                }
            }

#if UNITY_ANDROID

            List<string> TxtList = new List<string>();
            TxtList.Clear();
            foreach (var content in dataList)
            {
                var temp = content.Replace("\r", "");
                TxtList.Add(temp);
            }
            return TxtList;

#else
            return sourceList;
#endif 
        }
    }
}