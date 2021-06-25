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

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class PartnerTypesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PartnerTypes
        public IQueryable<PartnerType> GetPartnerType()
        {
            return db.PartnerType;
        }

        // GET: api/PartnerTypes/5
        [ResponseType(typeof(PartnerType))]
        public async Task<IHttpActionResult> GetPartnerType(int id)
        {
            PartnerType partnerType = await db.PartnerType.FindAsync(id);
            if (partnerType == null)
            {
                return NotFound();
            }

            return Ok(partnerType);
        }

        // PUT: api/PartnerTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPartnerType(int id, PartnerType partnerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != partnerType.Id)
            {
                return BadRequest();
            }

            db.Entry(partnerType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerTypeExists(id))
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
        public async Task<IHttpActionResult> PutPartnerType(PartnerTypeVisible partnerTypeVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (partnerTypeVisible == null)
            {
                return BadRequest();
            }
            PartnerType partnerType = await db.PartnerType.FindAsync(partnerTypeVisible.Id);
            partnerType.Status = partnerTypeVisible.Status;
            partnerType.UpdatedBy = userDetails.Id;
            partnerType.UpdatedOn = DateTime.Now;
            db.Entry(partnerType).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/PartnerTypes
        [ResponseType(typeof(PartnerType))]
        public async Task<IHttpActionResult> PostPartnerType(PartnerType partnerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PartnerType.Add(partnerType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = partnerType.Id }, partnerType);
        }

        // DELETE: api/PartnerTypes/5
        [ResponseType(typeof(PartnerType))]
        public async Task<IHttpActionResult> DeletePartnerType(int id)
        {
            PartnerType partnerType = await db.PartnerType.FindAsync(id);
            if (partnerType == null)
            {
                return NotFound();
            }

            db.PartnerType.Remove(partnerType);
            await db.SaveChangesAsync();

            return Ok(partnerType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartnerTypeExists(int id)
        {
            return db.PartnerType.Count(e => e.Id == id) > 0;
        }
    }
}