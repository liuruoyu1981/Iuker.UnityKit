using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// ���л�����
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 18:26:24")]
    [ClassPurposeDesc("���л�����", "")]
#endif
    public static class SerializeUitlity
    {
        /// <summary>
        /// C#ԭ�����л�
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Serialize(object value)
        {
            if (value == null) return null;

            var ms = new MemoryStream();//�������������ڴ�������
            var bw = new BinaryFormatter();//�����������л�����
            //��obj�������л��ɶ��������� д�뵽 �ڴ���
            bw.Serialize(ms, value);
            var result = new byte[ms.Length];
            //�������� �������������
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            ms.Close();
            return result;
        }

        /// <summary>
        /// C#ԭ�������л�
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(byte[] value) where T : class, new()
        {
            if (value == null) return default(T);
            var ms = new MemoryStream(value);//�������������ڴ������� ������Ҫ�����л�������д������
            var bw = new BinaryFormatter();//�����������л�����

            //�������ݷ����л�Ϊobj����
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
