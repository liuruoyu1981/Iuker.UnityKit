using System;
using System.Collections.Generic;
using Iuker.Common.Module.Socket;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// 根协议解析器
    /// 由其持有各个子项目的协议解析器，然后做统一的协议编号解析
    /// </summary>
    public class RootProtoResolver : IProtoIdResolver
    {
        private readonly List<IProtoIdResolver> AllProtoIdResolvers = new List<IProtoIdResolver>();

        /// <summary>
        /// 添加一个子项目的协议id解析器
        /// </summary>
        /// <param name="protoIdResolver"></param>
        public void AddSonProjectProtoIdResolver(IProtoIdResolver protoIdResolver)
        {
            AllProtoIdResolvers.Add(protoIdResolver);
        }

        public int GetProtoId(string protoname)
        {
            foreach (var protoIdResolver in AllProtoIdResolvers)
            {
                var id = protoIdResolver.GetProtoId(protoname);
                if (id == int.MaxValue)
                {
                    continue;
                }
                return id;
            }

            if (protoname == "IukMessageBase") return -1;

            throw new Exception(string.Format("名为{0}的协议没有找到对应的协议Id，请检查！", protoname));
        }

        public string GetProtoName(int CommandId)
        {
            foreach (var protoIdResolver in AllProtoIdResolvers)
            {
                var protoName = protoIdResolver.GetProtoName(CommandId);
                if (protoName == null)
                {
                    continue;
                }
                return protoName;
            }

            throw new Exception(string.Format("Id为{0}的协议没有找到对应的协议名，请检查！", CommandId));
        }

        public void Init()
        {
        }
    }
}