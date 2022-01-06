using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.ViewModels;
using Newtonsoft.Json;
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
                List<SupportViewDetails> supportViewDetails = new List<SupportViewDetails>();
                var list = await db.Support.OrderByDescending(x => x.CreatedDate).ToListAsync();

                foreach (var a in list)
                {
                    supportViewDetails.Add(new SupportViewDetails
                    {
                        CreatedDate = a.CreatedDate,
                        id = a.id,
                        SupportType = a.SupportType,
                        UpdatedDate = a.UpdatedDate,
                        UserId = a.UserId,
                        UserName = a.UserName,
                        reportAndSupportViewModels = ConvertJsonSupport(userDetails.Id, a.Description),
                        Status = a.Status,
                    });
                }
                return Ok(supportViewDetails); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<ReportAndSupportViewModel> ConvertJsonSupport(int id, string description)
        {
            var list = JsonConvert.DeserializeObject<List<ReportAndSupportViewModel>>(description);
            List<ReportAndSupportViewModel> reportAndSupportViewModels = new List<ReportAndSupportViewModel>();
            foreach (var a in list)
            {
                string side = "left";
                if (id == a.UserId)
                {
                    side = "right";
                }
                a.Side = side;
                reportAndSupportViewModels.Add(a);
            }
            return reportAndSupportViewModels;
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
