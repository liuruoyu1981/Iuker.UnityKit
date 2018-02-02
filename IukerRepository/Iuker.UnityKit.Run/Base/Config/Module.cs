/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/27 13:55:59
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

namespace Iuker.UnityKit.Run.Base.Config
{
    [Serializable]
    public class Module
    {
        /// <summary>
        /// 模块类型名
        /// </summary>
        public string ModuleType;

        /// <summary>
        /// 模块实例名
        /// </summary>
        public string CsharpType;

        /// <summary>
        /// Jint（JavaScript）模块类型
        /// </summary>
        public string JintType;

        /// <summary>
        /// 模块启动索引
        /// </summary>
        public int LaucherIndex = 9999;


    }
}
