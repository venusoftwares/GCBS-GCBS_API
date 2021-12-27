using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Admin
{
    public class AdminCommonController : BaseApiController
    {
        [Route("api/UpdateUserAccessStatus/UserId/{userid}/Status/{status}")]
        [HttpGet]
        public IHttpActionResult UpdateUserAccessStatus(int userid,int status )
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    UserManagement userManagement = db.UserManagement.Find(userid);
                    userManagement.AccessStatus = status;
                    if (status == 1)
                    {
                        userManagement.Status = true;
                    }
                    if (status == 2)
                    {
                        userManagement.Status = false;
                    }
                    userManagement.AccessStatus = status;
                    db.Entry(userManagement).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Ok();
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
