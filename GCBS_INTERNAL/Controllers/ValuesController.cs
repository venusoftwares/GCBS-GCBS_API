using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static GCBS_INTERNAL.WebApiConfig;

namespace GCBS_INTERNAL.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values  
        public IEnumerable Get()
        {
            Global.LogMessage("Data from Controller");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5  
        public string Get(string id)
        {
            Global.LogMessage("Request param : " + id);
            return "value";
        }
    }
}
