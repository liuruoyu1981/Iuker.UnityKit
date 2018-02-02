var Iuker;
(function (Iuker) {
    var ViewModule = /** @class */ (function () {
        function ViewModule() {
        }
        /**
         * 获得指定视图id的视图对象。
         * @param viewId 目标视图的身份id。
         */
        ViewModule.GetView = function (viewId) {
            var view = Iuker_ViewModule_GetView(viewId);
            return view;
        };
        /**
         * 创建指定视图身份Id的目标视图。
         * @param viewId 目标视图的身份。
         */
        ViewModule.CreateView = function (viewId, assetId, isCache) {
            if (assetId === void 0) { assetId = null; }
            if (isCache === void 0) { isCache = true; }
            Iuker_ViewModule_CreateView(viewId, assetId, isCache);
        };
        /**
         * 获得一个C#层面注入的JintView实例，用于视图控件初始化。
         * @param viewId 目标视图的身份。
         */
        ViewModule.GetJintView = function (viewId) {
            var v = Iuker_ViewModule_GetJintView(viewId);
            return v;
        };
        ViewModule.GetViewModel = function (modelName, typeDesc) {
            if (this.ViewModels[modelName] == null) {
                Iuker.Debug.Log("\u76EE\u6807\u89C6\u56FE\u6570\u636E\u6A21\u578B" + modelName + "\u5F53\u524D\u4E3A\u7A7A\uFF0C\u5C06\u521B\u5EFA\u65B0\u5B9E\u4F8B\uFF01");
                var model = new typeDesc();
                model.Init();
                this.ViewModels[modelName] = model;
            }
            return this.ViewModels[modelName];
        };
        ViewModule.WatchViewLiefEvent = function (viewId, viewLifeType, lamada) {
            Iuker_ViewModule_WatchViewLiefEvent(viewId, viewLifeType, lamada);
        };
        /**
         * 关闭一个目标视图
         * @param viewId 视图身份Id
         */
        ViewModule.CloseView = function (viewId) {
            Iuker_ViewModule_CloseView(viewId);
        };
        /**
         * 打开一个会话视图
         * @param content   会话的文本内容
         * @param dialogViewId  目标会话视图资源Id同时也用于数据Id
         * @param jsFunction    js脚本函数
         * @param isShowCancel  是否显示取消按钮
         */
        ViewModule.OpenDialog = function (content, dialogViewId, jsFunction, isShowCancel) {
            Iuker_ViewModule_OpenDialog(content, dialogViewId, jsFunction, isShowCancel);
        };
        ViewModule.ActionProcessers = {};
        ViewModule.Views = {};
        ViewModule.PipelineProcessers = {};
        ViewModule.ViewModels = {};
        return ViewModule;
    }());
    Iuker.ViewModule = ViewModule;
})(Iuker || (Iuker = {}));
//# sourceMappingURL=view_module.js.map