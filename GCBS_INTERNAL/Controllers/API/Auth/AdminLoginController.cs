using GCBS_INTERNAL.Helper;
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
    public class AdminLoginController : ApiController
    {
        public DatabaseContext db = new DatabaseContext();
        private readonly GetAccessToken getAccessToken = new GetAccessToken();
        private readonly SMTPService sMTPService = new SMTPService();
        private readonly Algorithum algorithum = new Algorithum();

        [ResponseType(typeof(AdminResponse))]
        public async Task<IHttpActionResult> AdminLogin(AdminLogin adminLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = db.UserManagement.Where(x => x.Username == adminLogin.Email && x.Password == adminLogin.Password).FirstOrDefault();
                if(result==null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new AdminResponse { AccessToken = await getAccessToken.GetToken(result) });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }    
        }

        [ResponseType(typeof(ForgetPassword))]
        public async Task<IHttpActionResult> ForgetPassword(ForgetPassword forgetPassword)
        {      
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userManagement =await db.UserManagement.Where(x => x.EmailId == forgetPassword.Email).FirstOrDefaultAsync();
                if(userManagement != null)
                {
                    userManagement.Password = algorithum.RandomPassword();
                    string htmlString = "Email:" + userManagement.EmailId + " Password:" + userManagement.Password;
                    sMTPService.Email(userManagement.EmailId, "Forget Password", htmlString);
                    db.Entry(userManagement).State = EntityState.Modified;
                    await db.SaveChangesAsync();  
                    return Ok(forgetPassword);
                }
                return Content(HttpStatusCode.BadRequest,"No User found");
            }
            catch(Exception ex)
            {
                throw ex;
            }   
        }    
    }
}
