/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 17:04:54
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

using Iuker.Common;
using Iuker.Common.Base;

namespace Iuker.UnityKit.Run.Module.Il8n
{
    public interface IU3dIl8nModule : IModule
    {
        /// <summary>
        /// 根据Il8N编码获得当前版本的多语言文字数据
        /// </summary>
        /// <param name="index"></param>
        string GetTextByIndex(int index);

        /// <summary>
        /// 获得指定文本的多语言索引
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        int GetIndex(string text);

        /// <summary>
        /// 设置多语言版本并实时替换内容
        /// </summary>
        /// <param name="version"></param>
        void SetVersion(Il8nVersion version);
    }
}
