using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Help
{
    public class Mail
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public static string SmtpServer = "";

        /// <summary>
        /// 邮箱号
        /// </summary>
        public static string FromMail = "";

        /// <summary>
        /// 登录帐号
        /// </summary>
        public static string LoginAccount = "";

        /// <summary>
        /// 登录密码
        /// </summary>
        public static string Loginpassword = "";

        public Mail()
        {

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="subject">邮箱标题</param>
        /// <param name="toMail">目标邮箱</param>
        /// <param name="listAttachment">附件</param>
        public static void SendMessage(string body, string subject, string toMail, List<Attachment> listAttachment = null)
        {
            SendMessage(body, subject, new string[] { toMail }, listAttachment);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="body">邮件内容</param>
        /// <param name="subject">邮箱标题</param>
        /// <param name="arrToMail">目标邮箱</param>
        /// <param name="listAttachment">附件</param>
        public static void SendMessage(string body, string subject, string[] arrToMail, List<Attachment> listAttachment = null)
        {
            var emailAcount = LoginAccount;
            var emailPassword = Loginpassword;
            MailMessage message = new MailMessage();
            //message.Headers.Add("X-Priority", "3");
            //message.Headers.Add("X-MSMail-Priority", "Normal");
            //message.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");
            //message.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
            //message.Headers.Add("ReturnReceipt", "1");
            message.Priority = MailPriority.High;
            MailAddress fromAddr = new MailAddress(FromMail, "目标科技");
            message.From = fromAddr;
            string[] arrDistincyMail = arrToMail.GroupBy(e => e).Select(g => g.Key).ToArray();
            foreach (string item in arrDistincyMail)
            {
                if (!string.IsNullOrEmpty(item))
                    message.To.Add(item.TrimEnd());
            }
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            if (listAttachment != null)
            {
                foreach (Attachment item in listAttachment)
                {
                    message.Attachments.Add(item);
                }
            }
            try
            {
                SmtpClient client = new SmtpClient(SmtpServer, 587);
                client.Credentials = new NetworkCredential(emailAcount, emailPassword);
                client.EnableSsl = true;
                client.Send(message);
            }
            catch { }
        }
    }
}
