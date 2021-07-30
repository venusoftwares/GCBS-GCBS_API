using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Commons
{
    public class CommonDropController : ApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();
        [HttpGet]
        [Route("api/DropDownContactEnquiryType")]
        public List<DropDownCommon> DropDownContactEnquiryType()
        {
            var result = db.ContactEnquiryTypes
                .Where(x => x.Status)
                .Select(x => new DropDownCommon { Key = x.Id, Value = x.ContactEnquiryType1 }).ToList();
            return result;
        }
    }
}
