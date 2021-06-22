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
    public class LocationMastersController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/LocationMasters
        public IQueryable<LocationMaster> GetLocationMaster()
        {
            return db.LocationMaster;
        }

        // GET: api/LocationMasters/5
        [ResponseType(typeof(LocationMaster))]
        public async Task<IHttpActionResult> GetLocationMaster(int id)
        {
            LocationMaster locationMaster = await db.LocationMaster.FindAsync(id);
            if (locationMaster == null)
            {
                return NotFound();
            }

            return Ok(locationMaster);
        }

        // PUT: api/LocationMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocationMaster(int id, LocationMaster locationMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(locationMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationMasterExists(id))
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

        // POST: api/LocationMasters
        [ResponseType(typeof(LocationMaster))]
        public async Task<IHttpActionResult> PostLocationMaster(LocationMaster locationMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LocationMaster.Add(locationMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = locationMaster.Id }, locationMaster);
        }

        // DELETE: api/LocationMasters/5
        [ResponseType(typeof(LocationMaster))]
        public async Task<IHttpActionResult> DeleteLocationMaster(int id)
        {
            LocationMaster locationMaster = await db.LocationMaster.FindAsync(id);
            if (locationMaster == null)
            {
                return NotFound();
            }

            db.LocationMaster.Remove(locationMaster);
            await db.SaveChangesAsync();

            return Ok(locationMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationMasterExists(int id)
        {
            return db.LocationMaster.Count(e => e.Id == id) > 0;
        }
    }
}