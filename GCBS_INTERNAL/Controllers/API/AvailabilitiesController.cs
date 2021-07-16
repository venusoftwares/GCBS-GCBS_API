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
    public class AvailabilitiesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();     

        // GET: api/Availabilities/5
        [ResponseType(typeof(Availability))]
        public async Task<IHttpActionResult> GetAvailability()
        {
            Availability availability = await db.Availability.Where(x=>x.UserId == userDetails.Id).FirstOrDefaultAsync();
            if (availability == null)
            {
                DateTime now = DateTime.Now;
                return Ok(new Availability
                {
                    Id = 0,
                    UserId = 0,
                    Friday = now.TimeOfDay,
                    Monday = now.TimeOfDay,
                    Thursday = now.TimeOfDay,
                    Wednesday = now.TimeOfDay,
                    Tuesday = now.TimeOfDay,
                    Saturday = now.TimeOfDay,
                    Sunday = now.TimeOfDay                     
                });
            }    
            return Ok(availability);
        }

        // PUT: api/Availabilities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAvailability(int id, Availability availability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != availability.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.Availability.FindAsync(id);
                availability.CreatedBy = re.CreatedBy;
                availability.CreatedOn = re.CreatedOn;
                availability.UserId = userDetails.Id;
                d.Dispose();
            }
            availability.UpdatedBy = userDetails.Id;
            availability.UpdatedOn = DateTime.Now;
            db.Entry(availability).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvailabilityExists(id))
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

        // POST: api/Availabilities
        [ResponseType(typeof(Availability))]
        public async Task<IHttpActionResult> PostAvailability(Availability availability)
        {
            availability.CreatedBy = userDetails.Id;
            availability.CreatedOn = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            availability.UserId = userDetails.Id;
            db.Availability.Add(availability);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = availability.Id }, availability);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AvailabilityExists(int id)
        {
            return db.Availability.Count(e => e.Id == id) > 0;
        }
    }
}