using GCBS_INTERNAL.Models;
using IERP.Algorithum;
using log4net;
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
    { private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string FrontEndUrl = ConfigurationManager.AppSettings["FrontEndUrl"];

        private readonly Algorithum algorithum = new Algorithum();

        private DatabaseContext db = new DatabaseContext();

        // GET: EmailVerification
        [HttpGet]
        public async Task<RedirectResult> token(string code)
        {
            try
            {
                log.Info("EmailVerificationController call" + code);
                if (!string.IsNullOrEmpty(code))
                {
                    string userid = algorithum.Decrypt(code);

                    int userID = Convert.ToInt32(userid);

                    var userDetails = db.UserManagement.Find(userID);
                    userDetails.Status = true;
                    log.Info("EmailVerificationController code id " + userDetails.EmailId);
                    db.Entry(userDetails).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectPermanent(FrontEndUrl);
                }
                else
                {
                    log.Info("EmailVerificationController code id is not valid");
                    return RedirectPermanent("https://www.google.com");
                }
            }
           catch(Exception ex)
            {
                log.Error("EmailVerificationController code id is not valid" , ex);
                return RedirectPermanent("https://www.google.com");
            }
        }
    }
}