using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Auth
{
    public class PartnerSignUpController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DatabaseContext db = new DatabaseContext();
        public async Task<IHttpActionResult> PostPartnerSignUp(PartnerSignUp partnerSignUp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }   
                if (partnerSignUp == null)
                {
                    return BadRequest();
                }
                UserManagement userManagement = new UserManagement();
                userManagement.FirstName = partnerSignUp.FirstName;
                userManagement.SecondName = partnerSignUp.SecondName;
                userManagement.Name = partnerSignUp.NickName;
                userManagement.Username = partnerSignUp.NickName;
                userManagement.EmailId = partnerSignUp.Email;
                userManagement.MobileCountryCode = partnerSignUp.MobileCountryCode;
                userManagement.MobileNo = partnerSignUp.MobileNumber;
                userManagement.Password = partnerSignUp.Password;
                userManagement.DateOfBirth = partnerSignUp.Dob;
                userManagement.DateOfSignUp = DateTime.Now;
                userManagement.Gender = partnerSignUp.Gender;
                userManagement.RoleId = Constant.PARTNER_ROLE_ID;
                userManagement.Status = false;
                userManagement.Image = Constant.image;
                userManagement.CreatedOn = DateTime.Now;
                userManagement.CreatedBy = 1;
                db.UserManagement.Add(userManagement);
                await db.SaveChangesAsync();
                return Content(HttpStatusCode.Created,"created successfully");
            }
            catch (Exception ex)
            {
                log.Error("PostPartnerSignUp Exception", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
        }
    }
}
