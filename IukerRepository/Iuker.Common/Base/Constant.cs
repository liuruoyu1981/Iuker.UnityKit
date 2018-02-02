/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/28 08:37:32
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

namespace Iuker.Common.Constant
{
    public static class Constant
    {
        /// <summary>
        /// CN：txt分隔符
        /// EN：txt separator
        /// </summary>
        public static readonly string[] TxtSeparators = { "[__]" };

        /// <summary>
        /// 获取当前系统时间月日分秒无分隔符偶数单位格式的数字格式表示，年份默认为当前年。
        /// 例如：0327180903
        /// </summary>
        public static string GetTimeToken
        {
            get
            {
                return DateTime.Now.ToString("MMddHHmmss");
            }
        }

        public static string DateAndTime
        {
            get
            {
                return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            }
        }

        /// <summary>
        /// 获得当前时间的时间戳格式表示字符串
        /// </summary>
        public static string TimeStamp
        {
            get
            {
                return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
            }
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public static string RandomStr
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

    }
}
