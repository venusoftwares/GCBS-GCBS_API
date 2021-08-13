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
    public class BookingTermsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();   

        // GET: api/BookingTerms/5
        [ResponseType(typeof(BookingTerms))]
        public async Task<IHttpActionResult> GetBookingTerms()
        {
            BookingTerms bookingTerms = await db.BookingTerms.FindAsync(1);
            if (bookingTerms == null)
            {
                return NotFound();
            }

            return Ok(bookingTerms);
        }

        // PUT: api/BookingTerms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBookingTerms(BookingTerms bookingTerms)
        {
            using (var d = new DatabaseContext())
            {
                var re = await d.BookingTerms.FindAsync(1);
                bookingTerms.CreatedBy = re.CreatedBy;
                bookingTerms.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            bookingTerms.UpdatedBy = userDetails.Id;
            bookingTerms.UpdatedOn = DateTime.Now;
            db.Entry(bookingTerms).State = EntityState.Modified;
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

        private bool BookingTermsExists(int id)
        {
            return db.BookingTerms.Count(e => e.Id == id) > 0;
        }
    }
}