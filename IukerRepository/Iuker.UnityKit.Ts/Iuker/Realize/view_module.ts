namespace Iuker {

    export class ViewModule {

        public static ActionProcessers = {};
        public static Views = {};
        public static PipelineProcessers = {};

        /**
         * 获得指定视图id的视图对象。
         * @param viewId 目标视图的身份id。
         */
        public static GetView(viewId: string): IJintView {
            var view = Iuker_ViewModule_GetView(viewId);
            return view;
        }

        /**
         * 创建指定视图身份Id的目标视图。
         * @param viewId 目标视图的身份。
         */
        public static CreateView(viewId: string, assetId: string = null, isCache: boolean = true): void {
            Iuker_ViewModule_CreateView(viewId, assetId, isCache);
        }

        /**
         * 获得一个C#层面注入的JintView实例，用于视图控件初始化。
         * @param viewId 目标视图的身份。
         */
        public static GetJintView(viewId: string): IJintView {

            let v = Iuker_ViewModule_GetJintView(viewId);
            return v;
        }

        private static ViewModels = {};

        public static GetViewModel<T extends IViewModel>(modelName: string, typeDesc: { new (): T; }): any {

            if (this.ViewModels[modelName] == null) {
                Debug.Log(`目标视图数据模型${modelName}当前为空，将创建新实例！`);
                let model = new typeDesc();
                model.Init();
                this.ViewModels[modelName] = model;
            }

            return this.ViewModels[modelName] as T;
        }

        public static WatchViewLiefEvent(viewId: string, viewLifeType: string, lamada: IWatchViewLifeEventLamada): void {

            Iuker_ViewModule_WatchViewLiefEvent(viewId, viewLifeType, lamada);

        }

        /**
         * 关闭一个目标视图
         * @param viewId 视图身份Id
         */
        public static CloseView(viewId: string): void {

            Iuker_ViewModule_CloseView(viewId);
        }

        /**
         * 打开一个会话视图
         * @param content   会话的文本内容
         * @param dialogViewId  目标会话视图资源Id同时也用于数据Id
         * @param jsFunction    js脚本函数
         * @param isShowCancel  是否显示取消按钮
         */
        public static OpenDialog(content: string, dialogViewId: string, jsFunction: any, isShowCancel: boolean): void {

            Iuker_ViewModule_OpenDialog(content, dialogViewId, jsFunction, isShowCancel);

        }

    }
}