using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    ///
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 18:01:10")]
    [ClassPurposeDesc("", "")]
#endif
    public static class Md5Uitlity
    {
        /// <summary>
        /// 获得强化后的MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string input)
        {
            string hashKey = "Ry#(l,.[~88j99{>.<;'//:_key";
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                string hashCode = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(input)))
                                      .Replace("_", "") + BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(hashKey)))
                                      .Replace("_", "");

                return BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(hashCode)))
                    .Replace("_", "");
            }
        }

        /// <summary>
        /// 获得文件的md5值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMd5(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                throw new Exception(string.Format("目标文件{0}不存在！", filePath));
            }

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(fileStream);
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var t in retVal)
                    {
                        stringBuilder.Append(t.ToString("x2"));
                    }
                    return stringBuilder.ToString();
                }
            }
            catch (Exception exception)
            {
                Debuger.LogException("创建assetbundle文件Md5值失败，错误为：" + exception.Message);
                return null;
            }
        }
    }
}
