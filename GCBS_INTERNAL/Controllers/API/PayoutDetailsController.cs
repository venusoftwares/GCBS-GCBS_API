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
    public class PayoutDetailsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PayoutDetails
        public IQueryable<PayoutDetails> GetPayoutDetails()
        {
            return db.PayoutDetails;
        }

        // GET: api/PayoutDetails/5
        [ResponseType(typeof(PayoutDetails))]
        public async Task<IHttpActionResult> GetPayoutDetails(int id)
        {
            PayoutDetails payoutDetails = await db.PayoutDetails.FindAsync(id);
            if (payoutDetails == null)
            {
                return NotFound();
            }

            return Ok(payoutDetails);
        }

        // PUT: api/PayoutDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPayoutDetails(int id, PayoutDetails payoutDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payoutDetails.Id)
            {
                return BadRequest();
            }

            db.Entry(payoutDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayoutDetailsExists(id))
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

        // POST: api/PayoutDetails
        [ResponseType(typeof(PayoutDetails))]
        public async Task<IHttpActionResult> PostPayoutDetails(PayoutDetails payoutDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PayoutDetails.Add(payoutDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = payoutDetails.Id }, payoutDetails);
        }

        // DELETE: api/PayoutDetails/5
        [ResponseType(typeof(PayoutDetails))]
        public async Task<IHttpActionResult> DeletePayoutDetails(int id)
        {
            PayoutDetails payoutDetails = await db.PayoutDetails.FindAsync(id);
            if (payoutDetails == null)
            {
                return NotFound();
            }

            db.PayoutDetails.Remove(payoutDetails);
            await db.SaveChangesAsync();

            return Ok(payoutDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PayoutDetailsExists(int id)
        {
            return db.PayoutDetails.Count(e => e.Id == id) > 0;
        }
    }
}