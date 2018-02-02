/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/12 23:58:37
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

using Iuker.Common.Module.Socket;

namespace Iuker.Common.Module.Net.Http
{
    /// <summary>
    /// 默认http通信数据模型编码解码器
    /// </summary>
    public class DefualtHttpModelEncoder : IHttpModelEncoder
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static readonly DefualtHttpModelEncoder Instance = new DefualtHttpModelEncoder();

        /// <summary>
        /// 网络答复数据模型编码
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public byte[] Encode(IHttpResp resp)
        {
            Binaryer binaryer = new Binaryer();
            binaryer.Write(resp.ModuleCode); // 写入模块码
            binaryer.Write(resp.SonModuleCode); // 写入子模块码
            binaryer.Write(resp.ErrorCode); // 写入错误码

            if (resp.MessageBytes != null)
            {
                binaryer.Write(resp.MessageBytes);
            }

            byte[] result = binaryer.GetBuff();
            binaryer.Close();
            return result;
        }

        /// <summary>
        /// 网络请求数据模型编码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public byte[] Encode(IHttpReq req)
        {
            Binaryer binaryer = new Binaryer();
            binaryer.Write(req.ModuleCode); // 写入模块码
            binaryer.Write(req.SonModuleCode); // 写入子模块码
            binaryer.Write(req.LogicCode); // 写入逻辑码

            if (req.MessageBytes != null)
            {
                binaryer.Write(req.MessageBytes);
            }

            byte[] result = binaryer.GetBuff();
            binaryer.Close();
            return result;
        }

        /// <summary>
        /// 网络请求数据模型解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHttpReq GetReq(byte[] value)
        {
            Binaryer binaryer = new Binaryer(value);
            byte moduleCode;
            byte sonModuleCode;
            ushort logicCode;
            byte[] messageBytes = null;

            //从数据中读取 三层协议  读取数据顺序必须和写入顺序保持一致
            binaryer.Read(out moduleCode);
            binaryer.Read(out sonModuleCode);
            binaryer.Read(out logicCode);

            if (binaryer.ResidueCount > 0)
            {
                binaryer.Read(out messageBytes, binaryer.Length - binaryer.Position);
            }

            IHttpReq httpReq = new HttpReq(moduleCode, logicCode, messageBytes, sonModuleCode);
            binaryer.Close();
            return httpReq;
        }

        /// <summary>
        /// 答复数据解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHttpResp GetResp(byte[] value)
        {
            Binaryer binaryer = new Binaryer();
            byte moduleCode;
            byte sonModuleCode;
            ushort errorCode;
            byte[] messageBytes = null;

            //从数据中读取 三层协议  读取数据顺序必须和写入顺序保持一致
            binaryer.Read(out moduleCode);
            binaryer.Read(out sonModuleCode);
            binaryer.Read(out errorCode);

            if (binaryer.ResidueCount > 0)
            {
                binaryer.Read(out messageBytes, binaryer.Length - binaryer.Position);
            }

            IHttpResp httpResp = new HttpResp(moduleCode, errorCode, messageBytes, sonModuleCode);
            binaryer.Close();
            return httpResp;
        }
    }
}
