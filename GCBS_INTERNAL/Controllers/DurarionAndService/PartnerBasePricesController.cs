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
using GCBS_INTERNAL.Models.DurartionAndServiceType;

namespace GCBS_INTERNAL.Controllers.DurarionAndService
{
    public class PartnerBasePricesController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PartnerBasePrices
        public IQueryable<PartnerBasePrice> GetPartnerBasePrice()
        {
            return db.PartnerBasePrice;
        }

        // GET: api/PartnerBasePrices/5
        [ResponseType(typeof(PartnerBasePrice))]
        public async Task<IHttpActionResult> GetPartnerBasePrice(int id)
        {
            PartnerBasePrice partnerBasePrice = await db.PartnerBasePrice.FindAsync(id);
            if (partnerBasePrice == null)
            {
                return NotFound();
            }

            return Ok(partnerBasePrice);
        }

        // PUT: api/PartnerBasePrices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPartnerBasePrice(int id, PartnerBasePrice partnerBasePrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != partnerBasePrice.id)
            {
                return BadRequest();
            }

            db.Entry(partnerBasePrice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerBasePriceExists(id))
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

        // POST: api/PartnerBasePrices
        [ResponseType(typeof(PartnerBasePrice))]
        public async Task<IHttpActionResult> PostPartnerBasePrice(PartnerBasePrice partnerBasePrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PartnerBasePrice.Add(partnerBasePrice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = partnerBasePrice.id }, partnerBasePrice);
        }

        // DELETE: api/PartnerBasePrices/5
        [ResponseType(typeof(PartnerBasePrice))]
        public async Task<IHttpActionResult> DeletePartnerBasePrice(int id)
        {
            PartnerBasePrice partnerBasePrice = await db.PartnerBasePrice.FindAsync(id);
            if (partnerBasePrice == null)
            {
                return NotFound();
            }

            db.PartnerBasePrice.Remove(partnerBasePrice);
            await db.SaveChangesAsync();

            return Ok(partnerBasePrice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartnerBasePriceExists(int id)
        {
            return db.PartnerBasePrice.Count(e => e.id == id) > 0;
        }
    }
}