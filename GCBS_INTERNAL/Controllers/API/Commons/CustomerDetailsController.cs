using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Commons
{
    [CustomAuthorize]
    public class CustomerDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        [HttpPut]
        [Route("api/UpdateCustomerImage")]
        public async Task<IHttpActionResult> UpdateImage(Base64string base64String)
        {
            var us = await db.UserManagement.FindAsync(userDetails.Id);
            us.Image = base64String.base64stringFormat;
            us.UpdatedBy = userDetails.Id;
            us.UpdatedOn = DateTime.Now;
            db.Entry(us).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/UserChangePassword")]
        public async Task<IHttpActionResult> UserChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var us = await db.UserManagement.FindAsync(userDetails.Id); 
            if(us.Password == changePasswordViewModel.OldPassword && changePasswordViewModel.OldPassword != changePasswordViewModel.NewPassword)
            {
                if(changePasswordViewModel.NewPassword == changePasswordViewModel.ReTypePassword)
                {
                    us.Password = changePasswordViewModel.NewPassword;
                    us.UpdatedBy = userDetails.UpdatedBy;
                    us.UpdatedOn = DateTime.Now;

                    db.Entry(us).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return StatusCode(HttpStatusCode.OK);
                }
                else
                {
                    
                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest );
            } 
           
        }
        public class Base64string
        {
           public string base64stringFormat { get; set; }
        }

        public class ChangePasswordViewModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }

            public string ReTypePassword { get; set; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
