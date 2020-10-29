using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace vestibularCNG
{
    public class EmailNG
    {
        private static EmailNG instance;
        private EmailNG() { }
        public static EmailNG Instance
        {
            get
            {
                if (instance == null)
                    instance = new EmailNG();
                return instance;
            }
        }

        public void EnviarEmail(string email, string title, string msg)
        {

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("vestibularsenai@gmail.com");
            mail.To.Add(email);

            //define o conteúdo
            mail.Subject = title;
            mail.Body = msg;
            mail.IsBodyHtml = true;
            //envia a mensagem
            SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("luish075@gmail.com", "05052001"),
                EnableSsl = true,
            };
            smtp.Send(mail);
        }

    }
}
