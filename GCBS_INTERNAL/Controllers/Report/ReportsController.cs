using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Support;
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

    public class ReportsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        [Route("api/GetReportType")]
        public async Task<IHttpActionResult> GetSupportType()
        {
            try
            {
                return Ok(await db.ReportType.ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/GetUserDetails")]
        public async Task<IHttpActionResult> GetUserDetails()
        {
            try
            {
                List<UserManagement> userManagements = new List<UserManagement>();



                string str = "";

                if (userDetails.RoleId == 9)
                {
                    userManagements.AddRange(db.CustomerBooking.Include(x => x.UserManagement)
                        .Where(x => x.CustomerId == userDetails.Id).Select(x => x.UserManagement).Distinct().ToList());

                    str = "GP00";
                }
                else if (userDetails.RoleId == 3)
                {
                    userManagements.AddRange(db.CustomerBooking.Include(x => x.CustomerManagement)
                      .Where(x => x.ProviderId == userDetails.Id).Select(x => x.CustomerManagement).Distinct().ToList());

                    str = "GC00";
                }



                return Ok(userManagements.Select(x => new UserDetails
                {
                    Id = x.Id,
                    Username = $"{x.FirstName} ({str}{x.Id})"
                }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/SumbitReportTypes")]
        [HttpPost]
        public async Task<IHttpActionResult> SumbitReportTypes(ReportType reportType)
        {
            try
            {
                reportType.CreatedBy = userDetails.Id;
                reportType.CreatedOn = DateTime.Now;
                db.ReportType.Add(reportType);
                await db.SaveChangesAsync();
                return Ok(reportType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("api/SubmitReportDetails")]
        public async Task<IHttpActionResult> SubmitSupportDetails(ReportDetails reportDetails)
        {
            try
            {
                List<ReportAndSupportViewModel> reportAndSupportViewModels = new List<ReportAndSupportViewModel>()
                {
                       new ReportAndSupportViewModel
                       {
                           CreatedDate = DateTime.Now,
                           Message = reportDetails.Description,
                           Name = userDetails.FirstName,
                           UserId = userDetails.Id
                       }
                };
                ;
                Reports reports = new Reports
                {
                    CreatedBy = userDetails.Id,
                    ReportFrom = userDetails.Id,
                    ReportTo = reportDetails.ReportTo,
                    Status = 1,
                    Description = JsonConvert.SerializeObject(reportAndSupportViewModels),
                    CreatedOn = DateTime.Now,
                    Date = DateTime.Now,
                    ReportType = reportDetails.Type
                };
                db.Reports.Add(reports);
                await db.SaveChangesAsync();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class ReportDetails
        {
            public string Type { get; set; }
            public string Description { get; set; }
            public int ReportTo { get; set; }
        }
        public class UserDetails
        {
            public string Username { get; set; }
            public int Id { get; set; }
        }
    }
}
