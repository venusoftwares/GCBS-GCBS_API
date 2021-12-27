using GCBS_INTERNAL.Controllers.API;
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

namespace GCBS_INTERNAL.Controllers.Report
{
    [CustomAuthorize]
    public class ReportEnquiryViewsController : BaseApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();

        [Route("api/getReportEnquiryViews/{fromdate}/{todate}")]
        public async Task<IHttpActionResult> GetReportView(string fromdate,string todate)
        {
            try
            {
                return Ok(await db.Reports
                    .Include(x=>x.userManagementFrom)
                    .Include(x=>x.userManagementTo)
                    .Where(x=>x.Status == 1)
                    .OrderByDescending(x=>x.CreatedBy).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
