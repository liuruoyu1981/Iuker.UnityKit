/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 7/24/2017 23:02
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

using Iuker.Common;
using Iuker.UnityKit.Run.Module.ReactiveDataModel.BaseModel;

namespace Iuker.UnityKit.Run.Module.ReactiveDataModel
{
    /// <summary>
    /// 响应式数据模型模块
    /// </summary>
    public interface IU3dReactiveDataModelModule : IModule
    {
        /// <summary>
        /// 网络通信答复事件处理
        /// </summary>
        /// <param name="protoId"></param>
        /// <param name="message"></param>
        void OnNetResponse(int protoId, object message);


        #region 数据模型快捷属性

        /// <summary>
        /// 玩家数据
        /// </summary>
        PlayerModel Player { get; }





        #endregion
    }
}