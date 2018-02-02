/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 7/24/2017 23:01
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

using System;
using System.Collections.Generic;
using System.Linq;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.ReactiveDataModel.BaseModel;

namespace Iuker.UnityKit.Run.Module.ReactiveDataModel
{
    /// <summary>
    /// 响应式数据模型模块基础实现
    /// </summary>
    public class DefaultU3dModule_ReactiveDataModel : AbsU3dModule, IU3dReactiveDataModelModule
    {
        private string mModuleName;

        public override string ModuleName
        {
            get
            {
                return mModuleName ?? (mModuleName = ModuleType.ReactiveDataModule.ToString());
            }
        }

        public override void Init(IFrame frame)
        {
            base.Init(frame);

            InitAllReactiveDataModel();
            InitReactiveDataModelMap();
        }


        /// <summary>
        /// 项目中响应式数据模型类型字典
        /// </summary>
        private Dictionary<string, Type> mReactiveDataModelTypeDictionary;

        /// <summary>
        /// 项目中响应式数据模型实例字典
        /// </summary>
        private readonly Dictionary<string, IReactiveDataModel> mReactiveDataModelDictionary = new Dictionary<string, IReactiveDataModel>();

        /// <summary>
        /// 项目中响应式数据模型实例列表字典
        /// 用于建立和通信答复的映射关系
        /// </summary>
        private readonly Dictionary<int, List<IReactiveDataModel>> mReactiveDataModelsMap = new Dictionary<int, List<IReactiveDataModel>>();

        /// <summary>
        /// 初始化当前项目中的所有响应式数据模型
        /// </summary>
        private void InitAllReactiveDataModel()
        {
            var currentProject = RootConfig.GetCurrentProject().ProjectName;
            mReactiveDataModelTypeDictionary = ReflectionUitlity.GetTypeDictionary<IReactiveDataModel>(U3DFrame.ProjectAssemblys, t => t.Namespace != null && t.Namespace.StartsWith(currentProject));

            foreach (KeyValuePair<string, Type> keyValuePair in mReactiveDataModelTypeDictionary)
            {
                var dataModel = Activator.CreateInstance(keyValuePair.Value) as IReactiveDataModel;
                if (dataModel == null)
                    throw new ArgumentNullException("dataModel");

                dataModel.Init(U3DFrame);
                dataModel.InitConcernedProtoIdList();
                mReactiveDataModelDictionary.Add(keyValuePair.Key, dataModel);
            }
        }

        /// <summary>
        /// 建立数据模型和关注协议号的映射关系
        /// </summary>
        private void InitReactiveDataModelMap()
        {
            var tempModels = mReactiveDataModelDictionary.Values.ToList();
            foreach (var dataModel in tempModels)
            {
                if (dataModel.ConcernedProtoIdList == null) continue;

                foreach (var protoId in dataModel.ConcernedProtoIdList)
                {
                    if (mReactiveDataModelsMap.ContainsKey(protoId))
                    {
                        var modelList = mReactiveDataModelsMap[protoId];
                        modelList.Add(dataModel);
                    }
                    else
                    {
                        mReactiveDataModelsMap.Add(protoId, new List<IReactiveDataModel> { dataModel });
                    }
                }
            }
        }

        public void OnNetResponse(int protoId, object message)
        {
            if (mReactiveDataModelsMap.ContainsKey(protoId))
            {
                var models = mReactiveDataModelsMap[protoId];
                models.ForEach(m => m.OnNetResponse(message));
            }
        }

        #region 数据模型快捷属性

        /// <summary>
        /// 玩家数据
        /// </summary>
        public PlayerModel Player { get; protected set; }



        #endregion



    }
}