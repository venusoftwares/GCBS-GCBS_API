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

namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class BookingDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/ContactEnquiryViews
        public IQueryable<BookingDetail> GetContactEnquiryView()
        {
            return db.BookingDetails;
        }
        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBookingDetail(BookingDetailVisible bookingDetailVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bookingDetailVisible == null)
            {
                return BadRequest();
            }
            BookingDetail priceMaster = await db.BookingDetails.FindAsync(bookingDetailVisible.Id);
            priceMaster.Status = bookingDetailVisible.Visible;
            db.Entry(priceMaster).State = EntityState.Modified;
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

        private bool BookingDetailsExists(int id)
        {
            return db.BookingDetails.Count(e => e.Id == id) > 0;
        }
    }
}