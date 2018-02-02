/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 7/24/2017 23:09
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

using System.Collections.Generic;
using Iuker.Common.DataTypes.ReactiveDatas;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.ReactiveDataModel.BaseModel
{
    /// <summary>
    /// 玩家数据模型
    /// </summary>
    public abstract class PlayerModel : IReactiveDataModel
    {
        #region 响应式数据字段

        /// <summary>
        /// 玩家名
        /// </summary>
        public readonly ReactiveString Name = new ReactiveString();

        /// <summary>
        /// 金钱数
        /// </summary>
        public readonly ReactiveInt32 GoldCount = new ReactiveInt32();


        #endregion

        public List<int> ConcernedProtoIdList { get; private set; }

        public abstract void Init(IU3dFrame frame);

        public abstract void InitConcernedProtoIdList();

        public abstract void OnNetResponse(object message);
    }
}