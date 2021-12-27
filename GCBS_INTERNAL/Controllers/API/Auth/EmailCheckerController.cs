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
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.Auth
{
    public class EmailCheckerController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DatabaseContext db = new DatabaseContext();
        [HttpGet]
        [ResponseType(typeof(EmailResponse))]
        [Route("api/emailChecker/{email}")]
        public async Task<IHttpActionResult> GetEmailChecker(string email)
        {
            try
            {
                if(!db.UserManagement.Any(x=>x.EmailId==email))
                {
                    return Ok(new EmailResponse { Message = "Success" });

                }
                return Content(HttpStatusCode.NotAcceptable, new EmailResponse { Message = "Already Exits" });
            }
            catch(Exception ex)
            {
                log.Error("emailChecker Exception", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }    
        }
    }
}
