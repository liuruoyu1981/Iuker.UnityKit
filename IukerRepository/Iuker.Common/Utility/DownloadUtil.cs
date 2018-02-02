using System;
using System.IO;
using System.Net;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    ///下载工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 17:30:55")]
    [ClassPurposeDesc("下载工具", "下载工具")]
#endif
    public static class DownloadUtil
    {
        public static bool FtpDownload(string filePath, string fileName, string ftpServerIp, string ftpUserName,
            string ftpPassword)
        {
            try
            {
                FileStream outputFileStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                var ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIp + "/" + fileName));
                ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                Stream ftpStream = ftpWebResponse.GetResponseStream();
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];

                if (ftpStream != null)
                {
                    var readCount = ftpStream.Read(buffer, 0, buffer.Length);
                    while (readCount > 0)
                    {
                        outputFileStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }
                    ftpStream.Close();
                }

                outputFileStream.Close();
                ftpWebResponse.Close();
                return true;
            }
            catch (Exception exception)
            {
                throw new Exception("Ftp下载发生了异常，异常信息为： " + exception.Message);
            }
        }

        /// <summary>
        /// Ftp上传
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="ftpServerIp">Ftp服务器地址</param>
        /// <param name="ftpUserName">Ftp连接用户名</param>
        /// <param name="ftpPassword">Ftp连接密码</param>
        public static void FtpUpload(string filePath, string fileName, string ftpServerIp, string ftpUserName,
            string ftpPassword)
        {
            FileInfo fileInfo = new FileInfo(filePath + "\\" + fileName);
            string uri = "ftp://" + ftpServerIp + "/" + fileName;

            var ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(uri));
            try
            {
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.ContentLength = fileInfo.Length;
                int buffLenth = 2048;
                byte[] buff = new byte[buffLenth];
                FileStream fs = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Stream stream = ftpWebRequest.GetRequestStream();
                var contentLen = fs.Read(buff, 0, buffLenth);
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLenth);
                }

                stream.Close();
                fs.Close();
            }
            catch (Exception exception)
            {
                throw new Exception("Ftp上传发生了异常，异常信息为： " + exception.Message);
            }
        }
    }
}
