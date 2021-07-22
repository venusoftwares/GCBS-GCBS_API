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

namespace GCBS_INTERNAL.Controllers.API
{
    [CustomAuthorize]
    public class ReportEnquiryViewsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/ReportEnquiryViews
        public IQueryable<ReportEnquiryView> GetReportEnquiryView()
        {
            return db.ReportEnquiryView.Include(x => x.PartnerManagements).Include(x => x.UserManagements);
        }

        // GET: api/ReportEnquiryViews/5
        [ResponseType(typeof(ReportEnquiryView))]
        public async Task<IHttpActionResult> GetReportEnquiryView(int id)
        {
            ReportEnquiryView reportEnquiryView = await db.ReportEnquiryView.FindAsync(id);
            if (reportEnquiryView == null)
            {
                return NotFound();
            }

            return Ok(reportEnquiryView);
        }

        // PUT: api/ReportEnquiryViews/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReportEnquiryView(int id, ReportEnquiryView reportEnquiryView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reportEnquiryView.Id)
            {
                return BadRequest();
            }

            db.Entry(reportEnquiryView).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportEnquiryViewExists(id))
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

        // POST: api/ReportEnquiryViews
        [ResponseType(typeof(ReportEnquiryView))]
        public async Task<IHttpActionResult> PostReportEnquiryView(ReportEnquiryView reportEnquiryView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ReportEnquiryView.Add(reportEnquiryView);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reportEnquiryView.Id }, reportEnquiryView);
        }

        // DELETE: api/ReportEnquiryViews/5
        [ResponseType(typeof(ReportEnquiryView))]
        public async Task<IHttpActionResult> DeleteReportEnquiryView(int id)
        {
            ReportEnquiryView reportEnquiryView = await db.ReportEnquiryView.FindAsync(id);
            if (reportEnquiryView == null)
            {
                return NotFound();
            }

            db.ReportEnquiryView.Remove(reportEnquiryView);
            await db.SaveChangesAsync();

            return Ok(reportEnquiryView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportEnquiryViewExists(int id)
        {
            return db.ReportEnquiryView.Count(e => e.Id == id) > 0;
        }
    }
}