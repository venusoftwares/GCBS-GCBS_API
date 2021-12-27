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
    public class DurationAndBasePricesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/DurationAndBasePrices
        public IQueryable<DurationAndBasePrice> GetDurationAndBasePrice()
        {
            return db.DurationAndBasePrice;
        }

        // GET: api/DurationAndBasePrices/5
        [ResponseType(typeof(DurationAndBasePrice))]
        public async Task<IHttpActionResult> GetDurationAndBasePrice(int id)
        {
            DurationAndBasePrice durationAndBasePrice = await db.DurationAndBasePrice.FindAsync(id);
            if (durationAndBasePrice == null)
            {
                return NotFound();
            }

            return Ok(durationAndBasePrice);
        }

        // PUT: api/DurationAndBasePrices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDurationAndBasePrice(int id, DurationAndBasePrice durationAndBasePrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != durationAndBasePrice.Id)
            {
                return BadRequest();
            }

            db.Entry(durationAndBasePrice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DurationAndBasePriceExists(id))
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

        // POST: api/DurationAndBasePrices
        [ResponseType(typeof(DurationAndBasePrice))]
        public async Task<IHttpActionResult> PostDurationAndBasePrice(DurationAndBasePrice durationAndBasePrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DurationAndBasePrice.Add(durationAndBasePrice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = durationAndBasePrice.Id }, durationAndBasePrice);
        }

        // DELETE: api/DurationAndBasePrices/5
        [ResponseType(typeof(DurationAndBasePrice))]
        public async Task<IHttpActionResult> DeleteDurationAndBasePrice(int id)
        {
            DurationAndBasePrice durationAndBasePrice = await db.DurationAndBasePrice.FindAsync(id);
            if (durationAndBasePrice == null)
            {
                return NotFound();
            }

            db.DurationAndBasePrice.Remove(durationAndBasePrice);
            await db.SaveChangesAsync();

            return Ok(durationAndBasePrice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DurationAndBasePriceExists(int id)
        {
            return db.DurationAndBasePrice.Count(e => e.Id == id) > 0;
        }
    }
}