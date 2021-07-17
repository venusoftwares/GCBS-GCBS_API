﻿using GCBS_INTERNAL.Helper;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
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
    public class CustomerOrPartnerLoginController : ApiController
    {
        public DatabaseContext db = new DatabaseContext();
        private readonly GetAccessToken getAccessToken = new GetAccessToken();


        //[HttpPost]
        [ResponseType(typeof(AdminResponse))]
        public async Task<IHttpActionResult> CustomerOrPartnerLogin(AdminLogin adminLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = db.UserManagement.Where(x => x.EmailId == adminLogin.Email
                && x.Password == adminLogin.Password).FirstOrDefault();
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    if (result.RoleId == 3 || result.RoleId == 9)
                    {
                        result.LastLogin = DateTime.Now;
                        result.LastActivateTime = DateTime.Now.AddMinutes(Constant.ExpireTime);
                        result.OnlineStatus = true;
                        db.Entry(result).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return Ok(new AdminResponse { AccessToken = await getAccessToken.GetToken(result), Key = result.RoleId == 3 ? "Partner" : "Customer" });
                    }
                    else
                    {
                        return Content(HttpStatusCode.InternalServerError, "Invalid Role");
                    }  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
