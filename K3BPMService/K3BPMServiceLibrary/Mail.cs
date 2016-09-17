using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace K3BPMServiceLibrary
{
    public class Mail
    {
        public string SMTPServer = "";
        public int Port = 25;
        public bool EnableSSL = false;
        public string UserName = "";
        public string Password = "";
        public string FromAddress = "";
        public string ToAddress = "";
        public string Subject = "";
        public string Body = "";
        public string AttachmentFilePath = "";

        public void Send()
        {
            
            //SmtpClient SmtpServer = new SmtpClient(SMTPServer);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(FromAddress);
            string[] arrToAddress = ToAddress.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in arrToAddress)
            {
                mail.To.Add(item.Trim());
            }

            mail.Subject = Subject;
            mail.IsBodyHtml = true;
            mail.Body = Body;

            if (AttachmentFilePath.Trim().Length > 0)
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(AttachmentFilePath);
                mail.Attachments.Add(attachment);
            }

            //SmtpServer.Port = Port;
            //SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
            //SmtpServer.EnableSsl = EnableSSL;
            SmtpClient SmtpServer = new SmtpClient(SMTPServer, Port)
            {
                Credentials = new NetworkCredential(UserName, Password),
                EnableSsl = EnableSSL
            };

            SmtpServer.Send(mail);
        }
    }
}
