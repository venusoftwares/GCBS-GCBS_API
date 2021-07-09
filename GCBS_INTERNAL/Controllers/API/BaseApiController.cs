using GCBS_INTERNAL.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API
{
    public class BaseApiController : ApiController
    {
        protected UserManagement userDetails = new UserManagement();
        private DatabaseContext context = new DatabaseContext();
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseApiController()
        {
            var response = HttpContext.Current.User.Identity.Name;
            userDetails = JsonConvert.DeserializeObject<UserManagement>(response);
        }
              
    }
}
