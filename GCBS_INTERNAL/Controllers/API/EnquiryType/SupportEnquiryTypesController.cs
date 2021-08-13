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
    public class SupportEnquiryTypesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/SupportEnquiryTypes
        public IQueryable<SupportEnquiryType> GetSupportEnquiryTypes()
        {
            return db.SupportEnquiryTypes;
        }

        // GET: api/SupportEnquiryTypes/5
        [ResponseType(typeof(SupportEnquiryType))]
        public async Task<IHttpActionResult> GetSupportEnquiryType(int id)
        {
            SupportEnquiryType supportEnquiryType = await db.SupportEnquiryTypes.FindAsync(id);
            if (supportEnquiryType == null)
            {
                return NotFound();
            }

            return Ok(supportEnquiryType);
        }

        // PUT: api/SupportEnquiryTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupportEnquiryType(int id, SupportEnquiryType supportEnquiryType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supportEnquiryType.Id)
            {
                return BadRequest();
            }

            using (var d = new DatabaseContext())
            {
                var re = await d.SupportEnquiryTypes.FindAsync(id);
                supportEnquiryType.CreatedBy = re.CreatedBy;
                supportEnquiryType.CreatedOn = re.CreatedOn;
                supportEnquiryType.Status = re.Status;
                d.Dispose();
            }
            supportEnquiryType.UpdatedBy = userDetails.Id;
            supportEnquiryType.UpdatedOn = DateTime.Now;
            db.Entry(supportEnquiryType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupportEnquiryTypeExists(id))
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
        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupportEnquiryType(SupportEnquiryTypeVisible supportEnquiryTypeVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (supportEnquiryTypeVisible == null)
            {
                return BadRequest();
            }
            SupportEnquiryType supportEnquiryType = await db.SupportEnquiryTypes.FindAsync(supportEnquiryTypeVisible.Id);
            supportEnquiryType.Status = supportEnquiryTypeVisible.Status;
            supportEnquiryType.UpdatedBy = userDetails.Id;
            supportEnquiryType.UpdatedOn = DateTime.Now;
            db.Entry(supportEnquiryType).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/SupportEnquiryTypes
        [ResponseType(typeof(SupportEnquiryType))]
        public async Task<IHttpActionResult> PostSupportEnquiryType(SupportEnquiryType supportEnquiryType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            supportEnquiryType.CreatedBy = userDetails.Id;
            supportEnquiryType.CreatedOn = DateTime.Now;
            db.SupportEnquiryTypes.Add(supportEnquiryType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = supportEnquiryType.Id }, supportEnquiryType);
        }

        // DELETE: api/SupportEnquiryTypes/5
        [ResponseType(typeof(SupportEnquiryType))]
        public async Task<IHttpActionResult> DeleteSupportEnquiryType(int id)
        {
            SupportEnquiryType supportEnquiryType = await db.SupportEnquiryTypes.FindAsync(id);
            if (supportEnquiryType == null)
            {
                return NotFound();
            }

            db.SupportEnquiryTypes.Remove(supportEnquiryType);
            await db.SaveChangesAsync();

            return Ok(supportEnquiryType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupportEnquiryTypeExists(int id)
        {
            return db.SupportEnquiryTypes.Count(e => e.Id == id) > 0;
        }
    }
}