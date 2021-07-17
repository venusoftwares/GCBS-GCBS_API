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
    public class SupportEnquiryViewsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/SupportEnquiryViews
        public IQueryable<SupportEnquiryView> GetSupportEnquiryView()
        {
            return db.SupportEnquiryView;
        }

        // GET: api/SupportEnquiryViews/5
        [ResponseType(typeof(SupportEnquiryView))]
        public async Task<IHttpActionResult> GetSupportEnquiryView(int id)
        {
            SupportEnquiryView supportEnquiryView = await db.SupportEnquiryView.FindAsync(id);
            if (supportEnquiryView == null)
            {
                return NotFound();
            }

            return Ok(supportEnquiryView);
        }

        // PUT: api/SupportEnquiryViews/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupportEnquiryView(int id, SupportEnquiryView supportEnquiryView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supportEnquiryView.Id)
            {
                return BadRequest();
            }

            db.Entry(supportEnquiryView).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupportEnquiryViewExists(id))
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

        // POST: api/SupportEnquiryViews
        [ResponseType(typeof(SupportEnquiryView))]
        public async Task<IHttpActionResult> PostSupportEnquiryView(SupportEnquiryView supportEnquiryView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SupportEnquiryView.Add(supportEnquiryView);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = supportEnquiryView.Id }, supportEnquiryView);
        }

        // DELETE: api/SupportEnquiryViews/5
        [ResponseType(typeof(SupportEnquiryView))]
        public async Task<IHttpActionResult> DeleteSupportEnquiryView(int id)
        {
            SupportEnquiryView supportEnquiryView = await db.SupportEnquiryView.FindAsync(id);
            if (supportEnquiryView == null)
            {
                return NotFound();
            }

            db.SupportEnquiryView.Remove(supportEnquiryView);
            await db.SaveChangesAsync();

            return Ok(supportEnquiryView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupportEnquiryViewExists(int id)
        {
            return db.SupportEnquiryView.Count(e => e.Id == id) > 0;
        }
    }
}