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
    public class CityMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/CityMasters
        [Route("api/CityMasters/{CountryId}/{StateId}/{Status}")]
        public IQueryable<CityMaster> GetCityMaster(int CountryId,int StateId, int Status)
        {
            IQueryable<CityMaster> filter = db.CityMaster;
            if (CountryId > 0)
            {
                filter = filter.Where(x => x.CountryId == CountryId);
            }
            if (StateId > 0)
            {
                filter = filter.Where(x => x.StateId == StateId);
            }
            if (Status == 0 || Status == 1)
            {
                var status = Status == 1 ? true : false;
                filter = filter.Where(x => x.Status == status);
            }
            return filter;
        }

        // GET: api/CityMasters/5
        [ResponseType(typeof(CityMaster))]
        public async Task<IHttpActionResult> GetCityMaster(int id)
        {
            CityMaster cityMaster = await db.CityMaster.FindAsync(id);
            if (cityMaster == null)
            {
                return NotFound();
            }

            return Ok(cityMaster);
        }

        // PUT: api/CityMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCityMaster(int id, CityMaster cityMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cityMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(cityMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityMasterExists(id))
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

        // POST: api/CityMasters
        [ResponseType(typeof(CityMaster))]
        public async Task<IHttpActionResult> PostCityMaster(CityMaster cityMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CityMaster.Add(cityMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cityMaster.Id }, cityMaster);
        }

        // DELETE: api/CityMasters/5
        [ResponseType(typeof(CityMaster))]
        public async Task<IHttpActionResult> DeleteCityMaster(int id)
        {
            CityMaster cityMaster = await db.CityMaster.FindAsync(id);
            if (cityMaster == null)
            {
                return NotFound();
            }

            db.CityMaster.Remove(cityMaster);
            await db.SaveChangesAsync();

            return Ok(cityMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityMasterExists(int id)
        {
            return db.CityMaster.Count(e => e.Id == id) > 0;
        }
    }
}