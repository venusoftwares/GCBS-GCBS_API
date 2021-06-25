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
    public class BookingDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private BookingDetails oldbookingDetails = new BookingDetails();
        // GET: api/BookingDetails
        public IQueryable<BookingDetails> GetBookingDetails()
        {
            return db.BookingDetails;
        }

        // GET: api/BookingDetails/5
        [ResponseType(typeof(BookingDetails))]
        public async Task<IHttpActionResult> GetBookingDetails(int id)
        {
            BookingDetails bookingDetails = await db.BookingDetails.FindAsync(id);
            if (bookingDetails == null)
            {
                return NotFound();
            }

            return Ok(bookingDetails);
        }

        // PUT: api/BookingDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBookingDetails(int id, BookingDetails bookingDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookingDetails.Id)
            {
                return BadRequest();
            }
            using(var co =new DatabaseContext())
            {
                oldbookingDetails = await co.BookingDetails.FindAsync(id);
                co.Dispose();
            }
           // BookingDetails oldbookingDetails = await db.BookingDetails.FindAsync(id);
            bookingDetails.UpdatedBy = userDetails.Id;
            bookingDetails.UpdatedOn = DateTime.Now;
            bookingDetails.CreatedOn = oldbookingDetails.CreatedOn;
            bookingDetails.CreatedBy = oldbookingDetails.CreatedBy;
            db.Entry(bookingDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingDetailsExists(id))
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

        // POST: api/BookingDetails
        [ResponseType(typeof(BookingDetails))]
        public async Task<IHttpActionResult> PostBookingDetails(BookingDetails bookingDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bookingDetails.CreatedBy = userDetails.Id;
            bookingDetails.CreatedOn = DateTime.Now;
            db.BookingDetails.Add(bookingDetails);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = bookingDetails.Id }, bookingDetails);
        }

        // DELETE: api/BookingDetails/5
        [ResponseType(typeof(BookingDetails))]
        public async Task<IHttpActionResult> DeleteBookingDetails(int id)
        {
            BookingDetails bookingDetails = await db.BookingDetails.FindAsync(id);
            if (bookingDetails == null)
            {
                return NotFound();
            }

            db.BookingDetails.Remove(bookingDetails);
            await db.SaveChangesAsync();

            return Ok(bookingDetails);
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