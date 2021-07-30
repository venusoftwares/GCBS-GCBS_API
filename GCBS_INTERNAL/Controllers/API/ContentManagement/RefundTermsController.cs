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

namespace GCBS_INTERNAL.Controllers.API.ContentManagement
{
    [CustomAuthorize]
    public class RefundTermsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

     
        // GET: api/RefundTerms/5
        [ResponseType(typeof(RefundTerm))]
        public async Task<IHttpActionResult> GetRefundTerm()
        {
            RefundTerm refundTerm = await db.RefundTerms.FindAsync(1);
            if (refundTerm == null)
            {
                return NotFound();
            }

            return Ok(refundTerm);
        }

        // PUT: api/RefundTerms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRefundTerm(RefundTerm refundTerm)
        {
            using (var d = new DatabaseContext())
            {
                var re = await d.BookingTerms.FindAsync(1);
                refundTerm.CreatedBy = re.CreatedBy;
                refundTerm.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            refundTerm.UpdatedBy = userDetails.Id;
            refundTerm.UpdatedOn = DateTime.Now;
            db.Entry(refundTerm).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
              
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }    
    }
}