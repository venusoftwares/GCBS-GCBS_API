using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Services;
using GCBS_INTERNAL.ViewModels;
using IERP.Algorithum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.Auth
{
    public class ForgetPasswordController : ApiController
    {
        private readonly SMTPService sMTPService = new SMTPService();
        private readonly Algorithum algorithum = new Algorithum();
        public DatabaseContext db = new DatabaseContext();
        
        [ResponseType(typeof(ForgetPassword))]
        public async Task<IHttpActionResult> ForgetPassword(ForgetPassword forgetPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userManagement = await db.UserManagement.Where(x => x.EmailId == forgetPassword.Email).FirstOrDefaultAsync();
                if (userManagement != null)
                {
                    string path = HttpContext.Current.Server.MapPath("~/Template/ForgetPassword.html"); 
                    string text = File.ReadAllText(path);
                    userManagement.Password = algorithum.RandomPassword();
                    string htmlString = text.Replace("{Email}", userManagement.EmailId).Replace("{Password}", userManagement.Password);
                    var res = sMTPService.Email(userManagement.EmailId, "Forget Password", htmlString);
                    if(res)
                    {
                        db.Entry(userManagement).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return Ok(forgetPassword);
                    }
                    else
                    {
                        return Content(HttpStatusCode.InternalServerError, "Sending failed");
                    }  
                }
                return Content(HttpStatusCode.BadRequest, "No User found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
