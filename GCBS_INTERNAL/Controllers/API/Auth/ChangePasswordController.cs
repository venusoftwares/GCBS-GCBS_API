using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Services;
using GCBS_INTERNAL.ViewModels;
using IERP.Algorithum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.Auth
{
    [Authorize]
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
                    userManagement.Password = changePassword.NewPassword;
                    //userManagement.Password = algorithum.RandomPassword();
                    string htmlString = "Email:" + userManagement.EmailId + " Password:" + userManagement.Password;
                    sMTPService.Email(userManagement.EmailId, "Change Password", htmlString);
                    db.Entry(userManagement).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok(new ChangePasswordReponse {  Message = "success"});
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
