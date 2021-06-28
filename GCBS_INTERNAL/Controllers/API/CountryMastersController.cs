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
    public class CountryMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/CountryMasters
        public IQueryable<CountryMaster> GetCountryMaster()
        {
            return db.CountryMaster;
        }

        // GET: api/CountryMasters/5
        [ResponseType(typeof(CountryMaster))]
        public async Task<IHttpActionResult> GetCountryMaster(int id)
        {
            CountryMaster countryMaster = await db.CountryMaster.FindAsync(id);
            if (countryMaster == null)
            {
                return NotFound();
            }

            return Ok(countryMaster);
        }

        // PUT: api/CountryMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCountryMaster(int id, CountryMaster countryMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != countryMaster.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.CountryMaster.FindAsync(id);
                countryMaster.CreatedBy = re.CreatedBy;
                countryMaster.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            db.Entry(countryMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryMasterExists(id))
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

        // POST: api/CountryMasters
        [ResponseType(typeof(CountryMaster))]
        public async Task<IHttpActionResult> PostCountryMaster(CountryMaster countryMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CountryMaster.Add(countryMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = countryMaster.Id }, countryMaster);
        }

        // DELETE: api/CountryMasters/5
        [ResponseType(typeof(CountryMaster))]
        public async Task<IHttpActionResult> DeleteCountryMaster(int id)
        {
            CountryMaster countryMaster = await db.CountryMaster.FindAsync(id);
            if (countryMaster == null)
            {
                return NotFound();
            }

            db.CountryMaster.Remove(countryMaster);
            await db.SaveChangesAsync();

            return Ok(countryMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CountryMasterExists(int id)
        {
            return db.CountryMaster.Count(e => e.Id == id) > 0;
        }
    }
}