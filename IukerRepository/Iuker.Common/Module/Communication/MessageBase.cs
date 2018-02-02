namespace Iuker.Common.Module.Communication
{
    public class MessageBase
    {
        /// <summary>
        /// 业务协议长度
        /// </summary>
        public int BodyLength;

        /// <summary>
        /// 协议类型
        /// </summary>
        public byte ProtoType = 1;

        /// <summary>
        /// 协议流向Id
        /// 1. 客户端请求
        /// 2. 握手
        /// 3. 业务服务器答复
        /// </summary>
        public byte DirectionId = 1;

        /// <summary>
        /// 会话身份Id
        /// </summary>
        public long SessionId = -1;

        /// <summary>
        /// 模块Id
        /// </summary>
        public short ModuleId = -1;

        /// <summary>
        /// 业务服务器Id
        /// </summary>
        public short BusinessId = 1;

        /// <summary>
        /// 业务逻辑Id
        /// </summary>
        public short LogicId = -1;

        public byte[] BodyMessage;

        public MessageBase(int body, byte type, byte directId, long sessionId, short moduleId, short businessId,
            short logicId, byte[] bodyMessage)
        {
            BodyLength = body;
            ProtoType = type;
            DirectionId = directId;
            SessionId = sessionId;
            ModuleId = moduleId;
            BusinessId = businessId;
            LogicId = logicId;
            BodyMessage = bodyMessage;
        }


    }
}
