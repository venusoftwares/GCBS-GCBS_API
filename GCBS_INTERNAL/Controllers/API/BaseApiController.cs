using GCBS_INTERNAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API
{
    public class BaseApiController : ApiController
    {
        protected UserManagement userDetails = new UserManagement();
        public BaseApiController()
        {
            var response = HttpContext.Current.User.Identity.Name;
            userDetails = JsonConvert.DeserializeObject<UserManagement>(response);
        }    
    }
}
