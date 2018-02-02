/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/12 15:08:43
Email: 35490136@qq.com
QQCode: 35490136
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

namespace Iuker.Common.Module.Net.Http
{
    public abstract class AbsProtocol : IProtocol
    {
        protected AbsProtocol(byte moduleCode, ushort logicCode)
        {
            ModuleCode = moduleCode;
            LogicCode = logicCode;
        }

        public ProtocolSendType ProtocolSendType { get; private set; }

        public IHttpReq HttpReq { get; protected set; }

        public IHttpResp HttpResp { get; protected set; }

        public byte ModuleCode { get; private set; }

        public ushort LogicCode { get; private set; }

        public abstract void SendProtocol(Action<IProtocol> oopsatCallback = null);

        public abstract void OnCompleted(Dictionary<string, string> headers, IHttpResp resp);

        public abstract bool? GetBool(string key);

        public abstract int? GetInt(string key);

        public abstract float? GetFloat(string key);

        public abstract double? GetDouble(string key);

        public abstract string GetString(string key);

        public abstract IProtocol AppendData(string key, string data);

        public abstract IProtocol AppendData(string key, float data);

        public abstract IProtocol AppendData(string key, double data);

        public abstract IProtocol AppendData(string key, int data);

        public abstract T GetMessage<T>() where T : class, new();
    }
}
