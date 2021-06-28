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
    public class PaymentDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PaymentDetails
        public IQueryable<PaymentDetails> GetPaymentDetails()
        {
            return db.PaymentDetails;
        }

        // GET: api/PaymentDetails/5
        [ResponseType(typeof(PaymentDetails))]
        public async Task<IHttpActionResult> GetPaymentDetails(int id)
        {
            PaymentDetails paymentDetails = await db.PaymentDetails.FindAsync(id);
            if (paymentDetails == null)
            {
                return NotFound();
            }

            return Ok(paymentDetails);
        }

        // PUT: api/PaymentDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPaymentDetails(int id, PaymentDetails paymentDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paymentDetails.Id)
            {
                return BadRequest();
            }

            db.Entry(paymentDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentDetailsExists(id))
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

        // POST: api/PaymentDetails
        [ResponseType(typeof(PaymentDetails))]
        public async Task<IHttpActionResult> PostPaymentDetails(PaymentDetails paymentDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PaymentDetails.Add(paymentDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = paymentDetails.Id }, paymentDetails);
        }

        // DELETE: api/PaymentDetails/5
        [ResponseType(typeof(PaymentDetails))]
        public async Task<IHttpActionResult> DeletePaymentDetails(int id)
        {
            PaymentDetails paymentDetails = await db.PaymentDetails.FindAsync(id);
            if (paymentDetails == null)
            {
                return NotFound();
            }

            db.PaymentDetails.Remove(paymentDetails);
            await db.SaveChangesAsync();

            return Ok(paymentDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentDetailsExists(int id)
        {
            return db.PaymentDetails.Count(e => e.Id == id) > 0;
        }
    }
}