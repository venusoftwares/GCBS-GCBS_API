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
    public class SupportEnquiryViewsController : BaseApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();

        [Route("api/getSupportEnquiryViews/{fromdate}/{todate}")]
        public async Task<IHttpActionResult> GetReportView(string fromdate,string todate)
        {
            try
            {
                return Ok(await db.Support.ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/getFullProfileView/{id}")]
        public async Task<IHttpActionResult> GetFullProfileView(int id)
        {
            try
            {
                var res = await db.CustomerBooking
                    .Include(x => x.UserManagement)
                    .Include(x => x.CustomerManagement)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                return Ok(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
