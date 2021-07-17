using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;                                                                       using GCBS_INTERNAL.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GCBS_INTERNAL.Provider;

namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class PaymentGatewaysController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PaymentGateways
        public IQueryable<PaymentGateway> GetPaymentGateway()
        {
            return db.PaymentGateway;
        }

        // GET: api/PaymentGateways/5
        [ResponseType(typeof(PaymentGateway))]
        public async Task<IHttpActionResult> GetPaymentGateway(int id)
        {
            PaymentGateway paymentGateway = await db.PaymentGateway.FindAsync(id);
            if (paymentGateway == null)
            {
                return NotFound();
            }

            return Ok(paymentGateway);
        }

        // PUT: api/PaymentGateways/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPaymentGateway(int id, PaymentGateway paymentGateway)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paymentGateway.Id)
            {
                return BadRequest();
            }

            db.Entry(paymentGateway).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentGatewayExists(id))
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

        // POST: api/PaymentGateways
        [ResponseType(typeof(PaymentGateway))]
        public async Task<IHttpActionResult> PostPaymentGateway(PaymentGateway paymentGateway)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PaymentGateway.Add(paymentGateway);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = paymentGateway.Id }, paymentGateway);
        }

         

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaymentGatewayExists(int id)
        {
            return db.PaymentGateway.Count(e => e.Id == id) > 0;
        }
    }
}