namespace Iuker.Common.Module.Socket
{
    /// <summary>
    /// 通信协议CommandId解析器
    /// </summary>
    public interface IProtoIdResolver
    {
        /// <summary>
        /// 返回指定协议类的commandId
        /// </summary>
        /// <param name="protoname"></param>
        /// <returns></returns>
        int GetProtoId(string protoname);

        /// <summary>
        /// 返回指定协议id的协议类名
        /// </summary>
        /// <param name="CommandId"></param>
        /// <returns></returns>
        string GetProtoName(int CommandId);

        void Init();

    }
}