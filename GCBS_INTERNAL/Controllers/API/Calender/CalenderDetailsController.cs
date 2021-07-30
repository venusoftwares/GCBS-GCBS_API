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

namespace GCBS_INTERNAL.Controllers.API.Calender
{
    [CustomAuthorize]
    public class CalenderDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/CalenderDetails
        public IQueryable<CalenderDetails> GetCalenderDetails()
        {
            return db.CalenderDetails.OrderByDescending(x=>x.Start).Where(x=>x.UserId == userDetails.Id);
        }

        // GET: api/CalenderDetails/5
        [ResponseType(typeof(CalenderDetails))]
        public async Task<IHttpActionResult> GetCalenderDetails(int id)
        {
            CalenderDetails calenderDetails = await db.CalenderDetails.FindAsync(id); 
            if (calenderDetails == null)
            {
                return NotFound();
            }     
            if(calenderDetails.UserId != userDetails.Id)
            {
                return NotFound();
            }
            return Ok(calenderDetails);
        }

        // PUT: api/CalenderDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCalenderDetails(int id, CalenderDetails calenderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calenderDetails.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.CalenderDetails.FindAsync(id);
                calenderDetails.CreatedBy = re.CreatedBy;
                calenderDetails.CreatedOn = re.CreatedOn;    
                d.Dispose();
            }
            calenderDetails.UpdatedBy = userDetails.Id;
            calenderDetails.UpdatedOn = DateTime.Now;
            db.Entry(calenderDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalenderDetailsExists(id))
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

        // POST: api/CalenderDetails
        [ResponseType(typeof(CalenderDetails))]
        public async Task<IHttpActionResult> PostCalenderDetails(CalenderDetails calenderDetails)
        {
            if (!ModelState.IsValid)
            {              
                return BadRequest(ModelState);
            }
            calenderDetails.CreatedOn = DateTime.Now;
            calenderDetails.CreatedBy = userDetails.Id;
            calenderDetails.UserId = userDetails.Id;
            db.CalenderDetails.Add(calenderDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = calenderDetails.Id }, calenderDetails);
        }

        // DELETE: api/CalenderDetails/5
        [ResponseType(typeof(CalenderDetails))]
        public async Task<IHttpActionResult> DeleteCalenderDetails(int id)
        {
            CalenderDetails calenderDetails = await db.CalenderDetails.FindAsync(id);
            if (calenderDetails == null)
            {
                return NotFound();
            }
            if (calenderDetails.UserId != userDetails.Id)
            {
                return NotFound();
            }
            db.CalenderDetails.Remove(calenderDetails);
            await db.SaveChangesAsync();

            return Ok(calenderDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalenderDetailsExists(int id)
        {
            return db.CalenderDetails.Count(e => e.Id == id) > 0;
        }
    }
}