/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 22:31
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


using System.Text;

namespace Iuker.YourSharp.Common
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 清空一个字符串构建实例
        /// </summary>
        /// <param name="stringBuilder"></param>
        public static void Clear(this StringBuilder stringBuilder)
        {
            stringBuilder.Remove(0, stringBuilder.Length);
        }


    }
}
