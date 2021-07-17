using GCBS_INTERNAL.Models;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool Email(string To,string Subject,string htmlString)
        {   
            try
            {
                log.Info("sending mail called : "+ To + ":" + Subject);
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
                log.Info("sending mail end : ");
                return true;
            }
            catch (Exception ex) 
            {
                log.Error("sending failed", ex);
                return false;
            }
        }
    }
}