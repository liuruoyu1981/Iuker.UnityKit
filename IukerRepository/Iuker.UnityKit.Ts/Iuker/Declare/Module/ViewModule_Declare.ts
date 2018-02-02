//  视图模块

interface IWatchViewLifeEventLamada {

    (view: IJintView): void;
}

declare function Iuker_ViewModule_GetView(viewId: string): IJintView;
declare function Iuker_ViewModule_CreateView(viewId: string, assetId: string, isCache: boolean): void;
declare function Iuker_ViewModule_CloseView(viewId: string): void;
declare function Iuker_ViewModule_GetJintView(viewId: string): IJintView;
declare function Iuker_ViewModule_WatchViewLiefEvent(viewId: string, viewLifeType: string, lamada: IWatchViewLifeEventLamada): void;
declare function Iuker_ViewModule_OpenDialog(content: string, dialogViewId: string, jsFunction: any, isShowCancel: boolean): void;