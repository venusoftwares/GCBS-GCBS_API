using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;

namespace GCBS_INTERNAL.Controllers.API.EnquiryType
{
    [CustomAuthorize]
    public class ReportTypesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/ReportTypes
        public IQueryable<ReportType> GetReportType()
        {
            return db.ReportType;
        }

        // GET: api/ReportTypes/5
        [ResponseType(typeof(ReportType))]
        public async Task<IHttpActionResult> GetReportType(int id)
        {
            ReportType reportType = await db.ReportType.FindAsync(id);
            if (reportType == null)
            {
                return NotFound();
            }

            return Ok(reportType);
        }

        // PUT: api/ReportTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReportType(int id, ReportType reportType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reportType.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.ReportType.FindAsync(id);
                reportType.CreatedBy = re.CreatedBy;
                reportType.CreatedOn = re.CreatedOn;
                reportType.Status = re.Status;
                d.Dispose();
            }
            reportType.UpdatedBy = userDetails.Id;
            reportType.UpdatedOn = DateTime.Now;
            db.Entry(reportType).State = EntityState.Modified;
            db.Entry(reportType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReportType(ReportTypeVisible reportTypeVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reportTypeVisible == null)
            {
                return BadRequest();
            }
            ReportType reportType  = await db.ReportType.FindAsync(reportTypeVisible.Id);
            reportType.Status = reportTypeVisible.Status;
            reportType.UpdatedBy = userDetails.Id;
            reportType.UpdatedOn = DateTime.Now;
            db.Entry(reportType).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/ReportTypes
        [ResponseType(typeof(ReportType))]
        public async Task<IHttpActionResult> PostReportType(ReportType reportType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            reportType.CreatedBy = userDetails.Id;
            reportType.CreatedOn = DateTime.Now;
            reportType.Status = true;
            db.ReportType.Add(reportType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reportType.Id }, reportType);
        }

        // DELETE: api/ReportTypes/5
        [ResponseType(typeof(ReportType))]
        public async Task<IHttpActionResult> DeleteReportType(int id)
        {
            ReportType reportType = await db.ReportType.FindAsync(id);
            if (reportType == null)
            {
                return NotFound();
            }

            db.ReportType.Remove(reportType);
            await db.SaveChangesAsync();

            return Ok(reportType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportTypeExists(int id)
        {
            return db.ReportType.Count(e => e.Id == id) > 0;
        }
    }
}