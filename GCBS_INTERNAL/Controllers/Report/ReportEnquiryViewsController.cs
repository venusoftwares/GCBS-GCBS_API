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
    public class ReportEnquiryViewsController : BaseApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();

        [Route("api/getReportEnquiryViews/{fromdate}/{todate}")]
        public async Task<IHttpActionResult> GetReportView(string fromdate,string todate)
        {
            try
            { 
                List<ReportViewDetails> reportViewDetails = new List<ReportViewDetails>();
                var list = await db.Reports.Include(x => x.userManagementFrom)
                    .Include(x => x.userManagementTo).Where(x => x.Status == 1).OrderByDescending(x => x.CreatedOn).ToListAsync();

                foreach (var a in list)
                {
                    reportViewDetails.Add(new ReportViewDetails
                    {
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        Date = a.Date,
                        Description = "",
                        Id = a.Id,
                        reportAndSupportViewModels = ConvertJsonSupport(userDetails.Id, a.Description),
                        ReportFrom = a.ReportFrom,
                        ReportTo = a.ReportTo,
                        ReportType = a.ReportType,
                        Status = a.Status,
                        UpdatedBy = a.UpdatedBy,
                        UpdatedOn = a.UpdatedOn,
                        userManagementFrom = a.userManagementFrom,
                        userManagementTo = a.userManagementTo

                    });
                }


                return Ok(reportViewDetails);
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
    }
}
