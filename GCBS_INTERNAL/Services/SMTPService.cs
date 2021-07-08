using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace GCBS_INTERNAL.Services
{
    public class SMTPService
    {
        private DatabaseContext db = new DatabaseContext();
        public void Email(string To,string Subject,string htmlString)
        {   
            try
            {
                var result = db.EmailSettings.FirstOrDefault();
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(result.Username);
                message.To.Add(new MailAddress(To));
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = (int)result.Port;
                smtp.Host = result.HostAddress; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(result.Username, result.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }
    }
}