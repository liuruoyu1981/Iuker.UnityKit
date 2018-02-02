using Iuker.Common;
using Iuker.Common.Base;
using Iuker.Common.Event;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// 可继承枚举-事件码
    /// </summary>
    [EnumTypeName("U3dEventCode")]
    public class U3dEventCode : EventCode
    {
        static U3dEventCode()
        {

        }

        protected U3dEventCode(ushort index, string name, string instanceEnumExplain)
        : base(index, name, instanceEnumExplain)
        {
        }

        private U3dEventCode(string name, string instanceEnumExplain)
        : base(name, instanceEnumExplain)
        {
        }

        private U3dEventCode() { }

        private static void StaticInit()
        {
            var temp = new U3dEventCode();
        }



        // 枚举项列表，在这里写入枚举选项。
        // 例如：public static readonly MyEnum Common_Login = new MyEnum("Common_Login","普通_登录"
        // 字段必须使用静态只读修饰符！
        // 字段变量名必须和构造函数的第一个参数一致！

        #region 框架

        /// <summary>
        /// 框架初始化完成
        /// </summary>
        [EnumExplain("框架初始化完成")]
        public static readonly U3dEventCode Frame_Inited = new U3dEventCode("Frame_Inited", "框架初始化完成");

        /// <summary>
        /// 安卓Apk下载进度更新
        /// </summary>
        [EnumExplain("安卓Apk下载进度更新")]
        public static readonly U3dEventCode Frame_ApkDownUpdate = new U3dEventCode("Frame_ApkDownUpdate", "安卓Apk下载进度更新");

        #endregion

        #region 视图

        [EnumExplain("视图初始化加载第一个视图")]
        public static readonly U3dEventCode View_InitFirst = new U3dEventCode("View_InitFirst", "视图初始化加载第一个视图");

        [EnumExplain("视图被挂载")]
        public static readonly U3dEventCode View_Mounted = new U3dEventCode("View_Mounted", "视图被挂载");

        [EnumExplain("有视图被关闭")]
        public static readonly U3dEventCode View_Closed = new U3dEventCode("View_Closed", "有视图被关闭");

        [EnumExplain("点击了表情按钮")]
        public static readonly U3dEventCode View_ClickFace = new U3dEventCode("View_ClickFace", "点击了表情按钮");

        #endregion

        #region 应用

        /// <summary>
        /// 多语言版本改变
        /// </summary>
        [EnumExplain("多语言版本改变")]
        public static readonly U3dEventCode App_Il8nChange = new U3dEventCode("App_Il8nChange", "多语言版本改变");

        /// <summary>
        /// 返回初始状态
        /// </summary>
        [EnumExplain("返回初始状态")]
        public static readonly U3dEventCode App_ReturnInit = new U3dEventCode("App_ReturnInit", "返回初始状态");

        /// <summary>
        /// 新手引导下发生交互
        /// </summary>
        [EnumExplain("新手引导下发生交互")]
        public static readonly U3dEventCode App_InGuideClick = new U3dEventCode("App_InGuideClick", "新手引导下发生交互");

        /// <summary>
        /// 资源热更新完成
        /// </summary>
        [EnumExplain("资源热更新完成")]
        public static readonly U3dEventCode App_HotUpdateComplete = new U3dEventCode("App_HotUpdateComplete", "资源热更新完成");

        #endregion

        #region 通信

        /// <summary>
        /// 首次建立连接
        /// </summary>
        [EnumExplain("首次建立连接")]
        public static readonly U3dEventCode Net_OnFirstConnected = new U3dEventCode("Net_OnFirstConnected", "首次建立连接");

        /// <summary>
        /// 重新建立连接
        /// </summary>
        [EnumExplain("重新建立连接")]
        public static readonly U3dEventCode Net_ReConnected = new U3dEventCode("Net_ReConnected", "重新建立连接");

        /// <summary>
        /// 连接成功
        /// </summary>
        [EnumExplain("连接成功")]
        public static readonly U3dEventCode Net_ConnectSucceed = new U3dEventCode("Net_ConnectSucceed", "连接成功");

        /// <summary>
        /// 网络错误尝试重连
        /// </summary>
        [EnumExplain("网络错误尝试重连")]
        public static readonly U3dEventCode Net_TryReConnect = new U3dEventCode("Net_TryReConnect", "网络错误尝试重连");

        /// <summary>
        /// 重连尝试到达最大次数
        /// </summary>
        [EnumExplain("重连尝试到达最大次数")]
        public static readonly U3dEventCode Net_GiveUpReConnect = new U3dEventCode("Net_GiveUpReConnect", "重连尝试到达最大次数");

        /// <summary>
        /// 网络消息到达(具体类型消息已获知时抛出)
        /// </summary>
        [EnumExplain("网络消息到达")]
        public static readonly U3dEventCode Net_MessageArrived = new U3dEventCode("Net_MessageArrived", "网络消息到达(具体类型消息已获知时抛出)");

        /// <summary>
        /// 有发送消息加入队列
        /// </summary>
        [EnumExplain("有发送消息加入队列")]
        public static readonly U3dEventCode Net_SendEnqueue = new U3dEventCode("Net_SendEnqueue", "有发送消息加入队列");

        /// <summary>
        /// 网络消息接收完成（完整长度的消息接收完成）
        /// </summary>
        [EnumExplain("网络消息接收完成（完整长度的消息接收完成）")]
        public static readonly U3dEventCode Net_Received = new U3dEventCode("Net_Received", "网络消息接收完成（完整长度的消息接收完成）");

        /// <summary>
        /// 连接超时
        /// </summary>
        [EnumExplain("连接超时")]
        public static readonly U3dEventCode Net_ConnectTimeOut = new U3dEventCode("Net_ConnectTimeOut", "连接超时");

        /// <summary>
        /// 通信错误
        /// </summary>
        [EnumExplain("通信错误")]
        public static readonly U3dEventCode Net_ErrorBreak = new U3dEventCode("Net_ErrorBreak", "通信错误");

        /// <summary>
        /// 开启通信调度
        /// </summary>
        [EnumExplain("开启通信调度")]
        public static readonly U3dEventCode Net_DispatchOpen = new U3dEventCode("Net_DispatchOpen", "开启通信调度");

        /// <summary>
        /// 关闭通信调度
        /// </summary>
        [EnumExplain("关闭通信调度")]
        public static readonly U3dEventCode Net_DispatchClose = new U3dEventCode("Net_DispatchClose", "关闭通信调度");

        #endregion

        #region 按键输入

        [EnumExplain("安卓返回键被点击")]
        public static readonly U3dEventCode Input_Escape = new U3dEventCode("Input_Escape", "安卓返回键被点击");


        #endregion


        /// <summary>
        /// 强制转换为ushort
        /// </summary>
        public static explicit operator U3dEventCode(ushort sourceIndex)
        {
            var temp = SonTypeInstanceIndexDictionary["U3dEventCode"] + 1;
            var count = (ushort)temp;
            if (sourceIndex > count) return null;
            return SonTypeInstanceListDictionary["U3dEventCode"][sourceIndex] as U3dEventCode;
        }

        public override string TypeName { get { return "U3dEventCode"; } }

        public override string ToString()
        {
            return Literals;
        }
    }
}
