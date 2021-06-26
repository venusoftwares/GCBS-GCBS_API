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
    public class LocationMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/LocationMasters
        public IQueryable<LocationMasters> GetLocationMasters()
        {
            return db.LocationMasters;
        }

        // GET: api/LocationMasters/5
        [ResponseType(typeof(LocationMasters))]
        public async Task<IHttpActionResult> GetLocationMasters(int id)
        {
            LocationMasters locationMasters = await db.LocationMasters.FindAsync(id);
            if (locationMasters == null)
            {
                return NotFound();
            }

            return Ok(locationMasters);
        }

        // PUT: api/LocationMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocationMasters(int id, LocationMasters locationMasters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationMasters.Id)
            {
                return BadRequest();
            }

            db.Entry(locationMasters).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationMastersExists(id))
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

        // PUT: api/LocationMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocationMasters(LocationMasterVisible locationMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (locationMasterVisible == null)
            {
                return BadRequest();
            }
            LocationMasters locationMasters = await db.LocationMasters.FindAsync(locationMasterVisible.Id);
            locationMasters.Status= locationMasterVisible.Status;
            db.Entry(locationMasters).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        


        // POST: api/LocationMasters
        [ResponseType(typeof(LocationMasters))]
        public async Task<IHttpActionResult> PostLocationMasters(LocationMasters locationMasters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LocationMasters.Add(locationMasters);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = locationMasters.Id }, locationMasters);
        }

        // DELETE: api/LocationMasters/5
        [ResponseType(typeof(LocationMasters))]
        public async Task<IHttpActionResult> DeleteLocationMasters(int id)
        {
            LocationMasters locationMasters = await db.LocationMasters.FindAsync(id);
            if (locationMasters == null)
            {
                return NotFound();
            }

            db.LocationMasters.Remove(locationMasters);
            await db.SaveChangesAsync();

            return Ok(locationMasters);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationMastersExists(int id)
        {
            return db.LocationMasters.Count(e => e.Id == id) > 0;
        }
    }
}