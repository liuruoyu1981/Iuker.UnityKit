/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/18 14:18:46
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
    /// unity3d运行时配置
    /// </summary>
    [Serializable]
    public class RuntimeConfig
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public string PlayerAccount;

        /// <summary>
        /// 玩家密码
        /// </summary>
        public string PlayerPassword;

        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode;

        /// <summary>
        /// 项目基础公共配置
        /// </summary>
        public ProjectBaseConfig ProjectBaseConfig;

        /// <summary>
        /// 当前项目所有视图配置节点列表
        /// </summary>
//        public readonly List<View> ProjectViews = new List<View>();

        /// <summary>
        /// 当前项目所有场景配置节点列表
        /// </summary>
        public readonly List<Scene> ProjectScenes = new List<Scene>();

        

    }
}
