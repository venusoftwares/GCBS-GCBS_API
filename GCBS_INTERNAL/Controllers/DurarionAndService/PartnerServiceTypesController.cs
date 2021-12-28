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
using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.DurartionAndServiceType;
using GCBS_INTERNAL.Provider;

namespace GCBS_INTERNAL.Controllers.DurarionAndService
{
    [CustomAuthorize]
    public class PartnerServiceTypesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PartnerServiceTypes
        public IQueryable<PartnerServiceType> GetPartnerServiceType()
        {
            return db.PartnerServiceType;
        }

        // GET: api/PartnerServiceTypes/5
        [ResponseType(typeof(PartnerServiceType))]
        public async Task<IHttpActionResult> GetPartnerServiceType(int id)
        {
            PartnerServiceType partnerServiceType = await db.PartnerServiceType.FindAsync(id);
            if (partnerServiceType == null)
            {
                return NotFound();
            }

            return Ok(partnerServiceType);
        }

        // PUT: api/PartnerServiceTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPartnerServiceType(int id, PartnerServiceType partnerServiceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != partnerServiceType.id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.PartnerServiceType.FindAsync(id);
                partnerServiceType.CreatedBy = re.CreatedBy;
                partnerServiceType.CreatedAt = re.CreatedAt;
                d.Dispose();
            } 

            db.Entry(partnerServiceType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerServiceTypeExists(id))
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

        // POST: api/PartnerServiceTypes
        [ResponseType(typeof(PartnerServiceType))]
        public async Task<IHttpActionResult> PostPartnerServiceType(PartnerServiceType partnerServiceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            partnerServiceType.CreatedAt = DateTime.Now;
            partnerServiceType.CreatedBy = userDetails.Id;
            db.PartnerServiceType.Add(partnerServiceType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = partnerServiceType.id }, partnerServiceType);
        }

        // DELETE: api/PartnerServiceTypes/5
        [ResponseType(typeof(PartnerServiceType))]
        public async Task<IHttpActionResult> DeletePartnerServiceType(int id)
        {
            PartnerServiceType partnerServiceType = await db.PartnerServiceType.FindAsync(id);
            if (partnerServiceType == null)
            {
                return NotFound();
            }

            db.PartnerServiceType.Remove(partnerServiceType);
            await db.SaveChangesAsync();

            return Ok(partnerServiceType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartnerServiceTypeExists(int id)
        {
            return db.PartnerServiceType.Count(e => e.id == id) > 0;
        }
    }
}