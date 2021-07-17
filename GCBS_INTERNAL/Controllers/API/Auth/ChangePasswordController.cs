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
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API.Auth
{
     [CustomAuthorize]
    public class ChangePasswordController : BaseApiController
    {
        private readonly SMTPService sMTPService = new SMTPService();
        private readonly Algorithum algorithum = new Algorithum();
        public DatabaseContext db = new DatabaseContext();

        [ResponseType(typeof(ChangePasswordReponse))]
        public async Task<IHttpActionResult> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userManagement = await db.UserManagement.Where(x => x.EmailId == userDetails.EmailId && x.Password == changePassword.OldPassword).FirstOrDefaultAsync();
                if (userManagement != null)
                {
                    string path = HttpContext.Current.Server.MapPath("~/Template/ChangePassword.html");
                    string text = File.ReadAllText(path);
                    userManagement.Password = changePassword.NewPassword;
                    //userManagement.Password = algorithum.RandomPassword();
                    string htmlString = text.Replace("{Email}", userManagement.EmailId).Replace("{Password}", userManagement.Password);
                    var res = sMTPService.Email(userManagement.EmailId, "Change Password", htmlString);
                    if(res)
                    {
                        db.Entry(userManagement).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return Ok(new ChangePasswordReponse { Message = "success" });
                    }
                    else
                    {
                        return Content(HttpStatusCode.InternalServerError, new ChangePasswordReponse { Message = "Sending failed" });
                    }   
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, new ChangePasswordReponse { Message = "Incorrect Old password" });
                }  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
