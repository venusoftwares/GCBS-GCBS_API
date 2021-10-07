using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Services;
using GCBS_INTERNAL.ViewModels;
using IERP.Algorithum;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Auth
{
    public class CustomerSignUpController : ApiController
    {
        public string urlLink = ConfigurationManager.AppSettings["ApiUrl"];
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DatabaseContext db = new DatabaseContext(); 
        private readonly SMTPService sMTPService = new SMTPService();
        private readonly Algorithum algorithum = new Algorithum();
        public async Task<IHttpActionResult> PostCustomerSignUp(CustomerSignUp customerSignUp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (customerSignUp == null)
                {
                    return BadRequest();
                }
                UserManagement userManagement = new UserManagement();
                userManagement.FirstName = customerSignUp.FirstName;
                userManagement.SecondName = customerSignUp.SecondName;
                userManagement.Name = customerSignUp.NickName;
                userManagement.Username = customerSignUp.NickName;
                userManagement.EmailId = customerSignUp.Email;
                userManagement.MobileCountryCode = customerSignUp.MobileCountryCode;
                userManagement.MobileNo = customerSignUp.MobileNumber;
                userManagement.Password = customerSignUp.Password;
                userManagement.DateOfBirth = customerSignUp.Dob;
                userManagement.DateOfSignUp = DateTime.Now;
                userManagement.Gender = customerSignUp.Gender;
                userManagement.RoleId = Constant.CUSTOMER_ROLE_ID;
                userManagement.CreatedOn = DateTime.Now;
                userManagement.CreatedBy = 1;
                userManagement.Status = false;
                userManagement.Image = Constant.image;
                db.UserManagement.Add(userManagement);
                await db.SaveChangesAsync();
                string path = HttpContext.Current.Server.MapPath("~/Template/EmailVerification.html");

                string text = File.ReadAllText(path);

                string userid = algorithum.Encrypt(userManagement.Id.ToString());  

                string htmlString = text

                    .Replace("{Email}", userManagement.EmailId)
                    .Replace("{url}", $"{urlLink}EmailVerification/token?code={userid}");

                var res = sMTPService.Email(userManagement.EmailId, "Welcome to Golden Circle", htmlString);

                return Content(HttpStatusCode.Created, "created successfully");
            }
            catch (Exception ex)
            {
                log.Error("PostCustomerSignUp Exception", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
        }
    }
}
