using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// 序列化工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 18:26:24")]
    [ClassPurposeDesc("序列化工具", "")]
#endif
    public static class SerializeUitlity
    {
        /// <summary>
        /// C#原生序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Serialize(object value)
        {
            if (value == null) return null;

            var ms = new MemoryStream();//创建编码解码的内存流对象
            var bw = new BinaryFormatter();//二进制流序列化对象
            //将obj对象序列化成二进制数据 写入到 内存流
            bw.Serialize(ms, value);
            var result = new byte[ms.Length];
            //将流数据 拷贝到结果数组
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            ms.Close();
            return result;
        }

        /// <summary>
        /// C#原生反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(byte[] value) where T : class, new()
        {
            if (value == null) return default(T);
            var ms = new MemoryStream(value);//创建编码解码的内存流对象 并将需要反序列化的数据写入其中
            var bw = new BinaryFormatter();//二进制流序列化对象

            //将流数据反序列化为obj对象
            var result = bw.Deserialize(ms);
            var t = result as T;
            ms.Close();
            return t;
        }

        public static T DeSerialize<T>(string path) where T : class, new()
        {
            var bytes = File.ReadAllBytes(path);
            var result = DeSerialize<T>(bytes);
            return result;
        }
    }
}
