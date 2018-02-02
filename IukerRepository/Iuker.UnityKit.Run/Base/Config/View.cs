/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/18 14:30:40
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

namespace Iuker.UnityKit.Run.Base.Config
{
    /// <summary>
    /// 视图配置单节点
    /// </summary>
    [Serializable]
    public class View
    {
        public string ViewId = "ViewId";

        public string ViewType = "ViewType";

        public bool IsMain = false;

        public string AssetName;

        public bool IsBlankClose;

        public bool IsHideOther;

        public bool IsCloseTop;

        /// <summary>
        /// 寄生视图列表
        /// </summary>
        public List<string> ParasiticList;

        /// <summary>
        /// 遮罩视图Id
        /// </summary>
        public string MaskViewId;

    }



}
