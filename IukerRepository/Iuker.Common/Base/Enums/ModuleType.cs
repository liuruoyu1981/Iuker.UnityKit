namespace Iuker.Common.Base.Enums
{
#if DEBUG
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 12:14:45")]
    [EmumPurposeDesc("模块类型", "模块类型")]
#endif
    public enum ModuleType
    {
        /// <summary>
        /// 热更新模块
        /// </summary>
        HotUpdate,

        /// <summary>
        /// 事件模块
        /// </summary>
        Event,

        /// <summary>
        /// 资源模块
        /// </summary>
        Asset,

        /// <summary>
        /// 路由模块
        /// </summary>
        Router,

        /// <summary>
        /// 依赖注入解析模块
        /// </summary>
        Inject,

        /// <summary>
        /// 响应式数据模型模块
        /// 强类型事件响应式的数据对象
        /// </summary>
        ReactiveDataModule,

        /// <summary>
        /// 数据模块
        /// </summary>
        Data,

        /// <summary>
        /// 调试器模块
        /// </summary>
        Debugger,

        /// <summary>
        /// 错误处理模块
        /// </summary>
        Error,

        /// <summary>
        /// 日志模块
        /// </summary>
        Log,

        /// <summary>
        /// 本地数据模块
        /// </summary>
        LocalData,

        /// <summary>
        /// JavaScript脚本模块
        /// </summary>
        JavaScript,

        /// <summary>
        /// 多语言模块
        /// </summary>
        Il8N,

        /// <summary>
        /// 视图模块
        /// </summary>
        View,

        /// <summary>
        /// Socket通信模块
        /// </summary>
        Socket,

        /// <summary>
        /// Http通信模块
        /// </summary>
        Http,

        /// <summary>
        /// 计时器模块
        /// </summary>
        Timer,

        /// <summary>
        /// 测试模块
        /// </summary>
        Test,

        /// <summary>
        /// 输入响应控制模块
        /// </summary>
        InputResponse,

        /// <summary>
        /// 管理器模块
        /// </summary>
        Manager,

        /// <summary>
        /// 视频模块
        /// </summary>
        Video,

        /// <summary>
        /// 音乐模块
        /// </summary>
        Music,

        /// <summary>
        /// 音效模块
        /// </summary>
        SoundEffect,

        /// <summary>
        /// 控制台模块
        /// </summary>
        Console,

        /// <summary>
        /// 场景模块
        /// </summary>
        Scene,

        /// <summary>
        /// 性能分析器模块
        /// </summary>
        Profiler,
    }
}
