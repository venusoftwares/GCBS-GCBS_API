using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Support;
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
                int passUserRole = 0; 

                if(userDetails.RoleId == 9)
                {
                    passUserRole = 3;
                }
                else if (userDetails.RoleId == 3)
                {
                    passUserRole = 9;
                }

                var list = await db.UserManagement.Where(x=>x.RoleId == passUserRole).ToListAsync();

                return Ok(list.Select(x=> new UserDetails {  Id = x.Id , Username = x.Username}));
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
                Reports reports = new Reports
                {
                    CreatedBy = userDetails.Id,
                    ReportFrom = userDetails.Id,
                    ReportTo = reportDetails.ReportTo,
                    Status = 1,
                    Description = reportDetails.Description,
                    CreatedOn = DateTime.Now,
                    Date = DateTime.Now ,
                    ReportType =reportDetails.Type

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
