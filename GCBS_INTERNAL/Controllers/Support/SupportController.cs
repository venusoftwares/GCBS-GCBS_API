

using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Support;
using GCBS_INTERNAL.Provider;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace GCBS_INTERNAL.Controllers.Support
{
    [CustomAuthorize]
    public class SupportController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        [System.Web.Http.Route("api/GetSupportType")]
        public async Task<IHttpActionResult> GetSupportType()
        {
            try
            {
                return Ok(await db.SupportType.ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/SumbitSupportType")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> SumbitSupportType(SupportType supportType)
        {
            try
            {
               
                db.SupportType.Add(supportType);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.Route("api/deleteSupportType/{id}")]
        [System.Web.Http.HttpDelete]
        public async Task<IHttpActionResult> DeleteSupportType(int id)
        {
            try
            {
                var list = db.SupportType.Find(id);
                var a = db.SupportType.Remove(list);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/SubmitSupportDetails")]
        public async Task<IHttpActionResult> SubmitSupportDetails(SupportDetails supportDetails)
        {
            try
            {
                SupportTable supportTable = new SupportTable
                {
                    CreatedDate = DateTime.Now,
                    Description = supportDetails.Description,
                    Status = 1,
                    SupportType = supportDetails.Type,
                    UserId = userDetails.Id,
                    UserName = userDetails.Username
                };
                db.Support.Add(supportTable);
                await db.SaveChangesAsync();
                return Ok(supportDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.Route("api/GetOpenSupportDetails")]
        public async Task<IHttpActionResult> GetOpenSupportDetails()
        {
            try
            {
                return Ok(await db.Support.Where(x => x.UserId == userDetails.Id && x.Status == 1).OrderByDescending(x=>x.CreatedDate).ToListAsync());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/GetOpenReportDetails")]
        public async Task<IHttpActionResult> GetOpenReportDetails()
        {
            try
            {
                return Ok(await db.Reports.Where(x => x.ReportFrom == userDetails.Id && x.Status == 1).OrderByDescending(x => x.CreatedOn).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.Route("api/GetClosedSupportDetails")]
        public async Task<IHttpActionResult> GetClosedSupportDetails()
        {
            try
            {
                return Ok(await db.Support.Where(x => x.UserId == userDetails.Id && x.Status == 2).OrderByDescending(x => x.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/GetClosedReportDetails")]
        public async Task<IHttpActionResult> GetClosedReportDetails()
        {
            try
            {
                return Ok(await db.Reports.Where(x => x.ReportFrom == userDetails.Id && x.Status == 2).OrderByDescending(x => x.CreatedOn).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public class SupportDetails
        {
            public string Type { get; set; }
            public string Description { get; set; }
        }
    }
  
}