/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/21 18:13
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

using System.Collections.Generic;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.ReactiveDataModel
{
    /// <summary>
    /// 响应式数据模型
    /// </summary>
    public interface IReactiveDataModel
    {
        /// <summary>
        /// 数据模型所关注的网络协议Id列表
        /// </summary>
        List<int> ConcernedProtoIdList { get; }

        /// <summary>
        /// 响应式数据模型初始化
        /// </summary>
        /// <param name="frame"></param>
        void Init(IU3dFrame frame);

        /// <summary>
        /// 初始化数据模型所关注的协议id列表
        /// </summary>
        void InitConcernedProtoIdList();

        /// <summary>
        /// 关注的网络答复到达时
        /// </summary>
        /// <param name="message"></param>
        void OnNetResponse(object message);


    }
}