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
    public class PartnerAdditionalPricesController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PartnerAdditionalPrices
        public IQueryable<PartnerAdditionalPrice> GetPartnerAdditionalPrice()
        {
            return db.PartnerAdditionalPrice;
        }

        // GET: api/PartnerAdditionalPrices/5
        [ResponseType(typeof(PartnerAdditionalPrice))]
        public async Task<IHttpActionResult> GetPartnerAdditionalPrice(int id)
        {
            PartnerAdditionalPrice partnerAdditionalPrice = await db.PartnerAdditionalPrice.FindAsync(id);
            if (partnerAdditionalPrice == null)
            {
                return NotFound();
            }

            return Ok(partnerAdditionalPrice);
        }

        // PUT: api/PartnerAdditionalPrices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPartnerAdditionalPrice(int id, PartnerAdditionalPrice partnerAdditionalPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != partnerAdditionalPrice.id)
            {
                return BadRequest();
            }

            db.Entry(partnerAdditionalPrice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerAdditionalPriceExists(id))
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

        // POST: api/PartnerAdditionalPrices
        [ResponseType(typeof(PartnerAdditionalPrice))]
        public async Task<IHttpActionResult> PostPartnerAdditionalPrice(PartnerAdditionalPrice partnerAdditionalPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PartnerAdditionalPrice.Add(partnerAdditionalPrice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = partnerAdditionalPrice.id }, partnerAdditionalPrice);
        }

        // DELETE: api/PartnerAdditionalPrices/5
        [ResponseType(typeof(PartnerAdditionalPrice))]
        public async Task<IHttpActionResult> DeletePartnerAdditionalPrice(int id)
        {
            PartnerAdditionalPrice partnerAdditionalPrice = await db.PartnerAdditionalPrice.FindAsync(id);
            if (partnerAdditionalPrice == null)
            {
                return NotFound();
            }

            db.PartnerAdditionalPrice.Remove(partnerAdditionalPrice);
            await db.SaveChangesAsync();

            return Ok(partnerAdditionalPrice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartnerAdditionalPriceExists(int id)
        {
            return db.PartnerAdditionalPrice.Count(e => e.id == id) > 0;
        }
    }
}