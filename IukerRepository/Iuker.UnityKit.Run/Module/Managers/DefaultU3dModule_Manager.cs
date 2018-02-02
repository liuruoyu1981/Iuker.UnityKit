/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/27 20:45
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
using Iuker.Common.Base.Enums;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config.Develop;

namespace Iuker.UnityKit.Run.Module.Managers
{
    /// <summary>
    /// 管理器模块
    /// 管理器定义为一个项目中具体逻辑实现
    /// </summary>
    public class DefaultU3dModule_Manager : AbsU3dModule, IU3dManagerModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Manager.ToString();
            }
        }

        private readonly Dictionary<string, IManager> _managers = new Dictionary<string, IManager>();

        protected override void onFrameInited()
        {
            base.onFrameInited();

            var types = ReflectionUitlity.GetTypeList<IManager>(U3DFrame.ProjectAssemblys).Where(
                t => t.Namespace != null &&
                t.Namespace.StartsWith(RootConfig.GetCurrentProject().ProjectName) &&
                !t.IsAbstract);
            var enumerable = types as Type[] ?? types.ToArray();
            U3DFrame.AppContext.CsManagerTypes.AddRange(enumerable.Select(t => t.Name).ToList());

            foreach (var type in enumerable)
            {
                var manager = Activator.CreateInstance(type) as IManager;
                if (manager == null)
                    throw new Exception(string.Format("目标管理器类型{0}为空", type.Name));

                _managers.Add(type.Name, manager);
                // 执行管理器的初始化
                manager.Init(U3DFrame);
            }
        }

        /// <summary>
        /// 获取一个管理器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetManager<T>() where T : class, IManager
        {
            var typename = typeof(T).Name;
            if (_managers.ContainsKey(typename))
            {
                return _managers[typename] as T;
            }

            throw new Exception(string.Format("指定的{0}管理器不存在！", typename));
        }
    }
}
