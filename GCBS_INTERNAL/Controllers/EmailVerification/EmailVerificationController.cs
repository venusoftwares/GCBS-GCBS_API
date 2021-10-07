using GCBS_INTERNAL.Models;
using IERP.Algorithum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GCBS_INTERNAL.Controllers.EmailVerification
{
    public class EmailVerificationController : Controller
    {
        public string FrontEndUrl = ConfigurationManager.AppSettings["FrontEndUrl"];

        private readonly Algorithum algorithum = new Algorithum();

        private DatabaseContext db = new DatabaseContext();

        // GET: EmailVerification
        [HttpGet]
        public async Task<RedirectResult> token(string code)
        {
            if(!string.IsNullOrEmpty(code))
            {
                string userid = algorithum.Decrypt(code);

                int userID = Convert.ToInt32(userid);

                var userDetails =  db.UserManagement.Find(userID);
                userDetails.Status = true;

                db.Entry(userDetails).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectPermanent(FrontEndUrl);
            }
            else
            {
                return RedirectPermanent("https://www.google.com");
            } 
        }
    }
}