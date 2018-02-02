/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/4/4 上午7:44:00
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
using System.Linq;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;


namespace Iuker.UnityKit.Run.Module.View.MVDA
{
    /// <summary>
    /// 默认视图数据模型调度器
    /// </summary>
    public sealed class DefaultViewModelDispatcher : IViewModelDispatcher
    {
        private IU3dFrame U3DFrame;

        /// <summary>
        /// 视图容器类型列表
        /// </summary>
        private List<Type> viewModelTypes;

        public IViewModel GetModel(string viewId)
        {
            var model = CreateViewModel(viewId);
            return model;
        }

        public IViewModelDispatcher Init(IU3dFrame u3DFrame)
        {
            U3DFrame = u3DFrame;
            viewModelTypes = ReflectionUitlity.GetTypeList<IViewModel>(U3DFrame.ProjectAssemblys);

            return this;
        }

        /// <summary>
        /// 当前程序集中所有视图数据模型类型字典
        /// </summary>
        private readonly Dictionary<string, List<Type>> viewModelTypeDictionary = new Dictionary<string, List<Type>>();

        private readonly Dictionary<string, IViewModel> CachaViewMoelDictionary = new Dictionary<string, IViewModel>();

        private IViewModel CreateViewModel(string viewId)
        {
            Type targetType;

            if (CachaViewMoelDictionary.ContainsKey(viewId))    //  如果视图数据模型实例已缓存，则返回缓存实例
            {
                return CachaViewMoelDictionary[viewId];
            }

            if (!viewModelTypeDictionary.ContainsKey(viewId))
            {
                var modeTypes = viewModelTypes.FindAll(t => t.Name.StartsWith(viewId)).OrderByDescending(t => t.Name).ToList();
                if (modeTypes.Count == 0)
                {
                    Debuger.LogWarning(string.Format("没有找到指定的视图数据模型类型： {0}_Model", viewId));
                    return null;
                }

                viewModelTypeDictionary.Add(viewId, modeTypes);
                targetType = modeTypes[0];
            }
            else
            {
                targetType = viewModelTypeDictionary[viewId][0];
            }

            var model = Activator.CreateInstance(targetType) as IViewModel;

            if (model != null)
            {
                model.Init(U3DFrame);
            }
            CachaViewMoelDictionary.Add(viewId, model);   // 缓存视图数据实例
            return model;
        }

    }
}
