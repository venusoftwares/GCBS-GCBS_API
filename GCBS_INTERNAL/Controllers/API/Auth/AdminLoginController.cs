using GCBS_INTERNAL.Helper;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
using System;
using System.Collections.Generic;
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
    }
}
