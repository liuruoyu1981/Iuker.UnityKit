using Iuker.Common.Base.Interfaces;

namespace Iuker.Common.Module.Communication
{
    /// <summary>
    /// 通信调度器
    /// 请求调度
    /// 答复调度
    /// </summary>
    public interface ICommunicationDispatcher
    {
        /// <summary>
        /// 通信调度器初始化
        /// </summary>
        /// <param name="u3DFrame"></param>
        ICommunicationDispatcher Init(IFrame u3DFrame);

        /// <summary>
        /// 请求通信
        /// </summary>
        /// <param name="protoname"></param>
        void RequestCommunication(string protoname);

    }
}