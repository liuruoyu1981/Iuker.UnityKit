/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/28 10:17:30
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
using System.Linq;

namespace Iuker.Common.Utility
{
    /// <summary>
    /// 校验工具
    /// </summary>
    public static class VerifyUitlity
    {
        /// <summary>
        /// 验证邮箱地址是否合法
        /// </summary>
        /// <param name="email">要验证的Email</param>
        public static bool IsEmail(string email)
        {
            //如果为空，认为验证不合格
            if (string.IsNullOrEmpty(email)) return false;
            email = email.Trim();
            string valid = "abcdefghijklmnopqrstuvwxyz_.0123456789";
            if (email.IndexOf("@", StringComparison.Ordinal) < 0) return false;
            var list = email.Split('@');
            if (list[1].Length < 5) return false;
            if (list.Any(str => str.Length == 0)) return false;
            var index = list[1].Length - 4;
            if (list[1].Substring(index, 4) != ".com") return false;

            foreach (var str in list)
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                for (int j = 0; j < str.Length; j++)
                {
                    var strSon = str.Substring(j, 1).ToLower();
                    if (valid.IndexOf(strSon, StringComparison.Ordinal) < 0) return false;
                }
            }

            return true;
        }
    }
}