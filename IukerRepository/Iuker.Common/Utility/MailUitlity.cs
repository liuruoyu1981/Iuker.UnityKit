using System;
using System.Net;
using System.Net.Mail;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    /// 邮件工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 17:51:48")]
    [ClassPurposeDesc("", "")]
#endif
    public static class MailUitlity
    {
        /// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <param name="mailMessage">邮件消息对象</param>
        /// <param name="formAccount">发送方邮箱账号</param>
        /// <param name="formMailPassword">发送方邮箱密码</param>
        /// <param name="formSmtp">发送方smtp服务器地址</param>
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
                throw new Exception(string.Format("邮件发送出现异常，异常为{0}", exception.Message));
            }
        }
    }
}
