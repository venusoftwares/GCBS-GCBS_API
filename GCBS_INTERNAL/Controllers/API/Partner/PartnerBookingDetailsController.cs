using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Partner
{
    [CustomAuthorize]
    public class PartnerBookingDetailsController : BaseApiController
    {
        public async Task<IHttpActionResult> GetPartnerBookingDetails(int id)
        {

            return Ok();
        }

    }
}
//All - 0
//Pending - 1
//Inprogress - 2
//Completed - 3
//Rejected - 4
//Cancelled - 5
//Completed - 6