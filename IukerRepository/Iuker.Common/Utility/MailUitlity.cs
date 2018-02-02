using System;
using System.Net;
using System.Net.Mail;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// �ʼ�����
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 17:51:48")]
    [ClassPurposeDesc("", "")]
#endif
    public static class MailUitlity
    {
        /// <summary>
        /// ͬ�������ʼ�
        /// </summary>
        /// <param name="mailMessage">�ʼ���Ϣ����</param>
        /// <param name="formAccount">���ͷ������˺�</param>
        /// <param name="formMailPassword">���ͷ���������</param>
        /// <param name="formSmtp">���ͷ�smtp��������ַ</param>
        public static void SendMail(MailMessage mailMessage, string formAccount, string formMailPassword,
            string formSmtp)
        {
            try
            {
                if (mailMessage == null) return;
                var smtpClient = new SmtpClient
                {
                    Credentials = (ICredentialsByHost)new NetworkCredential(formAccount, formMailPassword),
                    Host = formSmtp
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("�ʼ����ͳ����쳣���쳣Ϊ{0}", exception.Message));
            }
        }
    }
}
