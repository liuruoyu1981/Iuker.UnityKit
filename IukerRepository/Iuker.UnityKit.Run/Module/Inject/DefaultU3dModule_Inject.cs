/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/17/2017 11:55
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
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.DataTypes.ReactiveDatas;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Module.View.MVDA;
using Iuker.UnityKit.Run.Module.View.ViewWidget;

namespace Iuker.UnityKit.Run.Module.Inject
{
    /// <summary>
    /// 默认依赖注入解析模块
    /// </summary>
    public class DefaultU3dModule_Inject : AbsU3dModule, IU3dInjectModule
    {
        private readonly Dictionary<string, Type> _produceTypesDictionary = new Dictionary<string, Type>();

        public override void Init(IFrame frame)
        {
            base.Init(frame);

            InitInject();
        }

        protected virtual void InitInject()
        {
            BindType<IViewActionRequest<IView>>(typeof(ViewActionRequest<IView>));
            BindType<IViewActionRequest<IButton>>(typeof(ViewActionRequest<IButton>));
            BindType<IViewActionRequest<IInputField>>(typeof(ViewActionRequest<IInputField>));
            BindType<IViewActionRequest<IToggle>>(typeof(ViewActionRequest<IToggle>));
            BindType<IViewActionRequest<ISlider>>(typeof(ViewActionRequest<ISlider>));
            BindType<IReactiveStruct<int>>(typeof(ReactiveInt32));
            BindType<IReactiveStruct<long>>(typeof(ReactiveLong));
            BindType<IReactiveStruct<string>>(typeof(ReactiveString));
            BindType<IReactiveStruct<float>>(typeof(ReactiveFloat));

            //BindType<IDynamicParseRunner>(typeof(DefaultDynamicParseRunner));
        }

        /// <summary>
        /// 绑定一个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        private void BindType<T>(Type type)
        {
            var typename = typeof(T).FullName;
            if (!_produceTypesDictionary.ContainsKey(typename))
            {
                _produceTypesDictionary.Add(typename, type);
            }
        }

        public T GetInstance<T>()
        {
            var typename = typeof(T).FullName;
            if (_produceTypesDictionary.ContainsKey(typename))
            {
                Type createType = _produceTypesDictionary[typename];
                T t = (T)Activator.CreateInstance(createType);
                return t;
            }
            return default(T);
        }

        public override string ModuleName
        {
            get
            {
                return ModuleType.Inject.ToString();
            }
        }
    }
}