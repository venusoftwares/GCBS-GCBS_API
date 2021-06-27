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
        [Route("api/LocationMasters/{CountryId}/{StateId}/{CityId}/{Status}")]
        public IQueryable<LocationMasterViewModel> GetLocationMasters(int CountryId, int StateId,int CityId, int Status)
        {
            IQueryable<LocationMasterViewModel> filter = db.LocationMasters
                .Include(x=>x.CityMaster)
                .Include(x=>x.CountryMaster)
                .Include(x=>x.StateMaster)
                .Select(x=> new LocationMasterViewModel
                {
                    City = x.CityMaster.CityName  ,
                    State = x.StateMaster.StateName,
                    Location = x.Location,
                    PinCode = x.PinCode,
                    Status = x.Status,
                    Country = x.CountryMaster.CountryName,
                    CountryId = x.CountryId,
                    StateId =x .StateId,
                    CityId = x.CityId,
                    Id = x.Id
                });
            if (CountryId > 0)
            {
                filter = filter.Where(x => x.CountryId == CountryId);
            }
            if (StateId > 0)
            {
                filter = filter.Where(x => x.StateId == StateId);
            }
            if (CityId > 0)
            {
                filter = filter.Where(x => x.CityId == CityId);
            }
            if (Status == 0 || Status == 1)
            {
                var status = Status == 1 ? true : false;
                filter = filter.Where(x => x.Status == status);
            }
            return filter;
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